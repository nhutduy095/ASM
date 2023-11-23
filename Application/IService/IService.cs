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
    }
}
