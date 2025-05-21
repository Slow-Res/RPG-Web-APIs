 
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Project.Data
{
    public class AuthRepository : IAuthRepository
    {
        public DataContext _context { get; }
        public IConfiguration _configuration { get; }


        public AuthRepository(DataContext context, IConfiguration configuration)
        {   
            // test
            _context = context;
            _configuration = configuration;

        }
        public async Task<ServiceResponse<string>> Login(string username, string passowrd)
        {
            var user = await _context.Users.FirstOrDefaultAsync( u => u.UserName == username);
            return  user is null ?
            new ServiceResponse<string> { Message = "Incorrect username or passowrd", Success= false } :
                VerifyPasswordHash(passowrd, user.PasswordHash, user.PasswordSalt) ?
                    new ServiceResponse<string> { Data = CreateToken(user) } :
                    new ServiceResponse<string> { Message = "Incorrect username or passowrd", Success= false };
        }

        public Task<ServiceResponse<bool>> Logout(string username)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<int>> Register(User user, string passowrd)
        {
            if( await UserExists(user.UserName))
                return new ServiceResponse<int>(){ Success = false, Message="User Already Exists" };

            CreatePasswordHash(passowrd, out byte[] passwordHash, out byte[] passwordSalt );
            user.PasswordSalt = passwordSalt;
            user.PasswordHash = passwordHash;
            await  _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return new ServiceResponse<int>() { Data = user.Id, Message="A new user created sucessfully"};
        }

        public async Task<bool> UserExists(string? userName)
        {
            return (await _context.Users.FirstOrDefaultAsync( c => c.UserName == userName)) is not null;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512() )
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string passowrd, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt) )
            {
                var ComputeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(passowrd));
                return ComputeHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("UserName" , user.UserName)
            };
            var appSettingsToken = _configuration.GetSection("AppSettings:AccessTokenSeceret").Value ?? throw new Exception("Missing Token Secret");
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(appSettingsToken));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds,
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}