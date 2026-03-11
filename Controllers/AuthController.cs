using Microsoft.AspNetCore.Mvc;
using RR.Models;

namespace RR.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            // Si ya está autenticado, redirigir al dashboard
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Aquí iría tu lógica de autenticación real
                // Por ahora usamos credenciales de ejemplo
                if (model.Usuario == "admin" && model.Contraseña == "123456")
                {
                    // Aquí implementarías la autenticación real
                    // Por ejemplo usando Authentication HttpContext.SignInAsync

                    TempData["Success"] = "Login exitoso";
                    //return RedirectToAction("Index", "Home");
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos");
                }
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            // Aquí implementarías el logout
            return RedirectToAction("Login");
        }
    }
}
