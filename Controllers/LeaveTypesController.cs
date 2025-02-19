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

namespace LeaveManagementSystem.Web.Controllers
{
    /// <summary>
    /// Automatski generirana izvedena klasa za tablicu LeaveTypes
    /// </summary>
    public class LeaveTypesController : Controller
    {
        #region Dependency injection
        /// <summary>
        /// Privatan field za pristup DbContextu
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Private field za Dependency injection, koristi se za pristup Automapperu
        /// </summary>
        private readonly IMapper _autoMapper;

        /// <summary>
        /// Custom konstruktor koji postavlja objekt DbContext
        /// </summary>
        public LeaveTypesController(ApplicationDbContext context, IMapper autoMapper)
        {
            _context = context;
            _autoMapper = autoMapper;
        }
        #endregion

        /// <summary>
        /// Metoda koja poziva Index view sa podacima iz tablice sa baze podataka
        /// </summary>
        public async Task<IActionResult> Index()
        {
            //Koristi se lokalna (private) konekcija na bazu da bi se dohvatila lista slogova iz tablice:
            //var data = SELECT * FROM LeaveTypes;
            var data = await _context.LeaveTypes.ToListAsync();

            //1.korak: Konverzija data-modela (LeaveType.cs instanca) u view-model
            //Znači, za svaki slog koji se dohvat iz baze želim kreirati novu instancu view-model klase:

            /* RUČNA KONVERZIJA:
            var viewData = data.Select(x => new LeaveTypeReadOnlyVM
            {
                Id = x.Id,
                Name = x.Name,
                Days = x.NumberOfDays
            });
            */

            //AUTOMAPPER KONVERZIJA:
            //Mapira se data (data-model) u listu view-model instanca:
            var viewData = _autoMapper.Map<List<LeaveTypeReadOnlyVM>>(data);

            //2.korak: Prosljeđivanje instance klase view-modela u view            
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

            //Ovdje pretražujemo našu tablicu i dohvaćamo slog sa ID=5:
            //SELECT * FROM LeaveTypes WHERE ID=5;
            var dataModelInstance = await _context.LeaveTypes.FirstOrDefaultAsync(m => (m.Id == id));            

            //Ako ne pronalazimo takav slog u tablici onda baci 404:
            if (dataModelInstance == null)
            {
                return NotFound();
            }

            //Konverzija u view-model instancu klase
            var viewModelInstance = _autoMapper.Map<LeaveTypeReadOnlyVM>(dataModelInstance);

            //Ako dohvatimo podatke sa baze onda preusmjeri na view:
            return View(viewModelInstance);
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
        public async Task<IActionResult> Create([Bind("Id,Name,NumberOfDays")] LeaveType leaveType)
        {
            if (ModelState.IsValid) //ako je forma ispravno popunjena:
            {
                _context.Add(leaveType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); //preusmjeravanje na Index stranicu
            }

            //Ako uneseni podaci nisu validni ponovno otvori Create view sa tim podacima:
            return View(leaveType);
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

            //SELECT * FROM LeaveTypes WHERE ID = 5;
            var leaveType = await _context.LeaveTypes.FindAsync(id);

            //Ako nema sloga sa zadanim ID-om na bazi onda baci 404:
            if (leaveType == null)
            {
                return NotFound();
            }

            //Pozovi razor view za pronađeni slog na bazi:
            return View(leaveType);
        }

        /// <summary>
        /// Kada kliknemo na gumb SUBMIT na formi dolazimo do POST metode koja ažurira unesene podatke na bazi
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,NumberOfDays")] LeaveType leaveType)
        {
            //Ako iz nekog razloga ne proslijedimo ID
            //sloga kojeg želimo ažurirati na bazi onda baci 404:
            if (id != leaveType.Id)
            {
                return NotFound();
            }

            //Prvo se validiraju uneseni podaci sa forme:
            if (ModelState.IsValid)
            {
                try
                {
                    //Ažuriranje sloga na bazi:
                    _context.Update(leaveType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    //Ako dođe do nekog exceptiona na bazi:
                    if (!LeaveTypeExists(leaveType.Id))
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
            return View(leaveType);
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

            //Pronađi željeni slog za proslijeđivanje viewu:
            var leaveType = await _context.LeaveTypes.FirstOrDefaultAsync(m => m.Id == id);

            //Ako se na bazi ne pronađe slog onda baci 404:
            if (leaveType == null)
            {
                return NotFound();
            }

            //Prikaži view sa pronađenim slogom sa baze:
            return View(leaveType);
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
            //Pronalazak određenog sloga na bazi:
            var leaveType = await _context.LeaveTypes.FindAsync(id);

            //Ako pronađemo željeni slog na bazi:
            if (leaveType != null)
            {
                //onda ga obriši sa baze:
                _context.LeaveTypes.Remove(leaveType);
            }

            //Spremi promjene na bazi:
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Metoda koja provjerava postoji sloga sa određenim ID-em u našoj tablici na bazi
        /// </summary>
        private bool LeaveTypeExists(int id)
        {
            return _context.LeaveTypes.Any(e => e.Id == id);
        }
    }
}
