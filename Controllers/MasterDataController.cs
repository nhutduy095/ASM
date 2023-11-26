using Application.Authorization;
using Application.Entities;
using Application.IService;
using Application.IService.Configuration;
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
        //private readonly IExecutionContextAccessor _executionContextAccessor;

        public MasterDataController(IServices iServices)
        {
            _iServices = iServices;
            //_executionContextAccessor = executionContextAccessor;
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
                var res =await _iServices.fnCoUCollectionCommonAsync(lstCollectionCommon, string.Empty);
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
                var res =await _iServices.fnCoUCollectionDepartmentAsync(collectionDepartment, string.Empty);
                return Ok(res);
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
                var res =await _iServices.fnCoUCollectionMajorAsync(collectionMajor, string.Empty);
                return Ok(res);
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
                var res =await _iServices.fnCoUCollectionMajorDtlAsync(lstCollectionMajorDtl, string.Empty);
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
                var res =await _iServices.fnCoUCollectionMarksAsync(collectionMarks, string.Empty);
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
                var res =await _iServices.fnCoUCollectionMarkDtlAsync(lstCollectionMarkDtl, string.Empty);
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
                var res =await _iServices.fnCoUCollectionMarkDtl1Async(lstCollectionMarkDtl1, string.Empty);
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
                var res =await _iServices.fnCoUCollectionRoomAsync(collectionRoom, string.Empty);
                return Ok(res);
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
                var res =await _iServices.fnCoUCollectionScheduleAsync(collectionSchedule, string.Empty);
                return Ok(res);
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
                var res =await _iServices.fnCoUCollectionScheduleDtlAsync(lstCollectionScheduleDtl, string.Empty);
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
                var res =await _iServices.fnCoUCollectionCheckIOAsync(collectionCheckIO, string.Empty);
                return Ok(res);
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
        #region servicemMst
        [HasPermission("ASM")]
        [HttpPost("CreateOrUpdateServiceMst")]
        public async Task<IActionResult> CreateOrUpdateServiceMst(CollectionServiceMst collectionServiceMst)
        {
            try
            {
                var res =await _iServices.fnCoUCollectionServiceMstAsync(collectionServiceMst, string.Empty);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }

        [HasPermission("ASM")]
        [HttpPost("GetCollectionServiceMst")]
        public async Task<IActionResult> fnGetCollectionServiceMst(RequestPaging request)
        {
            try
            {
                var result = await _iServices.fnGetCollectionServiceMstAsync(request); 				return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }
        [HasPermission("ASM")]
        [HttpGet("GetCollectionServiceMstById")]
        public async Task<IActionResult> fnGetCollectionServiceMstById(string ServiceId)
        {
            try
            {

                var result = await _iServices.fnGetCollectionServiceMstByIdAsync(ServiceId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }

        [HasPermission("ASM")]
        [HttpGet("GetCollServiceForComb")]
        public async Task<IActionResult> fnGetCollServiceForCombo()
        {
            try
            {
                var result = await _iServices.fnGetCollServiceForComboAsync(); 
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }
        #endregion		
        [HasPermission("ASM")]
        [HttpGet("GetScheduleForUser")]
        public async Task<IActionResult> fnGetScheduleForUser(string userId,int month,int year)
        {
            try
            {
                var res =await _iServices.fnGetScheduleForUserAsync(userId, month, year);
                return Ok(res);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }
        #region serviceReg
        [HasPermission("ASM")]
        [HttpPost("CreateOrUpdateServiceReg")]
        public async Task<IActionResult> CreateOrUpdateServiceReg(CollectionServiceReg collectionServiceReg)
        {
            try
            {
                var userId = HttpContext.User.Claims.First(x => x.Type == "userId").Value;
                var res =await _iServices.fnCoUCollectionServiceRegAsync(collectionServiceReg, userId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }
        [HasPermission("ASM")]
        [HttpPost("GetCollectionServiceReg")]
        public async Task<IActionResult> fnGetCollectionServiceReg(RequestPaging request)
        {
            try
            {
                var result = await _iServices.fnGetCollectionServiceRegAsync(request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }
        [HasPermission("ASM")]
        [HttpGet("GetCollectionServiceRegById")]
        public async Task<IActionResult> fnGetCollectionServiceRegById(string ServiceId)
        {
            try
            {

                var result = await _iServices.fnGetCollectionServiceRegByIdAsync(ServiceId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }
        #endregion
        #region subject
        [HasPermission("ASM")]
        [HttpPost("CreateOrUpdateSubject")]
        public async Task<IActionResult> CreateOrUpdateSubject(CollectionSubject collectionSubject)
        {
            try
            {
                var res =await _iServices.fnCoUCollectionSubjectAsync(collectionSubject, string.Empty);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }
        [HasPermission("ASM")]
        [HttpPost("GetCollectionSubject")]
        public async Task<IActionResult> fnGetCollectionSubject(RequestPaging request)
        {
            try
            {
                var result = await _iServices.fnGetCollectionSubjectAsync(request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }
        [HasPermission("ASM")]
        [HttpGet("GetCollectionSubject")]
        public async Task<IActionResult> fnGetCollectionSubjectById(string subjectId)
        {
            try
            {

                var result = await _iServices.fnGetCollectionSubjectByIdAsync(subjectId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }
        #endregion
        #region user
        [HasPermission("ASM")]
        [HttpPost("CreateOrUpdateUser")]
        public async Task<IActionResult> CreateOrUpdateUser(CollectionUser collectionUser)
        {
            try
            {
                var res =await  _iServices.fnCoUCollectionUserAsync(collectionUser, string.Empty);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }
        [HasPermission("ASM")]
        [HttpPost("GetCollectionUser")]
        public async Task<IActionResult> fnGetCollectionUser(RequestPaging request)
        {
            try
            {
                var result = await _iServices.fnGetCollectionUserAsync(request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }
        [HasPermission("ASM")]
        [HttpGet("GetCollectionUser")]
        public async Task<IActionResult> fnGetCollectionUserById(string userId)
        {
            try
            {

                var result = await _iServices.fnGetCollectionUserByIdAsync(userId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }
        #endregion
        #region userinfo
        [HasPermission("ASM")]
        [HttpPost("CreateOrUpdateUserInfo")]
        public async Task<IActionResult> CreateOrUpdateUserInfo(List<CollectionUserInfo> lstCollectionUserInfo)
        {
            try
            {
                var res =await _iServices.fnCoUCollectionUserInfoAsync(lstCollectionUserInfo, string.Empty);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }
        [HasPermission("ASM")]
        [HttpPost("GetCollectionUserInfo")]
        public async Task<IActionResult> fnGetCollectionUserInfo(RequestPaging request)
        {
            try
            {
                var result = await _iServices.fnGetCollectionUserInfoAsync(request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }
        [HasPermission("ASM")]
        [HttpGet("GetCollectionUserInfo")]
        public async Task<IActionResult> fnGetCollectionUserInfoById(string userId)
        {
            try
            {

                var result = await _iServices.fnGetCollectionUserInfoByIdAsync(userId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }
        #endregion
        [HasPermission("ASM")]
        [HttpPost("GetDataPointforUser")]
        public async Task<IActionResult> fnGetDataPointforUser(GetDataPointForReq res)
        {
            try
            {

                var result = await _iServices.fnGetDataPointforUserAsync(res);
                 return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest(new { message = "Error" });
        }
    }
}
