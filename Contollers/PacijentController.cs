using System;
using System.Text.RegularExpressions;
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

        [Route("DajPacijente")]
        [HttpGet]
        public async Task<ActionResult> DajPacijente()//Dobar
        {
            try
            {
                return Ok(await Context.Pacijent.Select(p =>
                new
                {
                    ID = p.ID,
                    Ime = p.Ime,
                    Prezime = p.Prezime,
                    JMBG = p.JMBG
                }).ToListAsync());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Route("DajZdravePacijente")]
        [HttpGet]
        public async Task<ActionResult> DajZdravePacijente()//Dobar
        {
            try
            {
                return Ok(await Context.Pacijent.Where(p => !(Context.Lecenje.Where(w => w.Kraj == DateTime.MinValue).Select(q => q.Pacijent).ToList()).Contains(p)).ToListAsync());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Route("DodajPacijenta/{ime}/{prezime}/{JMBG}")]
        [HttpPost]
        public async Task<ActionResult> DodajPacijenta(string ime, string prezime, string JMBG)//Dobar
        {
            try
            {

                var rgx = new Regex("^[0-9]+$");
                if (!rgx.IsMatch(JMBG) || JMBG.Length != 13) return BadRequest("Neispravan JMBG.");
                rgx = new Regex(@"^\p{L}+$");
                if (ime.Length > 20 || ime.Length < 3 || (!rgx.IsMatch(ime))) return BadRequest("Neispravno ime.");
                if (prezime.Length > 20 || prezime.Length < 3 || (!rgx.IsMatch(prezime))) return BadRequest("Neispravno prezime.");
                if (Context.Pacijent.Where(p => p.JMBG == JMBG).ToList().Count != 0) return BadRequest("Već postoji pacijent sa tim JMBG.");
                var pacijent = new Pacijent();
                pacijent.Ime = ime;
                pacijent.Prezime = prezime;
                pacijent.JMBG = JMBG;
                Context.Pacijent.Add(pacijent);
                await Context.SaveChangesAsync();
                return Ok("Pacijent je dodat!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Route("ObrisiPacijenta/{id}")]
        [HttpDelete]
        public async Task<ActionResult> ObrisiPacijenta(int id)//Dobar
        {
            try
            {
                var pacijent = await Context.Pacijent.Where(p => p.ID == id).FirstOrDefaultAsync();
                if (pacijent == null) return BadRequest("Pacijent ne postoji.");
                var bolovanja = await Context.Lecenje.Where(p => p.Pacijent == pacijent).ToListAsync();
                foreach (Lecenje bolovanje in bolovanja)
                {
                    Context.Lecenje.Remove(bolovanje);
                }
                Context.Pacijent.Remove(pacijent);
                await Context.SaveChangesAsync();
                return Ok("Pacijent je obrisan!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}