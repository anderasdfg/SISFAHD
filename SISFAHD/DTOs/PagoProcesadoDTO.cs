using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Linq;
using System.Threading.Tasks;
using SISFAHD.Entities;

namespace SISFAHD.DTOs
{
    public class PagoProcesadoDTO
    {
        public Header header { get; set; } = new Header();
        public Order order { get; set; } = new Order();
        public DataMap dataMap { get; set; } = new DataMap();
    }
    public class Header
    {
        public string ecoreTransactionUUID { get; set; }
        public int ecoreTransactionDate { get; set; }
        public int millis { get; set; }
    }
    public class Order
    {
        public string tokenId { get; set; }
        public string purchaseNumber { get; set; }
        public string productId { get; set; }
        public Double amount { get; set; }
        public string currency { get; set; }
        public Double authirizedAmount { get; set; }
        public string authirizedCode { get; set; }
        public string actionCode { get; set; }
        public string traceNumber { get; set; }
        public string transactionDate { get; set; }
        public string transactionId { get; set; }
    }
    public class DataMap
    {
        public string CURRENCY { get; set; }
        public string TRANSACTION_DATE { get; set; }
        public string TERMINAL { get; set; }
        public string ACTION_CODE { get; set; }
        public string TRACE_NUMBER { get; set; }
        public string ECI_DESCRIPTION { get; set; }
        public string ECI { get; set; }
        public string BRAND { get; set; }
        public string CARD { get; set; }
        public string MERCHANT { get; set; }
        public string STATUS { get; set; }
        public string ADQUIRENTE { get; set; }
        public string ACTION_DESCRIPTION { get; set; }
        public string ID_UNICO { get; set; }
        public string AMOUNT { get; set; }
        public string PROCES_CODE { get; set; }
        public string RECURRENCE_STATUS { get; set; }
        public string TRANSACTION_ID { get; set; }
        public string AUTHORIZATION_CODE { get; set; }
    }
    
}
