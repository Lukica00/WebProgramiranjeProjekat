using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
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
        [Route("DajLekare")]
        [HttpGet]
        public async Task<ActionResult> DajLekare()//Dobar
        {
            try
            {
                return Ok(await Context.Lekar.Select(p =>
                new
                {
                    ID = p.ID,
                    Ime = p.Ime,
                    Prezime = p.Prezime,
                    Bolnice = p.Bolnice.Select(t => t.ID).ToList(),
                    Pacijenti = Context.Lecenje.Where(q => q.Lekar == p).Where(w => w.Kraj == DateTime.MinValue).Select(t => t.ID).ToList()
                }).ToListAsync());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Route("DajNezaposljeneLekare/{idBolnice}")]
        [HttpGet]
        public async Task<ActionResult> DajNezaposljeneLekare(int idBolnice)//Dobar
        {
            try
            {
                var bolnica = await Context.Bolnica.Where(p => p.ID == idBolnice).FirstOrDefaultAsync();
                if (bolnica == null) return BadRequest("Nepostojeća bolnica.");
                return Ok(await Context.Lekar.Where(q => !q.Bolnice.Contains(bolnica)).Select(p =>
                  new
                  {
                      ID = p.ID,
                      Ime = p.Ime,
                      Prezime = p.Prezime
                  }).ToListAsync());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Route("DajZaposljeneLekare/{idBolnice}")]
        [HttpGet]
        public async Task<ActionResult> DajZaposljeneLekare(int idBolnice)//Dobar
        {
            try
            {
                var bolnica = await Context.Bolnica.Where(p => p.ID == idBolnice).FirstOrDefaultAsync();
                if (bolnica == null) return BadRequest("Nepostojeća bolnica.");
                return Ok(await Context.Lekar.Where(q => q.Bolnice.Contains(bolnica)).Select(p =>
                  new
                  {
                      ID = p.ID,
                      Ime = p.Ime,
                      Prezime = p.Prezime,
                      Pacijenti = (Context.Lecenje.Where(q => q.Lekar == p).Where(w => w.Bolnica == bolnica).Where(t => t.Kraj == DateTime.MinValue).ToList()).Count
                  }).ToListAsync());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Route("DodajLekara/{ime}/{prezime}")]
        [HttpPost]
        public async Task<ActionResult> DodajLekara(string ime, string prezime)//Dobar
        {
            try
            {
                var rgx = new Regex(@"^\p{L}+$");
                if (ime.Length > 20 || ime.Length < 3 || (!rgx.IsMatch(ime))) return BadRequest("Neispravno ime.");
                if (prezime.Length > 20 || prezime.Length < 3 || (!rgx.IsMatch(prezime))) return BadRequest("Neispravno prezime.");
                Lekar lekar = new Lekar();
                lekar.Ime = ime;
                lekar.Prezime = prezime;
                lekar.Bolnice = new List<Bolnica>();
                Context.Lekar.Add(lekar);
                await Context.SaveChangesAsync();
                return Ok("Lekar je dodat!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
