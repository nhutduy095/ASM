using Application.Entities;
using Application.IService;
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
    public class MasterDataController : ControllerBase
    {
        private readonly IServices _iServices;
        public MasterDataController(IServices iServices)
        {
            _iServices = iServices;
        }

        [AllowAnonymous]
        [HttpPost("CreateOrUpdateClass")]
        public async Task<IActionResult> CreateOrUpdateClass(List<CollectionClass> lstCollectionClass)
        {
            try
            {
                var res = _iServices.fnCoUCollectionClassAsync(lstCollectionClass, string.Empty);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }

    }
}
