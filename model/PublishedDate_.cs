using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace UF3_test.model
{
    [Serializable]
    public class PublishedDate
    {
        public String date { get; set; }
    
        public override string ToString()
        {
            return 
                "PublishedDate{" + 
                "$date = '" + date + '\'' + 
                "}";
        }
    }
}