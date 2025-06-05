using Microsoft.AspNetCore.Authentication.Cookies;
using MovimientoEstudiantil.API;
using Newtonsoft.Json.Serialization; // necesario si querés usar camelCase opcional

var builder = WebApplication.CreateBuilder(args);

// ------------------------------
// CONFIGURACIÓN DE SERVICIOS
// ------------------------------
builder.Services.AddControllersWithViews(options =>
{
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
})
.AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
});

// Autenticación con Cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Error/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(1);

        // IMPORTANTE para trabajar con API que está en otro puerto (cross-site)
        options.Cookie.Name = ".AspNetCore.Cookies";
        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ImplementacionAPI>();
builder.Services.AddSession();

// ------------------------------
// CONFIGURACIÓN DE MIDDLEWARES
// ------------------------------
var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
