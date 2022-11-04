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
    [Authorize]
    public class SitesController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public SitesController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Sites
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Site>>> GetSites()
        {
            
            return await _context.Sites.ToListAsync();
        }

        // GET: api/Sites/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Site>> GetSite(int id)
        {
            var site = await _context.Sites.FindAsync(id);

            if (site == null)
            {
                return NotFound();
            }

            var SCollection = new ServiceCollection();

            //add protection services
            SCollection.AddDataProtection();
            var LockerKey = SCollection.BuildServiceProvider();

            // create an instance of classfile using 'CreateInstance' method
            var locker = ActivatorUtilities.CreateInstance<Security>(LockerKey);
            string deencryptKey = locker.Decrypt(site.SPassword);
            site.SPassword = deencryptKey;
            return site;
        }

        // PUT: api/Sites/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSite(int id, Site site)
        {
            if (id != site.SId)
            {
                return BadRequest();
            }
            var SCollection = new ServiceCollection();
            //add protection services
            SCollection.AddDataProtection();
            var LockerKey = SCollection.BuildServiceProvider();

            // create an instance of classfile using 'CreateInstance' method
            var locker = ActivatorUtilities.CreateInstance<Security>(LockerKey);
            string encryptKey = locker.Encrypt(site.SPassword);
            site.SPassword = encryptKey;

            _context.Entry(site).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SiteExists(id))
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

        // POST: api/Sites
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Site>> PostSite(Site site)
        {
            var SCollection = new ServiceCollection();

            //add protection services
            SCollection.AddDataProtection();
            var LockerKey = SCollection.BuildServiceProvider();

            // create an instance of classfile using 'CreateInstance' method
            var locker = ActivatorUtilities.CreateInstance<Security>(LockerKey);
            string encryptKey = locker.Encrypt(site.SPassword);
            site.SPassword = encryptKey;
            _context.Sites.Add(site);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSite", new { id = site.SId }, site);
        }

        // DELETE: api/Sites/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSite(int id)
        {
            var site = await _context.Sites.FindAsync(id);
            if (site == null)
            {
                return NotFound();
            }

            _context.Sites.Remove(site);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SiteExists(int id)
        {
            return _context.Sites.Any(e => e.SId == id);
        }
    }
}
