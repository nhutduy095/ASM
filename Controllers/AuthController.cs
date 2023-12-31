﻿using Application.IService;
using Application.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASM_Student_MS.Controllers
{
    [Authorize]
    [Route("ASM/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IServices _iServices;
        public AuthController(IServices iServices)
        {
            _iServices = iServices;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _iServices.Login(request);
                    if (!result.Status)
                    {
                        return BadRequest(result);
                    }
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }

        [AllowAnonymous]
        [HttpPost("Test")]
        public async Task<IActionResult> Test()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _iServices.GetAsync();
                    
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }
    }
}
