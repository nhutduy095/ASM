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
        private MongoClient client;
        public IConfiguration _configuration;
        public Services(IOptions<MongoDBSettings> mongoDBSettings, IConfiguration configuration)
        {
            client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _collClass = database.GetCollection<CollectionClass>(mongoDBSettings.Value.CollectionName);
            _collUserInfo = database.GetCollection<CollectionUserInfo>("tblIUser_Info");
            _collUser = database.GetCollection<CollectionUser>("tblUsers");
            _collCommon = database.GetCollection<CollectionCommon>("tblCommon");
            _configuration = configuration;
        }
        #region Auth
        public async Task<ResponeModel> Login(LoginRequest reqData)
        {
            ResponeModel res = new ResponeModel();
            try
            {
                //FilterDefinition<Playlist> filter = Builders<Playlist>.Filter.Eq("Id", id);
                //UpdateDefinition<Playlist> update = Builders<Playlist>.Update.AddToSet<string>("movieIds", movieId);
                //await _playlistCollection.UpdateOneAsync(filter, update);
                var dataUser = await _collUser.Find(new BsonDocument()).ToListAsync();
                
                var userinfor = dataUser.FirstOrDefault(x=>x.UserId==reqData.username);
                if (userinfor == null)
                {
                    return new ResponeModel("ER001", "User không tồn tại");

                }
                if (userinfor.Password != reqData.password)
                {
                    return new ResponeModel("ER002", "Sai mật khẩu");
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
                res.ErrMessage = new JwtSecurityTokenHandler().WriteToken(token);
                res.Data = userInfo;
            }
            catch (System.Exception ex)
            {

                return new ResponeModel("EX001",ex.Message);
            }
            return res;
        }
        #endregion
        #region master data
        #endregion
        public async Task<ResponeModel> fnGetCollectionClassAsync()
        {
            ResponeModel res = new ResponeModel();
            try
            {
                var data= await _collClass.Find(new BsonDocument()).ToListAsync();
                res.Data = data;
            }
            catch (System.Exception ex)
            {

                return new ResponeModel("EX001", ex.Message);
            }
            return res;
            
        }
        public async Task<ResponeModel> fnCoUCollectionClassAsync(List<CollectionClass> lstClass,string userId) {
            ResponeModel res = new ResponeModel();
            string dt = CommonBase.fnGertDateTimeNow();
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
                    return new ResponeModel("EX001", ex.Message);
                }
            }
            return res;
        }
        public async Task<ResponeModel> fnCoUCollectionCommonAsync(List<CollectionCommon> lstCommon, string userId)
        {
            ResponeModel res = new ResponeModel();
            try
            {
                string dt = CommonBase.fnGertDateTimeNow();
                foreach (var itm in lstCommon)
                {
                    var dataColCommon = await _collCommon.Find(new BsonDocument()).ToListAsync();

                    var classInfo = dataColCommon.FirstOrDefault(x => x.CommonCd == itm.CommonCd);
                    if (classInfo == null)
                    {
                        itm.CreateBy = userId;
                        itm.CreateDate = dt;
                        await _collCommon.InsertOneAsync(itm);

                    }
                    else
                    {
                        itm.UpdateBy = userId;
                        itm.UpdateDate = dt;
                        await _collCommon.InsertOneAsync(itm);
                    }

                }

            }
            catch (System.Exception ex)
            {

                return new ResponeModel("EX001", ex.Message);
            }
            return res;
        }
        public async Task AddToPlaylistAsync(string id, string movieId) { }
        public async Task DeleteAsync(string id) { }
    }
}
