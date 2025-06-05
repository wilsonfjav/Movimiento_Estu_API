using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovimientoEstudiantil.API;
using MovimientoEstudiantil.Models;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using MovimientoEstudiantil.DTO;

namespace MovimientoEstudiantil.Controllers
{
    [Route("Estudiante")]
    [Authorize(Roles = "Administrador")]

    public class EstudianteController : Controller
    {
        private readonly HttpClient _client;

        public EstudianteController(ImplementacionAPI implementacionApi)
        {
            _client = implementacionApi.Iniciar();
        }

        //------------------------------------------------------------------------------
        // LISTADO
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            List<Estudiante> listado = new List<Estudiante>();
            var response = await _client.GetAsync("/Estudiante/ListaEstudiantes");

            if (response.IsSuccessStatusCode)
            {
                var resultados = await response.Content.ReadAsStringAsync();
                listado = JsonConvert.DeserializeObject<List<Estudiante>>(resultados);
            }

            return View("IndexEstudianteView", listado);
        }

        //------------------------------------------------------------------------------
        // FORMULARIO DE REGISTRO
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View("CreateEstudianteView");
        }

        //------------------------------------------------------------------------------
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EstudianteDTO estudianteDTO)
        {
            try
            {
                // Obtener el ID del usuario autenticado desde los claims
                var claimId = User.FindFirst("Id")?.Value;

                if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out int idUsuario))
                {
                    ViewData["Mensaje"] = "No se pudo obtener el ID del usuario autenticado.";
                    return View("CreateEstudianteView", estudianteDTO);
                }

                // Crear objeto combinado que incluye estudiante + id del usuario
                var dtoCombinado = new
                {
                    estudiante = estudianteDTO,
                    idUsuario = idUsuario
                };

                Console.WriteLine("🎯 Enviando POST a API...");
                var response = await _client.PostAsJsonAsync("/Estudiante/AgregarEstudiante", dtoCombinado);
                Console.WriteLine($"📬 StatusCode: {response.StatusCode}");

                var contenido = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"📄 Respuesta API: {contenido}");

                if (!response.IsSuccessStatusCode)
                {
                    ViewData["Mensaje"] = "Error desconocido al registrar.";
                    ViewData["Detalle"] = contenido;
                    return View("CreateEstudianteView", estudianteDTO);
                }

                TempData["MensajeExito"] = "Estudiante registrado correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error inesperado: {ex.Message}");
                ViewData["Mensaje"] = "Error al conectar con la API.";
                ViewData["Detalle"] = ex.Message;
                return View("CreateEstudianteView", estudianteDTO);
            }
        }

    }
}