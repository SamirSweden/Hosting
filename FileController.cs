using Microsoft.AspNetCore.Mvc;

namespace WebAppApiCs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : Controller
    {
        [HttpGet("GetFiles")]
        public IActionResult GetFiles()
        {
            var curPlace = Directory.GetCurrentDirectory();
            var files = Directory.GetFiles(curPlace);
            return Ok(files);
        }

        public readonly string pathFile = "logs.txt";

        [HttpGet("list")]
        public IActionResult GetFiles([FromQuery] string? folderPath = null)
        {
            string curPlace = folderPath ?? Directory.GetCurrentDirectory();

            if (!Directory.Exists(curPlace))
            {
                return NotFound($"folder {curPlace} not found");
            }

            var listOfFiles = Directory.GetFiles(curPlace);
            return Ok(listOfFiles);
        }

        [HttpPost("create")]
        public IActionResult PostFiles([FromQuery] string fileName, [FromBody] string content, [FromQuery] string? folderPath = null)
        {
            string curPlace = folderPath ?? Directory.GetCurrentDirectory(); // 
            if (!Directory.Exists(curPlace))
            {
                Directory.CreateDirectory(curPlace);
            }

            string fullPath = Path.Combine(curPlace, fileName);
            System.IO.File.WriteAllText(fullPath, content);

            return Ok(new { Message = "file created", Path = fullPath });
        }

        [HttpDelete("delete")]
        public IActionResult Delete([FromQuery] string fileName, [FromQuery] string? folderPath = null)
        {
            string curPlace = folderPath ?? Directory.GetCurrentDirectory();
            string fullPath = Path.Combine(curPlace, fileName);

            if (!Directory.Exists(fullPath))
            {
                return NotFound($"The file {fileName} not found");
            }

            return NoContent();
        }

    }
}
