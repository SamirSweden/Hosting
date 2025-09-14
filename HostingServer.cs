using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebAppApiCs.Models;
using System.Collections.Generic;

namespace WebAppApiCs.Controllers
{
   [ApiController]
   [Route("api/[controller]")]

   public class HostingController : ControllerBase
   {
      private readonly string filePath = "urls.json";


      [HttpPost]
      public async Task<IActionResult> SaveUrl([FromBody] UrlRequest request)
      {
         try
         {
            if (string.IsNullOrEmpty(request.Url) || !Uri.IsWellFormedUriString(request.Url , UriKind.Absolute))
            {
               return BadRequest("Некорректный URL");
            }

            List<string> urls = new List<string>();
            if (System.IO.File.Exists(filePath))
            {
               var json =  await System.IO.File.ReadAllTextAsync(filePath);
               urls = JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
            }

            if (!urls.Contains(request.Url))
            {
               urls.Add(request.Url);
               var options = new JsonSerializerOptions{WriteIndented = true};
               var newJson = JsonSerializer.Serialize(urls, options);
               await System.IO.File.WriteAllTextAsync(filePath, newJson);
            }
            
            return Ok(new {message = "url successfully registered "});
         }
         catch (Exception err)
         {
            return StatusCode(500, $" server error {err.Message}");
         }
      }

      [HttpGet]
      public async Task<IActionResult> Get()
      {
         try
         {
            if (System.IO.File.Exists(filePath))
            {
               return Ok(new List<string>());
            }
            var json = await System.IO.File.ReadAllTextAsync(filePath);
            var urls = JsonSerializer.Deserialize<List<string>>(json);
            return Ok(urls);
         }
         catch (Exception e)
         {
            return StatusCode(500, $" server error {e.Message}");
         }
      }
   }



   public class UrlRequest
   {
      public string Url { get; set; } = string.Empty;
   }
   
}
