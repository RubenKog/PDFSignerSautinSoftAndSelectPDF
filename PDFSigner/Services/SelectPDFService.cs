using Microsoft.AspNetCore.Hosting.Server;
using SelectPdf;

namespace PDFSigner.Services;
public interface ISelectPDFService 
{

    public void DigitalSignature(string pdfPath, string signaturePath, string resultPath, string certificatePath, string certificatePassword);



}
public class SelectPDFService : ISelectPDFService
{
    public void DigitalSignature(string pdfPath, string signaturePath, string resultPath, string certificatePath, string certificatePassword)
    {
        // create a new pdf document
        PdfDocument doc = new PdfDocument(pdfPath);

        // add a new page to the document
        PdfPage page = doc.Pages[0];

        // get image path 
        // the image will be used to display the digital signature over it
        string imgFile = signaturePath;

        // get certificate path
        string certFile = certificatePath;

        // define a rendering result object
        PdfRenderingResult result;

        // create image element from file path 
        PdfImageElement img = new PdfImageElement(0, 0, imgFile);
        result = page.Add(img);

        // get the #PKCS12 certificate from file
        PdfDigitalCertificatesCollection certificates =
            PdfDigitalCertificatesStore.GetCertificates(certFile, certificatePassword);
        PdfDigitalCertificate certificate = certificates[0];

        // create the digital signature object
        PdfDigitalSignatureElement signature =
            new PdfDigitalSignatureElement(result.PdfPageLastRectangle, certificate);
        signature.Reason = "SelectPdf testing";
        signature.ContactInfo = "Graduaatsproef example";
        signature.Location = "België";
        page.Add(signature);

        // save pdf document
        doc.Save(resultPath);

        // close pdf document
        doc.Close();
    }


}
