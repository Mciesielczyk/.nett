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

public class RepairCostsModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public RepairCostsModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty] public int? SelectedCustomerId { get; set; }
    [BindProperty] public int? SelectedVehicleId { get; set; }
    [BindProperty] public int? SelectedMonth { get; set; }
    [BindProperty] public int? SelectedYear { get; set; }

    public List<Customer> Customers { get; set; } = new();
    public List<Vehicle> Vehicles { get; set; } = new();
    public List<RepairReportItem> ReportItems { get; set; } = new();

    public decimal TotalLaborCost => ReportItems.Sum(r => r.LaborCost);
    public decimal TotalPartsCost => ReportItems.Sum(r => r.PartsCost);
    public decimal TotalCost => TotalLaborCost + TotalPartsCost;

    public async Task OnGetAsync()
    {
        Customers = await _context.Customers.ToListAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Customers = await _context.Customers.ToListAsync();

        if (SelectedCustomerId.HasValue)
        {
            Vehicles = await _context.Vehicles
                .Where(v => v.CustomerId == SelectedCustomerId)
                .ToListAsync();
        }

        var query = _context.Orders
            .Include(o => o.Vehicle)
            .ThenInclude(v => v.Customer)
            .Include(o => o.Tasks)
                .ThenInclude(ot => ot.Task)
                    .ThenInclude(t => t.TaskParts)
                        .ThenInclude(tp => tp.Part)
            .AsQueryable();

        if (SelectedCustomerId.HasValue)
            query = query.Where(o => o.Vehicle.CustomerId == SelectedCustomerId);

        if (SelectedVehicleId.HasValue)
            query = query.Where(o => o.VehicleId == SelectedVehicleId);

        if (SelectedMonth.HasValue && SelectedYear.HasValue)
            query = query.Where(o => o.CreatedDate.Month == SelectedMonth && o.CreatedDate.Year == SelectedYear);

        var orders = await query.ToListAsync();

        ReportItems = orders.Select(o => new RepairReportItem
        {
            OrderDate = o.CreatedDate,
            VehicleInfo = $"{o.Vehicle.Brand} {o.Vehicle.Model} ({o.Vehicle.RegistrationNumber})",
            Description = o.Description,
            LaborCost = o.Tasks.Sum(ot => ot.Task?.LaborCost ?? 0),
            PartsCost = o.Tasks.Sum(ot => ot.Task?.TaskParts?.Sum(tp => (tp.Part?.Price ?? 0) * tp.Quantity) ?? 0)
        }).ToList();

        return Page();
    }

    
    public async Task<IActionResult> OnPostGeneratePdfAsync(int customerId, int? selectedVehicleId, int? selectedMonth)
{
    // załaduj dane jak w OnPost()
    var customer = await _context.Customers
        .Include(c => c.Vehicles)
        .ThenInclude(v => v.Orders)
        .ThenInclude(o => o.OrderTasks)
        .ThenInclude(ot => ot.Task)
        .ThenInclude(t => t.TaskParts)
        .ThenInclude(tp => tp.Part)
        .FirstOrDefaultAsync(c => c.Id == customerId);

    if (customer == null)
    {
        return NotFound();
    }

    // filtruj tak samo jak wcześniej
    var orders = customer.Vehicles
        .Where(v => !selectedVehicleId.HasValue || v.Id == selectedVehicleId)
        .SelectMany(v => v.Orders)
        .Where(o => !selectedMonth.HasValue || o.CreatedDate.Month == selectedMonth)
        .ToList();

    var reportItems = orders.Select(o =>
    {
        var laborCost = o.Tasks.Sum(ot => ot.Task.LaborCost);
        var partsCost = o.Tasks.Sum(ot => ot.Task?.TaskParts?.Sum(tp => (tp.Part?.Price ?? 0) * tp.Quantity) ?? 0);
        return new
        {
            OrderId = o.Id,
            Date = o.CreatedDate.ToString("dd.MM.yyyy"),
            Vehicle = o.Vehicle.Brand + " " + o.Vehicle.Model,
            LaborCost = laborCost,
            PartsCost = partsCost,
            Total = laborCost + partsCost
        };
    }).ToList();

    // generuj PDF
    var stream = new MemoryStream();
    var doc = Document.Create(container =>
    {
        container.Page(page =>
        {
            page.Margin(30);
            page.Header().Text($"Raport kosztów - klient: {customer.FullName}").FontSize(18).Bold();
            page.Content().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(2);
                    columns.RelativeColumn(2);
                    columns.RelativeColumn(2);
                    columns.RelativeColumn(2);
                });

                table.Header(header =>
                {
                    header.Cell().Text("Zlecenie").Bold();
                    header.Cell().Text("Data").Bold();
                    header.Cell().Text("Pojazd").Bold();
                    header.Cell().Text("Robocizna").Bold();
                    header.Cell().Text("Części").Bold();
                });

                foreach (var item in reportItems)
                {
                    table.Cell().Text(item.OrderId.ToString());
                    table.Cell().Text(item.Date);
                    table.Cell().Text(item.Vehicle);
                    table.Cell().Text(item.LaborCost.ToString("C", new CultureInfo("pl-PL")));
                    table.Cell().Text(item.PartsCost.ToString("C", new CultureInfo("pl-PL")));
                }

                table.Cell().ColumnSpan(3).Text("Suma").Bold();
                table.Cell().Text(reportItems.Sum(i => i.LaborCost).ToString("C", new CultureInfo("pl-PL"))).Bold();
                table.Cell().Text(reportItems.Sum(i => i.PartsCost).ToString("C", new CultureInfo("pl-PL"))).Bold();
            });
        });
    });

    doc.GeneratePdf(stream);
    stream.Position = 0;

    return File(stream.ToArray(), "application/pdf", "raport_kosztow.pdf");
}

    
    public class RepairReportItem
    {
        public DateTime OrderDate { get; set; }
        public string VehicleInfo { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal LaborCost { get; set; }
        public decimal PartsCost { get; set; }
    }
}
