using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.Json;
using System_Zarz.Data;
using System_Zarz.Models; // Upewnij się, że ścieżka do modelu się zgadza
using Task = System.Threading.Tasks.Task;

namespace System_Zarz.Pages.Customers;

public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public IndexModel(IHttpClientFactory clientFactory,  IHttpContextAccessor httpContextAccessor)
    {
        _clientFactory = clientFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public List<Customer> Customers { get; set; } = new();

    public async Task OnGetAsync()
    {
        try
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
                Customers = JsonSerializer.Deserialize<List<Customer>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<Customer>();
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Błąd API: {response.StatusCode}");
                Console.WriteLine($"Treść odpowiedzi: {error}");
                Customers = new List<Customer>();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            Customers = new List<Customer>();
        }
    }


    }
