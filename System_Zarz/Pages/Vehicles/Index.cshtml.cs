using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System_Zarz.Data;
using System_Zarz.Models;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Task = System.Threading.Tasks.Task;

namespace System_Zarz.Pages.Vehicles
{
    [Authorize(Roles = "Admin,Mechanik")]
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IndexModel(IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _clientFactory = clientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty] public Vehicle Vehicle { get; set; } = new();

        [BindProperty] public IFormFile? UploadPhoto { get; set; }

        public bool SuccessMessage { get; set; } = false;
        public string? ErrorMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "❌ Upewnij się, że wszystkie pola są poprawnie wypełnione.";
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

            var request = new HttpRequestMessage(HttpMethod.Post, "api/Vehicles");
            request.Content = form;

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

            return Page();
        }
    }
}