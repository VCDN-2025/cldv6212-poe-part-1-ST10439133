using ABCRetailPOE.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABCRetailPOE.Controllers
{
    public class ContractController : Controller
    {
        private readonly FileStorageService _fileStorageService;
        private const string ShareName = "contracts";
        private const string DirectoryName = "uploaded";

        public ContractController(FileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        public async Task<IActionResult> Index()
        {
            var files = await _fileStorageService.ListFilesAsync(ShareName, DirectoryName);
            return View(files);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile contract)
        {
            if (contract != null && contract.Length > 0)
            {
                using var stream = contract.OpenReadStream();
                await _fileStorageService.UploadFileAsync(ShareName, DirectoryName, contract.FileName, stream);
            }

            return RedirectToAction(nameof(Index));
        }

        // ✅ NEW Download feature
        public async Task<IActionResult> Download(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return BadRequest("File name is required.");

            var stream = await _fileStorageService.DownloadFileAsync(ShareName, DirectoryName, fileName);

            if (stream == null)
                return NotFound();

            return File(stream, "application/octet-stream", fileName);
        }
    }
}
