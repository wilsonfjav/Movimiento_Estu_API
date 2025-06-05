using Microsoft.AspNetCore.Mvc;

namespace MovimientoEstudiantil.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/AccessDenied")]
        public IActionResult AccessDenied()
        {
            // Redirige al inicio, por ejemplo
            return RedirectToAction("Index", "Home");
        }
    }
}
