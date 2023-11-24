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
        Task<ResponeModel> fnGetCollectionClassAsync();
        //Task<ResponeModel> fnGetCollectionCommonAsync();
        Task<ResponeModel> Login(LoginRequest reqData);
        Task<ResponeModel> fnCoUCollectionClassAsync(List<CollectionClass> lstClass, string userId);
        Task<ResponeModel> fnCoUCollectionCommonAsync(List<CollectionCommon> lstCommon, string userId);
        Task<ResponeModel> fnCoUCollectionDepartmentAsync(CollectionDepartment department, string userId);
        Task<ResponeModel> fnCoUCollectionMajorAsync(CollectionMajor major, string userId);
        Task<ResponeModel> fnCoUCollectionMajorDtlAsync(List<CollectionMajorDtl> lstMajorDtl, string userId);
        Task<ResponeModel> fnCoUCollectionMarksAsync(CollectionMarks marks, string userId);
        Task<ResponeModel> fnCoUCollectionMarkDtlAsync(List<CollectionMarkDtl> lstMarkDtl, string userId);
        Task<ResponeModel> fnCoUCollectionMarkDtl1Async(List<CollectionMarkDtl1> lstMarkDtl1, string userId);
        Task<ResponeModel> fnCoUCollectionRoomAsync(CollectionRoom room, string userId);
        Task<ResponeModel> fnCoUCollectionScheduleAsync(CollectionSchedule schedule, string userId);
        Task<ResponeModel> fnCoUCollectionScheduleDtlAsync(List<CollectionScheduleDtl> lstScheduleDtl, string userId);
        Task<ResponeModel> fnCoUCollectionCheckIOAsync(CollectionCheckIO checkIO, string userId);


    }
}
