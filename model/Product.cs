using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace UF3_test.model
{
    [Serializable]
    public class Product
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string name { get; set; }
        public int price { get; set; }
        public int stock { get; set; }
        public string picture { get; set; }
        public List<string> categories { get; set; }
    }
}