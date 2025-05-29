using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System_Zarz.Data;
using Task = System.Threading.Tasks.Task;

namespace System_Zarz.Pages.SpareParts
{
    [Authorize(Roles = "Admin,Recepcjonista")]
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IndexModel(IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _clientFactory = clientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public List<SparePart> SparePartsList { get; set; } = new();

        [BindProperty]
        public SparePart InputPart { get; set; } = new();

        [BindProperty]
        public int? EditPartId { get; set; }

        [BindProperty]
        public bool IsEditing { get; set; } = false;

        public string? ErrorMessage { get; set; }
        public bool SuccessMessage { get; set; } = false;

        public async Task OnGetAsync()
        {
            await LoadSparePartsAsync();
        }

        private async Task LoadSparePartsAsync()
        {
            var client = _clientFactory.CreateClient("API");
            var cookie = _httpContextAccessor.HttpContext.Request.Headers["Cookie"].ToString();

            var request = new HttpRequestMessage(HttpMethod.Get, "api/SpareParts");
            if (!string.IsNullOrEmpty(cookie))
            {
                request.Headers.Add("Cookie", cookie);
            }

            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                SparePartsList = JsonSerializer.Deserialize<List<SparePart>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
            }
            else
            {
                ErrorMessage = $"Nie udało się załadować listy części. Status: {response.StatusCode}";
            }
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Niepoprawne dane.";
                await LoadSparePartsAsync();
                return Page();
            }

            var client = _clientFactory.CreateClient("API");
            var cookie = _httpContextAccessor.HttpContext.Request.Headers["Cookie"].ToString();

            var json = JsonSerializer.Serialize(InputPart);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, "api/SpareParts")
            {
                Content = content
            };

            if (!string.IsNullOrEmpty(cookie))
            {
                request.Headers.Add("Cookie", cookie);
            }

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                SuccessMessage = true;
                InputPart = new SparePart();
                await LoadSparePartsAsync();
                ModelState.Clear();
            }
            else
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                ErrorMessage = $"Błąd zapisu: {response.StatusCode} - {errorMsg}";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostEditAsync(int id)
        {
            EditPartId = id;
            IsEditing = true;

            var client = _clientFactory.CreateClient("API");
            var cookie = _httpContextAccessor.HttpContext.Request.Headers["Cookie"].ToString();

            var request = new HttpRequestMessage(HttpMethod.Get, $"api/SpareParts/{id}");
            if (!string.IsNullOrEmpty(cookie))
            {
                request.Headers.Add("Cookie", cookie);
            }

            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                InputPart = JsonSerializer.Deserialize<SparePart>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
            }
            else
            {
                ErrorMessage = $"Nie udało się pobrać danych części. Status: {response.StatusCode}";
                IsEditing = false;
            }

            await LoadSparePartsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            if (!ModelState.IsValid || InputPart.Id == 0)
            {
                ErrorMessage = "Niepoprawne dane.";
                await LoadSparePartsAsync();
                return Page();
            }

            var client = _clientFactory.CreateClient("API");
            var cookie = _httpContextAccessor.HttpContext.Request.Headers["Cookie"].ToString();

            var json = JsonSerializer.Serialize(InputPart);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Put, $"api/SpareParts/{InputPart.Id}")
            {
                Content = content
            };

            if (!string.IsNullOrEmpty(cookie))
            {
                request.Headers.Add("Cookie", cookie);
            }

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                SuccessMessage = true;
                InputPart = new SparePart();
                IsEditing = false;
                EditPartId = null;
                await LoadSparePartsAsync();
                ModelState.Clear();
            }
            else
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                ErrorMessage = $"Błąd aktualizacji: {response.StatusCode} - {errorMsg}";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var client = _clientFactory.CreateClient("API");
            var cookie = _httpContextAccessor.HttpContext.Request.Headers["Cookie"].ToString();

            var request = new HttpRequestMessage(HttpMethod.Delete, $"api/SpareParts/{id}");
            if (!string.IsNullOrEmpty(cookie))
            {
                request.Headers.Add("Cookie", cookie);
            }

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                SuccessMessage = true;
                await LoadSparePartsAsync();
            }
            else
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                ErrorMessage = $"Błąd usunięcia: {response.StatusCode} - {errorMsg}";
            }

            return Page();
        }
    }
}
