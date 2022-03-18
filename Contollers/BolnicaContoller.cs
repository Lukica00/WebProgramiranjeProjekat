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
                    Lekari = p.Lekari,
                    ZauzeteSobe = (Context.Lecenje.Where(q => q.Bolnica == p).Where(w => w.Kraj == DateTime.MinValue).Select(t => t.SobaID)).ToList()
                }).ToListAsync());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Route("DajLekaruBolnice/{idLekara}")]
        [HttpGet]
        public async Task<ActionResult> DajLekaruBolnice(int idLekara)//Dobar
        {
            try
            {
                var lekar = await Context.Lekar.Where(p => p.ID == idLekara).FirstOrDefaultAsync();
                if (lekar == null)
                {
                    return BadRequest("Nepostojeći lekar.");
                }
                return Ok(await Context.Bolnica.Where(p => p.Lekari.Contains(lekar)).Select(p =>
                new
                {
                    ID = p.ID,
                    Ime = p.Ime,
                    BrojSoba = (Context.Lecenje.Where(q => q.Bolnica == p).Where(w => w.Lekar == lekar).Where(t => t.Kraj == DateTime.MinValue).ToList().Count)
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
                if (ime.Length > 30) return BadRequest("Predugačko ime.");
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
                if (bolnica == null) return BadRequest("Nepostojeća bolnica.");
                if (lekar == null) return BadRequest("Nepostojeći lekar.");
                if (bolnica.Lekari.Contains(lekar)) return BadRequest("Već je zapošljen u ovoj bolnici.");

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
        public async Task<ActionResult> OtpustiLekara(int idBolnice, int idLekara)//Dobar
        {
            try
            {
                var bolnica = await Context.Bolnica.Include(p => p.Lekari).Where(p => p.ID == idBolnice).FirstOrDefaultAsync();
                var lekar = await Context.Lekar.Include(p => p.Bolnice).Where(p => p.ID == idLekara).FirstOrDefaultAsync();
                if (bolnica == null) return BadRequest("Nepostojeća bolnica.");
                if (lekar == null) return BadRequest("Nepostojeći lekar.");
                if (!bolnica.Lekari.Contains(lekar)) return BadRequest("Nije zapošljen lekar.");

                var lekari = await Context.Lekar.Include(p => p.Bolnice).Where(p => p.Bolnice.Contains(bolnica)).ToListAsync();
                if (lekari.Count <= 1) return BadRequest("Nemoguće otpustiti poslednjeg lekara.");
                Random rnd = new Random();
                var bolovanja = await Context.Lecenje.Where(p => p.Bolnica == bolnica).Where(p => p.Lekar == lekar).ToListAsync();
                lekari.Remove(lekar);
                foreach (Lecenje bolovanje in bolovanja)
                {
                    bolovanje.Lekar = lekari.ElementAt(rnd.Next(lekari.Count));
                    Context.Lecenje.Update(bolovanje);
                }
                bolnica.Lekari.Remove(lekar);
                Context.Bolnica.Update(bolnica);
                lekar.Bolnice.Remove(bolnica);
                Context.Lekar.Update(lekar);
                await Context.SaveChangesAsync();
                return Ok("Otpušten lekar!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Route("PreimenujBolnicu/{idBolnice}/{imeBolnice}")]
        [HttpPut]
        public async Task<ActionResult> PreimenujBolnicu(int idBolnice, string imeBolnice)//Dobar
        {
            try
            {
                var bolnica = await Context.Bolnica.Where(p => p.ID == idBolnice).FirstOrDefaultAsync();
                if (bolnica == null) return BadRequest("Nepostojeća bolnica.");
                if (imeBolnice.Length > 30) return BadRequest("Predugačko ime.");
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
        public async Task<ActionResult> ObrisiBolnicu(int idBolnice)//Dobar
        {
            try
            {
                var bolnica = await Context.Bolnica.Where(p => p.ID == idBolnice).FirstOrDefaultAsync();
                if (bolnica == null) return BadRequest("Nepostojeća bolnica.");
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
                    if (bolovanje.Kraj == DateTime.MinValue)
                        bolovanje.Kraj = DateTime.Now;
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
