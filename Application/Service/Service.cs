using Application.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.IService;
using Application.Model;
using System.Linq;
using System;

namespace Application.Service
{
    public class Services : IServices
    {
        private readonly IMongoCollection<CollectionClass> _collClass;

        private readonly IMongoCollection<CollectionUserInfo> _collUserInfo;

        private readonly IMongoCollection<CollectionUser> _collUser;

        public Services(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _collClass = database.GetCollection<CollectionClass>(mongoDBSettings.Value.CollectionName);
            _collUserInfo = database.GetCollection<CollectionUserInfo>("tblIUser_Info");
            _collUser = database.GetCollection<CollectionUser>("tblUsers");
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
        public async Task<ResponeModel> fnCoUCollectionClassAsync(List<CollectionClass> lstPlaylist,string userId) {
            ResponeModel res = new ResponeModel();
            try
            {
                foreach(var itm in lstPlaylist)
                {
                    var dataColClass = await _collClass.Find(new BsonDocument()).ToListAsync();

                    var classInfo = dataColClass.FirstOrDefault(x => x.ClassId == itm.ClassId);
                    if (classInfo == null)
                    {
                        itm.CreateBy = userId;
                        itm.CreateDate = DateTime.Now;
                        await _collClass.InsertOneAsync(itm);

                    }
                    else
                    {
                        itm.UpdateBy = userId;
                        itm.UpdateDate = DateTime.Now;
                        await _collClass.InsertOneAsync(itm);
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
