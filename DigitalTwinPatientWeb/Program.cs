using DigitalTwinPatientWeb.Services;

namespace DigitalTwinPatientWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorPages();

            builder.Services.AddScoped<PatientService>();

            builder.Services.AddHttpClient<AuthApiService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7070/");
            });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.Use(async (context, next) =>
            {
                var path = context.Request.Path.Value?.ToLower();

                var isPublicPage =
                    path == "/" ||
                    path == "/index" ||
                    path.StartsWith("/index") ||
                    path.StartsWith("/css") ||
                    path.StartsWith("/js") ||
                    path.StartsWith("/lib");

                if (!isPublicPage)
                {
                    if (!context.Request.Cookies.ContainsKey("jwt"))
                    {
                        context.Response.Redirect("/Index");
                        return;
                    }
                }

                await next();
            });

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapRazorPages()
               .WithStaticAssets();

            app.Run();
        }
    }
}
