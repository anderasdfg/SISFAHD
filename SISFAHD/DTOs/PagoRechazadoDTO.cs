using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Linq;
using System.Threading.Tasks;
using SISFAHD.Entities;

namespace SISFAHD.DTOs
{
    public class PagoRechazadoDTO
    {
        public string errorCode { get; set; }
        public string errorMessage { get; set; }
        public Header header { get; set; } = new Header();
        public DataIncorrecto data { get; set; } = new DataIncorrecto();
    }
    public class DataIncorrecto
    {
        public string CURRENCY { get; set; }
        public string TRANSACTION_DATE { get; set; }
        public string TERMINAL { get; set; }
        public string ACTION_CODE { get; set; }
        public string TRACE_NUMBER { get; set; }
        public string ECI_DESCRIPTION { get; set; }
        public string ECI { get; set; }
        public string CARD { get; set; }
        public string BRAND { get; set; }
        public string MERCHANT { get; set; }
        public string STATUS { get; set; }
        public string ADQUIRENTE { get; set; }
        public string ACTION_DESCRIPTION { get; set; }
        public string ID_UNICO { get; set; }
        public string AMOUNT { get; set; }
        public string PROCES_CODE { get; set; }
        public string TRANSACTION_ID { get; set; }
    }
}
