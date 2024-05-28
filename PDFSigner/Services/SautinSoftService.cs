using SautinSoft.Document;
using SautinSoft.Document.Drawing;

namespace PDFSigner.Services;


public interface ISautinSoftService
{
    public void DigitalSignature(string pdfPath, string signaturePath, string resultPath, string certificatePath, string certificatePassword);
}

//Based on example code from
//https://sautinsoft.com/products/document/help/net/developer-guide/digital-signature-net-csharp-vb.php
public class SautinSoftService : ISautinSoftService
{
    
    public void DigitalSignature(string pdfPath, string signaturePath, string resultPath, string certificatePath, string certificatePassword)
    {

        string getPdfPath = pdfPath;
        string savePath = resultPath;

        DocumentCore documentCore = DocumentCore.Load(pdfPath);


        Shape signatureShape = new Shape(documentCore, Layout.Floating(new HorizontalPosition
            (0f, LengthUnit.Millimeter, HorizontalPositionAnchor.LeftMargin)
            ,new VerticalPosition(0f, LengthUnit.Millimeter, VerticalPositionAnchor.TopMargin), new Size(1, 1)));
            ((FloatingLayout)signatureShape.Layout).WrappingStyle = WrappingStyle.InFrontOfText;
            signatureShape.Outline.Fill.SetEmpty();

        Paragraph firstParagraph = documentCore.GetChildElements(true).OfType<Paragraph>().FirstOrDefault();
        firstParagraph.Inlines.Add(signatureShape);

        Picture signaturePicture = new Picture(documentCore, signaturePath);

        signaturePicture.Layout = Layout.Floating(
              new HorizontalPosition(4.5, LengthUnit.Centimeter, HorizontalPositionAnchor.Page),
              new VerticalPosition(14.5, LengthUnit.Centimeter, VerticalPositionAnchor.Page),
              new Size(20, 10, LengthUnit.Millimeter));
        PdfSaveOptions options = GetPdfSaveOptions(signaturePicture, signatureShape, certificatePath, certificatePassword);
        documentCore.Save(savePath, options);
    }

    private PdfSaveOptions GetPdfSaveOptions(Picture signaturePicture, Shape signatureShape, string certificatePath, string certificatePassword )
    {
        PdfSaveOptions options = new PdfSaveOptions();


        options.DigitalSignature.CertificatePath = certificatePath;
        options.DigitalSignature.CertificatePassword = certificatePassword;
        options.DigitalSignature.Signature = signaturePicture;
        options.DigitalSignature.SignatureLine = signatureShape;
        options.DigitalSignature.Reason = "A demonstration for this graduate thesis.";
        options.DigitalSignature.Location = "Ruben's personal laptop.";
        options.DigitalSignature.ContactInfo = "ruben.kog@student.pxl.be"; 
        return options;
    }

}
