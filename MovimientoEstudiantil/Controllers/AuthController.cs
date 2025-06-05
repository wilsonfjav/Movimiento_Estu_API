using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MovimientoEstudiantil.API;
using MovimientoEstudiantil.DTO;
using MovimientoEstudiantil.Models;
using Newtonsoft.Json;
using System.Drawing;
using System.Net.Http.Json;
using System.Security.Claims;

namespace MovimientoEstudiantil.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _client;

        // Inyección de dependencia para ImplementacionAPI
        public AuthController(ImplementacionAPI implementacionApi)
        {
            _client = implementacionApi.Iniciar(); // Ya tiene las cookies y BaseAddress configuradas
        }

        // GET: /Auth/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View("AuthLoginView");
        }

        // POST: /Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind] loginDTO model)
        {
            if (!ModelState.IsValid)
                return View("AuthLoginView", model);

            // Consumir API
            var response = await _client.PostAsJsonAsync("/api/Auth/login", model);

            if (!response.IsSuccessStatusCode)
            {
                TempData["MensajeError"] = "Correo o contraseña incorrectos.";
                return View("AuthLoginView", model);
            }

            // Deserializar usuario desde la API
            var json = await response.Content.ReadAsStringAsync();
            var usuario = JsonConvert.DeserializeObject<UsuarioRespuestaDTO>(json);

            // Crear claims
            var claims = new List<Claim>
            {
                new Claim("Id", usuario.id.ToString()),
                new Claim(ClaimTypes.Name, usuario.correo),
                new Claim(ClaimTypes.Role, usuario.rol),



            };



            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // Iniciar sesión
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            // Crear cookie JS para usar en HeaderFooter.js
            HttpContext.Response.Cookies.Append("rol", usuario.rol, new CookieOptions
            {
                HttpOnly = false,
                //  Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            /// colocar el nombre de la persona logueada/
            HttpContext.Response.Cookies.Append("usuarioCorreo", usuario.correo, new CookieOptions
            {
                HttpOnly = false,
                //    Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            return RedirectToAction("Index", "Home");
        }

        // POST: /Auth/Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Response.Cookies.Delete("rol");
            return RedirectToAction("Login", "Auth");
        }
    }
}