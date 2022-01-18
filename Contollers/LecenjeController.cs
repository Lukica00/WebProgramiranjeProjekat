using System;
using System.Collections.Generic;
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
    public class LecenjeController : ControllerBase
    {
        public Context Context { get; set; }

        public LecenjeController(Context context)
        {
            Context = context;
        }
        [Route("DajBolovanja")]
        [HttpGet]
        public async Task<ActionResult> DajBolovanja()
        {
            try
            {
                return Ok(await Context.Lecenje.Select(p =>
                new
                {
                    ID = p.ID,
                    Pocetak = p.Pocetak,
                    Kraj = p.Kraj,
                    Pacijent = p.Pacijent,
                    Bolnica = p.Bolnica,
                    Lekar = p.Lekar,
                    Soba = p.SobaID
                }).ToListAsync());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Route("DodajBolovanje/{pacijentId}/{bolnicaId}/{lekarId}/{sobaId}")]
        [HttpPost]
        public async Task<ActionResult> DodajBolovanje(int pacijentId, int bolnicaId, int lekarId, int sobaId)
        {
            try
            {
                var bolnica = await Context.Bolnica.Include(p=>p.Lekari).Where(p => p.ID == bolnicaId).FirstOrDefaultAsync();
                var pacijent = await Context.Pacijent.Where(p => p.ID == pacijentId).FirstOrDefaultAsync();
                var lekar = await Context.Lekar.Where(p => p.ID == lekarId).FirstOrDefaultAsync();

                if (bolnica == null) return BadRequest("Nepostojeca bolnica.");
                if (pacijent == null) return BadRequest("Nepostojeci pacijent.");
                if (lekar == null) return BadRequest("Nepostojeci lekar.");
                if (!bolnica.Lekari.Contains(lekar)) return BadRequest("Nezaposljeni lekar.");
                if (sobaId < 0 || sobaId > bolnica.BrMesta) return BadRequest("Nepostojeca soba");
                var bolovanja = await Context.Lecenje.Where(p=>p.Bolnica == bolnica).Where(p=>p.Kraj==DateTime.MinValue).Where(p=>p.SobaID == sobaId).ToListAsync();
                if(bolovanja.Count>0) return BadRequest("Zauzeta soba.");
                
                bolovanja = await Context.Lecenje.Where(p=>p.Kraj==DateTime.MinValue).Where(p=>p.Pacijent == pacijent).ToListAsync();
                if(bolovanja.Count>0) return BadRequest("Vec boluje pacijent.");

                Lecenje bolovanje = new Lecenje();
                bolovanje.SobaID = sobaId;
                bolovanje.Pacijent = pacijent;
                bolovanje.Lekar = lekar;
                bolovanje.Bolnica = bolnica;
                bolovanje.Pocetak = DateTime.Now;
                Context.Lecenje.Add(bolovanje);
                await Context.SaveChangesAsync();
                return Ok("Bolovanje je otvoreno!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [Route("ZatvoriBolovanje/{lecenjeId}")]
        [HttpPut]
        public async Task<ActionResult> ZatvoriBolovanje(int lecenjeId){
                var bolovanje = await Context.Lecenje.Where(p => p.ID == lecenjeId).FirstOrDefaultAsync();
                if (bolovanje == null) return BadRequest("Nepostojece bolovanje.");
                bolovanje.Kraj = DateTime.Now;
                Context.Lecenje.Update(bolovanje);
                await Context.SaveChangesAsync();
                return Ok("Bolovanje je zatvoreno!");

        }
    }
}
