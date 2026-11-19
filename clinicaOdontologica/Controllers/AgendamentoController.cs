using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using clinicaOdontologica.Models;

namespace clinicaOdontologica.Controllers
{
    public class AgendamentoController : Controller
    {
        private readonly DbOdontoContext _context;

        public AgendamentoController(DbOdontoContext context)
        {
            _context = context;
        }

        // GET: Agendamento
        public async Task<IActionResult> Index()
        {
            var agendamentosOrdenados = _context.Agendamentos
                .Include(a => a.Paciente)
                .Include(a => a.Procedimento)
                .OrderBy(a => a.DataHora); // Ordena pela data da consulta (mais próxima primeiro)

            return View(await agendamentosOrdenados.ToListAsync());
        }

        // GET: Agendamento/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agendamento = await _context.Agendamentos
                .Include(a => a.Paciente)
                .Include(a => a.Procedimento)
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (agendamento == null)
            {
                return NotFound();
            }

            return View(agendamento);
        }

        // GET: Agendamento/Create
        public IActionResult Create()
        {
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Codigo", "Codigo");
            ViewData["ProcedimentoId"] = new SelectList(_context.Procedimentos, "Codigo", "Codigo");
            return View();
        }

        // POST: Agendamento/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Codigo,DataHora,StatusAgendamento,PacienteId,ProcedimentoId")] Agendamento agendamento)
        {

            ModelState.Remove("Paciente");
            ModelState.Remove("Procedimento");

            if (ModelState.IsValid)
            {
                _context.Add(agendamento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Codigo", "Codigo", agendamento.PacienteId);
            ViewData["ProcedimentoId"] = new SelectList(_context.Procedimentos, "Codigo", "Codigo", agendamento.ProcedimentoId);
            return View(agendamento);
        }

        // GET: Agendamento/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agendamento = await _context.Agendamentos.FindAsync(id);
            if (agendamento == null)
            {
                return NotFound();
            }
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Codigo", "Codigo", agendamento.PacienteId);
            ViewData["ProcedimentoId"] = new SelectList(_context.Procedimentos, "Codigo", "Codigo", agendamento.ProcedimentoId);
            return View(agendamento);
        }

        // POST: Agendamento/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Codigo,DataHora,StatusAgendamento,PacienteId,ProcedimentoId")] Agendamento agendamento)
        {
            if (id != agendamento.Codigo)
            {
                return NotFound();
            }

            ModelState.Remove("Paciente");
            ModelState.Remove("Procedimento");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(agendamento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AgendamentoExists(agendamento.Codigo))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Codigo", "Codigo", agendamento.PacienteId);
            ViewData["ProcedimentoId"] = new SelectList(_context.Procedimentos, "Codigo", "Codigo", agendamento.ProcedimentoId);
            return View(agendamento);
        }

        // GET: Agendamento/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agendamento = await _context.Agendamentos
                .Include(a => a.Paciente)
                .Include(a => a.Procedimento)
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (agendamento == null)
            {
                return NotFound();
            }

            return View(agendamento);
        }

        // POST: Agendamento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var agendamento = await _context.Agendamentos.FindAsync(id);
            if (agendamento != null)
            {
                _context.Agendamentos.Remove(agendamento);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AgendamentoExists(int id)
        {
            return _context.Agendamentos.Any(e => e.Codigo == id);
        }
        public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
        {
            if (HttpContext.Session.GetInt32("UsuarioId") == null)
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
            base.OnActionExecuting(context);
        }
    }
}
