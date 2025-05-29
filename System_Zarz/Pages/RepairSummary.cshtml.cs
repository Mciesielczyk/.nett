using System.Globalization;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System_Zarz.Data;
using Task = System.Threading.Tasks.Task;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Helpers;
using Document = QuestPDF.Fluent.Document;
public class RepairSummaryModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public RepairSummaryModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public int? SelectedMonth { get; set; }

    [BindProperty]
    public int? SelectedYear { get; set; }

    public List<RepairSummaryItem> ReportItems { get; set; } = new();

    public async Task<IActionResult> OnPostAsync()
    {
        if (!SelectedMonth.HasValue || !SelectedYear.HasValue)
        {
            ModelState.AddModelError("", "Wybierz miesi¹c i rok.");
            return Page();
        }

        var orders = await _context.Orders
            .Include(o => o.Vehicle).ThenInclude(v => v.Customer)
            .Include(o => o.Tasks).ThenInclude(ot => ot.Task).ThenInclude(t => t.TaskParts).ThenInclude(tp => tp.Part)
            .Where(o => o.CreatedDate.Month == SelectedMonth && o.CreatedDate.Year == SelectedYear)
            .ToListAsync();

        Console.WriteLine($"Znaleziono {orders.Count} zleceñ.");
        foreach (var o in orders)
        {
            Console.WriteLine($"Zlecenie: {o.Id}, Data: {o.CreatedDate}, Pojazd: {o.Vehicle?.RegistrationNumber}");
        }
        ReportItems = orders
            .GroupBy(o => new { o.Vehicle.Customer.FullName, o.Vehicle.RegistrationNumber, o.Vehicle.Brand, o.Vehicle.Model })
            .Select(g => new RepairSummaryItem
            {
                Customer = g.Key.FullName,
                Vehicle = $"{g.Key.Brand} {g.Key.Model} ({g.Key.RegistrationNumber})",
                TotalCost = g.Sum(o =>
                    o.Tasks.Sum(ot =>
                        (ot.Task?.LaborCost ?? 0) +
                        (ot.Task?.TaskParts?.Sum(tp => (tp.Part?.Price ?? 0) * tp.Quantity) ?? 0))),
                OrderCount = g.Count()
            })
            .ToList();

        return Page();
    }

    public async Task<IActionResult> OnPostGeneratePdfAsync()
    {
        // SprawdŸ czy wybrano miesi¹c i rok
        if (!SelectedMonth.HasValue || !SelectedYear.HasValue)
        {
            ModelState.AddModelError("", "Wybierz miesi¹c i rok.");
            return Page();
        }

        // Za³aduj dane tak samo jak w OnPostAsync()
        var orders = await _context.Orders
            .Include(o => o.Vehicle).ThenInclude(v => v.Customer)
            .Include(o => o.Tasks).ThenInclude(ot => ot.Task).ThenInclude(t => t.TaskParts).ThenInclude(tp => tp.Part)
            .Where(o => o.CreatedDate.Month == SelectedMonth && o.CreatedDate.Year == SelectedYear)
            .ToListAsync();

        // Przygotuj dane do raportu
        var reportItems = orders
            .GroupBy(o => new { o.Vehicle.Customer.FullName, o.Vehicle.RegistrationNumber, o.Vehicle.Brand, o.Vehicle.Model })
            .Select(g => new RepairSummaryItem
            {
                Customer = g.Key.FullName,
                Vehicle = $"{g.Key.Brand} {g.Key.Model} ({g.Key.RegistrationNumber})",
                TotalCost = g.Sum(o =>
                    o.Tasks.Sum(ot =>
                        (ot.Task?.LaborCost ?? 0) +
                        (ot.Task?.TaskParts?.Sum(tp => (tp.Part?.Price ?? 0) * tp.Quantity) ?? 0))),
                OrderCount = g.Count()
            })
            .ToList();

        // Generuj PDF
        var stream = new MemoryStream();
        var doc = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);

                // Nag³ówek
                page.Header().Text($"Raport napraw - podsumowanie ({CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(SelectedMonth.Value)} {SelectedYear})")
                    .FontSize(18).Bold();

                // Zawartoœæ
                page.Content().Table(table =>
                {
                    // Definicja kolumn
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3); // Klient
                        columns.RelativeColumn(3); // Pojazd
                        columns.RelativeColumn(2); // Koszt
                        columns.RelativeColumn(1); // Zlecenia
                    });

                    // Nag³ówki tabeli
                    table.Header(header =>
                    {
                        header.Cell().Text("Klient").Bold();
                        header.Cell().Text("Pojazd").Bold();
                        header.Cell().Text("Koszt").Bold();
                        header.Cell().Text("Zlecenia").Bold();
                    });

                    // Wiersze z danymi
                    foreach (var item in reportItems)
                    {
                        table.Cell().Text(item.Customer);
                        table.Cell().Text(item.Vehicle);
                        table.Cell().Text(item.TotalCost.ToString("C", new CultureInfo("pl-PL")));
                        table.Cell().Text(item.OrderCount.ToString());
                    }

                    // Wiersz podsumowania
                    if (reportItems.Any())
                    {
                        table.Cell().ColumnSpan(2).Text("SUMA").Bold();
                        table.Cell().Text(reportItems.Sum(i => i.TotalCost).ToString("C", new CultureInfo("pl-PL"))).Bold();
                        table.Cell().Text(reportItems.Sum(i => i.OrderCount).ToString()).Bold();
                    }
                });
            });
        });

        doc.GeneratePdf(stream);
        stream.Position = 0;

        // Zwróæ plik PDF
        string fileName = $"raport_napraw_{SelectedMonth}_{SelectedYear}.pdf";
        return File(stream.ToArray(), "application/pdf", fileName);
    }

    public class RepairSummaryItem
    {
        public string Customer { get; set; }
        public string Vehicle { get; set; }
        public decimal TotalCost { get; set; }
        public int OrderCount { get; set; }
    }
}
