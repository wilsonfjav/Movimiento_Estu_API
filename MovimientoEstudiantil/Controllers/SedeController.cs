using Microsoft.AspNetCore.Mvc;
using MovimientoEstudiantil.API;
using MovimientoEstudiantil.Models;
using Newtonsoft.Json;

namespace MovimientoEstudiantil.Controllers
{
    public class SedeController : Controller
    {
        private readonly HttpClient _client;

        // Inyección de dependencia para ImplementacionAPI
        public SedeController(ImplementacionAPI implementacionApi)
        {
            _client = implementacionApi.Iniciar(); // Ya tiene las cookies y BaseAddress configuradas
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Sede> listado = new List<Sede>();

            HttpResponseMessage response = await _client.GetAsync("/ListaSedes");

            if (response.IsSuccessStatusCode)
            {
                var resultados = response.Content.ReadAsStringAsync().Result;
                listado = JsonConvert.DeserializeObject<List<Sede>>(resultados);

            }
            return View(listado);
        }
    }
}
