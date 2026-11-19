using appOdonto.Models;
using clinicaOdontologica.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace appOdonto.Controllers
{
    public class AccountController : Controller
    {
        private readonly DbOdontoContext _context;

        public AccountController(DbOdontoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var paciente = await _context.Pacientes
                .FirstOrDefaultAsync(p => p.Cpf == model.Cpf);

            if (paciente == null)
            {
                ModelState.AddModelError("", "Falha na autenticação: CPF não encontrado.");
                return View(model);
            }

            // Salva os dados na Sessão
            HttpContext.Session.SetInt32("UsuarioId", paciente.Codigo);
            HttpContext.Session.SetString("UsuarioNome", paciente.Nome);

            return RedirectToAction("Index", "Agendamento");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}