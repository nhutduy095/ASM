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
        Task<ResponeModel> GetAsync();
        Task<ResponeModel> Login(LoginRequest reqData);
    }
}
