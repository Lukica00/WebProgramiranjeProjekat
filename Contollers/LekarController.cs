using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LekarController : ControllerBase
    {
        public Context Context { get; set; }

        public LekarController(Context context)
        {
            Context = context;
        }
    }
}
