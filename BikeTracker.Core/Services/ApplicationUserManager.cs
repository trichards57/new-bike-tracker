using BikeTracker.Core.Data;
using BikeTracker.Core.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BikeTracker.Core.Services
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        private readonly ApplicationDbContext _context;

        public ApplicationUserManager(IUserStore<ApplicationUser> store, IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<ApplicationUser> passwordHasher,
            IEnumerable<IUserValidator<ApplicationUser>> userValidators,
            IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<ApplicationUser>> logger,
            ApplicationDbContext context) :
            base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors,
                services, logger)
        {
            _context = context;
        }

        public async Task ClearTokensAync(ApplicationUser user)
        {
            var tokens = _context.RefreshTokens.Where(t => t.UserId == user.Id);

            _context.RefreshTokens.RemoveRange(tokens);

            await _context.SaveChangesAsync();
        }

        public async Task<string> GetRefreshTokenAsync(ApplicationUser user)
        {
            var rng = new RNGCryptoServiceProvider();

            var tokenBuffer = new byte[32];
            var saltBuffer = new byte[32];

            rng.GetNonZeroBytes(tokenBuffer);
            rng.GetNonZeroBytes(saltBuffer);

            var hash = HashToken(saltBuffer, tokenBuffer);

            var token = new RefreshToken
            {
                Expires = DateTimeOffset.UtcNow.AddDays(7),
                TokenHash = hash,
                TokenSalt = Convert.ToBase64String(saltBuffer),
                User = user
            };

            _context.RefreshTokens.Add(token);
            await _context.SaveChangesAsync();

            return Convert.ToBase64String(tokenBuffer);
        }

        public async Task<ApplicationUser> ValidateTokenAsync(string token)
        {
            var tokens = _context.RefreshTokens.Include(t=>t.User).Where(t=>t.Expires > DateTimeOffset.UtcNow);

            var tokenBuffer = Convert.FromBase64String(token);

            var hashes = tokens.Select(t => new { Hash = HashToken(Convert.FromBase64String(t.TokenSalt), tokenBuffer), Token = t });

           return (await hashes.FirstOrDefaultAsync(h => h.Hash == h.Token.TokenHash))?.Token.User;
        }

        private static string HashToken(byte[] saltBuffer, byte[] tokenBuffer)
        {
            var total = saltBuffer.Concat(tokenBuffer).ToArray();

            var hasher = new SHA512Cng();
            var hash = Convert.ToBase64String(hasher.ComputeHash(total));
            return hash;
        }
    }
}
