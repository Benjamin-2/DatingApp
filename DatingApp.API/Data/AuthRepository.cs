using System;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{

    public class AuthRepository : IAuthRepository
    {

        private readonly DataContext _context;

        public AuthRepository(DataContext context)
        {
            _context = context;

        }
        public async Task<User> Login(string username, string password)
        {
            // if username is matches then login otherwize null for use x.Username == username from FirstOrDefaultAsync..
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
            if(user == null)
                return null;

            // not match user then send unauthorized pass to user
            if (!VarifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            // this method pass then return user
            return user;

        }

        private bool VarifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computeHash.Length; i++)
                {
                    if(computeHash[i] != passwordHash[i] )
                        return false;
                }
            }
            // if password match
            return true;
        }

        public async Task<User> Register(User user, string password)
        {
            // store data here byte
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            // database add
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        //Method create 
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                // auto generate password
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string username)
        {
            // if username is match on database
            if(await _context.Users.AnyAsync(x => x.Username == username))
            {
                return true;
            }

            return false;
        }
    }
}