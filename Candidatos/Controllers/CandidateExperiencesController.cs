using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BD;
using DB;
using Candidatos.DataAccess.Interfaces;

namespace Candidatos.Controllers
{
    public class CandidateExperiencesController : Controller
    {
        private readonly IRepositoryAsync<Candidates> _candedates;
        private readonly IRepositoryAsync<CandidateExperiences> _CandidateExperiences;

        public CandidateExperiencesController
            (
            IRepositoryAsync<Candidates> candedates
            , IRepositoryAsync<CandidateExperiences> CandidateExperiences
            )
        {
            _candedates = candedates;
            _CandidateExperiences = CandidateExperiences;
        }

        #region LECTURA
        public async Task<IActionResult> Index(int? id)
        {

            if (id == null)
            {
                var testContext = _CandidateExperiences.Include(c => c.IdCandidateFK);
                return View(await testContext.ToListAsync());
            }
            else
            {
                // Filtra las experiencias por el 'id' del candidato
                var candidateExperiences = await _CandidateExperiences
                    .Include(ce => ce.IdCandidateFK)
                    .Where(ce => ce.IdCandidate == id)
                    .ToListAsync();

                ViewBag.IdCandidate = id;

                return View(candidateExperiences);
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidateExperience = await _CandidateExperiences.Include(ce => ce.IdCandidateFK).FirstOrDefaultAsync(ce => ce.IdCandidateExperience == id);
            return View(candidateExperience);
        }
        #endregion

        #region ESCRITURA
        #region CREATE
        public async Task<IActionResult> Create(int? id)
        {
            ViewData["IdCandidate"] = new SelectList(await _candedates.GetAll(), "IdCandidate", "Name", id);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CandidateExperiences candidateExperiences)
        {
            candidateExperiences.InsertDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                await _CandidateExperiences.Insert(candidateExperiences);
                // Obtén el IdCandidate de la experiencia creada
                int idCandidate = candidateExperiences.IdCandidate;

                // Redirige a la vista Index con el IdCandidate como parámetro
                return RedirectToAction("Index", new { id = idCandidate });
            }
            ViewData["IdCandidate"] = new SelectList(await _CandidateExperiences.GetAll(), "IdCandidate", "Name", candidateExperiences.IdCandidate);
            return RedirectToAction("Index", new { id = candidateExperiences.IdCandidate });
        }
        #endregion 

        #region EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidateExperiences = await _CandidateExperiences.GetByID(id);
            if (candidateExperiences == null)
            {
                return NotFound();
            }
            ViewData["IdCandidate"] = new SelectList(await _candedates.GetAll(), "IdCandidate", "Name", candidateExperiences.IdCandidate);
            return View(candidateExperiences);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CandidateExperiences candidateExperiences)
        {
            if (id != candidateExperiences.IdCandidateExperience)
            {
                return NotFound();
            }

            candidateExperiences.ModifyDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                try
                {
                    await _CandidateExperiences.Update(candidateExperiences);

                    int idCandidate = candidateExperiences.IdCandidate;

                    // Redirige a la vista Index con el IdCandidate como parámetro
                    return RedirectToAction("Index", new { id = idCandidate });

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await CandidateExperiencesExists(candidateExperiences.IdCandidateExperience))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewData["IdCandidate"] = new SelectList(await _candedates.GetAll(), "IdCandidate", "Name", candidateExperiences.IdCandidate);
            return View(candidateExperiences);
        }
        #endregion
        #endregion

        #region DELETE
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidateExperiences = await _CandidateExperiences
                .Include(c => c.IdCandidateFK)
                .FirstOrDefaultAsync(m => m.IdCandidateExperience == id);

            return View(candidateExperiences);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var candidateExperiences = await _CandidateExperiences.GetByID(id);
            if (candidateExperiences != null)
            {
                await _CandidateExperiences.Delete(candidateExperiences.IdCandidateExperience);
                int idCandidate = candidateExperiences.IdCandidate;

                // Redirige a la vista Index con el IdCandidate como parámetro
                return RedirectToAction("Index", new { id = idCandidate });
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region PRIVATE
        private async Task<bool> CandidateExperiencesExists(int id)
        {
            var entity = await _CandidateExperiences.GetByID(id);
            return (entity != null);
        }
        #endregion
    }
}
