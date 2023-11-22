using System;
using System.Collections.Generic;
using System.Text;

namespace Application
{
    public class MongoDBSettings
    {
        public string ConnectionURI { get; set; } = "mongodb://localhost:27017";
        public string DatabaseName { get; set; } = "ASM_StudentManagement";
        public string CollectionName { get; set; } ="tblClass";
    }
}
