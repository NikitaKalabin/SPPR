using Microsoft.AspNetCore.Mvc;

namespace WEB_253504_LIANHA.API.Controllers
{
    [Route("api/imageupload")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly string _imagePath;

        public FileController(IWebHostEnvironment webHostEnvironment)
        {
            _imagePath = Path.Combine(webHostEnvironment.WebRootPath, "Images");
        }

        [HttpPost]
        public async Task<IActionResult> SaveFile(IFormFile file)
        {
            if (file is null)
            {
                return BadRequest();
            }

            var filePath = Path.Combine(_imagePath, file.FileName);

            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }

            using var fileStream = fileInfo.Create();
            await file.CopyToAsync(fileStream);

            var host = HttpContext.Request.Host;
            var fileUrl = $"https://{host}/Images/{file.FileName}";
            return Ok(fileUrl);
        }

        [HttpDelete]
        public IActionResult DeleteFile(string fileName)
        {
            var filePath = Path.Combine(_imagePath, fileName);
            var fileInfo = new FileInfo(filePath);

            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }

            return Ok();
        }
    }
}