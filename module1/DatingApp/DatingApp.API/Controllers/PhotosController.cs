
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/photos")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public PhotosController(IDatingRepository repo, IMapper mapper,       
            IOptions<CloudinarySettings> cloudinaryConfig)
        {
            this._cloudinaryConfig = cloudinaryConfig;
            this._mapper = mapper;
            this._repo = repo;
            
            Account acc = new Account(
                    _cloudinaryConfig.Value.CloudName,
                    _cloudinaryConfig.Value.ApiKey,
                    _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }

        [HttpGet("{id}", Name= "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromRepo = await _repo.GetPhoto(id);
            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);
            return Ok(photo);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoForuser(int userId, 
            [FromForm] PhotoForCreationDto photoForCreationDto)
        {
             //check the current user if the token is same with the token server
            if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var userFromRepo = await _repo.GetUser(userId);

            var file = photoForCreationDto.File;
            // upload to Cloudinarry
            var uploadResult = new  ImageUploadResult();

            if(file.Length > 0) {
                using ( var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams                       
                        {
                             File = new  FileDescription(file.Name, stream),
                             Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                        };

                        uploadResult = _cloudinary.Upload(uploadParams);
                
                }
            }
            photoForCreationDto.Url = uploadResult.Uri.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            var phot = _mapper.Map<Photo>(photoForCreationDto);

            if(!userFromRepo.Photos.Any(u => u.IsMain)) { 
                phot.IsMain = true;
            }

            //Add photo details to DB
            userFromRepo.Photos.Add(phot);            

            if(await _repo.SaveAll()) // execute save
            {
                var photoToReturn = _mapper.Map<PhotoForReturnDto>(phot);
                //GetPhoto came from the upper method
                return CreatedAtRoute("GetPhoto",new { id = phot.Id}, photoToReturn);
            }
            return BadRequest("Could not Add the photo");
        }
}
}