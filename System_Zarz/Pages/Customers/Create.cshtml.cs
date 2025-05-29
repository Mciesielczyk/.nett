using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System_Zarz.Data;
using System_Zarz.Models;
using Task = System.Threading.Tasks.Task;

namespace System_Zarz.Pages.Customers;

[Authorize(Roles = "Admin,Recepcjonista")]
public class CreateModel : PageModel
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateModel(IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _clientFactory = clientFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    [BindProperty]
    public Customer Customer { get; set; } = new();

    public bool SuccessMessage { get; set; } = false;
    public string? ErrorMessage { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            ErrorMessage = "❌ Wystąpił błąd. Upewnij się, że wszystkie pola są poprawnie wypełnione.";
            return Page();
        }

        var client = _clientFactory.CreateClient("API");
        var cookie = _httpContextAccessor.HttpContext.Request.Headers["Cookie"].ToString();

        var json = JsonSerializer.Serialize(Customer);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(HttpMethod.Post, "api/CustomersApi");
        request.Content = content;

        if (!string.IsNullOrEmpty(cookie))
        {
            request.Headers.Add("Cookie", cookie);
        }

        var response = await client.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            SuccessMessage = true;
            ModelState.Clear();
            Customer = new Customer();
            ErrorMessage = null;
        }
        else
        {
            var errorMsg = await response.Content.ReadAsStringAsync();
            ErrorMessage = $"❌ Błąd zapisu: {response.StatusCode} - {errorMsg}";
            SuccessMessage = false;
        }

        return Page();
    }
}
