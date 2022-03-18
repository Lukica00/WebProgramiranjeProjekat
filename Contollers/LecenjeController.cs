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
        [Route("DajBolovanja/{idPacijenta}")]
        [HttpGet]
        public async Task<ActionResult> DajBolovanja(int idPacijenta)//Dobar
        {
            try
            {
                var pacijent = await Context.Pacijent.Where(p => p.ID == idPacijenta).FirstOrDefaultAsync();
                if (pacijent == null)return BadRequest("Nepostojeći pacijent.");
                return Ok(await Context.Lecenje.Where(p => p.Pacijent == pacijent).Select(p =>
                  new
                  {
                      ID = p.ID,
                      Pocetak = p.Pocetak,
                      Kraj = p.Kraj,
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
        [Route("DajBolesnike/{idBolnice}/{idSobe}")]
        [HttpGet]
        public async Task<ActionResult> DajBolesnika(int idBolnice, int idSobe)//Dobar
        {
            try
            {
                var bolnica = await Context.Bolnica.Where(p => p.ID == idBolnice).FirstOrDefaultAsync();
                if (bolnica == null)return BadRequest("Nepostojeća bolnica.");
                return Ok(await Context.Lecenje.Where(p => p.Bolnica == bolnica).Where(p => p.SobaID == idSobe).Where(p => p.Kraj == DateTime.MinValue).Select(p =>
                          new
                          {
                              ID = p.ID,
                              Pocetak = p.Pocetak,
                              Kraj = p.Kraj,
                              Pacijent = p.Pacijent,
                              Lekar = p.Lekar
                          }).ToListAsync());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Route("DodajBolovanje/{pacijentId}/{bolnicaId}/{lekarId}/{sobaId}")]
        [HttpPost]
        public async Task<ActionResult> DodajBolovanje(int pacijentId, int bolnicaId, int lekarId, int sobaId)//Dobar
        {
            try
            {
                var bolnica = await Context.Bolnica.Include(p => p.Lekari).Where(p => p.ID == bolnicaId).FirstOrDefaultAsync();
                var pacijent = await Context.Pacijent.Where(p => p.ID == pacijentId).FirstOrDefaultAsync();
                var lekar = await Context.Lekar.Where(p => p.ID == lekarId).FirstOrDefaultAsync();

                if (bolnica == null) return BadRequest("Nepostojeća bolnica.");
                if (pacijent == null) return BadRequest("Nepostojeći pacijent.");
                if (lekar == null) return BadRequest("Nepostojeći lekar.");
                if (!bolnica.Lekari.Contains(lekar)) return BadRequest("Nezapošljeni lekar.");
                if (sobaId < 1 || sobaId > bolnica.BrMesta) return BadRequest("Nepostojeća soba.");
                var bolovanja = await Context.Lecenje.Where(p => p.Bolnica == bolnica).Where(p => p.Kraj == DateTime.MinValue).Where(p => p.SobaID == sobaId).ToListAsync();
                if (bolovanja.Count > 0) return BadRequest("Zauzeta soba.");

                bolovanja = await Context.Lecenje.Where(p => p.Kraj == DateTime.MinValue).Where(p => p.Pacijent == pacijent).ToListAsync();
                if (bolovanja.Count > 0) return BadRequest("Već boluje pacijent.");

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
        public async Task<ActionResult> ZatvoriBolovanje(int lecenjeId)//Dobar
        {
            var bolovanje = await Context.Lecenje.Where(p => p.ID == lecenjeId).FirstOrDefaultAsync();
            if (bolovanje == null) return BadRequest("Nepostojeće bolovanje.");
            bolovanje.Kraj = DateTime.Now;
            Context.Lecenje.Update(bolovanje);
            await Context.SaveChangesAsync();
            return Ok("Bolovanje je zatvoreno!");

        }
    }
}
