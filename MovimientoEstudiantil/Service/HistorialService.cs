using MovimientoEstudiantil.API;
using MovimientoEstudiantil.Models;
using System.Net.Http.Json;

namespace MovimientoEstudiantil.Services
{
    public class HistorialService
    {
        private readonly HttpClient _client;

        public HistorialService(ImplementacionAPI implementacionApi)
        {
            _client = implementacionApi.Iniciar();
        }

        public async Task RegistrarAccionAsync(int idUsuario, string accion, string descripcion = null)
        {
            var historial = new HistorialRegistro
            {
                idUsuario = idUsuario,
                accion = accion,
                descripcion = descripcion,
                fechaRegistro = DateTime.Now.Date,
                hora = DateTime.Now.TimeOfDay
            };

            var response = await _client.PostAsJsonAsync("/HistorialRegistro/AgregarHistorial", historial);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al registrar historial: {error}");
            }
        }
    }
}
