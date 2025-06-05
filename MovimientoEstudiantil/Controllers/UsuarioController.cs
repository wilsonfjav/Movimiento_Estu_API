using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovimientoEstudiantil.API;
using MovimientoEstudiantil.Models;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Security.Claims;
using MovimientoEstudiantil.DTO;

namespace MovimientoEstudiantil.Controllers
{
    [Authorize(Roles = "Coordinador")]
    [Route("Usuario")]
    public class UsuarioController : Controller
    {
        private readonly HttpClient _client;

        public UsuarioController(ImplementacionAPI implementacionApi)
        {
            _client = implementacionApi.Iniciar();
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            List<Usuario> listado = new List<Usuario>();
            var response = await _client.GetAsync("/api/Usuario/ListaUsuarios");

            if (response.IsSuccessStatusCode)
            {
                var resultados = await response.Content.ReadAsStringAsync();
                listado = JsonConvert.DeserializeObject<List<Usuario>>(resultados);
            }

            return View("IndexUsuarioView", listado);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View("CreateUsuarioView");
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuarioCreateDTO usuario)
        {
            try
            {
                var claimId = User.FindFirst("Id")?.Value;

                if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out int idUsuario))
                {
                    ViewData["Mensaje"] = "No se pudo obtener el ID del usuario autenticado.";
                    return View("CreateUsuarioView", usuario);
                }

                var dtoCombinado = new UsuarioConUsuarioDTO
                {
                    usuario = usuario,
                    idUsuario = idUsuario
                };

                var response = await _client.PostAsJsonAsync("/api/Usuario/AgregarUsuario", dtoCombinado);
                var contenido = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    ViewData["Mensaje"] = "Error al registrar el usuario.";
                    ViewData["Detalle"] = contenido;
                    return View("CreateUsuarioView", usuario);
                }

                TempData["MensajeExito"] = "Usuario registrado correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = "Error inesperado.";
                ViewData["Detalle"] = ex.Message;
                return View("CreateUsuarioView", usuario);
            }
        }


        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Usuario usuario = new Usuario();
            var response = await _client.GetAsync($"/api/Usuario/BuscarUsuario/{id}");

            if (response.IsSuccessStatusCode)
            {
                var resultado = await response.Content.ReadAsStringAsync();
                usuario = JsonConvert.DeserializeObject<Usuario>(resultado);
            }

            return View(usuario);
        }

        [HttpPost("DeleteUsuario/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            await _client.DeleteAsync($"/api/Usuario/EliminarUsuario{id}");
            return RedirectToAction("Index");
        }

        [HttpGet("Panel")]
        public IActionResult Panel()
        {
            return View("InicioUsuarioView");
        }

        // LISTA GENERAL DE HISTORIAL
        [HttpGet("Historial")]
        public async Task<IActionResult> Index_Historial()
        {
            List<HistorialRegistro> listaHistorial = new List<HistorialRegistro>();
            List<Usuario> listaUsuarios = new List<Usuario>();

            // 1. Obtener historial
            var responseHistorial = await _client.GetAsync("/HistorialRegistro/ListaHistorial");
            if (responseHistorial.IsSuccessStatusCode)
            {
                var json = await responseHistorial.Content.ReadAsStringAsync();
                listaHistorial = JsonConvert.DeserializeObject<List<HistorialRegistro>>(json);
            }

            // 2. Obtener usuarios
            var responseUsuarios = await _client.GetAsync("/api/Usuario/ListaUsuarios");
            if (responseUsuarios.IsSuccessStatusCode)
            {
                var jsonUsuarios = await responseUsuarios.Content.ReadAsStringAsync();
                listaUsuarios = JsonConvert.DeserializeObject<List<Usuario>>(jsonUsuarios);
            }

            // 3. Obtener los IDs de usuarios con rol "Coordinador"
            var idsCoordinadores = listaUsuarios
                .Where(u => u.rol != null && u.rol.Equals("Coordinador", StringComparison.OrdinalIgnoreCase))
                .Select(u => u.idUsuario)
                .ToHashSet();

            // 4. Filtrar historial excluyendo coordinadores
            var historialFiltrado = listaHistorial
                .Where(h => !idsCoordinadores.Contains(h.idUsuario))
                .ToList();

            return View("HistorialRegistroUsuarioView", historialFiltrado);
        }

    }
}