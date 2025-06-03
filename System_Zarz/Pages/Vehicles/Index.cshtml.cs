using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System_Zarz.Models;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace System_Zarz.Pages.Vehicles
{
    [Authorize(Roles = "Admin,Mechanik,Recepcjonista")]
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IndexModel(IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _clientFactory = clientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public List<Vehicle>? Vehicles { get; set; }

        [BindProperty] public Vehicle Vehicle { get; set; } = new();

        [BindProperty] public IFormFile? UploadPhoto { get; set; }

        [BindProperty] public int VehicleIdToDelete { get; set; }

        public bool SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }

        public async Task OnGetAsync()
        {
            var client = _clientFactory.CreateClient("API");
            var cookie = _httpContextAccessor.HttpContext.Request.Headers["Cookie"].ToString();

            var request = new HttpRequestMessage(HttpMethod.Get, "api/Vehicles");
            if (!string.IsNullOrEmpty(cookie))
            {
                request.Headers.Add("Cookie", cookie);
            }

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                Vehicles = await response.Content.ReadAsAsync<List<Vehicle>>();
            }
            else
            {
                ErrorMessage = $"❌ Nie udało się załadować pojazdów: {response.StatusCode}";
            }
        }

        public async Task<IActionResult> OnPostAddAsync()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "❌ Upewnij się, że wszystkie pola są poprawnie wypełnione.";
                await OnGetAsync();
                return Page();
            }

            var client = _clientFactory.CreateClient("API");
            var cookie = _httpContextAccessor.HttpContext.Request.Headers["Cookie"].ToString();

            using var form = new MultipartFormDataContent();

            form.Add(new StringContent(Vehicle.CustomerId.ToString()), "CustomerId");
            form.Add(new StringContent(Vehicle.Brand ?? ""), "Brand");
            form.Add(new StringContent(Vehicle.Model ?? ""), "Model");
            form.Add(new StringContent(Vehicle.VIN ?? ""), "VIN");
            form.Add(new StringContent(Vehicle.RegistrationNumber ?? ""), "RegistrationNumber");
            form.Add(new StringContent(Vehicle.Year.ToString()), "Year");

            if (UploadPhoto != null && UploadPhoto.Length > 0)
            {
                var streamContent = new StreamContent(UploadPhoto.OpenReadStream());
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(UploadPhoto.ContentType);
                form.Add(streamContent, "UploadPhoto", UploadPhoto.FileName);
            }

            var request = new HttpRequestMessage(HttpMethod.Post, "api/Vehicles")
            {
                Content = form
            };

            if (!string.IsNullOrEmpty(cookie))
            {
                request.Headers.Add("Cookie", cookie);
            }

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                SuccessMessage = true;
                ModelState.Clear();
                Vehicle = new Vehicle();
                UploadPhoto = null;
                ErrorMessage = null;
            }
            else
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                ErrorMessage = $"❌ Błąd zapisu: {response.StatusCode} - {errorMsg}";
                SuccessMessage = false;
            }

            await OnGetAsync(); // załaduj listę pojazdów ponownie
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var client = _clientFactory.CreateClient("API");
            var cookie = _httpContextAccessor.HttpContext.Request.Headers["Cookie"].ToString();

            var request = new HttpRequestMessage(HttpMethod.Delete, $"api/Vehicles/{id}");

            if (!string.IsNullOrEmpty(cookie))
            {
                request.Headers.Add("Cookie", cookie);
            }

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                SuccessMessage = true;
                ErrorMessage = null;
            }
            else
            {
                ErrorMessage = $"❌ Nie udało się usunąć pojazdu: {response.StatusCode}";
                SuccessMessage = false;
            }

            await OnGetAsync(); // odśwież listę po usunięciu
            return Page();
        }
    }
}
