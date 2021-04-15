    using MailKit.Net.Smtp;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using MimeKit;
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using ZooMag.Models;
    using ZooMag.ViewModels;

    namespace ZooMag.Controllers
    {
        [Route("Auth")]
        [ApiController]
        public class AuthenticateController : ControllerBase
        {
            private readonly UserManager<User> userManager;
            private readonly IConfiguration _configuration;

            public AuthenticateController(UserManager<User> userManager, IConfiguration configuration)
            {
                this.userManager = userManager;
                _configuration = configuration;
            }

            [HttpPost]
            [Route("login")]
            public async Task<IActionResult> Login(LoginModel model)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return Unauthorized(new Response { Status = "Error", Message = "Неправильный логин!" });
                }
                if (await userManager.CheckPasswordAsync(user, model.Password))
                {
                    var userRoles = await userManager.GetRolesAsync(user);

                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim("UserId", user.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddHours(3),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );

                    return Ok(new
                    {
                        Position = userRoles,
                        token = new JwtSecurityTokenHandler().WriteToken(token)
                    });
                }
                return Unauthorized(new Response { Status = "Error", Message = "Неправильный пароль!" });
            }

            [HttpGet]
            [Route("userData")]
            [Authorize]
            public async Task<IActionResult> GetUserData()
            {
                var user = await userManager.FindByEmailAsync(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value);
                if(user==null)
                {
                    return BadRequest();
                }
                UserModel userModel = new UserModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    BirthDay = user.BirthDay,
                    GenderId = user.GenderId,
                    Image = user.Image,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                };
                var userRoles = await userManager.GetRolesAsync(user);
                userModel.Position = userRoles.ToList<string>();
                return Ok(userModel);
            }



            [HttpPost]
            [Route("register")]
            public async Task<IActionResult> Register([FromBody] RegisterModel model)
            {
                if (model.Password == model.ConfirmPassword && model.Password != null)
                {
                    var userExists = await userManager.FindByEmailAsync(model.Email);
                    if (userExists != null)
                    {
                        return Unauthorized(new Response { Status = "Error", Message = "Пользователь с таким логин существует!" });
                    }

                    User user = new User()
                    {
                        Email = model.Email,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        UserName = model.Email,
                        GenderId = 1,
                        Image = "Resources/Images/Users/useravatar.svg"
                    };
                    var result = await userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "Клиент");
                        return await Login(new LoginModel { Email = model.Email, Password = model.Password });
                        //Ok(new Response { Status = "Success", Message = "Пользователь успешно добавлен!" });
                    }
                    else
                    {
                        var errorMessages = result.Errors.FirstOrDefault();
                        return Unauthorized(new Response { Status = errorMessages.Code, Message = errorMessages.Description });
                    }
                }
                else
                {
                    return Unauthorized(new Response { Status = "Password", Message = "Пароль не совподает!" });
                }
            }


            [HttpPost]
            [Route("forgotpassword")]
            public async Task<IActionResult> ForgotPassword(string Email)
            {
                try
                {
                    var user = await userManager.FindByEmailAsync(Email);
                    if (user == null)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "Такого пользователья нет в нашей базе" });
                    }
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var emailMessage = new MimeMessage();
                    emailMessage.From.Add(new MailboxAddress("Администрация сайта", "ZooMagazin"));
                    emailMessage.To.Add(new MailboxAddress(user.UserName, user.Email));
                    emailMessage.Subject = "Введите новый пароль";
                    emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                    {
                        Text = $"Введите новый пароль, перейдя по ссылке: <a href='http://localhost:3000/resetpwd?email=" + Email + "&token=" + token + "'>link</a>"
                    };
                    using (var client = new SmtpClient())
                    {
                        await client.ConnectAsync("smtp.gmail.com", 25, false);
                        await client.AuthenticateAsync("ZooMagazin2021@gmail.com", "ZooMagazin2021_A");
                        await client.SendAsync(emailMessage);
                        await client.DisconnectAsync(true);
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = ex.Message });
                }
                return Ok(new Response { Status = "Success", Message = "Вам на почту отправлена ссылка, перейдя по которой вы можете добавить новый пароль" });
            }

            [HttpPost]
            [Route("resetpassword")]
            public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                var ress = new List<Response>();
                if (user != null)
                {
                    var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                        return Ok(new Response { Status = "Success", Message = "Пароль успешно иземенен" });
                    foreach (var item in result.Errors)
                    {
                        ress.Add(new Response { Status = item.Code, Message = item.Description });
                    }
                }
                else
                {
                    ress.Add(new Response { Status = "Email", Message = "Такого пользователья нет в нашей базе" });
                }
                return StatusCode(StatusCodes.Status400BadRequest, ress);
            }
        }
    }