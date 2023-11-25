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
        [HasPermission("ASM_T_S")]
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

        [HasPermission("ASM_T_S")]
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
        #region common
        [HasPermission("ASM_T_S")]
        [HttpPost("GetCollectionCommon")]
        public async Task<IActionResult> fnGetCollectionCommon(RequestPaging request)
        {
            try
            {
                var result = await _iServices.fnGetCollectionCommonAsync(request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }

        [HasPermission("ASM_T_S")]
        [HttpGet("GetCollectionCommonByCommonCd")]
        public async Task<IActionResult> fnGetCollectionCommonByCommonCd(string commonCd)
        {
            try
            {

                var result = await _iServices.fnGetCollectionCommonByCommonCdAsync(commonCd);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }

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
        #endregion
        #region department
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
        [HasPermission("ASM_T_S")]
        [HttpPost("GetCollectionDepartment")]
        public async Task<IActionResult> fnGetCollectionDepartment(RequestPaging request)
        {
            try
            {
                var result = await _iServices.fnGetCollectionDepartmentAsync(request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }
        [HasPermission("ASM_T_S")]
        [HttpGet("GetCollectionDepartmentByDeptId")]
        public async Task<IActionResult> fnGetCollectionDepartmentByDeptId(string deptId)
        {
            try
            {

                var result = await _iServices.fnGetCollectionDepartmentByDeptIdCdAsync(deptId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }
        #endregion
        #region major
        [HasPermission("ASM")]
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
        [HasPermission("ASM_T_S")]
        [HttpPost("GetCollectionMajor")]
        public async Task<IActionResult> fnGetCollectionMajor(RequestPaging request)
        {
            try
            {
                var result = await _iServices.fnGetCollectionMajorAsync(request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }
        [HasPermission("ASM_T_S")]
        [HttpGet("GetCollectionMajorById")]
        public async Task<IActionResult> fnGetCollectionMajorById(string majorId)
        {
            try
            {

                var result = await _iServices.fnGetCollectionMajorByIdAsync(majorId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }
        #endregion
        #region majordtl
        [HasPermission("ASM")]
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
        [HasPermission("ASM_T_S")]
        [HttpPost("GetCollectionMajorDtl")]
        public async Task<IActionResult> fnGetCollectionMajorDtl(RequestPaging request)
        {
            try
            {
                var result = await _iServices.fnGetCollectionMajorDtlAsync(request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }
        [HasPermission("ASM_T_S")]
        [HttpGet("GetCollectionMajorDtlById")]
        public async Task<IActionResult> fnGetCollectionMajorDtlById(string majorId)
        {
            try
            {

                var result = await _iServices.fnGetCollectionMajorDtlByIdAsync(majorId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }
        #endregion
        #region marks
        [HasPermission("ASM_T")]
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
        [HasPermission("ASM_T_S")]
        [HttpPost("GetCollectionMarks")]
        public async Task<IActionResult> fnGetCollectionMarks(RequestPaging request)
        {
            try
            {
                var result = await _iServices.fnGetCollectionMarksAsync(request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }
        [HasPermission("ASM_T_S")]
        [HttpGet("GetCollectionMarksById")]
        public async Task<IActionResult> fnGetCollectionMarksById(int markId)
        {
            try
            {

                var result = await _iServices.fnGetCollectionMarksByIdAsync(markId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }
        #endregion
        #region markDtl
        [HasPermission("ASM_T")]
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
        [HasPermission("ASM_T_S")]
        [HttpPost("GetCollectionMarkDtl")]
        public async Task<IActionResult> fnGetCollectionMarkDtl(RequestPaging request)
        {
            try
            {
                var result = await _iServices.fnGetCollectionMarkDtlAsync(request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }
        [HasPermission("ASM_T_S")]
        [HttpGet("GetCollectionMarkDtlById")]
        public async Task<IActionResult> fnGetCollectionMarkDtlById(int markId)
        {
            try
            {

                var result = await _iServices.fnGetCollectionMarkDtlByIdAsync(markId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }
        #endregion
        #region markDtl1
        [HasPermission("ASM_T")]
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
        [HasPermission("ASM_T_S")]
        [HttpPost("GetCollectionMarkDtl1")]
        public async Task<IActionResult> fnGetCollectionMarkDtl1(RequestPaging request)
        {
            try
            {
                var result = await _iServices.fnGetCollectionMarkDtl1Async(request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }
        [HasPermission("ASM_T_S")]
        [HttpGet("GetCollectionMarkDtl1ById")]
        public async Task<IActionResult> fnGetCollectionMarkDtl1ById(int markId)
        {
            try
            {

                var result = await _iServices.fnGetCollectionMarkDtl1ByIdAsync(markId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }
        #endregion
        #region room
        [HasPermission("ASM")]
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
        [HasPermission("ASM_T_S")]
        [HttpPost("GetCollectionRoom")]
        public async Task<IActionResult> fnGetCollectionRoom(RequestPaging request)
        {
            try
            {
                var result = await _iServices.fnGetCollectionRoomAsync(request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }
        [HasPermission("ASM_T_S")]
        [HttpGet("GetCollectionRoomById")]
        public async Task<IActionResult> fnGetCollectionRoomById(string roomId)
        {
            try
            {

                var result = await _iServices.fnGetCollectionRoomByIdAsync(roomId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }
        #endregion
        #region schedule
        [HasPermission("ASM")]
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
        [HasPermission("ASM_T_S")]
        [HttpPost("GetCollectionSchedule")]
        public async Task<IActionResult> fnGetCollectionSchedule(RequestPaging request)
        {
            try
            {
                var result = await _iServices.fnGetCollectionScheduleAsync(request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }
        [HasPermission("ASM_T_S")]
        [HttpGet("GetCollectionScheduleById")]
        public async Task<IActionResult> fnGetCollectionScheduleById(int schedule)
        {
            try
            {

                var result = await _iServices.fnGetCollectionScheduleByIdAsync(schedule);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }
        #endregion
        #region scheduleDtl
        [HasPermission("ASM")]
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
        [HasPermission("ASM_T_S")]
        [HttpPost("GetCollectionScheduleDtl")]
        public async Task<IActionResult> fnGetCollectionScheduleDtl(RequestPaging request)
        {
            try
            {
                var result = await _iServices.fnGetCollectionScheduleDtlAsync(request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }
        [HasPermission("ASM_T_S")]
        [HttpGet("GetCollectionScheduleDtlById")]
        public async Task<IActionResult> fnGetCollectionScheduleDtlById(int dtlId)
        {
            try
            {

                var result = await _iServices.fnGetCollectionScheduleDtlByIdAsync(dtlId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }
        #endregion
        #region checkIO
        [HasPermission("ASM")]
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
        [HasPermission("ASM")]
        [HttpPost("GetCollectionCheckIO")]
        public async Task<IActionResult> fnGetCollectionCheckIO(RequestPaging request)
        {
            try
            {
                var result = await _iServices.fnGetCollectionCheckIOAsync(request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }
        [HasPermission("ASM")]
        [HttpGet("GetCollectionCheckIOById")]
        public async Task<IActionResult> fnGetCollectionCheckIOById(string userId)
        {
            try
            {

                var result = await _iServices.fnGetCollectionCheckIOByIdAsync(userId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }
        #endregion
        #region
        [HasPermission("ASM")]
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

        #endregion
    }
}
