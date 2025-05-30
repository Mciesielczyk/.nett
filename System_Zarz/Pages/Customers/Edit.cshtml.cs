using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using System_Zarz.Data;
using System_Zarz.Models;
using Task = System.Threading.Tasks.Task;

namespace System_Zarz.Pages.Customers;

[Authorize(Roles = "Admin,Recepcjonista")]
public class EditModel : PageModel
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public EditModel(IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _clientFactory = clientFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    [BindProperty]
    public Customer Customer { get; set; } = new();

    public List<Customer> AllCustomers { get; set; } = new();
    public string? Message { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? SearchId { get; set; }
    
    [BindProperty]
    public int DeleteCustomerId { get; set; }

    public async Task OnGetAsync()
    {
        var client = _clientFactory.CreateClient("API");

        // Przekazujemy ciasteczka (auth) z przeglądarki
        var cookie = _httpContextAccessor.HttpContext.Request.Headers["Cookie"].ToString();
        var requestAll = new HttpRequestMessage(HttpMethod.Get, "api/CustomersApi");
        if (!string.IsNullOrEmpty(cookie))
        {
            requestAll.Headers.Add("Cookie", cookie);
        }
        var responseAll = await client.SendAsync(requestAll);
        if (responseAll.IsSuccessStatusCode)
        {
            var contentAll = await responseAll.Content.ReadAsStringAsync();
            AllCustomers = JsonSerializer.Deserialize<List<Customer>>(contentAll, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
        }
        else
        {
            Message = $"❌ Błąd pobierania listy klientów: {responseAll.StatusCode}";
        }

        if (SearchId.HasValue)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/CustomersApi/{SearchId.Value}");
            if (!string.IsNullOrEmpty(cookie))
            {
                request.Headers.Add("Cookie", cookie);
            }
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Customer = JsonSerializer.Deserialize<Customer>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new Customer();
                Message = null;
            }
            else
            {
                Message = $"❌ Nie znaleziono klienta o ID {SearchId}";
            }
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            // Reload list on error
            await LoadAllCustomersAsync();
            return Page();
        }

        var client = _clientFactory.CreateClient("API");
        var cookie = _httpContextAccessor.HttpContext.Request.Headers["Cookie"].ToString();

        var json = JsonSerializer.Serialize(Customer);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(HttpMethod.Put, $"api/CustomersApi/{Customer.Id}");
        request.Content = content;
        if (!string.IsNullOrEmpty(cookie))
        {
            request.Headers.Add("Cookie", cookie);
        }

        var response = await client.SendAsync(request);

        if (response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            Message = "✅ Dane klienta zostały zaktualizowane.";
        }
        else
        {
            var errorMsg = await response.Content.ReadAsStringAsync();
            Message = $"❌ Błąd aktualizacji: {response.StatusCode} - {errorMsg}";
        }

        await LoadAllCustomersAsync();

        return Page();
    }

    private async Task LoadAllCustomersAsync()
    {
        var client = _clientFactory.CreateClient("API");
        var cookie = _httpContextAccessor.HttpContext.Request.Headers["Cookie"].ToString();
        var request = new HttpRequestMessage(HttpMethod.Get, "api/CustomersApi");
        if (!string.IsNullOrEmpty(cookie))
        {
            request.Headers.Add("Cookie", cookie);
        }
        var response = await client.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            AllCustomers = JsonSerializer.Deserialize<List<Customer>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
        }
        else
        {
            AllCustomers = new List<Customer>();
        }
    }
    public async Task<IActionResult> OnPostDeleteAsync()
    {
        var client = _clientFactory.CreateClient("API");
        var cookie = _httpContextAccessor.HttpContext.Request.Headers["Cookie"].ToString();

        var request = new HttpRequestMessage(HttpMethod.Delete, $"api/CustomersApi/{DeleteCustomerId}");
        if (!string.IsNullOrEmpty(cookie))
        {
            request.Headers.Add("Cookie", cookie);
        }

        var response = await client.SendAsync(request);

        if (response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            Message = "✅ Klient został usunięty.";
        }
        else
        {
            Message = $"❌ Błąd usuwania klienta: {response.StatusCode}";
        }

        await LoadAllCustomersAsync();
        return Page();
    }

}
