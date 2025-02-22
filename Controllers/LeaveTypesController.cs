using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LeaveManagementSystem.Web.Data;
using LeaveManagementSystem.Web.Models.LeaveTypes;
using AutoMapper;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Manage.Internal;
using LeaveManagementSystem.Web.MappingProfiles;
using static System.Runtime.InteropServices.JavaScript.JSType;
using LeaveManagementSystem.Web.Services;

namespace LeaveManagementSystem.Web.Controllers
{
    /// <summary>
    /// Automatski generirana izvedena klasa za tablicu LeaveTypes
    /// </summary>
    public class LeaveTypesController : Controller
    {
        #region Dependency injection
        /// <summary>
        /// Field za contract servicea
        /// </summary>
        private readonly ILeaveTypesService _service;

        /// <summary>
        /// Custom konstruktor koji postavlja service
        /// </summary>
        public LeaveTypesController(ILeaveTypesService service)
        {
            _service = service;
        }
        #endregion

        #region Fields, properties

        private const string _nameExistsValidationMessage = "This leave type already exists in the database!";

        #endregion

        /// <summary>
        /// Metoda koja poziva Index view sa podacima iz tablice sa baze podataka
        /// </summary>
        public async Task<IActionResult> Index()
        {
            //Poziv servicea kako bi dohvatili sve zapise iz tablice sa baze:
            var viewData = await _service.GetAllRecords();

            if (viewData == null)
            {
                return NotFound();
            }

            //Proslijeđivanje instance klase view-modela u view            
            return View(viewData);
        }

        /// <summary>
        /// Primjer pristupa ovom viewu: LeaveTypes/Details/5
        /// Broj 5 predstavlja record key (ID)
        /// </summary>
        public async Task<IActionResult> Details(int? id)
        {
            //Ako nije proslijeđen ID onda baci 404:
            if (id == null)
            {
                return NotFound();
            }

            //Poziv servicea kako bi dohvatili određeni zapis iz baze:
            var viewModel = await _service.GetRecordAsync<LeaveTypeReadOnlyVM>(id.Value);

            if (viewModel == null)
            {
                return NotFound();
            }

            //Ako dohvatimo podatke sa baze onda preusmjeri na view:
            return View(viewModel);
        }

        /// <summary>
        /// Path kojim dolazimo do viewa: LeaveTypes/Create
        /// </summary>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// POST metoda: LeaveTypes/Create
        /// Ova se metoda poziva samo na POST akciju kada korisnik na našem Razor viewu klikne na gumb Submit
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken] //zaštita od cross-referencing napada
        public async Task<IActionResult> Create(LeaveTypeCreateVM viewModelData)
        {
            //Provjera da li vrsta odsustva s posla već postoji u bazi prije dodavanja:
            //Npr. ako u bazi već postoji vrsta "Bolovanje" onda nema potrebe za novim dodavanjem
            if (await _service.CheckIfLeaveTypeNameExists(viewModelData.Name))
            {
                ModelState.AddModelError(nameof(viewModelData.Name), _nameExistsValidationMessage);
            }

            if (ModelState.IsValid) //server-side validation
            {
                //Poziv servicea kako bi spremili zapis na bazu:
                await _service.CreateRecordInDatabase(viewModelData);

                return RedirectToAction(nameof(Index)); //preusmjeravanje na Index stranicu
            }

            //Ako uneseni podaci nisu validni ponovno otvori Create view sa tim podacima:
            return View(viewModelData);
        }

        /// <summary>
        /// Ažuriranje određenog sloga unutar tablice (GET metoda za prikaz podataka na ekranu)
        /// Path: LeaveTypes/Edit/5
        /// </summary>
        public async Task<IActionResult> Edit(int? id)
        {
            //Ako nam HTTP request nije proslijedio ID sloga onda baci 404:
            if (id == null)
            {
                return NotFound();
            }

            //Poziv servicea kako bi dohvatili određeni zapis iz baze:
            var viewModel = await _service.GetRecordAsync<LeaveTypeEditVM>((int)id);

            if (viewModel == null)
            {
                return NotFound();
            }

            //Pozovi razor view za pronađeni slog na bazi:
            return View(viewModel);
        }

        /// <summary>
        /// Kada kliknemo na gumb SUBMIT na formi dolazimo do POST metode koja ažurira unesene podatke na bazi
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LeaveTypeEditVM viewModel)
        {
            //Ako iz nekog razloga ne proslijedimo ID
            //sloga kojeg želimo ažurirati na bazi onda baci 404:
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            //Provjera da li se editira objekt tipa ako se mijenja iz "Vacation",ID=5 u "Bolovanje",ID=5 a recimo da Bolovanje-slog već postoji u bazi:
            if (await _service.NameAlreadyExistsInTheDatabaseUnderDifferentID(viewModel))
            {
                ModelState.AddModelError(nameof(viewModel.Name), _nameExistsValidationMessage);
            }

            //Prvo se validiraju uneseni podaci sa forme:
            if (ModelState.IsValid)
            {
                try
                {
                    //Poziv servicea kako bi ažurirali zapis na bazi:
                    await _service.UpdateRecordInDatabase(viewModel);
                }
                catch (DbUpdateConcurrencyException)
                {
                    //Ako dođe do nekog exceptiona na bazi:
                    if (!_service.LeaveTypeExists(viewModel.Id))
                    {
                        return NotFound();
                    }
                    else //Ako postoji slog sa određenim ID-om na bazi onda proslijedi Exception:
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            //Ako spremanje na bazu prođe u redu vrati se na view:
            return View(viewModel);
        }

        /// <summary>
        /// GET metoda za prikaz podataka na Delete viewu
        /// Path: LeaveTypes/Delete/5
        /// </summary>
        public async Task<IActionResult> Delete(int? id)
        {
            //Ako se actionu ne proslijedi ID onda baci 404:
            if (id == null)
            {
                return NotFound();
            }

            //Poziv servicea kako bi dohvatili određeni zapis iz baze:
            var viewModel = await _service.GetRecordAsync<LeaveTypeReadOnlyVM>((int)id);

            if (viewModel == null)
            {
                return NotFound();
            }

            //Prikaži view sa pronađenim slogom sa baze:
            return View(viewModel);
        }

        /// <summary>
        /// POST metoda za brisanje odabranog sloga iz tablice na bazi
        /// Path:LeaveTypes/Delete/5
        /// </summary>
        [HttpPost]
        [ActionName("Delete")] //Naziv metode je "DeleteConfirmed" ali akcija je "Delete"
        [ValidateAntiForgeryToken] //zato što je akcija pozvana iz FORM-a
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //Poziv servicea kako bi obrisali određeni zapis iz baze:
            await _service.DeteleRecordAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
