using BikeTracker.Core.Models;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BikeTracker.Core.Services
{
    public static class DateTimeExtensions
    {
        public static long ToUnixTimestamp(this DateTime d)
        {
            var epoch = d - new DateTime(1970, 1, 1, 0, 0, 0);

            return (long)epoch.TotalSeconds;
        }
    }

    [UsedImplicitly]
    public class TokenProvider
    {
        private readonly RequestDelegate _next;
        private readonly TokenProviderOptions _options;
        private readonly ApplicationUserManager _userManager;

        public TokenProvider(RequestDelegate next, IOptions<TokenProviderOptions> options,
            ApplicationUserManager userManager)
        {
            _next = next;
            _options = options.Value;
            _userManager = userManager;
        }

        public Task Invoke(HttpContext context)
        {
            // If the request path doesn't match, skip
            if (context.Request.Path.Equals(_options.TokenPath, StringComparison.Ordinal))
            {
                // Request must be POST with Content-Type: application/json
                if (context.Request.Method.Equals("POST") && context.Request.ContentType == "application/json")
                    return GenerateToken(context);
                context.Response.StatusCode = 400;
                return context.Response.WriteAsync("Bad request.");
            }

            if (context.Request.Path.Equals(_options.RefreshPath, StringComparison.Ordinal))
            {
                // Request must be POST with Content-Type: application/json
                if (context.Request.Method.Equals("POST")) return RefreshToken(context);
                context.Response.StatusCode = 400;
                return context.Response.WriteAsync("Bad request.");
            }

            return _next(context);
        }

        private async Task GenerateToken(HttpContext context)
        {
            var bodyReader = new StreamReader(context.Request.Body);
            var body = await bodyReader.ReadToEndAsync();

            dynamic model = JsonConvert.DeserializeObject(body);

            var username = model.username.Value;
            var password = model.password.Value;

            ApplicationUser identity = await GetIdentity(username, password);
            if (identity == null)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid username or password.");
                return;
            }

            await GetToken(context, identity);
        }

        private async Task<ApplicationUser> GetIdentity(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
                return null;

            return user;
        }

        private async Task GetToken(HttpContext context, ApplicationUser identity)
        {
            var now = DateTime.UtcNow;

            // Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
            // You can add other claims here, if you want:
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, identity.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, now.ToUnixTimestamp().ToString(), ClaimValueTypes.Integer64),
                new Claim(ClaimTypes.Name, identity.UserName)
            };

            // Create the JWT and write it to a string
            var jwt = new JwtSecurityToken(_options.Issuer, _options.Audience, claims, now,
                now.Add(_options.Expiration), _options.SigningCredentials);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var refreshToken = await _userManager.GetRefreshTokenAsync(identity);

            var response = new
            {
                access_token = encodedJwt,
                expires_in = (int)_options.Expiration.TotalSeconds,
            };

            context.Response.Cookies.Append("access_token", encodedJwt, new CookieOptions
            {
                Expires = new DateTimeOffset(jwt.ValidTo),
                HttpOnly = true
            });
            context.Response.Cookies.Append("refresh_token", refreshToken, new CookieOptions
            {
                HttpOnly = true
            });

            // Serialize and return the response
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(
                JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }

        private async Task RefreshToken(HttpContext context)
        {
            if (!context.Request.Cookies.ContainsKey("refresh_token"))
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("refresh_token is required.");
                return;
            }

            var token = context.Request.Cookies["refresh_token"];
            var user = await _userManager.ValidateTokenAsync(token);

            if (user == null)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid refresh_token.");
                return;
            }

            await GetToken(context, user);
        }
    }

    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
    public class TokenProviderOptions
    {
        public string Audience { get; set; }
        public TimeSpan Expiration { get; set; } = TimeSpan.FromMinutes(5);
        public string Issuer { get; set; }
        public string RefreshPath { get; set; } = "/refreshToken";
        public SigningCredentials SigningCredentials { get; set; }
        public string TokenPath { get; set; } = "/token";
    }
}
