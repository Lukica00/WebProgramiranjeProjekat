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
    public class PacijentController : ControllerBase
    {
        public Context Context { get; set; }

        public PacijentController(Context context)
        {
            Context = context;
        }

        [Route("Pacijenti/Get")]
        [HttpGet]
        public async Task<ActionResult> PacijentiGet()
        {
            try
            {
                return Ok(await Context.Pacijent.Select(p =>
                new
                {
                    ID = p.ID,
                    Ime = p.Ime
                }).ToListAsync());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Route("Pacijenti/Post")]
        [HttpPost]
        public async Task<ActionResult> PacijentiPost([FromBody] Pacijent pacijent)
        {
            try
            {
                Context.Pacijent.Add(pacijent);
                await Context.SaveChangesAsync();
                return Ok($"Pacijent je dodat! ID je: {pacijent.ID}");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}