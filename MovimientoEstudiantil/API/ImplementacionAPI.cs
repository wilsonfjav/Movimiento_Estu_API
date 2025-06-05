using System.Net;
using System.Net.Http;

namespace MovimientoEstudiantil.API
{
    public class ImplementacionAPI
    {
        // Esta propiedad nos permite acceder al contexto HTTP actual, incluyendo las cookies del usuario autenticado
        private readonly IHttpContextAccessor _httpContextAccessor;

        // Constructor que recibe el IHttpContextAccessor (lo inyectamos desde Program.cs)
        public ImplementacionAPI(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // Método que configura y retorna un HttpClient personalizado
        public HttpClient Iniciar()
        {
            // 1. Crear un manejador para el cliente HTTP que pueda almacenar cookies
            var handler = new HttpClientHandler
            {
                UseCookies = true, // Indica que vamos a trabajar con cookies
                CookieContainer = new CookieContainer() // Aquí se guardarán las cookies
            };

            // 2. Obtener el contexto HTTP actual (el usuario que está navegando en el sitio)
            var contexto = _httpContextAccessor.HttpContext;

            if (contexto != null)
            {
                // 3. Buscar la cookie de autenticación que usa ASP.NET para validar sesiones
                var cookie = contexto.Request.Cookies[".AspNetCore.Cookies"];

                if (!string.IsNullOrEmpty(cookie))
                {
                    // 4. Definir la URL base de la API (esto es importante para que la cookie sea válida)
                    var apiUri = new Uri("https://localhost:7205"); // 🔧 Cambiar esto en producción
                   
                    // 5. Agregar la cookie al contenedor del cliente HTTP
                    handler.CookieContainer.Add(apiUri, new Cookie(".AspNetCore.Cookies", cookie));
                }
            }

            // 6. Crear el cliente HTTP con el manejador que incluye la cookie
            var client = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://localhost:7205") // 🔧 Dirección base de la API
            };

            return client;
        }
    }
}
