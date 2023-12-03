using Application.Entities;
using Application.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.IService
{
    public interface IServices
    {
        Task<ResponseModel> fnGetCollectionClassAsync(RequestPaging request);
        Task<ResponseModel> fnGetCollectionClassByIDAsync(string classId);
        Task<ResponseModel> fnGetCollectionCommonAsync(RequestPaging request);
        Task<ResponseModel> fnGetCollectionCommonByCommonCdAsync(string commonCd);
        Task<ResponseModel> fnGetCollectionDepartmentAsync(RequestPaging request);
        Task<ResponseModel> fnGetCollectionDepartmentByDeptIdCdAsync(string deptId);
        Task<ResponseModel> fnGetCollectionMajorAsync(RequestPaging request);
        Task<ResponseModel> fnGetCollectionMajorByIdAsync(string majorId);
        Task<ResponseModel> fnGetCollectionMajorDtlAsync(RequestPaging request);
        Task<ResponseModel> fnGetCollectionMajorDtlByIdAsync(string majorId);
        Task<ResponseModel> fnGetCollectionMarkDtl1Async(RequestPaging request);
        Task<ResponseModel> fnGetCollectionMarkDtl1ByIdAsync(int markId);
        Task<ResponseModel> fnGetCollectionMarkDtlAsync(RequestPaging request);
        Task<ResponseModel> fnGetCollectionMarkDtlByIdAsync(int markId);
        Task<ResponseModel> fnGetCollectionMarksAsync(RequestPaging request);
        Task<ResponseModel> fnGetCollectionMarksByIdAsync(int markId);
        Task<ResponseModel> fnGetCollectionRoomAsync(RequestPaging request);
        Task<ResponseModel> fnGetCollectionRoomByIdAsync(string roomId);
        Task<ResponseModel> fnGetCollectionScheduleAsync(RequestPaging request);
        Task<ResponseModel> fnGetCollectionScheduleByIdAsync(int scheduleId);
        Task<ResponseModel> fnGetCollectionScheduleDtlAsync(RequestPaging request);
        Task<ResponseModel> fnGetCollectionScheduleDtlByIdAsync(int dtlId);
        Task<ResponseModel> fnGetCollectionServiceMstAsync(RequestPaging request);
        Task<ResponseModel> fnGetCollServiceForComboAsync();
        Task<ResponseModel> fnGetCollectionServiceMstByIdAsync(string serviceId);
        Task<ResponseModel> fnGetCollectionServiceRegAsync(RequestPaging request);
        Task<ResponseModel> fnGetCollectionServiceRegByIdAsync(string serviceId);
        Task<ResponseModel> fnGetCollectionSubjectAsync(RequestPaging request);
        Task<ResponseModel> fnGetCollectionSubjectByIdAsync(string subjectId);
        Task<ResponseModel> fnGetCollectionUserAsync(RequestPaging request);
        Task<ResponseModel> fnGetCollectionUserByIdAsync(string userId);
        Task<ResponseModel> fnGetCollectionUserInfoAsync(RequestPaging request);
        Task<ResponseModel> fnGetCollectionUserInfoByIdAsync(string userId);
        Task<ResponseModel> fnGetCollectionCheckIOAsync(RequestPaging request);
        Task<ResponseModel> fnGetCollectionCheckIOByIdAsync(string userId);
        Task<ResponseModel> Login(LoginRequest reqData);
        Task<ResponseModel> fnCoUCollectionClassAsync(List<CollectionClass> lstClass, string userId);
        Task<ResponseModel> fnCoUCollectionCommonAsync(List<CollectionCommon> lstCommon, string userId);
        Task<ResponseModel> fnCoUCollectionDepartmentAsync(CollectionDepartment department, string userId);
        Task<ResponseModel> fnCoUCollectionMajorAsync(CollectionMajor major, string userId);
        Task<ResponseModel> fnCoUCollectionMajorDtlAsync(List<CollectionMajorDtl> lstMajorDtl, string userId);
        Task<ResponseModel> fnCoUCollectionMarksAsync(CollectionMarks marks, string userId);
        Task<ResponseModel> fnCoUCollectionMarkDtlAsync(List<CollectionMarkDtl> lstMarkDtl, string userId);
        Task<ResponseModel> fnCoUCollectionMarkDtl1Async(List<CollectionMarkDtl1> lstMarkDtl1, string userId);
        Task<ResponseModel> fnCoUCollectionRoomAsync(CollectionRoom room, string userId);
        Task<ResponseModel> fnCoUCollectionScheduleAsync(CollectionSchedule schedule, string userId);
        Task<ResponseModel> fnCoUCollectionScheduleDtlAsync(List<CollectionScheduleDtl> lstScheduleDtl, string userId);
        Task<ResponseModel> fnGetScheduleForUserAsync(string userId, int month, int year, string shift);
        Task<ResponseModel> fnCoUCollectionCheckIOAsync(CollectionCheckIO checkIO, string userId);
        Task<ResponseModel> fnCoUCollectionServiceMstAsync(CollectionServiceMst serviceMst, string userId);
        Task<ResponseModel> fnCoUCollectionServiceRegAsync(CollectionServiceReg serviceReg, string userId);
        Task<ResponseModel> fnCoUCollectionSubjectAsync(CollectionSubject subject, string userId);
        Task<ResponseModel> fnCoUCollectionUserAsync(CollectionUser user, string userId);
        Task<ResponseModel> fnCoUCollectionUserInfoAsync(CollectionUserInfo userInfo, string userId);
        Task<ResponseModel> fnGetDataPointforUserAsync(GetDataPointForReq request);
        Task<ResponseModel> fnGetCollSubjectForComboAsync();
        Task<ResponseModel> fnGetCollMajorComboAsync();

    }
}
