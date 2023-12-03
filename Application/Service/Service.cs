using Application.Base;
using Application.Entities;
using Application.IService;
using Application.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service
{
    public class Services : IServices
    {
        private IMongoCollection<CollectionClass> _collClass;
        private IMongoCollection<CollectionUserInfo> _collUserInfo;
        private IMongoCollection<CollectionUser> _collUser;
        private IMongoCollection<CollectionCommon> _collCommon;
        private IMongoCollection<CollectionDepartment> _collDept;
        private IMongoCollection<CollectionMajor> _collMajor;
        private IMongoCollection<CollectionMajorDtl> _collMajorDtl;
        private IMongoCollection<CollectionMarks> _collMarks;
        private IMongoCollection<CollectionMarkDtl> _collMarkDtl;
        private IMongoCollection<CollectionMarkDtl1> _collMarkDtl1;
        private IMongoCollection<CollectionRoom> _collRoom;
        private IMongoCollection<CollectionSchedule> _collSchedule;
        private IMongoCollection<CollectionScheduleDtl> _collScheduleDtl;
        private IMongoCollection<CollectionCheckIO> _collCheckIO;
        private IMongoCollection<CollectionServiceMst> _collServiceMst;
        private IMongoCollection<CollectionServiceReg> _collServiceReg;
        private IMongoCollection<CollectionSubject> _collSubject;

        private static MongoClient client;
        public IConfiguration _configuration;
        private string ConnectionURI;
        public Services(IOptions<MongoDBSettings> mongoDBSettings, IConfiguration configuration)
        {
            ConnectionURI = mongoDBSettings.Value.ConnectionURI;
            client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _collClass = database.GetCollection<CollectionClass>(mongoDBSettings.Value.CollectionName);
            _collUserInfo = database.GetCollection<CollectionUserInfo>("tblIUser_Info");
            _collUser = database.GetCollection<CollectionUser>("tblUsers");
            _collCommon = database.GetCollection<CollectionCommon>("tblCommon");
            _collDept = database.GetCollection<CollectionDepartment>("tblDepartment");
            _collMajor = database.GetCollection<CollectionMajor>("tblMajor");
            _collMajorDtl = database.GetCollection<CollectionMajorDtl>("tblMajor_Dtl");
            _collMarks = database.GetCollection<CollectionMarks>("tblMarks");
            _collMarkDtl = database.GetCollection<CollectionMarkDtl>("tblMark_Dtl");
            //_collMarkDtl1 = database.GetCollection<CollectionMarkDtl1>("tblMark_Dtl1");
            _collRoom = database.GetCollection<CollectionRoom>("tblRoom");
            _collSchedule = database.GetCollection<CollectionSchedule>("tblSchedule");
            _collScheduleDtl = database.GetCollection<CollectionScheduleDtl>("tblSchedule_Dtl");
            //_collCheckIO = database.GetCollection<CollectionCheckIO>("tblCheckIO");
            _collServiceMst = database.GetCollection<CollectionServiceMst>("tblService_Mst");
            _collServiceReg = database.GetCollection<CollectionServiceReg>("tblService_Reg");
            _collSubject = database.GetCollection<CollectionSubject>("tblSubject");

            _configuration = configuration;
        }
        #region Auth
        public async Task<ResponseModel> Login(LoginRequest reqData)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                //FilterDefinition<Playlist> filter = Builders<Playlist>.Filter.Eq("Id", id);
                //UpdateDefinition<Playlist> update = Builders<Playlist>.Update.AddToSet<string>("movieIds", movieId);
                //await _playlistCollection.UpdateOneAsync(filter, update);
                var dataUser = await _collUser.Find(new BsonDocument()).ToListAsync();

                var userinfor = dataUser.FirstOrDefault(x => x.UserId == reqData.username);
                if (userinfor == null)
                {
                    return new ResponseModel(ErrorMessage.Error0002, "User không tồn tại");

                }
                if (userinfor.Password != reqData.password)
                {
                    return new ResponseModel(ErrorMessage.Error0003, "Sai mật khẩu");
                }
                var dataUserInfo = await _collUserInfo.Find(new BsonDocument()).ToListAsync();
                var userInfo = dataUserInfo.FirstOrDefault(x => x.UserId == reqData.username);
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("userId", userInfo.UserId),
                    new Claim("userType", userInfo.UserType),
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"], claims,
                    expires: DateTime.UtcNow.AddMinutes(260),
                    signingCredentials: signIn);
                //res.ErrMessage = new JwtSecurityTokenHandler().WriteToken(token);
                var loginRes = new LoginRespone();
                loginRes.Token = new JwtSecurityTokenHandler().WriteToken(token);
                loginRes.UserInfo = userInfo;
                res.Data = loginRes;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel(ErrorMessage.Error0001, ex.Message);
            }
            return res;
        }
        #endregion
        #region class
        public async Task<ResponseModel> fnCoUCollectionClassAsync(List<CollectionClass> lstClass, string userId) {
            ResponseModel res = new ResponseModel();
            string dt = CommonBase.fnGertDateTimeNow();
            client = new MongoClient(ConnectionURI);
            //using (var session = await client.StartSessionAsync())
            //{
            //    //Begin transaction
            //    session.StartTransaction();
                try
                {
                    foreach (var itm in lstClass)
                    {
                        var fillter = Builders<CollectionClass>.Filter.Eq("ClassId", itm.ClassId);

                        var dataColClass = await _collClass.Find(new BsonDocument()).ToListAsync();

                        var classInfo = dataColClass.FirstOrDefault(x => x.ClassId == itm.ClassId);
                        if (classInfo == null)
                        {
                            itm.CreateBy = userId;
                            itm.CreateDate = dt;
                            await _collClass.InsertOneAsync(itm);

                        }
                        else
                        {
                            itm.UpdateBy = userId;
                            itm.UpdateDate = dt;
                            await _collClass.ReplaceOneAsync(x => x.ClassId == itm.ClassId, itm, new ReplaceOptions { IsUpsert = true });//update
                        }

                    }
                    // Made it here without error? Let's commit the transaction
                    //await session.CommitTransactionAsync();

                }
                catch (System.Exception ex)
                {
                    //rollback
                    //await session.AbortTransactionAsync();
                    return new ResponseModel("EX001", ex.Message);
                }
           // }
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionClassAsync(RequestPaging request)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var data = await _collClass.Find(new BsonDocument())
                    .SortBy(x => x.ClassId)
                    .Skip((request.Page - 1) * request.PerPage)
                    .Limit(request.PerPage)
                    .ToListAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionClassByIDAsync(string classId)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var fillter = Builders<CollectionClass>.Filter.Eq(x => x.ClassId, classId);
                var data = await _collClass.Aggregate()
                   .Match(fillter).FirstOrDefaultAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        #endregion
        #region common
        public async Task<ResponseModel> fnCoUCollectionCommonAsync(List<CollectionCommon> lstCommon, string userId)
        {
            ResponseModel res = new ResponseModel();
            string dt = CommonBase.fnGertDateTimeNow();
            client = new MongoClient(ConnectionURI);
            //using (var session = await client.StartSessionAsync())
            //{
            //    //Begin transaction
            //    session.StartTransaction();
                try
                {
                    foreach (var itm in lstCommon)
                    {
                        var fillter = Builders<CollectionCommon>.Filter.Eq("CommonCd", itm.CommonCd);

                        var dataColCommon = await _collCommon.Find(new BsonDocument()).ToListAsync();

                        var commonInfo = dataColCommon.FirstOrDefault(x => x.CommonCd == itm.CommonCd);
                        if (commonInfo == null)
                        {
                            itm.CreateBy = userId;
                            itm.CreateDate = dt;
                            await _collCommon.InsertOneAsync(itm);

                        }
                        else
                        {
                            itm.UpdateBy = userId;
                            itm.UpdateDate = dt;
                            await _collCommon.ReplaceOneAsync(x => x.CommonCd == itm.CommonCd, itm, new ReplaceOptions { IsUpsert = true });//update
                        }

                    }
                    // Made it here without error? Let's commit the transaction
                    //await session.CommitTransactionAsync();

                }
                catch (System.Exception ex)
                {
                    //rollback
                    //await session.AbortTransactionAsync();
                    return new ResponseModel("EX001", ex.Message);
                }
            //}
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionCommonAsync(RequestPaging request)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var data = await _collCommon.Find(new BsonDocument())
                    .SortBy(x => x.CommonCd)
                    .Skip((request.Page - 1) * request.PerPage)
                    .Limit(request.PerPage)
                    .ToListAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionCommonByCommonCdAsync(string commonCd)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var fillter = Builders<CollectionCommon>.Filter.Eq(x => x.CommonCd, commonCd);
                var data = await _collCommon.Aggregate()
                   .Match(fillter).FirstOrDefaultAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        #endregion
        #region department
        public async Task<ResponseModel> fnCoUCollectionDepartmentAsync(CollectionDepartment department, string userId)
        {
            ResponseModel res = new ResponseModel();
            string dt = CommonBase.fnGertDateTimeNow();
            client = new MongoClient(ConnectionURI);
            //using (var session = await client.StartSessionAsync())
            //{
            //    //Begin transaction
            //    session.StartTransaction();
                try
                {

                    var fillter = Builders<CollectionDepartment>.Filter.Eq("DeptId", department.DeptId);

                    var dataColDept = await _collDept.Find(new BsonDocument()).ToListAsync();

                    var deptInfo = dataColDept.FirstOrDefault(x => x.DeptId == department.DeptId);
                    if (deptInfo == null)
                    {
                        department.CreateBy = userId;
                        department.CreateDate = dt;
                        await _collDept.InsertOneAsync(department);

                    }
                    else
                    {
                        department.UpdateBy = userId;
                        department.UpdateDate = dt;
                        await _collDept.ReplaceOneAsync(x => x.DeptId == department.DeptId, department, new ReplaceOptions { IsUpsert = true });//update
                    }


                    // Made it here without error? Let's commit the transaction
                    //await session.CommitTransactionAsync();

                }
                catch (System.Exception ex)
                {
                    //rollback
                    //await session.AbortTransactionAsync();
                    return new ResponseModel("EX001", ex.Message);
                }
            //}
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionDepartmentAsync(RequestPaging request)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var data = await _collDept.Find(new BsonDocument())
                    .SortBy(x => x.DeptId)
                    .Skip((request.Page - 1) * request.PerPage)
                    .Limit(request.PerPage)
                    .ToListAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionDepartmentByDeptIdCdAsync(string deptId)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var fillter = Builders<CollectionDepartment>.Filter.Eq(x => x.DeptId, deptId);
                var data = await _collDept.Aggregate()
                   .Match(fillter).FirstOrDefaultAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        #endregion
        #region major
        public async Task<ResponseModel> fnCoUCollectionMajorAsync(CollectionMajor major, string userId)
        {
            ResponseModel res = new ResponseModel();
            string dt = CommonBase.fnGertDateTimeNow();
            client = new MongoClient(ConnectionURI);
            //using (var session = await client.StartSessionAsync())
            //{
            //    //Begin transaction
            //    session.StartTransaction();
                try
                {

                    var fillter = Builders<CollectionMajor>.Filter.Eq("MajorID", major.MajorID);

                    var dataColMajor = await _collMajor.Find(new BsonDocument()).ToListAsync();

                    var majorInfo = dataColMajor.FirstOrDefault(x => x.MajorID == major.MajorID);
                    if (majorInfo == null)
                    {
                        major.CreateBy = userId;
                        major.CreateDate = dt;
                        await _collMajor.InsertOneAsync(major);

                    }
                    else
                    {
                        major.UpdateBy = userId;
                        major.UpdateDate = dt;
                        await _collMajor.ReplaceOneAsync(x => x.MajorID == major.MajorID, major, new ReplaceOptions { IsUpsert = true });//update
                    }


                    // Made it here without error? Let's commit the transaction
                    //await session.CommitTransactionAsync();

                }
                catch (System.Exception ex)
                {
                    //rollback
                    //await session.AbortTransactionAsync();
                    return new ResponseModel("EX001", ex.Message);
                }
            //}
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionMajorAsync(RequestPaging request)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var data = await _collMajor.Find(new BsonDocument())
                    .SortBy(x => x.MajorID)
                    .Skip((request.Page - 1) * request.PerPage)
                    .Limit(request.PerPage)
                    .ToListAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionMajorByIdAsync(string majorId)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var fillter = Builders<CollectionMajor>.Filter.Eq(x => x.MajorID, majorId);
                var data = await _collMajor.Aggregate()
                   .Match(fillter).FirstOrDefaultAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        public async Task<ResponseModel> fnGetCollMajorComboAsync()
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var data = await _collMajor.Find(new BsonDocument())
                    .SortBy(x => x.MajorID)
                    .ToListAsync();
                res.Data = data.Where(x => x.IsActive).Select(x => new {
                    MajorID = x.MajorID,
                    MajorName = x.MajorName
                }).ToList();
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        #endregion
        #region majorDtl
        public async Task<ResponseModel> fnCoUCollectionMajorDtlAsync(List<CollectionMajorDtl> lstMajorDtl, string userId)
        {
            ResponseModel res = new ResponseModel();
            string dt = CommonBase.fnGertDateTimeNow();
            client = new MongoClient(ConnectionURI);
            //using (var session = await client.StartSessionAsync())
            //{
            //    //Begin transaction
            //    session.StartTransaction();
                try
                {
                    foreach (var itm in lstMajorDtl)
                    {
                        var fillter = Builders<CollectionMajorDtl>.Filter.Eq("MajorID", itm.MajorID);

                        var dataColMajorDtl = await _collMajorDtl.Find(new BsonDocument()).ToListAsync();

                        var majorDtlInfo = dataColMajorDtl.FirstOrDefault(x => x.MajorID == itm.MajorID);
                        if (majorDtlInfo == null)
                        {
                            itm.CreateBy = userId;
                            itm.CreateDate = dt;
                            await _collMajorDtl.InsertOneAsync(itm);

                        }
                        else
                        {
                            itm.UpdateBy = userId;
                            itm.UpdateDate = dt;
                            await _collMajorDtl.ReplaceOneAsync(x => x.MajorID == itm.MajorID, itm, new ReplaceOptions { IsUpsert = true });//update
                        }

                    }
                    // Made it here without error? Let's commit the transaction
                    //await session.CommitTransactionAsync();

                }
                catch (System.Exception ex)
                {
                    //rollback
                    //await session.AbortTransactionAsync();
                    return new ResponseModel("EX001", ex.Message);
                }
            //}
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionMajorDtlAsync(RequestPaging request)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var data = await _collMajorDtl.Find(new BsonDocument())
                    .SortBy(x => x.MajorID)
                    .Skip((request.Page - 1) * request.PerPage)
                    .Limit(request.PerPage)
                    .ToListAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionMajorDtlByIdAsync(string majorId)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var fillter = Builders<CollectionMajorDtl>.Filter.Eq(x => x.MajorID, majorId);
                var data = await _collMajorDtl.Aggregate()
                   .Match(fillter).FirstOrDefaultAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        #endregion
        #region marks
        public async Task<ResponseModel> fnCoUCollectionMarksAsync(CollectionMarks marks, string userId)
        {
            ResponseModel res = new ResponseModel();
            string dt = CommonBase.fnGertDateTimeNow();
            client = new MongoClient(ConnectionURI);
            //using (var session = await client.StartSessionAsync())
            //{
            //    //Begin transaction
            //    session.StartTransaction();
                try
                {

                    var fillter = Builders<CollectionMarks>.Filter.Eq("MarkId", marks.MarkId);

                    var dataColMarks = await _collMarks.Find(new BsonDocument()).ToListAsync();

                    var marksInfo = dataColMarks.FirstOrDefault(x => x.MarkId == marks.MarkId);
                    if (marksInfo == null)
                    {
                        marks.CreateBy = userId;
                        marks.CreateDate = dt;
                        await _collMarks.InsertOneAsync(marks);

                    }
                    else
                    {
                        marks.UpdateBy = userId;
                        marks.UpdateDate = dt;
                        await _collMarks.ReplaceOneAsync(x => x.MarkId == marks.MarkId, marks, new ReplaceOptions { IsUpsert = true });//update
                    }


                    // Made it here without error? Let's commit the transaction
                    //await session.CommitTransactionAsync();

                }
                catch (System.Exception ex)
                {
                    //rollback
                    //await session.AbortTransactionAsync();
                    return new ResponseModel("EX001", ex.Message);
                }
            //}
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionMarksAsync(RequestPaging request)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var data = await _collMarks.Find(new BsonDocument())
                    .SortBy(x => x.MarkId)
                    .Skip((request.Page - 1) * request.PerPage)
                    .Limit(request.PerPage)
                    .ToListAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionMarksByIdAsync(int markId)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var fillter = Builders<CollectionMarks>.Filter.Eq(x => x.MarkId, markId);
                var data = await _collMarks.Aggregate()
                   .Match(fillter).FirstOrDefaultAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        #endregion
        #region markdtl
        public async Task<ResponseModel> fnCoUCollectionMarkDtlAsync(List<CollectionMarkDtl> lstMarkDtl, string userId)
        {
            ResponseModel res = new ResponseModel();
            string dt = CommonBase.fnGertDateTimeNow();
            client = new MongoClient(ConnectionURI);
            //using (var session = await client.StartSessionAsync())
            //{
            //    //Begin transaction
            //    session.StartTransaction();
                try
                {
                    foreach (var itm in lstMarkDtl)
                    {
                        var fillter = Builders<CollectionMarkDtl>.Filter.Eq("MarkDtlId", itm.MarkDtlId);

                        var dataColMarkDtl = await _collMarkDtl.Find(new BsonDocument()).ToListAsync();

                        var markDtlInfo = dataColMarkDtl.FirstOrDefault(x => x.MarkDtlId == itm.MarkDtlId);
                        if (markDtlInfo == null)
                        {
                            itm.CreateBy = userId;
                            itm.CreateDate = dt;
                            await _collMarkDtl.InsertOneAsync(itm);

                        }
                        else
                        {
                            itm.UpdateBy = userId;
                            itm.UpdateDate = dt;
                            await _collMarkDtl.ReplaceOneAsync(x => x.MarkDtlId == itm.MarkDtlId, itm, new ReplaceOptions { IsUpsert = true });//update
                        }

                    }
                    // Made it here without error? Let's commit the transaction
                    //await session.CommitTransactionAsync();

                }
                catch (System.Exception ex)
                {
                    //rollback
                    //await session.AbortTransactionAsync();
                    return new ResponseModel("EX001", ex.Message);
                }
            //}
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionMarkDtlAsync(RequestPaging request)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var data = await _collMarkDtl.Find(new BsonDocument())
                    .SortBy(x => x.MarkId)
                    .Skip((request.Page - 1) * request.PerPage)
                    .Limit(request.PerPage)
                    .ToListAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionMarkDtlByIdAsync(int markId)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var fillter = Builders<CollectionMarkDtl>.Filter.Eq(x => x.MarkId, markId);
                var data = await _collMarkDtl.Aggregate()
                   .Match(fillter).FirstOrDefaultAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        #endregion
        #region markdtl1
        public async Task<ResponseModel> fnCoUCollectionMarkDtl1Async(List<CollectionMarkDtl1> lstMarkDtl1, string userId)
        {
            ResponseModel res = new ResponseModel();
            string dt = CommonBase.fnGertDateTimeNow();
            client = new MongoClient(ConnectionURI);
            //using (var session = await client.StartSessionAsync())
            //{
            //    //Begin transaction
            //    session.StartTransaction();
                try
                {
                    foreach (var itm in lstMarkDtl1)
                    {
                        var fillter = Builders<CollectionMarkDtl>.Filter.Eq("MarkDtlId", itm.MarkDtlId);

                        var dataColMarkDtl1 = await _collMarkDtl1.Find(new BsonDocument()).ToListAsync();

                        var markDtl1Info = dataColMarkDtl1.FirstOrDefault(x => x.MarkDtlId == itm.MarkDtlId);
                        if (markDtl1Info == null)
                        {
                            itm.CreateBy = userId;
                            itm.CreateDate = dt;
                            await _collMarkDtl1.InsertOneAsync(itm);

                        }
                        else
                        {
                            itm.UpdateBy = userId;
                            itm.UpdateDate = dt;
                            await _collMarkDtl1.ReplaceOneAsync(x => x.MarkDtlId == itm.MarkDtlId, itm, new ReplaceOptions { IsUpsert = true });//update
                        }

                    }
                    // Made it here without error? Let's commit the transaction
                    //await session.CommitTransactionAsync();

                }
                catch (System.Exception ex)
                {
                    //rollback
                    //await session.AbortTransactionAsync();
                    return new ResponseModel("EX001", ex.Message);
                }
            //}
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionMarkDtl1Async(RequestPaging request)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var data = await _collMarkDtl1.Find(new BsonDocument())
                    .SortBy(x => x.MarkId)
                    .Skip((request.Page - 1) * request.PerPage)
                    .Limit(request.PerPage)
                    .ToListAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionMarkDtl1ByIdAsync(int markId)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var fillter = Builders<CollectionMarkDtl1>.Filter.Eq(x => x.MarkId, markId);
                var data = await _collMarkDtl1.Aggregate()
                   .Match(fillter).FirstOrDefaultAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        #endregion
        #region room
        public async Task<ResponseModel> fnCoUCollectionRoomAsync(CollectionRoom room, string userId)
        {
            ResponseModel res = new ResponseModel();
            string dt = CommonBase.fnGertDateTimeNow();
            client = new MongoClient(ConnectionURI);
            //using (var session = await client.StartSessionAsync())
            //{
            //    //Begin transaction
            //    session.StartTransaction();
                try
                {

                    var fillter = Builders<CollectionRoom>.Filter.Eq("RoomId", room.RoomId);

                    var dataColRoom = await _collRoom.Find(new BsonDocument()).ToListAsync();

                    var roomInfo = dataColRoom.FirstOrDefault(x => x.RoomId == room.RoomId);
                    if (roomInfo == null)
                    {
                        room.CreateBy = userId;
                        room.CreateDate = dt;
                        await _collRoom.InsertOneAsync(room);

                    }
                    else
                    {
                        room.UpdateBy = userId;
                        room.UpdateDate = dt;
                        await _collRoom.ReplaceOneAsync(x => x.RoomId == room.RoomId, room, new ReplaceOptions { IsUpsert = true });//update
                    }


                    // Made it here without error? Let's commit the transaction
                    //await session.CommitTransactionAsync();

                }
                catch (System.Exception ex)
                {
                    //rollback
                    //await session.AbortTransactionAsync();
                    return new ResponseModel("EX001", ex.Message);
                }
            //}
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionRoomAsync(RequestPaging request)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var data = await _collRoom.Find(new BsonDocument())
                    .SortBy(x => x.RoomId)
                    .Skip((request.Page - 1) * request.PerPage)
                    .Limit(request.PerPage)
                    .ToListAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionRoomByIdAsync(string roomId)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var fillter = Builders<CollectionRoom>.Filter.Eq(x => x.RoomId, roomId);
                var data = await _collRoom.Aggregate()
                   .Match(fillter).FirstOrDefaultAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        #endregion
        #region schedule
        public async Task<ResponseModel> fnCoUCollectionScheduleAsync(CollectionSchedule schedule, string userId)
        {
            ResponseModel res = new ResponseModel();
            string dt = CommonBase.fnGertDateTimeNow();
            client = new MongoClient(ConnectionURI);
            //using (var session = await client.StartSessionAsync())
            //{
            //    //Begin transaction
            //    session.StartTransaction();
                try
                {

                    var fillter = Builders<CollectionSchedule>.Filter.Eq("ScheduleId", schedule.ScheduleId);

                    var dataColSchedule = await _collSchedule.Find(new BsonDocument()).ToListAsync();

                    var scheduleInfo = dataColSchedule.FirstOrDefault(x => x.ScheduleId == schedule.ScheduleId);
                    if (scheduleInfo == null)
                    {
                        schedule.CreateBy = userId;
                        schedule.CreateDate = dt;
                        await _collSchedule.InsertOneAsync(schedule);

                    }
                    else
                    {
                        schedule.UpdateBy = userId;
                        schedule.UpdateDate = dt;
                        await _collSchedule.ReplaceOneAsync(x => x.ScheduleId == schedule.ScheduleId, schedule, new ReplaceOptions { IsUpsert = true });//update
                    }


                    // Made it here without error? Let's commit the transaction
                    //await session.CommitTransactionAsync();

                }
                catch (System.Exception ex)
                {
                    //rollback
                    //await session.AbortTransactionAsync();
                    return new ResponseModel("EX001", ex.Message);
                }
            //}
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionScheduleAsync(RequestPaging request)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var data = await _collSchedule.Find(new BsonDocument())
                    .SortBy(x => x.ScheduleId)
                    .Skip((request.Page - 1) * request.PerPage)
                    .Limit(request.PerPage)
                    .ToListAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionScheduleByIdAsync(int scheduleId)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var fillter = Builders<CollectionSchedule>.Filter.Eq(x => x.ScheduleId, scheduleId);
                var data = await _collSchedule.Aggregate()
                   .Match(fillter).FirstOrDefaultAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        #endregion
        #region scheduledtl
        public async Task<ResponseModel> fnCoUCollectionScheduleDtlAsync(List<CollectionScheduleDtl> lstScheduleDtl, string userId)
        {
            ResponseModel res = new ResponseModel();
            string dt = CommonBase.fnGertDateTimeNow();
            client = new MongoClient(ConnectionURI);
            //using (var session = await client.StartSessionAsync())
            //{
            //    //Begin transaction
            //    session.StartTransaction();
                try
                {
                    foreach (var itm in lstScheduleDtl)
                    {
                        var fillter = Builders<CollectionScheduleDtl>.Filter.Eq("DtlId", itm.DtlId);

                        var dataColScheduleDtl = await _collScheduleDtl.Find(new BsonDocument()).ToListAsync();

                        var scheduleDtlInfo = dataColScheduleDtl.FirstOrDefault(x => x.DtlId == itm.DtlId);
                        if (scheduleDtlInfo == null)
                        {
                            itm.CreateBy = userId;
                            itm.CreateDate = dt;
                            await _collScheduleDtl.InsertOneAsync(itm);

                        }
                        else
                        {
                            itm.UpdateBy = userId;
                            itm.UpdateDate = dt;
                            await _collScheduleDtl.ReplaceOneAsync(x => x.DtlId == itm.DtlId, itm, new ReplaceOptions { IsUpsert = true });//update
                        }

                    }
                    // Made it here without error? Let's commit the transaction
                    //await session.CommitTransactionAsync();

                }
                catch (System.Exception ex)
                {
                    //rollback
                    //await session.AbortTransactionAsync();
                    return new ResponseModel("EX001", ex.Message);
                }
            //}
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionScheduleDtlAsync(RequestPaging request)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var data = await _collScheduleDtl.Find(new BsonDocument())
                    .SortBy(x => x.DtlId)
                    .Skip((request.Page - 1) * request.PerPage)
                    .Limit(request.PerPage)
                    .ToListAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionScheduleDtlByIdAsync(int dtlId)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var fillter = Builders<CollectionScheduleDtl>.Filter.Eq(x => x.DtlId, dtlId);
                var data = await _collScheduleDtl.Aggregate()
                   .Match(fillter).FirstOrDefaultAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        public async Task<ResponseModel> fnGetScheduleForUserAsync(string userId, int month, int year, string shift)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                //var fillter = Builders<CollectionScheduleDtl>.Filter.Where(x=>x.UserId:userId && x.Day);
                //var data = await _collScheduleDtl.Aggregate()
                //   .Match({ userId}).ToListAsync();

                var data = _collScheduleDtl.AsQueryable()
                    .Where(x => x.UserId == userId && DateTime.Parse(x.Day).Year == year 
                    && DateTime.Parse(x.Day).Month == month 
                    && x.IsActive
                    && (string.IsNullOrEmpty(shift) || x.Shift==shift)
                    ).ToList();
                //var data = _collScheduleDtl.AsQueryable().Where(x=>x.UserId== userId && x.IsActive).ToList();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        #endregion
        #region checkIO
        public async Task<ResponseModel> fnCoUCollectionCheckIOAsync(CollectionCheckIO checkIO, string userId)
        {
            ResponseModel res = new ResponseModel();
            string dt = CommonBase.fnGertDateTimeNow();
            client = new MongoClient(ConnectionURI);
            //using (var session = await client.StartSessionAsync())
            //{
            //    //Begin transaction
            //    session.StartTransaction();
                try
                {

                    var fillter = Builders<CollectionCheckIO>.Filter.Eq("Id", checkIO.Id);

                    var dataColCheckIO = await _collCheckIO.Find(new BsonDocument()).ToListAsync();

                    var checkIOInfo = dataColCheckIO.FirstOrDefault(x => x.Id == checkIO.Id);
                    if (checkIOInfo == null)
                    {
                        checkIO.CreateBy = userId;
                        checkIO.CreateDate = dt;
                        await _collCheckIO.InsertOneAsync(checkIO);

                    }
                    else
                    {
                        checkIO.UpdateBy = userId;
                        checkIO.UpdateDate = dt;
                        await _collCheckIO.ReplaceOneAsync(x => x.Id == checkIO.Id, checkIO, new ReplaceOptions { IsUpsert = true });//update
                    }


                    // Made it here without error? Let's commit the transaction
                    //await session.CommitTransactionAsync();

                }
                catch (System.Exception ex)
                {
                    //rollback
                    //await session.AbortTransactionAsync();
                    return new ResponseModel("EX001", ex.Message);
                }
            //}
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionCheckIOAsync(RequestPaging request)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var data = await _collCheckIO.Find(new BsonDocument())
                    .SortBy(x => x.UserId)
                    .Skip((request.Page - 1) * request.PerPage)
                    .Limit(request.PerPage)
                    .ToListAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionCheckIOByIdAsync(string userId)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var fillter = Builders<CollectionCheckIO>.Filter.Eq(x => x.UserId, userId);
                var data = await _collCheckIO.Aggregate()
                   .Match(fillter).FirstOrDefaultAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        #endregion
        #region serviceMst
        public async Task<ResponseModel> fnCoUCollectionServiceMstAsync(CollectionServiceMst serviceMst, string userId)
        {
            ResponseModel res = new ResponseModel();
            string dt = CommonBase.fnGertDateTimeNow();
            client = new MongoClient(ConnectionURI);
            //using (var session = await client.StartSessionAsync())
            //{
            //    //Begin transaction
            //    session.StartTransaction();
                try
                {

                    var fillter = Builders<CollectionServiceMst>.Filter.Eq("IdService", serviceMst.IdService);

                    var dataColServiceMst = await _collServiceMst.Find(new BsonDocument()).ToListAsync();

                    var serviceMstInfo = dataColServiceMst.FirstOrDefault(x => x.IdService == serviceMst.IdService);
                    if (serviceMstInfo == null)
                    {
                        serviceMst.CreateBy = userId;
                        serviceMst.CreateDate = dt;
                        await _collServiceMst.InsertOneAsync(serviceMst);

                    }
                    else
                    {
                        serviceMst.UpdateBy = userId;
                        serviceMst.UpdateDate = dt;
                        await _collServiceMst.ReplaceOneAsync(x => x.IdService == serviceMst.IdService, serviceMst, new ReplaceOptions { IsUpsert = true });//update
                    }


                    // Made it here without error? Let's commit the transaction
                    //await session.CommitTransactionAsync();

                }
                catch (System.Exception ex)
                {
                    //rollback
                   //await session.AbortTransactionAsync();
                    return new ResponseModel("EX001", ex.Message);
                }
            //}
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionServiceMstAsync(RequestPaging request)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var data = await _collServiceMst.Find(new BsonDocument())
                    .SortBy(x => x.IdService)
                    .Skip((request.Page - 1) * request.PerPage)
                    .Limit(request.PerPage)
                    .ToListAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionServiceMstByIdAsync(string serviceId)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var fillter = Builders<CollectionServiceMst>.Filter.Eq(x => x.IdService, serviceId);
                var data = await _collServiceMst.Aggregate()
                   .Match(fillter).FirstOrDefaultAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        public async Task<ResponseModel> fnGetCollServiceForComboAsync()
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var data = await _collServiceMst.Find(new BsonDocument())
                    .SortBy(x => x.IdService)
                    .ToListAsync();
                res.Data = data.Where(x=>x.IsActive).Select(x=>new { 
                    ServiceId=x.IdService,
                    ServiceName = x.ServiceName
                }).ToList();
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        #endregion
        #region serviceReg
        public async Task<ResponseModel> fnCoUCollectionServiceRegAsync(CollectionServiceReg serviceReg, string userId)
        {
            ResponseModel res = new ResponseModel();
            string dt = CommonBase.fnGertDateTimeNow();
            var clientNew = new MongoClient(ConnectionURI);
            

            //using (var session = await clientNew.StartSessionAsync())
            //{
                
                try
                {
                    
                    var fillter = Builders<CollectionServiceReg>.Filter.Eq("DtlID", serviceReg.DtlID);

                    var dataColServiceReg = await _collServiceReg.Find(new BsonDocument()).ToListAsync();
                    int dtlLastId = dataColServiceReg.OrderByDescending(x => x.DtlID).FirstOrDefault().DtlID + 1;
                    var serviceRegInfo = dataColServiceReg.FirstOrDefault(x => x.DtlID == serviceReg.DtlID);
                    if (serviceRegInfo == null)
                    {
                        serviceReg.DtlID = dtlLastId;
                        serviceReg.Requester = userId;
                        serviceReg.CreateBy = userId;
                        serviceReg.CreateDate = dt;
                        serviceReg.RequestDate = dt;
                        serviceReg.Status = "N";
                        await _collServiceReg.InsertOneAsync(serviceReg);

                    }
                    else
                    {
                        serviceReg._id = serviceRegInfo._id;
                        serviceReg.CreateBy = serviceRegInfo.CreateBy;
                        serviceReg.CreateDate = serviceRegInfo.CreateDate;
                        serviceReg.UpdateBy = userId;
                        serviceReg.UpdateDate = dt;
                        await _collServiceReg.ReplaceOneAsync(x => x.DtlID == serviceReg.DtlID, serviceReg, new ReplaceOptions { IsUpsert = true });//update
                    }


                }
                catch (System.Exception ex)
                {
                    //rollback
                    //await session.AbortTransactionAsync();
                    return new ResponseModel("EX001", ex.Message);
                }
                

                //client.d
            //}
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionServiceRegAsync(RequestPaging request)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                //var data = await _collServiceReg.Find(new BsonDocument())
                //    .SortBy(x => x.ServiceId)
                //    .Skip((request.Page - 1) * request.PerPage)
                //    .Limit(request.PerPage)
                //    .ToListAsync();
                var data = from a in _collServiceReg.AsQueryable()
                           join b in _collServiceMst.AsQueryable() on a.ServiceId equals b.IdService
                           join c in _collUserInfo.AsQueryable() on a.Requester equals c.UserId
                           join d in _collUserInfo.AsQueryable() on a.Confirmby equals d.UserId into tmpUser
                           join e in _collMajor.AsQueryable() on a.MajorFrom equals e.MajorID into tmpMajorF
                           join f in _collMajor.AsQueryable() on a.MajorTo equals f.MajorID into tmpMajorT
                           join k in _collSubject.AsQueryable() on a.SubjectId equals k.SubjectId into tmpSubject
                           from k in tmpSubject.DefaultIfEmpty()
                           from f in tmpMajorT.DefaultIfEmpty()
                           from e in tmpMajorF.DefaultIfEmpty()
                           from d in tmpUser.DefaultIfEmpty()
                               //where a.UserId == request.userId //&& (!string.IsNullOrEmpty(request.subjectName) || request.subjectName.Contains(aaa.SubjectName))
                           select new
                           {
                               a._id,a.DtlID,a.ReciveDate,a.Requester,a.Remark,a.RejectType,a.Remark1,a.Confirmby,a.ConfirmDate,
                               a.SubjectId,a.CreateBy,a.CreateDate,a.IsActive,a.MajorFrom,a.MajorTo,a.ServiceId,
                               ServiceName = "[" + b.IdService + "] " + b.ServiceName,
                               RequestName = c.FirstName + c.LastName,
                               ConfirmByName = d.FirstName + d.LastName,
                               MajorFromName = "[" + e.MajorID + "] " + e.MajorName,
                               MajorToName = "[" + f.MajorID + "] " + f.MajorName,
                               SubjectName = "[" + k.SubjectId + "] " + k.SubjectName
                           };
                res.Data = data.AsEnumerable().ToList();
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionServiceRegByIdAsync(string serviceId)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var fillter = Builders<CollectionServiceReg>.Filter.Eq(x => x.ServiceId, serviceId);
                var data = await _collServiceReg.Aggregate()
                   .Match(fillter).FirstOrDefaultAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        #endregion
        #region subject
        public async Task<ResponseModel> fnCoUCollectionSubjectAsync(CollectionSubject subject, string userId)
        {
            ResponseModel res = new ResponseModel();
            string dt = CommonBase.fnGertDateTimeNow();
            client = new MongoClient(ConnectionURI);
            //using (var session = await client.StartSessionAsync())
            //{
            //    //Begin transaction
            //    session.StartTransaction();
                try
                {

                    var fillter = Builders<CollectionSubject>.Filter.Eq("ServiceId", subject.SubjectId);

                    var dataColSubject = await _collSubject.Find(new BsonDocument()).ToListAsync();

                    var subjectInfo = dataColSubject.FirstOrDefault(x => x.SubjectId == subject.SubjectId);
                    if (subjectInfo == null)
                    {
                        subject.CreateBy = userId;
                        subject.CreateDate = dt;
                        await _collSubject.InsertOneAsync(subject);

                    }
                    else
                    {
                        subject.UpdateBy = userId;
                        subject.UpdateDate = dt;
                        await _collSubject.ReplaceOneAsync(x => x.SubjectId == subject.SubjectId, subject, new ReplaceOptions { IsUpsert = true });//update
                    }


                    // Made it here without error? Let's commit the transaction
                    //await session.CommitTransactionAsync();

                }
                catch (System.Exception ex)
                {
                    //rollback
                    //await session.AbortTransactionAsync();
                    return new ResponseModel("EX001", ex.Message);
                }
            //}
            return res;
        }
        public async Task<ResponseModel> fnGetCollSubjectForComboAsync()
        {
            ResponseModel res = new ResponseModel();
            try
            {
               
                var data = await _collSubject.Find(new BsonDocument())
                    .SortBy(x => x.SubjectId)
                    .ToListAsync();
                var dt = data.Where(x => x.IsActive).Select(x => new
                {
                    SubjectId = x.SubjectId,
                    SubjectName = x.SubjectName
                }).ToList();
                res.Data = dt;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionSubjectAsync(RequestPaging request)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var data = await _collSubject.Find(new BsonDocument())
                    .SortBy(x => x.SubjectId)
                    .Skip((request.Page - 1) * request.PerPage)
                    .Limit(request.PerPage)
                    .ToListAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionSubjectByIdAsync(string subjectId)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var fillter = Builders<CollectionSubject>.Filter.Eq(x => x.SubjectId, subjectId);
                var data = await _collSubject.Aggregate()
                   .Match(fillter).FirstOrDefaultAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        #endregion
        #region user
        public async Task<ResponseModel> fnCoUCollectionUserAsync(CollectionUser user, string userId)
        {
            ResponseModel res = new ResponseModel();
            string dt = CommonBase.fnGertDateTimeNow();
            client = new MongoClient(ConnectionURI);
            //using (var session = await client.StartSessionAsync())
            //{
            //    //Begin transaction
            //    session.StartTransaction();
                try
                {

                    var fillter = Builders<CollectionUser>.Filter.Eq("UserId", user.UserId);

                    var dataColUser = await _collUser.Find(new BsonDocument()).ToListAsync();

                    var userInfo = dataColUser.FirstOrDefault(x => x.UserId == user.UserId);
                    if (userInfo == null)
                    {
                        user.CreateBy = userId;
                        user.CreateDate = dt;
                        await _collUser.InsertOneAsync(user);

                    }
                    else
                    {
                        user.UpdateBy = userId;
                        user.UpdateDate = dt;
                        await _collUser.ReplaceOneAsync(x => x.UserId == user.UserId, user, new ReplaceOptions { IsUpsert = true });//update
                    }


                    // Made it here without error? Let's commit the transaction
                    //await session.CommitTransactionAsync();

                }
                catch (System.Exception ex)
                {
                    //rollback
                    //await session.AbortTransactionAsync();
                    return new ResponseModel("EX001", ex.Message);
                }
            //}
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionUserAsync(RequestPaging request)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var data = await _collUser.Find(new BsonDocument())
                    .SortBy(x => x.UserId)
                    .Skip((request.Page - 1) * request.PerPage)
                    .Limit(request.PerPage)
                    .ToListAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionUserByIdAsync(string userId)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var fillter = Builders<CollectionUser>.Filter.Eq(x => x.UserId, userId);
                var data = await _collUser.Aggregate()
                   .Match(fillter).FirstOrDefaultAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        #endregion
        #region userinfo
        public async Task<ResponseModel> fnCoUCollectionUserInfoAsync(CollectionUserInfo userInfo, string userId)
        {
            ResponseModel res = new ResponseModel();
            string dt = CommonBase.fnGertDateTimeNow();
            client = new MongoClient(ConnectionURI);
            //using (var session = await client.StartSessionAsync())
            //{
            //    //Begin transaction
            //    session.StartTransaction();
                try
                {
                    
                    
                    var fillter = Builders<CollectionUserInfo>.Filter.Eq("UserId", userInfo.UserId);

                    var dataColUserInfo = await _collUserInfo.Find(new BsonDocument()).ToListAsync();

                    var userInfoInfo = dataColUserInfo.FirstOrDefault(x => x.UserId == userInfo.UserId);
                    if (userInfoInfo == null)
                    {
                        userInfo.CreateBy = userId;
                        userInfo.CreateDate = dt;
                        await _collUserInfo.InsertOneAsync(userInfo);
                        var user = new CollectionUser();
                        user.UserId = userInfo.UserId;
                        user.Password = "123456789a";
                        user.UserType = userInfo.UserType;
                        user.UserGroup = string.Empty;
                        user.IsLocked = false;
                        user.CreateBy = userId;
                        user.CreateDate = dt;
                        await _collUser.InsertOneAsync(user);
                    }
                    else
                    {
                        userInfo._id = userInfoInfo._id;
                        userInfo.UpdateBy = userId;
                        userInfo.UpdateDate = dt;
                        await _collUserInfo.ReplaceOneAsync(x => x.UserId == userInfo.UserId, userInfo, new ReplaceOptions { IsUpsert = true });//update
                    }

                    
                    // Made it here without error? Let's commit the transaction
                    //await session.CommitTransactionAsync();

                }
                catch (System.Exception ex)
                {
                    //rollback
                    //await session.AbortTransactionAsync();
                    return new ResponseModel("EX001", ex.Message);
                }
            //}
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionUserInfoAsync(RequestPaging request)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                List<CollectionUserInfo> lstData = await _collUserInfo.Find(new BsonDocument()).ToListAsync();
                
                //.SortBy(x => x.UserId)
                //.Skip((request.Page - 1) * request.PerPage)
                //.Limit(request.PerPage)
                //.ToListAsync();

                //Filltering fillter = HelperInfo.ConvertToType<Filltering>(request.Filltering);
                if (request.Filltering.Count>0)
                {
                    var fillter = request.Filltering.FirstOrDefault(x => x.CollName == "UserId");
                    if (fillter.CollName == "UserId")
                    {
                        var data = lstData.Where(x => x.UserId.Contains(fillter.ValueDefault));
                        res.Data = data;
                        return res;
                    }




                }
                
                res.Data = lstData;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionUserInfoByIdAsync(string userId)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var fillter = Builders<CollectionUserInfo>.Filter.Eq(x => x.UserId, userId);
                var data = await _collUserInfo.Aggregate()
                   .Match(fillter).FirstOrDefaultAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        #endregion
        #region Code for Winform
        public async Task<ResponseModel> fnGetDataPointforUserAsync(GetDataPointForReq request)
        {
            var res = new ResponseModel();
            try
            {
                
                var data = from a in _collMarks.AsQueryable()
                           join b in _collMarkDtl.AsQueryable() on a.MarkId equals b.MarkId
                           join c in _collUserInfo.AsQueryable() on a.UserId equals c.UserId
                           join d in _collClass.AsQueryable() on c.IdClass equals d.ClassId
                           join aa in _collMajor.AsQueryable() on d.MajorID equals aa.MajorID //into tmpMajor
                           join aaa in _collSubject.AsQueryable() on b.SubjectId equals aaa.SubjectId //into tmpSubject
                           join aaaa in _collUserInfo.AsQueryable() on b.Teacher equals aaaa.UserId //into tmpTecher
                           //from aaaa in tmpTecher.DefaultIfEmpty()
                           //from aaa in tmpSubject.DefaultIfEmpty()
                           //from aa in tmpMajor.DefaultIfEmpty()
                           where a.UserId == request.userId //&& (!string.IsNullOrEmpty(request.subjectName) || request.subjectName.Contains(aaa.SubjectName))
                           select new
                           {
                               a.MarkId,
                               b.MarkDtlId,
                               a.UserId,
                               LastName=c.FirstName +" "+ c.LastName,
                               AveragePointsMst = a.AveragePoints,
                               b.AveragePoints,
                               b.Teacher,
                               b.SubjectId,
                               TotalCreditPass=a.TotalCredit,
                               aa.NumberOfCredits,
                               a.TotalFail,
                               a.TotalNotYet,
                               a.TotalPass,
                               a.TotalStuding,
                               SubjectName="["+ b.SubjectId + "] "+aaa.SubjectName,
                              TeacherName=aaaa.FirstName+ aaaa.LastName
                           };
                var dats = data.AsEnumerable().ToList()
                .GroupBy(x => new { x.MarkId, x.UserId, x.LastName, x.AveragePointsMst,x.TotalCreditPass, x.TotalFail,x.TotalNotYet,x.TotalPass,x.TotalStuding,x.NumberOfCredits })
                .Select(x => new
                {
                    MarkId=x.Key.MarkId,
                    UserId = x.Key.UserId,
                    LastName = x.Key.LastName,
                    TotalCreditPass = x.Key.TotalCreditPass,
                    NumberOfCredits = x.Key.NumberOfCredits,
                    TotalFail = x.Key.TotalFail,
                    TotalNotYet = x.Key.TotalNotYet,
                    TotalPass = x.Key.TotalPass,
                    TotalStuding = x.Key.TotalStuding,
                    AveragePointsMst = x.Key.AveragePointsMst,
                    Details = x.ToList(),
                });
                res.Data = dats.FirstOrDefault();
            }
            catch (Exception ex)
            {

                return new ResponseModel(ErrorMessage.Error0001, ex.Message);
            }
            return res;
        }
        #endregion














    }
}
