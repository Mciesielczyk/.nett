using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;
using System.Threading.Tasks;
using System_Zarz.Data;
using Task = System.Threading.Tasks.Task;

public class EmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> options)
    {
        _settings = options.Value;
    }

    public async Task SendEmailWithPdfAsync()
    {
        var message = new MailMessage
        {
            From = new MailAddress(_settings.SmtpUsername),
            Subject = "Testowy PDF",
            Body = "Załączam raport PDF wygenerowany QuestPDF.",
            IsBodyHtml = false
        };

        message.To.Add(_settings.AdminEmail);

        var pdfBytes = GeneratePdfWithQuest();

        var attachment = new Attachment(new MemoryStream(pdfBytes), "raport.pdf", "application/pdf");
        message.Attachments.Add(attachment);

        using var client = new SmtpClient(_settings.SmtpServer, _settings.SmtpPort)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(_settings.SmtpUsername, _settings.SmtpPassword)
        };

        await client.SendMailAsync(message);
    }

    private byte[] GeneratePdfWithQuest()
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.Content()
                    .Text("To jest przykładowy prosty raport PDF generowany QuestPDF.")
                    .FontSize(20)
                    .SemiBold()
                    .AlignCenter();
            });
        });

        return document.GeneratePdf();
    }
}