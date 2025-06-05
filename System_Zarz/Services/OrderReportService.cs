using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System_Zarz.Data;
using System.Net.Mail;
using System.Net;
using Task = System.Threading.Tasks.Task;

namespace System_Zarz.Services
{
    public class OrderReportService : BackgroundService
    {
        private readonly ILogger<OrderReportService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly string _adminEmail;
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;

        public OrderReportService(
            ILogger<OrderReportService> logger,
            IServiceProvider serviceProvider,
            IConfiguration configuration)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            
            _adminEmail = _configuration["EmailSettings:AdminEmail"] ?? "admin@example.com";
            _smtpServer = _configuration["EmailSettings:SmtpServer"] ?? "smtp.gmail.com";
            _smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
            _smtpUsername = _configuration["EmailSettings:SmtpUsername"] ?? "";
            _smtpPassword = _configuration["EmailSettings:SmtpPassword"] ?? "";
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Rozpoczynam generowanie raportu zleceń...");
                    
                    var pdfBytes = await GenerateOrderReportAsync();
                    await SendEmailWithAttachmentAsync(pdfBytes);
                    
                    _logger.LogInformation("Raport został wygenerowany i wysłany pomyślnie.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Wystąpił błąd podczas generowania raportu.");
                }

                await Task.Delay(TimeSpan.FromMinutes(60), stoppingToken);
            }
        }

        private async Task<byte[]> GenerateOrderReportAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var orders = await dbContext.Orders
                .Include(o => o.Vehicle)
                .ThenInclude(v => v.Customer)
                .Include(o => o.Tasks)
                .ThenInclude(t => t.Task)
                .Where(o => o.Status != "Zakończone")
                .ToListAsync();

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(container => ComposeContent(container, orders));
                    page.Footer().Element(ComposeFooter);
                });
            });

            using var stream = new MemoryStream();
            document.GeneratePdf(stream);
            return stream.ToArray();
        }

        private void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("Raport Aktualnych Zleceń").FontSize(20).Bold();
                    column.Item().Text($"Data wygenerowania: {DateTime.Now:dd.MM.yyyy HH:mm}").FontSize(12);
                });
            });
        }

        private void ComposeContent(IContainer container, List<Order> orders)
        {
            container.Column(column =>
            {
                foreach (var order in orders)
                {
                    column.Item().Element(container => ComposeOrderDetails(container, order));
                    column.Item().Element(container => ComposeOrderTasks(container, order));
                    column.Item().PaddingBottom(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                }
            });
        }

        private void ComposeOrderDetails(IContainer container, Order order)
        {
            container.Padding(10).Background(Colors.Grey.Lighten3).Column(column =>
            {
                column.Item().Text($"Zlecenie #{order.Id}").FontSize(14).Bold();
                column.Item().Text($"Status: {order.Status}");
                column.Item().Text($"Data utworzenia: {order.CreatedDate:dd.MM.yyyy}");
                column.Item().Text($"Pojazd: {order.Vehicle.Brand} {order.Vehicle.Model} ({order.Vehicle.RegistrationNumber})");
                column.Item().Text($"Klient: {order.Vehicle.Customer.FullName}");
            });
        }

        private void ComposeOrderTasks(IContainer container, Order order)
        {
            container.Padding(10).Column(column =>
            {
                column.Item().Text("Zadania:").FontSize(12).Bold();
                foreach (var task in order.Tasks)
                {
                    column.Item().Text($"- {task.Task.Description} (Koszt robocizny: {task.Task.LaborCost:C})");
                }
            });
        }

        private void ComposeFooter(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Text(text =>
                {
                    text.Span("Strona ").FontSize(10);
                    text.CurrentPageNumber().FontSize(10);
                    text.Span(" z ").FontSize(10);
                    text.TotalPages().FontSize(10);
                });
            });
        }

        private async Task SendEmailWithAttachmentAsync(byte[] pdfBytes)
        {
            using var message = new MailMessage();
            message.From = new MailAddress(_smtpUsername);
            message.To.Add(_adminEmail);
            message.Subject = $"Raport zleceń - {DateTime.Now:dd.MM.yyyy}";
            message.Body = "W załączniku znajduje się raport aktualnych zleceń.";

            using var ms = new MemoryStream(pdfBytes);
            message.Attachments.Add(new Attachment(ms, "open_orders.pdf", "application/pdf"));

            using var client = new SmtpClient(_smtpServer, _smtpPort)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword)
            };

            await client.SendMailAsync(message);
        }
    }
}
