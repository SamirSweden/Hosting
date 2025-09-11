using Microsoft.AspNetCore.Mvc;
using WebAppApiCs.Models;

namespace WebAppApiCs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HostingController : ControllerBase
    {

        private static readonly List<Hosting> _hostings = new List<Hosting>();
        private static int _nextId = 1;

        [HttpGet]
        public ActionResult<Hosting> GetAll()
        {
            return Ok(_hostings); 
        }

        [HttpGet("{id}")]
        public ActionResult<Hosting> GetById(int id)
        {
            var hosting = _hostings.FirstOrDefault(z => z.Id == id);
            if (hosting == null) return NotFound(); 
            return Ok(hosting); 
        }

        [HttpPost]
        public ActionResult<Hosting> Create(Hosting hosting)
        {
            hosting.Id = _nextId++;
            _hostings.Add(hosting);
            return CreatedAtAction(nameof(GetById), new { id = hosting.Id }, hosting);
        }

        [HttpPut("{id}")]
        public ActionResult<Hosting> Update(int id,Hosting updatedHosting)
        {
            var hosting = _hostings.FirstOrDefault(s => s.Id == id);
            if (hosting == null) return NotFound();

            hosting.Url = updatedHosting.Url;
            hosting.Port = updatedHosting.Port;
            hosting.Rps = updatedHosting.Rps;
            
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult<Hosting> Delete(int id)
        {
            var hosting = _hostings.FirstOrDefault(s => s.Id == id);
            if (hosting == null) return NotFound();
            _hostings.Remove(hosting);
            
            return NoContent();
        }
    }
}
