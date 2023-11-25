using Application.Authorization;
using Application.Entities;
using Application.IService;
using Application.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASM_Student_MS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterDataController : ControllerBase
    {
        private readonly IServices _iServices;
        public MasterDataController(IServices iServices)
        {
            _iServices = iServices;
        }
        #region class
        [HasPermission("ASM")]
        [HttpPost("GetCollectionClass")]
        public async Task<IActionResult> fnGetCollectionClass(RequestPaging request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _iServices.fnGetCollectionClassAsync(request);

                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }

        [HasPermission("ASM")]
        [HttpGet("GetCollectionClassByID")]
        public async Task<IActionResult> fnGetCollectionClassByID(string classId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _iServices.fnGetCollectionClassByIDAsync(classId);

                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }
        [HasPermission("ASM")]
        [HttpPost("CreateOrUpdateClass")]
        public async Task<IActionResult> CreateOrUpdateClass(List<CollectionClass> lstCollectionClass)
        {

            try
            {
                var res =await _iServices.fnCoUCollectionClassAsync(lstCollectionClass, string.Empty);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }
        #endregion
        [HttpPost("CreateOrUpdateCommon")]
        public async Task<IActionResult> CreateOrUpdateCommon(List<CollectionCommon> lstCollectionCommon)
        {
            try
            {
                var res = _iServices.fnCoUCollectionCommonAsync(lstCollectionCommon, string.Empty);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }
        [HasPermission("ASM")]
        [HttpPost("CreateOrUpdateDepartment")]
        public async Task<IActionResult> CreateOrUpdateDepartment(CollectionDepartment collectionDepartment)
        {
            try
            {
                var res = _iServices.fnCoUCollectionDepartmentAsync(collectionDepartment, string.Empty);
                return Ok(res);
                //return Ok("res");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }

        [HttpPost("CreateOrUpdateMajor")]
        public async Task<IActionResult> CreateOrUpdateMajor(CollectionMajor collectionMajor)
        {
            try
            {
                var res = _iServices.fnCoUCollectionMajorAsync(collectionMajor, string.Empty);
                return Ok(res);
                return Ok("res");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }

        [HttpPost("CreateOrUpdateMajorDtl")]
        public async Task<IActionResult> CreateOrUpdateMajorDtl(List<CollectionMajorDtl> lstCollectionMajorDtl)
        {
            try
            {
                var res = _iServices.fnCoUCollectionMajorDtlAsync(lstCollectionMajorDtl, string.Empty);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }

        [HttpPost("CreateOrUpdateMarks")]
        public async Task<IActionResult> CreateOrUpdateMarks(CollectionMarks collectionMarks)
        {
            try
            {
                var res = _iServices.fnCoUCollectionMarksAsync(collectionMarks, string.Empty);
                return Ok(res);
                return Ok("res");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }

        [HttpPost("CreateOrUpdateMarkDtl")]
        public async Task<IActionResult> CreateOrUpdateMarkDtl(List<CollectionMarkDtl> lstCollectionMarkDtl)
        {
            try
            {
                var res = _iServices.fnCoUCollectionMarkDtlAsync(lstCollectionMarkDtl, string.Empty);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }

        [HttpPost("CreateOrUpdateMarkDtl1")]
        public async Task<IActionResult> CreateOrUpdateMarkDtl1(List<CollectionMarkDtl1> lstCollectionMarkDtl1)
        {
            try
            {
                var res = _iServices.fnCoUCollectionMarkDtl1Async(lstCollectionMarkDtl1, string.Empty);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }

        [HttpPost("CreateOrUpdateRoom")]
        public async Task<IActionResult> CreateOrUpdateRoom(CollectionRoom collectionRoom)
        {
            try
            {
                var res = _iServices.fnCoUCollectionRoomAsync(collectionRoom, string.Empty);
                return Ok(res);
                return Ok("res");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }

        [HttpPost("CreateOrUpdateSchedule")]
        public async Task<IActionResult> CreateOrUpdateSchedule(CollectionSchedule collectionSchedule)
        {
            try
            {
                var res = _iServices.fnCoUCollectionScheduleAsync(collectionSchedule, string.Empty);
                return Ok(res);
                return Ok("res");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }

        [HttpPost("CreateOrUpdateScheduleDtl")]
        public async Task<IActionResult> CreateOrUpdateScheduleDtl(List<CollectionScheduleDtl> lstCollectionScheduleDtl)
        {
            try
            {
                var res = _iServices.fnCoUCollectionScheduleDtlAsync(lstCollectionScheduleDtl, string.Empty);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }

        [HttpPost("CreateOrUpdateCheckIO")]
        public async Task<IActionResult> CreateOrUpdateCheckIO(CollectionCheckIO collectionCheckIO)
        {
            try
            {
                var res = _iServices.fnCoUCollectionCheckIOAsync(collectionCheckIO, string.Empty);
                return Ok(res);
                return Ok("res");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }

        [HttpPost("CreateOrUpdateServiceMst")]
        public async Task<IActionResult> CreateOrUpdateServiceMst(CollectionServiceMst collectionServiceMst)
        {
            try
            {
                var res = _iServices.fnCoUCollectionServiceMstAsync(collectionServiceMst, string.Empty);
                return Ok(res);
                return Ok("res");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }
        [HasPermission("ASM")]
        [HttpPost("GetScheduleForUser")]
        public async Task<IActionResult> fnGetScheduleForUser(CollectionServiceMst collectionServiceMst)
        {
            try
            {
                var res = _iServices.fnCoUCollectionServiceMstAsync(collectionServiceMst, string.Empty);
                return Ok(res);
                return Ok("res");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }

    }
}
