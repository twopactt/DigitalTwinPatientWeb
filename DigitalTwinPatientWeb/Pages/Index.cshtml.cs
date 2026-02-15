using DigitalTwinPatientWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DigitalTwinPatientWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AuthApiService _authApi;

        public IndexModel(AuthApiService authApi)
        {
            _authApi = authApi;
        }

        [BindProperty]
        public string Login { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var token = await _authApi.LoginAsync(Login, Password);

            if (token == null)
            {
                ModelState.AddModelError("", "Неверный логин или пароль");
                return Page();
            }

            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true
            });

            return RedirectToPage("/Home");
        }
    }
}
