using Microsoft.AspNetCore.Mvc;
using PDFSigner.Models;
using System.Diagnostics;
using System.Text;

namespace PDFSigner.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


    /*[HttpPost("upload"), DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue, ValueLengthLimit = int.MaxValue)]
    public IActionResult Upload(IFormFile pdfFile, string newFileName, IFormFile certificateFile, string certPassword, IFormFile signatureFile)
    {
        if (pdfFile != null && pdfFile.Length > 0 && !string.IsNullOrEmpty(newFileName) && newFileName.Length > 0 && certificateFile != null && certificateFile.Length > 0 && !string.IsNullOrEmpty(certPassword) && certPassword.Length > 0 && pdfFile != null && signatureFile.Length > 0)
        {
            // Validate file type
            if (true *//*Path.GetExtension(pdfFile.FileName).ToLower() == ".pdf" && Path.GetExtension(certificateFile.FileName).ToLower() == ".pfx" && Path.GetExtension(signatureFile.FileName).ToLower() == ".png"*//*)
            {
                try
                {
                    string pdfFileName = "Copy" + newFileName;
                    string pdfFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PdfFiles", pdfFileName);
                    string certFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PdfFiles", certificateFile.FileName);
                    string signatureFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PdfFiles", signatureFile.FileName);
                    using (var stream = new FileStream(pdfFilePath, FileMode.Create))
                    {
                        pdfFile.CopyTo(stream);
                    }
                    using (var stream = new FileStream(certFilePath, FileMode.Create))
                    {
                        certificateFile.CopyTo(stream);
                    }
                    using (var stream = new FileStream(signatureFilePath, FileMode.Create))
                    {
                        signatureFile.CopyTo(stream);
                    }

                    ViewBag.Message = "File uploaded successfully.";
                }
                catch
                {
                    ViewBag.Message = "Error uploading file.";
                }
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Please ensure the following files are correctly uploaded and have the right extension:");
                if (Path.GetExtension(pdfFile.FileName).ToLower() != ".pdf")
                {
                    sb.AppendLine(".pdf file");
                }
                if (Path.GetExtension(certificateFile.FileName).ToLower() != ".pfx")
                {
                    sb.AppendLine(".pfx certificate");
                }
                if (Path.GetExtension(signatureFile.FileName).ToLower() != ".png")
                {
                    sb.AppendLine(".png image");
                }
                ViewBag.Message = sb.ToString();
            }
        }
        else
        {
            ViewBag.Message = "Please ensure all needed files are uploaded!";
        }

        return View("Index");
    }*/

}
