using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISFAHD.Controllers
{
    public class MensajeController : ControllerBase
    {
        public IActionResult SendMessage()
        {
            return Ok();
        }
    }
}
