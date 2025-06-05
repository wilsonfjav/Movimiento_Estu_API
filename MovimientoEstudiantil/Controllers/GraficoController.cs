using Microsoft.AspNetCore.Mvc;
using MovimientoEstudiantil.API;
using MovimientoEstudiantil.DTO;
using MovimientoEstudiantil.Models;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http.Json;

namespace MovimientoEstudiantil.Controllers
{
    [Route("Grafico")]
    public class GraficoController : Controller
    {
        private readonly HttpClient _client;

        public GraficoController(ImplementacionAPI implementacionApi)
        {
            _client = implementacionApi.Iniciar();
        }

        // GET /Grafico
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            // Cargar la lista completa de objetos Provincia y Sede (no solo el ID)
            ViewBag.Provincias = await ObtenerProvinciasCompleto();
            ViewBag.Sedes = await ObtenerSedesCompleto();
            return View("IndexGraficoView");
        }

        // POST /Grafico/ObtenerDatos
        [HttpPost("ObtenerDatos")]
        public async Task<IActionResult> ObtenerDatos([FromBody] FiltroGraficoDTO filtro)
        {
            // 1) Traer todos los estudiantes
            var resEst = await _client.GetAsync("/Estudiante/ListaEstudiantes");
            if (!resEst.IsSuccessStatusCode)
                return Json(new List<GraficoDTO>());

            var jsonEst = await resEst.Content.ReadAsStringAsync();
            var listaEstudiantes = JsonConvert
                .DeserializeObject<List<EstudianteDTO>>(jsonEst)
                ?? new List<EstudianteDTO>();

            // 2) Traer todas las sedes para comparar provincia ↔ sede
            var resSedes = await _client.GetAsync("/ListaSedes");
            var listaSedes = new List<Sede>();
            if (resSedes.IsSuccessStatusCode)
            {
                var jsonSedes = await resSedes.Content.ReadAsStringAsync();
                listaSedes = JsonConvert
                    .DeserializeObject<List<Sede>>(jsonSedes)
                    ?? new List<Sede>();
            }

            // Convertir a diccionario: clave = idSede, valor = idProvincia de esa sede
            var dicSedes = listaSedes
                .ToDictionary(s => s.idSede, s => s.idProvincia);

            // 3) Aplicar filtros
            var query = listaEstudiantes.AsQueryable();

            // 3.1) Rango de años
            if (filtro.AnioInicio != 0 && filtro.AnioFin != 0)
            {
                query = query.Where(e =>
                    e.anioIngreso >= filtro.AnioInicio &&
                    e.anioIngreso <= filtro.AnioFin
                );
            }

            // 3.2) Provincia de origen (string con ID)
            if (!string.IsNullOrEmpty(filtro.Provincia))
            {
                query = query.Where(e =>
                    e.provincia.ToString() == filtro.Provincia
                );
            }

            // 3.3) Sede (string con ID)
            if (!string.IsNullOrEmpty(filtro.Sede))
            {
                query = query.Where(e =>
                    e.sede.ToString() == filtro.Sede
                );
            }

            // 3.4) Ingreso a Carrera Deseada (satisfaccionCarrera: "Sí"/"No")
            if (!string.IsNullOrEmpty(filtro.IngresoCarreraDeseada))
            {
                query = query.Where(e =>
                    e.satisfaccionCarrera == filtro.IngresoCarreraDeseada
                );
            }

            // 3.5) Traslado de Residencia
            if (!string.IsNullOrEmpty(filtro.TrasladoResidencia))
            {
                if (filtro.TrasladoResidencia == "Sí")
                {
                    // provinciaOrigen != provinciaSede 
                    query = query.Where(e =>
                        dicSedes.ContainsKey(e.sede) &&
                        dicSedes[e.sede] != e.provincia
                    );
                }
                else if (filtro.TrasladoResidencia == "No")
                {
                    // provinciaOrigen == provinciaSede
                    query = query.Where(e =>
                        dicSedes.ContainsKey(e.sede) &&
                        dicSedes[e.sede] == e.provincia
                    );
                }
            }

            var filtrados = query.ToList();

            // 4) Agrupar por provincia de origen
            var resultado = filtrados
                .GroupBy(e => e.provincia.ToString())
                .Select(g => new GraficoDTO
                {
                    Categoria = g.Key,   // IDProvincia en string
                    Cantidad = g.Count()
                })
                .ToList();

            return Json(resultado);
        }

        // ----------------------------
        // Métodos auxiliares para llenar los dropdowns con objetos completos
        // ----------------------------

        private async Task<List<Provincia>> ObtenerProvinciasCompleto()
        {
            var lista = new List<Provincia>();
            // Llamar al mismo endpoint que ProvinciaController: GET /ListaProvincias
            var res = await _client.GetAsync("/ListaProvincias");
            if (!res.IsSuccessStatusCode)
                return lista;

            var json = await res.Content.ReadAsStringAsync();
            lista = JsonConvert
                .DeserializeObject<List<Provincia>>(json)
                ?? new List<Provincia>();

            return lista;
        }

        private async Task<List<Sede>> ObtenerSedesCompleto()
        {
            var lista = new List<Sede>();
            // Llamar al mismo endpoint que SedeController: GET /ListaSedes
            var res = await _client.GetAsync("/ListaSedes");
            if (!res.IsSuccessStatusCode)
                return lista;

            var json = await res.Content.ReadAsStringAsync();
            lista = JsonConvert
                .DeserializeObject<List<Sede>>(json)
                ?? new List<Sede>();

            return lista;
        }
    }
}
