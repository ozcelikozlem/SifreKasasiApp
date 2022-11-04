using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SifreKasasi.API.Data;
using SifreKasasi.API.Models;

namespace SifreKasasi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class KayitsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public KayitsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Kayits
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Kayit>>> GetKayits()
        {
            return await _context.Kayits.ToListAsync();
        }

        // GET: api/Kayits/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Kayit>> GetKayit(int id)
        {
            var kayit = await _context.Kayits.FindAsync(id);

            if (kayit == null)
            {
                return NotFound();
            }

            return kayit;
        }

        // PUT: api/Kayits/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKayit(int id, Kayit kayit)
        {
            if (id != kayit.Id)
            {
                return BadRequest();
            }

            
            _context.Entry(kayit).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KayitExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Kayits
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Kayit>> PostKayit(Kayit kayit)
        {
            
            _context.Kayits.Add(kayit);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetKayit", new { id = kayit.Id }, kayit);
        }

        // DELETE: api/Kayits/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKayit(int id)
        {
            var kayit = await _context.Kayits.FindAsync(id);
            if (kayit == null)
            {
                return NotFound();
            }

            _context.Kayits.Remove(kayit);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool KayitExists(int id)
        {
            return _context.Kayits.Any(e => e.Id == id);
        }
    }
}
