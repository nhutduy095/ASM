using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Base
{
    public abstract class EntityBase
    {
        [BsonId]
        //[JsonIgnore]
        //[System.Text.Json.Serialization.JsonIgnore]
        [BsonRepresentation(BsonType.ObjectId)]
        //[BsonRepresentation(BsonType.Int32)]
        //[BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public string _id { get; set; }
        public bool IsActive { get; set; } = true;
        public string CreateBy { get; set; }
        public string CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public string UpdateDate { get; set; }
        //public EntityBase()
        //{
        //    _id = ObjectId.GenerateNewId();
        //}
    }
}
