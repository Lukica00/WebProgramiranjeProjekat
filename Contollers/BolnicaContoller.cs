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
    public class BolnicaController : ControllerBase
    {
        public Context Context { get; set; }

        public BolnicaController(Context context)
        {
            Context = context;
        }
    }
}
