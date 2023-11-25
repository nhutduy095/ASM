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
        private  IMongoCollection<CollectionUserInfo> _collUserInfo;
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
           // _collScheduleDtl = database.GetCollection<CollectionScheduleDtl>("tblSchedule_Dtl");
            //_collCheckIO = database.GetCollection<CollectionCheckIO>("tblCheckIO");
            _collServiceMst = database.GetCollection<CollectionServiceMst>("tblServiceMst");
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
                
                var userinfor = dataUser.FirstOrDefault(x=>x.UserId==reqData.username);
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
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"], claims,
                    expires: DateTime.UtcNow.AddMinutes(260),
                    signingCredentials: signIn);
                //res.ErrMessage = new JwtSecurityTokenHandler().WriteToken(token);
                var loginRes = new LoginRespone();
                loginRes.Token= new JwtSecurityTokenHandler().WriteToken(token);
                loginRes.UserInfo = userInfo;
                res.Data = loginRes;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel(ErrorMessage.Error0001,ex.Message);
            }
            return res;
        }
        #endregion
        #region master data
        #endregion
        
        public async Task<ResponseModel> fnCoUCollectionClassAsync(List<CollectionClass> lstClass,string userId) {
            ResponseModel res = new ResponseModel();
            string dt = CommonBase.fnGertDateTimeNow();
            client = new MongoClient(ConnectionURI);
            using (var session = await client.StartSessionAsync())
            {
                //Begin transaction
                session.StartTransaction();
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
                    await session.CommitTransactionAsync();

                }
                catch (System.Exception ex)
                {
                    //rollback
                    await session.AbortTransactionAsync();
                    return new ResponseModel("EX001", ex.Message);
                }
            }
            return res;
        }
        public async Task<ResponseModel> fnCoUCollectionCommonAsync(List<CollectionCommon> lstCommon, string userId)
        {
            ResponseModel res = new ResponseModel();
            string dt = CommonBase.fnGertDateTimeNow();
            client = new MongoClient(ConnectionURI);
            using (var session = await client.StartSessionAsync())
            {
                //Begin transaction
                session.StartTransaction();
                try
                {
                    foreach (var itm in lstCommon)
                    {
                        var fillter = Builders<CollectionClass>.Filter.Eq("CommonCd", itm.CommonCd);

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
                    await session.CommitTransactionAsync();

                }
                catch (System.Exception ex)
                {
                    //rollback
                    await session.AbortTransactionAsync();
                    return new ResponseModel("EX001", ex.Message);
                }
            }
            return res;
        }
        public async Task<ResponseModel> fnCoUCollectionDepartmentAsync(CollectionDepartment department, string userId)
        {
            ResponseModel res = new ResponseModel();
            string dt = CommonBase.fnGertDateTimeNow();
            client = new MongoClient(ConnectionURI);
            using (var session = await client.StartSessionAsync())
            {
                //Begin transaction
                session.StartTransaction();
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
                    await session.CommitTransactionAsync();

                }
                catch (System.Exception ex)
                {
                    //rollback
                    await session.AbortTransactionAsync();
                    return new ResponseModel("EX001", ex.Message);
                }
            }
            return res;
        }
        public async Task<ResponseModel> fnCoUCollectionMajorAsync(CollectionMajor major, string userId)
        {
            ResponseModel res = new ResponseModel();
            string dt = CommonBase.fnGertDateTimeNow();
            using (var session = await client.StartSessionAsync())
            {
                //Begin transaction
                session.StartTransaction();
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
                    await session.CommitTransactionAsync();

                }
                catch (System.Exception ex)
                {
                    //rollback
                    await session.AbortTransactionAsync();
                    return new ResponseModel("EX001", ex.Message);
                }
            }
            return res;
        }
        public async Task<ResponseModel> fnCoUCollectionMajorDtlAsync(List<CollectionMajorDtl> lstMajorDtl, string userId)
        {
            ResponseModel res = new ResponseModel();
            string dt = CommonBase.fnGertDateTimeNow();
            using (var session = await client.StartSessionAsync())
            {
                //Begin transaction
                session.StartTransaction();
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
                    await session.CommitTransactionAsync();

                }
                catch (System.Exception ex)
                {
                    //rollback
                    await session.AbortTransactionAsync();
                    return new ResponseModel("EX001", ex.Message);
                }
            }
            return res;
        }
        public async Task<ResponseModel> fnCoUCollectionMarksAsync(CollectionMarks marks, string userId)
        {
            ResponseModel res = new ResponseModel();
            string dt = CommonBase.fnGertDateTimeNow();
            using (var session = await client.StartSessionAsync())
            {
                //Begin transaction
                session.StartTransaction();
                try
                {

                    var fillter = Builders<CollectionMarks>.Filter.Eq("MajorID", marks.MarkId);

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
                    await session.CommitTransactionAsync();

                }
                catch (System.Exception ex)
                {
                    //rollback
                    await session.AbortTransactionAsync();
                    return new ResponseModel("EX001", ex.Message);
                }
            }
            return res;
        }
        public async Task<ResponseModel> fnCoUCollectionMarkDtlAsync(List<CollectionMarkDtl> lstMarkDtl, string userId)
        {
            ResponseModel res = new ResponseModel();
            string dt = CommonBase.fnGertDateTimeNow();
            using (var session = await client.StartSessionAsync())
            {
                //Begin transaction
                session.StartTransaction();
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
                    await session.CommitTransactionAsync();

                }
                catch (System.Exception ex)
                {
                    //rollback
                    await session.AbortTransactionAsync();
                    return new ResponseModel("EX001", ex.Message);
                }
            }
            return res;
        }
        public async Task<ResponseModel> fnCoUCollectionMarkDtl1Async(List<CollectionMarkDtl1> lstMarkDtl1, string userId)
        {
            ResponseModel res = new ResponseModel();
            string dt = CommonBase.fnGertDateTimeNow();
            using (var session = await client.StartSessionAsync())
            {
                //Begin transaction
                session.StartTransaction();
                try
                {
                    foreach (var itm in lstMarkDtl1)
                    {
                        var fillter = Builders<CollectionMarkDtl>.Filter.Eq("Id", itm._id);

                        var dataColMarkDtl1 = await _collMarkDtl1.Find(new BsonDocument()).ToListAsync();

                        var markDtl1Info = dataColMarkDtl1.FirstOrDefault(x => x._id == itm._id);
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
                            await _collMarkDtl1.ReplaceOneAsync(x => x._id == itm._id, itm, new ReplaceOptions { IsUpsert = true });//update
                        }

                    }
                    // Made it here without error? Let's commit the transaction
                    await session.CommitTransactionAsync();

                }
                catch (System.Exception ex)
                {
                    //rollback
                    await session.AbortTransactionAsync();
                    return new ResponseModel("EX001", ex.Message);
                }
            }
            return res;
        }
        public async Task<ResponseModel> fnCoUCollectionRoomAsync(CollectionRoom room, string userId)
        {
            ResponseModel res = new ResponseModel();
            string dt = CommonBase.fnGertDateTimeNow();
            using (var session = await client.StartSessionAsync())
            {
                //Begin transaction
                session.StartTransaction();
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
                    await session.CommitTransactionAsync();

                }
                catch (System.Exception ex)
                {
                    //rollback
                    await session.AbortTransactionAsync();
                    return new ResponseModel("EX001", ex.Message);
                }
            }
            return res;
        }
        public async Task<ResponseModel> fnCoUCollectionScheduleAsync(CollectionSchedule schedule, string userId)
        {
            ResponseModel res = new ResponseModel();
            string dt = CommonBase.fnGertDateTimeNow();
            using (var session = await client.StartSessionAsync())
            {
                //Begin transaction
                session.StartTransaction();
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
                    await session.CommitTransactionAsync();

                }
                catch (System.Exception ex)
                {
                    //rollback
                    await session.AbortTransactionAsync();
                    return new ResponseModel("EX001", ex.Message);
                }
            }
            return res;
        }
        public async Task<ResponseModel> fnCoUCollectionScheduleDtlAsync(List<CollectionScheduleDtl> lstScheduleDtl, string userId)
        {
            ResponseModel res = new ResponseModel();
            string dt = CommonBase.fnGertDateTimeNow();
            using (var session = await client.StartSessionAsync())
            {
                //Begin transaction
                session.StartTransaction();
                try
                {
                    foreach (var itm in lstScheduleDtl)
                    {
                        var fillter = Builders<CollectionScheduleDtl>.Filter.Eq("Id", itm.Id);

                        var dataColScheduleDtl = await _collScheduleDtl.Find(new BsonDocument()).ToListAsync();

                        var scheduleDtlInfo = dataColScheduleDtl.FirstOrDefault(x => x.Id == itm.Id);
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
                            await _collScheduleDtl.ReplaceOneAsync(x => x.Id == itm.Id, itm, new ReplaceOptions { IsUpsert = true });//update
                        }

                    }
                    // Made it here without error? Let's commit the transaction
                    await session.CommitTransactionAsync();

                }
                catch (System.Exception ex)
                {
                    //rollback
                    await session.AbortTransactionAsync();
                    return new ResponseModel("EX001", ex.Message);
                }
            }
            return res;
        }
        public async Task<ResponseModel> fnCoUCollectionCheckIOAsync(CollectionCheckIO checkIO, string userId)
        {
            ResponseModel res = new ResponseModel();
            string dt = CommonBase.fnGertDateTimeNow();
            using (var session = await client.StartSessionAsync())
            {
                //Begin transaction
                session.StartTransaction();
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
                    await session.CommitTransactionAsync();

                }
                catch (System.Exception ex)
                {
                    //rollback
                    await session.AbortTransactionAsync();
                    return new ResponseModel("EX001", ex.Message);
                }
            }
            return res;
        }
        public async Task<ResponseModel> fnCoUCollectionServiceMstAsync(CollectionServiceMst serviceMst, string userId)
        {
            ResponseModel res = new ResponseModel();
            string dt = CommonBase.fnGertDateTimeNow();
            using (var session = await client.StartSessionAsync())
            {
                //Begin transaction
                session.StartTransaction();
                try
                {

                    var fillter = Builders<CollectionServiceMst>.Filter.Eq("ServiceId", serviceMst.ServiceId);

                    var dataColServiceMst = await _collServiceMst.Find(new BsonDocument()).ToListAsync();

                    var serviceMstInfo = dataColServiceMst.FirstOrDefault(x => x.ServiceId == serviceMst.ServiceId);
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
                        await _collServiceMst.ReplaceOneAsync(x => x.ServiceId == serviceMst.ServiceId, serviceMst, new ReplaceOptions { IsUpsert = true });//update
                    }


                    // Made it here without error? Let's commit the transaction
                    await session.CommitTransactionAsync();

                }
                catch (System.Exception ex)
                {
                    //rollback
                    await session.AbortTransactionAsync();
                    return new ResponseModel("EX001", ex.Message);
                }
            }
            return res;
        }
        public async Task<ResponseModel> fnCoUCollectionServiceRegAsync(List<CollectionServiceReg> lstServiceReg, string userId)
        {
            ResponseModel res = new ResponseModel();
            string dt = CommonBase.fnGertDateTimeNow();
            using (var session = await client.StartSessionAsync())
            {
                //Begin transaction
                session.StartTransaction();
                try
                {
                    foreach (var itm in lstServiceReg)
                    {
                        var fillter = Builders<CollectionServiceReg>.Filter.Eq("Id", itm.DtlID);

                        var dataColServiceReg = await _collServiceReg.Find(new BsonDocument()).ToListAsync();

                        var serviceRegInfo = dataColServiceReg.FirstOrDefault(x => x.DtlID == itm.DtlID);
                        if (serviceRegInfo == null)
                        {
                            itm.CreateBy = userId;
                            itm.CreateDate = dt;
                            await _collServiceReg.InsertOneAsync(itm);

                        }
                        else
                        {
                            itm.UpdateBy = userId;
                            itm.UpdateDate = dt;
                            await _collServiceReg.ReplaceOneAsync(x => x.DtlID == itm.DtlID, itm, new ReplaceOptions { IsUpsert = true });//update
                        }

                    }
                    // Made it here without error? Let's commit the transaction
                    await session.CommitTransactionAsync();

                }
                catch (System.Exception ex)
                {
                    //rollback
                    await session.AbortTransactionAsync();
                    return new ResponseModel("EX001", ex.Message);
                }
            }
            return res;
        }
        public async Task<ResponseModel> fnCoUCollectionSubjectAsync(CollectionSubject subject, string userId)
        {
            ResponseModel res = new ResponseModel();
            string dt = CommonBase.fnGertDateTimeNow();
            using (var session = await client.StartSessionAsync())
            {
                //Begin transaction
                session.StartTransaction();
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
                    await session.CommitTransactionAsync();

                }
                catch (System.Exception ex)
                {
                    //rollback
                    await session.AbortTransactionAsync();
                    return new ResponseModel("EX001", ex.Message);
                }
            }
            return res;
        }
        public async Task<ResponseModel> fnCoUCollectionUserAsync(CollectionUser user, string userId)
        {
            ResponseModel res = new ResponseModel();
            string dt = CommonBase.fnGertDateTimeNow();
            using (var session = await client.StartSessionAsync())
            {
                //Begin transaction
                session.StartTransaction();
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
                    await session.CommitTransactionAsync();

                }
                catch (System.Exception ex)
                {
                    //rollback
                    await session.AbortTransactionAsync();
                    return new ResponseModel("EX001", ex.Message);
                }
            }
            return res;
        }
        public async Task<ResponseModel> fnCoUCollectionUserInfoAsync(List<CollectionUserInfo> lstUserInfo, string userId)
        {
            ResponseModel res = new ResponseModel();
            string dt = CommonBase.fnGertDateTimeNow();
            using (var session = await client.StartSessionAsync())
            {
                //Begin transaction
                session.StartTransaction();
                try
                {
                    foreach (var itm in lstUserInfo)
                    {
                        var fillter = Builders<CollectionUserInfo>.Filter.Eq("UserId", itm.UserId);

                        var dataColUserInfo = await _collUserInfo.Find(new BsonDocument()).ToListAsync();

                        var userInfoInfo = dataColUserInfo.FirstOrDefault(x => x.UserId == itm.UserId);
                        if (userInfoInfo == null)
                        {
                            itm.CreateBy = userId;
                            itm.CreateDate = dt;
                            await _collUserInfo.InsertOneAsync(itm);

                        }
                        else
                        {
                            itm.UpdateBy = userId;
                            itm.UpdateDate = dt;
                            await _collUserInfo.ReplaceOneAsync(x => x.UserId == itm.UserId, itm, new ReplaceOptions { IsUpsert = true });//update
                        }

                    }
                    // Made it here without error? Let's commit the transaction
                    await session.CommitTransactionAsync();

                }
                catch (System.Exception ex)
                {
                    //rollback
                    await session.AbortTransactionAsync();
                    return new ResponseModel("EX001", ex.Message);
                }
            }
            return res;
        }

        public async Task<ResponseModel> fnGetCollectionClassAsync()
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var data = await _collClass.Find(new BsonDocument()).ToListAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionCommonAsync()
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var data = await _collCommon.Find(new BsonDocument()).ToListAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionDepartmentAsync()
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var data = await _collDept.Find(new BsonDocument()).ToListAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionMajorAsync()
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var data = await _collMajor.Find(new BsonDocument()).ToListAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionMajorDtlAsync()
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var data = await _collMajorDtl.Find(new BsonDocument()).ToListAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionMarkDtl1Async()
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var data = await _collMarkDtl1.Find(new BsonDocument()).ToListAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionMarkDtlAsync()
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var data = await _collMarkDtl.Find(new BsonDocument()).ToListAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionMarksAsync()
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var data = await _collMarks.Find(new BsonDocument()).ToListAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionRoomAsync()
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var data = await _collRoom.Find(new BsonDocument()).ToListAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionScheduleAsync()
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var data = await _collSchedule.Find(new BsonDocument()).ToListAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionScheduleDtlAsync()
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var data = await _collScheduleDtl.Find(new BsonDocument()).ToListAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionServiceMstAsync()
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var data = await _collServiceMst.Find(new BsonDocument()).ToListAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionServiceRegAsync()
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var data = await _collServiceReg.Find(new BsonDocument()).ToListAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionSubjectAsync()
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var data = await _collSubject.Find(new BsonDocument()).ToListAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionUserAsync()
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var data = await _collUser.Find(new BsonDocument()).ToListAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionUserInfoAsync()
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var data = await _collUserInfo.Find(new BsonDocument()).ToListAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        public async Task<ResponseModel> fnGetCollectionCheckIOAsync()
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var data = await _collCheckIO.Find(new BsonDocument()).ToListAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponseModel("EX001", ex.Message);
            }
            return res;
        }
        public async Task AddToPlaylistAsync(string id, string movieId) { }
        public async Task DeleteAsync(string id) { }
    }
}
