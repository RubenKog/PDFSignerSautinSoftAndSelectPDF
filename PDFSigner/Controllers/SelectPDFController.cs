using Microsoft.AspNetCore.Mvc;
using PDFSigner.Services;
using System.Text;

namespace PDFSigner.Controllers;
public class SelectPDFController : Controller
{
    private readonly ISelectPDFService _softService;
    public SelectPDFController(ISelectPDFService softService)
    {
        _softService = softService;
    }

    public IActionResult Index()
    {
        return View();
    }
    [HttpPost("UploadSelectPDF"), DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue, ValueLengthLimit = int.MaxValue)]
    public IActionResult UploadSelectPDF(IFormFile pdfFile, string newFileName, IFormFile certificateFile, string certPassword, IFormFile signatureFile)
    {
        if (pdfFile != null && pdfFile.Length > 0 && !string.IsNullOrEmpty(newFileName) && newFileName.Length > 0 && certificateFile != null && certificateFile.Length > 0 && !string.IsNullOrEmpty(certPassword) && certPassword.Length > 0 && pdfFile != null && signatureFile.Length > 0)
        {
            // Validate file type
            if (Path.GetExtension(pdfFile.FileName).ToLower() == ".pdf" && Path.GetExtension(certificateFile.FileName).ToLower() == ".pfx" && Path.GetExtension(signatureFile.FileName).ToLower() == ".png")
            {
                try
                {
                    string pdfFileName = newFileName + ".pdf";
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
                    _softService.SignPDF(pdfFilePath, signatureFilePath, @$"C:\Users\ruben\source\repos\Gradproef\PDFSigner\PDFSigner\OutputFiles\{pdfFileName}", certFilePath, "pxl");
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
    }
    [HttpPost]
    public IActionResult PdfSigner()
    {
        return View();
    }
}
