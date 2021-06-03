using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Linq;
using System.Threading.Tasks;
using SISFAHD.Entities;

namespace SISFAHD.DTOs
{
    public class TransaccionDTO
    {
        public string antifraud { get; set; }
        public string captureType { get; set; }

        public string channel { get; set; }
        public bool countable { get; set; }
        public TransaccionOrder order { get; set; }
        //public string terminalId { get; set; }
        //public bool terminalUnattended { get; set; }
    }

    public class TransaccionOrder
    {
        public string amount { get; set; }
        public string currency { get; set; }
        public int purchaseNumber { get; set; }
        public string tokenId { get; set; }
    }
    public class ParametrosInsert
    {
        public string sessionkey { get; set; }
    }
}
