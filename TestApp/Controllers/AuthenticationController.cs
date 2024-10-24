using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestApp.Authmodels;
using TestApp.DAL.Entities;

namespace TestApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;
        private readonly JwtOptions _jwtOptions;

        public AuthenticationController(UserManager<Users>userManager , SignInManager<Users>signInManager , JwtOptions jwtOptions)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtOptions = jwtOptions;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(Register model)
        {
            if (ModelState.IsValid)
            {
                var user = new Users()
                {
                    UserName = model.Email.Split('@')[0],
                    Email = model.Email,   
                    FullName = model.FullName,
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return Ok("User registered successfully.");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return BadRequest(ModelState);
                }
            }
            return BadRequest(ModelState);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(login model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    bool isPasswordCorrect = await _userManager.CheckPasswordAsync(user, model.Password);

                    if (isPasswordCorrect)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

                        if (result.Succeeded)
                        {
                            // Create a list of claims (you can add more claims as needed)
                            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, user.Email),            // Email claim
                        new Claim(ClaimTypes.Name, user.FullName),           // Username claim
                        new Claim(ClaimTypes.NameIdentifier, user.Id)        // User ID claim
                        // You can add more claims here, e.g., roles
                    };

                            // Optionally, add role claims if the user has roles
                            var userRoles = await _userManager.GetRolesAsync(user);
                            foreach (var role in userRoles)
                            {
                                claims.Add(new Claim(ClaimTypes.Role, role));       // Role claim
                            }

                            // Token handler and descriptor
                            var tokenHandler = new JwtSecurityTokenHandler();
                            var tokenDescriptor = new SecurityTokenDescriptor
                            {
                                Subject = new ClaimsIdentity(claims),
                                Expires = DateTime.UtcNow.AddMinutes(30),              // Token expiration time
                                SigningCredentials = new SigningCredentials(
                                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningKey)),
                                    SecurityAlgorithms.HmacSha256),
                                Issuer = _jwtOptions.Issuer,
                                Audience = _jwtOptions.Audience
                            };

                            // Create the token
                            var token = tokenHandler.CreateToken(tokenDescriptor);
                            var tokenString = tokenHandler.WriteToken(token);

                            // Return the token
                            return Ok(new
                            {
                                Token = tokenString,
                                userId = user.Id,
                                Expiration = token.ValidTo
                            });
                        }
                        else
                        {
                            return BadRequest("Login failed.");
                        }
                    }
                    else
                    {
                        return BadRequest("Incorrect password.");
                    }
                }
                else
                {
                    return BadRequest("Email not registered.");
                }
            }
            return BadRequest(ModelState);
        }


    }
}
