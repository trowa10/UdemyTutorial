using System.Collections.Generic;
using System.Text;
using DatingApp.API.Models;
using Newtonsoft.Json;

namespace DatingApp.API.Data
{
    public class Seed 
    {
        private readonly DataContext _context;

        public Seed(DataContext context) {
            this._context = context;
        }

        public void SeedUsers() {
            var useData = System.IO.File.ReadAllText("Data/UserSeedData.json");
            var users = JsonConvert.DeserializeObject<List<User>>(useData);

            foreach (var user in users) {
                byte[] passwordHash, passwordSalt;
                this.CreatePasswordHash("password",out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.Username = user.Username.ToLower();

                this._context.Users.Add(user);
            }

            this._context.SaveChanges();
        }

         private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}