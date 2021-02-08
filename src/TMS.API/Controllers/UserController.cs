using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMS.Domain.Entities;
using TMS.Domain.Models;
using TMS.Domain.Services;

namespace TMS.API.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IMapper mapper, IUserService userService) : base(mapper, userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateRequest authenModel)
        {
            ServiceResult responseService = new ServiceResult();
            var response = await _userService.Authenticate(authenModel, IpAddress());

            if (response == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            SetTokenCookie(response.RefreshToken);

            responseService.Data = response;
            return Ok(responseService);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            ServiceResult serviceResult = new ServiceResult();

            try
            {
                var userEntity = _mapper.Map<RegisterRequest, User>(registerRequest);
                await _userService.Register(userEntity, Request.Headers["origin"]);
            }
            catch (Exception ex)
            {
            }

            return Ok(serviceResult);
        }

        /// <summary>
        /// set httponly cho refresh token
        /// </summary>
        /// <param name="token"></param>
        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
    }
}
