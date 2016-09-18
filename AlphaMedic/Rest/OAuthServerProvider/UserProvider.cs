using Rest.Models;
using Rest.Models.AlphaMedicContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Rest.OAuthServerProvider
{
    public class UserProvider
    {
        AlphaMedicContext _db;
        public UserProvider(AlphaMedicContext context)
        {
            _db = context;
        }


        public async Task AddUserAsync(User user, string password)
        {
            if (await UserExists(user))
            {
                throw new Exception(
                    "A user with that Email address already exists");
            }            
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
        }

        public async Task<MobileAuthentificator> FindMAuthByIdAsync(int id)
        {
            try
            {
                return await _db.MobileAuthentificators.FirstOrDefaultAsync(
                    u => u.UserId == id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task AddMobileAuthentificatorAsync(MobileAuthentificator mAuth)
        {
            _db.MobileAuthentificators.Add(mAuth);
            await _db.SaveChangesAsync();
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _db.Users
            .FirstOrDefaultAsync(u => u.Email == email);
        }


        public async Task<User> FindByIdAsync(int userId)
        {
            return await _db.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }


        public async Task<bool> UserExists(User user)
        {
            return await _db.Users
                .AnyAsync(u => u.UserId == user.UserId || u.Email == user.Email);
        }


        public async Task AddClaimAsync(int UserId, UserClaim claim)
        {
            var user = await FindByIdAsync(UserId);
            if (user == null)
            {
                throw new Exception("User does not exist");
            }
            user.UserClaim=claim;
            await _db.SaveChangesAsync();
        }
       
    }
}