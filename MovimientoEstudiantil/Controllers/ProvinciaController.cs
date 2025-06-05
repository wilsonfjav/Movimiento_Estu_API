using Microsoft.AspNetCore.Mvc;
using MovimientoEstudiantil.API;
using MovimientoEstudiantil.Models;
using Newtonsoft.Json;

namespace MovimientoEstudiantil.Controllers
{
    [Route("Provincia")]
    public class ProvinciaController : Controller
    {
        private readonly HttpClient _client;

        // Inyección de dependencia para ImplementacionAPI
        public ProvinciaController(ImplementacionAPI implementacionApi)
        {
            _client = implementacionApi.Iniciar(); // Ya tiene las cookies y BaseAddress configuradas
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            List<Provincia> listado = new List<Provincia>();

            HttpResponseMessage response = await _client.GetAsync("/ListaProvincias");

            if (response.IsSuccessStatusCode)
            {
                var resultados = response.Content.ReadAsStringAsync().Result;
                listado = JsonConvert.DeserializeObject<List<Provincia>>(resultados);

            }
            return View(listado);
        }
    }
}
