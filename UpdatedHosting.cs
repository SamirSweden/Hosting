using Microsoft.AspNetCore.Mvc;
using WebAppApiCs.Models;
using System.Collections.Generic;

namespace WebAppApiCs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HostingController : ControllerBase
    {
        private readonly HttpClient _client = new HttpClient();
        private static readonly List<Hosting> _hostings = new List<Hosting>();
        private static int _nextId = 1;
        
        
        [HttpGet]
        public ActionResult GetAll()
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
            string check = "https://";
            string portChecker = "use only 443 port";
            
            if (!hosting.Url.StartsWith(check))
                return BadRequest("use https:// , otherwise we dont except http://");
            else if (hosting.Port != 443)
                return BadRequest(portChecker);


            hosting.Id = _nextId;
            _hostings.Add(hosting);
            return CreatedAtAction(nameof(GetById) , new {id = hosting.Id} , hosting);
        }

        [HttpPut("{id}")]
        public ActionResult<Hosting> Update(int id , Hosting updateHosting)
        {
            var hosting = _hostings.FirstOrDefault(z => z.Id == id);
            if (hosting == null) return NotFound(); 
            hosting.Url = updateHosting.Url;
            hosting.Port = updateHosting.Port;
            hosting.Rps = updateHosting.Rps;


            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult<Hosting> Delete(int id)
        {
            var hosting = _hostings.FirstOrDefault(z => z.Id == id);
            if (hosting == null) return NotFound();

            _hostings.Remove(hosting);
            return NoContent();
        }

        
        // scanner 
        [HttpGet("{id}/status")]
        public async Task<ActionResult> GetStatus(int id)
        {
            var hosting = _hostings.FirstOrDefault(s => s.Id == id);
            if (hosting == null) return NotFound();

            try
            {
                var resp = await _client.GetAsync(hosting.Url);
                return Ok(resp.IsSuccessStatusCode ? "website works" : "website unavailable");
            }
            catch (Exception err)
            {
                return StatusCode(500, err.Message);
            }
        }
        
        
    }
}
