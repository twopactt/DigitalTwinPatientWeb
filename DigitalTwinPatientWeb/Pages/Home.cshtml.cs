using DigitalTwinPatientWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DigitalTwinPatientWeb.Pages
{
    public class HomeModel : PageModel
    {
        private readonly PatientService _patientService;

        public HomeModel(PatientService patientService)
        {
            _patientService = patientService;
        }

        public string PatientName { get; set; }

        public async Task OnGet()
        {
            if (Request.Cookies.TryGetValue("jwt", out var token))
            {
                PatientName = await _patientService.GetPatientNameFromJwtAsync(token);
                ViewData["PatientName"] = PatientName;
            }
        }

        public IActionResult OnPost()
        {
            Response.Cookies.Delete("jwt");
            return RedirectToPage("/Index");
        }

        public IActionResult OnPostLogout()
        {
            Response.Cookies.Delete("jwt");
            return RedirectToPage("/Index");
        }
    }
}