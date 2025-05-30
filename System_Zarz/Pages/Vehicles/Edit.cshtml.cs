using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System_Zarz.Models;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace System_Zarz.Pages.Vehicles
{
    [Authorize(Roles = "Admin,Mechanik,Recepcjonista")]
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
        public Vehicle Vehicle { get; set; } = new();

        [BindProperty]
        public IFormFile? UploadPhoto { get; set; }

        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _clientFactory.CreateClient("API");
            var cookie = _httpContextAccessor.HttpContext.Request.Headers["Cookie"].ToString();

            var request = new HttpRequestMessage(HttpMethod.Get, $"api/Vehicles/{id}");
            if (!string.IsNullOrEmpty(cookie))
            {
                request.Headers.Add("Cookie", cookie);
            }

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                Vehicle = await response.Content.ReadAsAsync<Vehicle>();
                return Page();
            }

            ErrorMessage = $"Nie udało się załadować pojazdu: {response.StatusCode}";
            return RedirectToPage("/Vehicles/Index");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var client = _clientFactory.CreateClient("API");
            var cookie = _httpContextAccessor.HttpContext.Request.Headers["Cookie"].ToString();

            using var form = new MultipartFormDataContent();

            form.Add(new StringContent(Vehicle.Id.ToString()), "Id");
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

            var request = new HttpRequestMessage(HttpMethod.Put, $"api/Vehicles/{Vehicle.Id}")
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
                return RedirectToPage("/Vehicles/Index", new { success = true });
            }

            var errorMsg = await response.Content.ReadAsStringAsync();
            ErrorMessage = $"Błąd podczas aktualizacji pojazdu: {response.StatusCode} - {errorMsg}";
            return Page();
        }
    }
}