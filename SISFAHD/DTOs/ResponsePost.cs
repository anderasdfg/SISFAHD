using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISFAHD.DTOs
{
    public class ResponsePost
    {
        public string transactionToken { get; set; }
        public string customerEmail { get; set; }
        public string channel { get; set; }
    }
}
