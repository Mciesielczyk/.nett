using NBomber.CSharp;
using NBomber.Contracts;
using System.Text;
using System.Text.Json;

namespace PerformanceTests
{
    internal class Program
    {
        static async Task Main()
        {
            // Najpierw uzyskaj cookie uwierzytelniania
            string authCookie = ".AspNetCore.Antiforgery.jPdbEl1RHOo = " +
                "CfDJ8AA8uDCF8xNImmfllPfWjsNqU - " +
                "c8fTctCa4yHrXrZF3ogT36_HrohW8Fo0WmVZWsgOGBZPF48Dpv12MlrdBEi_MxHzQrVUh4hx - " +
                "vyKvHf1iffgGmIp_ySB_P1rChECxsFsCCydGAYpyXFGBRLrVRR7I; " +
                ".AspNetCore.Identity.Application = " +
                "CfDJ8AA8uDCF8xNImmfllPfWjsPJ2g5UzGYFKJ5 - " +
                "I7rY3nGo_QPYSZJ27Vsdi - " +
                "ZFsy8N5KemJpXbM9NNcB0TzeZKPcNwYpl56ABEgSPqAQuVpA0KKEl5P5sNuqfEUVHlfH1gNRfEKzgdmWky8RMBH7dYyOrwqeuOFZod_RYekvppD0XBxra6Ou6KL6VIcAEg4I4Z4BSX6fMJsZ7tvhiz1PJE3olvuxC3rIOMT9o" +
                "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000nuVRWv7p_dL8XVveFOIOYKguP1TjXT1clnwPxXohIf5Y2YmenYvdsgPXiAaBFdK68I10xWLB1zP6nhVV0zjFZhXNEjBClZq4pzVsvVCdvAAWI_" +
                "VzPX4tuGhKH1IgjuyDM6w4eIwKwDWL3GgbLaDfTfs3ywpVCaNapa2gSH1SaKkpiBBb0JHNfXd6s9zTwc5kxCO5861xGtNQsoAEKtQILiDdoYJxg46YeUnjBbK_Pplm - BY6p6QzJraGNrhMMHoX0JMsh8FP1njeB3Nf1l - " +
                "trOyQycOANqpHezGWrf9sxcjjnClFBf3VaWMLR0VQc1sbLyrYKWJkoGdVh5IcDomFOGry0o61MroJIt - mHc4ArupBlYz339NgLZMPVSUoREev_SHzY5I_RxfCPPrcykHIOjHFc5HpU3rZOW2wuETNtzntvNNcEvF1huqnj0Nt5GUNFFNJIeBWpnkWTYgGV - 5yhtpFLxSEJrjP7ff288SHctgFCWUrTi" +
                " - HyYT1WZB6dxTCftO3dJoKLGdTUJPuAEVlxRAfwCKtVxw69D9r_fLt5cj4dB5khIDD8g0J1FJSeGNrIsVjSQKs3lSA67U_Kb7uFuS1cpKIKQ";

            if (string.IsNullOrEmpty(authCookie))
            {
                Console.WriteLine("Nie udało się uzyskać cookie uwierzytelniania!");
                return;
            }

            var httpClient = new HttpClient();

            var scenario = Scenario.Create("customers_api_test", async context =>
            {
                // Zmień port na swój - sprawdź w launchSettings.json
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5264/api/CustomersApi");

                // Dodaj cookie uwierzytelniania
                requestMessage.Headers.Add("Cookie", $".AspNetCore.Identity.Application={authCookie}");
                requestMessage.Headers.Add("Accept", "application/json");

                var response = await httpClient.SendAsync(requestMessage);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Błąd API: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                }

                return Response.Ok(
                    statusCode: response.StatusCode.ToString(),
                    sizeBytes: response.Content?.Headers?.ContentLength ?? 0
                );
            })
            .WithWarmUpDuration(TimeSpan.FromSeconds(5))
            .WithLoadSimulations(Simulation.KeepConstant(copies: 10, during: TimeSpan.FromSeconds(15)));

            NBomberRunner
                .RegisterScenarios(scenario)
                .Run();
        }

        private static async Task<string> GetAuthCookieAsync()
        {
            try
            {
                using var client = new HttpClient();

                // OPCJA 1: Spróbuj przez endpoint logowania (jeśli masz taki)
                var loginData = new
                {
                    Email = "admin@1",
                    Password = "Admin123!",
                    RememberMe = false
                };

                var json = JsonSerializer.Serialize(loginData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Sprawdź czy masz endpoint /api/account/login lub podobny
                var loginResponse = await client.PostAsync("http://localhost:5264/api/account/login", content);

                if (loginResponse.IsSuccessStatusCode)
                {
                    // Pobierz cookie z nagłówków odpowiedzi
                    if (loginResponse.Headers.TryGetValues("Set-Cookie", out var cookies))
                    {
                        foreach (var cookie in cookies)
                        {
                            if (cookie.Contains(".AspNetCore.Identity.Application="))
                            {
                                var start = cookie.IndexOf(".AspNetCore.Identity.Application=") + ".AspNetCore.Identity.Application=".Length;
                                var end = cookie.IndexOf(";", start);
                                if (end == -1) end = cookie.Length;
                                return cookie.Substring(start, end - start);
                            }
                        }
                    }
                }

                // OPCJA 2: Jeśli nie ma API endpointa, spróbuj przez formularz logowania
                Console.WriteLine("Nie znaleziono API logowania. Spróbuj opcji 2...");
                return await LoginThroughForm(client);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas logowania: {ex.Message}");
                return string.Empty;
            }
        }

        private static async Task<string> LoginThroughForm(HttpClient client)
        {
            try
            {
                // Pobierz stronę logowania aby uzyskać anti-forgery token
                var loginPageResponse = await client.GetAsync("http://localhost:5264/Account/Login");
                var loginPageContent = await loginPageResponse.Content.ReadAsStringAsync();

                // Znajdź anti-forgery token (uproszczona metoda)
                var tokenStart = loginPageContent.IndexOf("name=\"__RequestVerificationToken\" type=\"hidden\" value=\"");
                if (tokenStart == -1) return string.Empty;

                tokenStart += "name=\"__RequestVerificationToken\" type=\"hidden\" value=\"".Length;
                var tokenEnd = loginPageContent.IndexOf("\"", tokenStart);
                var antiForgeryToken = loginPageContent.Substring(tokenStart, tokenEnd - tokenStart);

                // Przygotuj dane formularza
                var formData = new List<KeyValuePair<string, string>>
                {
                    new("Email", "admin@1"),
                    new("Password", "Admin123!"),
                    new("RememberMe", "false"),
                    new("__RequestVerificationToken", antiForgeryToken)
                };

                var formContent = new FormUrlEncodedContent(formData);

                // Wyślij formularz logowania
                var loginResponse = await client.PostAsync("http://localhost:5264/Account/Login", formContent);

                // Sprawdź czy otrzymaliśmy cookie
                if (loginResponse.Headers.TryGetValues("Set-Cookie", out var cookies))
                {
                    foreach (var cookie in cookies)
                    {
                        if (cookie.Contains(".AspNetCore.Identity.Application="))
                        {
                            var start = cookie.IndexOf(".AspNetCore.Identity.Application=") + ".AspNetCore.Identity.Application=".Length;
                            var end = cookie.IndexOf(";", start);
                            if (end == -1) end = cookie.Length;
                            return cookie.Substring(start, end - start);
                        }
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas logowania przez formularz: {ex.Message}");
                return string.Empty;
            }
        }
    }
}