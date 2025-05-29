using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System_Zarz.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Task = System.Threading.Tasks.Task;

namespace System_Zarz.Pages.Comments
{
    [Authorize] // dostęp tylko dla zalogowanych
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IndexModel(IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _clientFactory = clientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        // Lista zleceń do wyboru (ładujemy na GET)
        public List<OrderDto> OrdersList { get; set; } = new();

        // Wybrane zlecenie
        [BindProperty]
        public int SelectedOrderId { get; set; }

        // Komentarze do wybranego zlecenia
        public List<CommentDto> CommentsList { get; set; } = new();

        // Nowy komentarz do wpisania
        [BindProperty]
        public string NewCommentText { get; set; } = string.Empty;

        public string? ErrorMessage { get; set; }
        public bool SuccessMessage { get; set; } = false;

        // DTO do komentarza
        public class CommentDto
        {
            public int Id { get; set; }
            public string Text { get; set; } = string.Empty;
            public string Author { get; set; } = string.Empty;
            public string CreatedAt { get; set; } = string.Empty; // możesz też DateTime
            public int OrderId { get; set; }
        }

        // DTO do zlecenia
        public class OrderDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }

        public async Task OnGetAsync()
        {
            await LoadOrdersAsync();
            if (OrdersList.Any())
            {
                SelectedOrderId = OrdersList.First().Id;
                await LoadCommentsAsync(SelectedOrderId);
            }
        }

        public async Task<IActionResult> OnPostLoadCommentsAsync()
        {
            await LoadOrdersAsync();
            await LoadCommentsAsync(SelectedOrderId);
            return Page();
        }

        public async Task<IActionResult> OnPostAddCommentAsync()
        {
            if (string.IsNullOrWhiteSpace(NewCommentText))
            {
                ErrorMessage = "Komentarz nie może być pusty.";
                await LoadOrdersAsync();
                await LoadCommentsAsync(SelectedOrderId);
                return Page();
            }

            var client = _clientFactory.CreateClient("API");

            var cookie = _httpContextAccessor.HttpContext.Request.Headers["Cookie"].ToString();
            var commentCreate = new
            {
                OrderId = SelectedOrderId,
                Text = NewCommentText
            };

            var json = JsonSerializer.Serialize(commentCreate);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, "api/comments")
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
                NewCommentText = string.Empty;
                ErrorMessage = null;
                await LoadOrdersAsync();
                await LoadCommentsAsync(SelectedOrderId);
                ModelState.Clear();
                return Page();
            }
            else
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                ErrorMessage = $"Błąd dodawania komentarza: {response.StatusCode} - {errorMsg}";
                SuccessMessage = false;
                await LoadOrdersAsync();
                await LoadCommentsAsync(SelectedOrderId);
                return Page();
            }
        }

        private async Task LoadOrdersAsync()
        {
            var client = _clientFactory.CreateClient("API");
            var cookie = _httpContextAccessor.HttpContext.Request.Headers["Cookie"].ToString();

            var request = new HttpRequestMessage(HttpMethod.Get, "api/orders");
            if (!string.IsNullOrEmpty(cookie))
                request.Headers.Add("Cookie", cookie);

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                var orders = await JsonSerializer.DeserializeAsync<List<OrderDto>>(stream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                OrdersList = orders ?? new List<OrderDto>();
            }
            else
            {
                OrdersList = new List<OrderDto>();
                ErrorMessage = $"Nie udało się załadować listy zleceń. Status: {response.StatusCode}";
            }
        }

        private async Task LoadCommentsAsync(int orderId)
        {
            var client = _clientFactory.CreateClient("API");
            var cookie = _httpContextAccessor.HttpContext.Request.Headers["Cookie"].ToString();

            var request = new HttpRequestMessage(HttpMethod.Get, $"api/comments?orderId={orderId}");
            if (!string.IsNullOrEmpty(cookie))
            {
                request.Headers.Add("Cookie", cookie);
            }

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                var comments = await JsonSerializer.DeserializeAsync<List<CommentDto>>(stream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                CommentsList = comments ?? new List<CommentDto>();
            }
            else
            {
                CommentsList = new List<CommentDto>();
                ErrorMessage = $"Nie udało się załadować komentarzy. Status: {response.StatusCode}";
            }
        }
    }
}
