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
    public class CandidatesController : Controller
    {
        private readonly IRepositoryAsync<Candidates> _candedates;
        private readonly IRepositoryAsync<CandidateExperiences> _CandidateExperiences;

        public CandidatesController
            (
            IRepositoryAsync<Candidates> candedates
            , IRepositoryAsync<CandidateExperiences> CandidateExperiences
            )
        {
            _candedates = candedates;
            _CandidateExperiences = CandidateExperiences;
        }

        #region LECTURA
        public async Task<IActionResult> Index()
        {
            var data = await _candedates.GetAll();

            return View(data);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidates = await _candedates.GetByID(id);

            return View(candidates);
        }
        #endregion

        #region ESCRITURA
        #region CREATE
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Candidates candidates)
        {
            candidates.InsertDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                if (!CandidatesEmailExists(candidates.Email))
                {
                    await _candedates.Insert(candidates);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "El correo electrónico ya está registrado.");
                    return View(candidates);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(candidates);
        }
        #endregion

        #region EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidates = await _candedates.GetByID(id);
            return View(candidates);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Candidates candidates)
        {
            if (id != candidates.IdCandidate)
            {
                return NotFound();
            }

            candidates.ModifyDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                try
                {
                    await _candedates.Update(candidates);
                }
                catch (Exception ex)
                {
                    // Maneja la excepción general aquí
                    if (ex is DbUpdateConcurrencyException)
                    {
                        if (!await CandidatesExists(candidates.IdCandidate))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "El correo electrónico ya está registrado.");
                        return View(candidates);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(candidates);

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

            var candidates = await _candedates.GetByID(id);
            return View(candidates);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var candidates = await _candedates.GetByID(id);
            if (candidates != null)
            {
                //var experiencesToDelete = _CandidateExperiences.Where(e => e.IdCandidate == id).ToList();
                //await _CandidateExperiences.Delete(experiencesToDelete.);
                //await _candedates.Delete(candidates.IdCandidate);

                var experiencesToDelete = _CandidateExperiences.Where(e => e.IdCandidate == id).ToList();

                foreach (var experience in experiencesToDelete)
                {
                    if(experience.IdCandidateExperience != 0)
                    {
                        await _CandidateExperiences.Delete(experience.IdCandidateExperience);
                    }
                }
                await _candedates.Delete(candidates.IdCandidate);
            }


            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region PRIVATE
        private async Task<bool> CandidatesExists(int id)
        {
            var entity = await _candedates.GetByID(id);
            return (entity != null);
        }

        private bool CandidatesEmailExists(string email)
        {
            var entity =  _candedates.Where(e => e.Email == email).ToList();
            return entity.Any();
        }
        #endregion
    }
}
