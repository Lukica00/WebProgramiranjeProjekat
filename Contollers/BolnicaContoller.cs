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
    public class BolnicaController : ControllerBase
    {
        public Context Context { get; set; }

        public BolnicaController(Context context)
        {
            Context = context;
        }
        [Route("DajBolnice")]
        [HttpGet]
        public async Task<ActionResult> DajBolnice()//Dobar
        {
            try
            {
                return Ok(await Context.Bolnica.Select(p =>
                new
                {
                    ID = p.ID,
                    Ime = p.Ime,
                    BrojMesta = p.BrMesta,
                    Lekari = p.Lekari
                }).ToListAsync());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Route("DodajBolnicu/{ime}/{brojMesta}")]
        [HttpPost]
        public async Task<ActionResult> DodajBolnicu(string ime, int brojMesta) //Dobar
        {
            try
            {
                if (ime.Length > 30) return BadRequest("Predugacko ime.");
                if (brojMesta > 20 || brojMesta < 6) return BadRequest("Neodgovarajuci broj mesta.");
                Bolnica bolnica = new Bolnica();
                bolnica.Ime = ime;
                bolnica.BrMesta = brojMesta;
                bolnica.Lekari = new List<Lekar>();
                Context.Bolnica.Add(bolnica);
                await Context.SaveChangesAsync();
                return Ok("Bolnica je dodata!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Route("ZaposliLekara/{idBolnice}/{idLekara}")]
        [HttpPost]
        public async Task<ActionResult> ZaposliLekara(int idBolnice, int idLekara)//Dobar
        {
            try
            {
                var bolnica = await Context.Bolnica.Include(p => p.Lekari).Where(p => p.ID == idBolnice).FirstOrDefaultAsync();
                var lekar = await Context.Lekar.Include(p => p.Bolnice).Where(p => p.ID == idLekara).FirstOrDefaultAsync();
                if (bolnica == null)
                {
                    return BadRequest("Nepostojeca bolnica.");
                }
                if (lekar == null)
                {
                    return BadRequest("Nepostojeci lekar.");
                }
                if (bolnica.Lekari.Contains(lekar))
                {
                    return BadRequest("Vec je zaposljen u ovoj bolnici.");
                }
                bolnica.Lekari.Add(lekar);
                Context.Bolnica.Update(bolnica);
                lekar.Bolnice.Add(bolnica);
                Context.Lekar.Update(lekar);
                await Context.SaveChangesAsync();
                return Ok("Lekar je dodat!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Route("OtpustiLekara/{idBolnice}/{idLekara}")]
        [HttpPut]
        public async Task<ActionResult> OtpustiLekara(int idBolnice, int idLekara)//dobar
        {
            try
            {
                var bolnica = await Context.Bolnica.Include(p => p.Lekari).Where(p => p.ID == idBolnice).FirstOrDefaultAsync();
                var lekar = await Context.Lekar.Include(p => p.Bolnice).Where(p => p.ID == idLekara).FirstOrDefaultAsync();
                if (bolnica == null)
                {
                    return BadRequest("Nepostojeca bolnica.");
                }
                if (lekar == null)
                {
                    return BadRequest("Nepostojeci lekar.");
                }
                if (!bolnica.Lekari.Contains(lekar))
                {
                    return BadRequest("Nije zaposljen lekar.");
                }
                var lekari = await Context.Lekar.Include(p => p.Bolnice).Where(p => p.Bolnice.Contains(bolnica)).ToListAsync();
                if (lekari.Count <= 1) return BadRequest("Nemoguce otpustiti poslednjeg lekara.");
                Random rnd = new Random();
                var bolovanja = await Context.Lecenje.Where(p => p.Bolnica == bolnica).Where(p => p.Lekar == lekar).ToListAsync();
                foreach (Lecenje bolovanje in bolovanja)
                {
                    bolovanje.Lekar = lekari.ElementAt(rnd.Next(bolovanja.Count));
                    Context.Lecenje.Update(bolovanje);
                }
                bolnica.Lekari.Remove(lekar);
                Context.Bolnica.Update(bolnica);
                lekar.Bolnice.Remove(bolnica);
                Context.Lekar.Update(lekar);
                await Context.SaveChangesAsync();
                return Ok("Otpusten lekar!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Route("PreimenujBolnicu/{idBolnice}/{imeBolnice}")]
        [HttpPut]
        public async Task<ActionResult> PreimenujBolnicu(int idBolnice, string imeBolnice)//dobar
        {
            try
            {
                var bolnica = await Context.Bolnica/*.Include(p=>p.Lekari)*/.Where(p => p.ID == idBolnice).FirstOrDefaultAsync();
                if (bolnica == null)
                {
                    return BadRequest("Nepostojeca bolnica.");
                }
                if (imeBolnice.Length > 30) return BadRequest("Predugacko ime.");
                bolnica.Ime = imeBolnice;
                Context.Bolnica.Update(bolnica);
                await Context.SaveChangesAsync();
                return Ok("Preimenovana bolnica!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Route("ObrisiBolnicu/{idBolnice}")]
        [HttpDelete]
        public async Task<ActionResult> ObrisiBolnicu(int idBolnice)
        {
            try
            {
                var bolnica = await Context.Bolnica/*.Include(p=>p.Lekari)*/.Where(p => p.ID == idBolnice).FirstOrDefaultAsync();
                if (bolnica == null)
                {
                    return BadRequest("Nepostojeca bolnica.");
                }
                var lekari = await Context.Lekar.Include(p => p.Bolnice).Where(p => p.Bolnice.Contains(bolnica)).ToListAsync();
                foreach (Lekar lekar in lekari)
                {
                    lekar.Bolnice.Remove(bolnica);
                    Context.Lekar.Update(lekar);
                }
                var bolovanja = await Context.Lecenje.Where(p => p.Bolnica == bolnica).ToListAsync();
                foreach (Lecenje bolovanje in bolovanja)
                {
                    bolovanje.Bolnica = null;
                    Context.Lecenje.Update(bolovanje);
                }
                Context.Bolnica.Remove(bolnica);
                await Context.SaveChangesAsync();
                return Ok("Obrisana bolnica!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
