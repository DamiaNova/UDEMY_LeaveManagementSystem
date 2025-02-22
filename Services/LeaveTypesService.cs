using AutoMapper;
using LeaveManagementSystem.Web.Data;
using LeaveManagementSystem.Web.Models.LeaveTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Services;

/// <summary>
/// Service layer za tablicu LEAVE_TYPES
/// Dependency injectali smo mapper i dbcontext odmah ispod:
/// </summary>
public class LeaveTypesService(IMapper _mapper, ApplicationDbContext _context) : ILeaveTypesService
{
    #region Fields, properties

    private const string _nameExistsValidationMessage = "This leave type already exists in the database!";

    #endregion

    #region Methods za rad sa bazom podataka (CRUD operacije)

    /// <summary>
    /// Dohvat svih slogova iz tablice LEAVE_TYPE
    /// </summary>
    public async Task<IEnumerable<LeaveTypeReadOnlyVM>> GetAllRecords()
    {
        //select * from LEAVE_TYPE;
        var dbData = await _context.LeaveTypes.ToListAsync();

        //Mapping to view-model:
        var viewData = _mapper.Map<List<LeaveTypeReadOnlyVM>>(dbData);

        return viewData;
    }

    /// <summary>
    /// Asinkrona metoda koja vraća određeni slog iz tablice sa baze
    /// </summary>
    public async Task<T?> GetRecordAsync<T>(int id) where T : class
    {
        //select * from LEAVE_TYPE where id = i_id;
        var dataModel = await _context.LeaveTypes.FirstOrDefaultAsync(x => x.Id.Equals(id));

        //Ako nema sloga sa zadanim ID-om na bazi onda baci 404:
        if (dataModel == null)
        {
            return (T?)null;
        }

        //Mapiranje iz data-modela u view-model za prikaz na razor viewu:
        var viewModel = _mapper.Map<T>(dataModel);

        return viewModel;
    }

    /// <summary>
    /// Asinkrona metoda koja briše određeni slog iz tablice sa baze sa željenim ID-em
    /// </summary>
    public async Task DeteleRecordAsync(int id)
    {
        //Pronalazak određenog sloga na bazi:
        var recordForDelete = await _context.LeaveTypes.FirstOrDefaultAsync(x => x.Id.Equals(id));

        //Ako pronađemo željeni slog na bazi:
        if (recordForDelete != null)
        {
            //onda ga obriši sa baze:
            _context.LeaveTypes.Remove(recordForDelete);

            //Spremi promjene na bazi:
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Asinkrona metoda za ažuriranje sloga u tablici na bazi
    /// </summary>
    public async Task UpdateRecordInDatabase(LeaveTypeEditVM viewModelRecord)
    {
        //VALIDACIJA?

        #region Spremanje na bazu
        //Mapiranje iz view-modela u data-model:
        var dataModelRecord = _mapper.Map<LeaveType>(viewModelRecord);

        //Ažuriranje sloga na bazi:
        _context.Update(dataModelRecord);

        //Spremanje promjena na bazi:
        await _context.SaveChangesAsync();
        #endregion
    }

    /// <summary>
    /// Asinkrona metoda za insert sloga u tablicu na bazi
    /// </summary>
    public async Task CreateRecordInDatabase(LeaveTypeCreateVM viewModelRecord)
    {
        //VALIDACIJA?

        #region Spremanje na bazu
        //Mapiranje iz view-modela u data-model:
        var dataModelRecord = _mapper.Map<LeaveType>(viewModelRecord);

        //Insertanje sloga na bazu:
        _context.Add(dataModelRecord);

        //Spremanje promjena na bazi:
        await _context.SaveChangesAsync();
        #endregion
    }
    #endregion

    #region Methods za bussiness logiku

    /// <summary>
    /// Metoda za provjeru postoji li već tip odsustva s posla u bazi podataka
    /// </summary>
    public async Task<bool> CheckIfLeaveTypeNameExists(string name)
    {
        var nameLowercase = name.ToLower();

        //Dohvat određenog podatka sa baze:
        //SELECT * FROM LeaveTypes WHERE Name = 'Bolovanje';
        return await _context.LeaveTypes.AnyAsync(x => x.Name.ToLower().Equals(nameLowercase));
    }

    /// <summary>
    /// TRUE = Leave type objekt sa tim nazivom VEĆ postoji na bazi i ima drugačiji ID od sloga kojeg mi pokušavamo ažurirati
    /// </summary>
    public async Task<bool> NameAlreadyExistsInTheDatabaseUnderDifferentID(LeaveTypeEditVM editedObject)
    {
        //Ako se mijenja ID=5 gdje je naziv="Vacation" u naziv="Bolovanje" ali Bolovanje (sa ID=7) već postoji na bazi --> vrati TRUE!
        var editedNameLowercase = editedObject.Name.ToLower();

        //Ako se na bazi pronađe slog sa istim nazivom ALI drugačijim ID-em onda se vraća TRUE:
        return await _context.LeaveTypes.AnyAsync(x => x.Name.ToLower().Equals(editedNameLowercase) && x.Id != editedObject.Id);
    }

    /// <summary>
    /// Metoda koja provjerava postoji sloga sa određenim ID-em u našoj tablici na bazi
    /// </summary>
    public bool LeaveTypeExists(int id)
    {
        return (_context.LeaveTypes.Any(e => e.Id == id));
    }
    #endregion
}
