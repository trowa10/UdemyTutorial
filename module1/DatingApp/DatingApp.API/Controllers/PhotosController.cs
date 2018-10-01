using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorize;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/photos")]
    [ApiController]
    public class PhotosController : ControllersBase
    {
        public PhotosController(IDatingRepository repo, IMapper mapper,
         IOptions<CloudinarySettings> cloudinaryConfig)
        {
            
        }
    }
}