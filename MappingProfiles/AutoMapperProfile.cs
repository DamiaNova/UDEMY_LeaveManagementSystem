using AutoMapper;
using LeaveManagementSystem.Web.Data;
using LeaveManagementSystem.Web.Models.LeaveTypes;

namespace LeaveManagementSystem.Web.MappingProfiles
{
    /// <summary>
    /// Izvedena klasa (bazna klasa: Profile iz Automapper namespacea) za korištenje Automappera
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        /// <summary>
        /// Custom-konstruktor
        /// </summary>
        public AutoMapperProfile()
        {
            //Kreiranje mapiranja između data-model klase i view-model klase
            //Automatikom se matchaju tipovi i imena (ako su isti), no u slučaju kada
            //unutar našeg view-modela nemamo isti naziv propertyja koji je u data-modelu
            //tada koristimo metodu ForMember kojom Automapperu kažemo koji properties su povezani:
            CreateMap<LeaveType, IndexVM>()
                .ForMember(dest => dest.Days, opt => opt.MapFrom(src => src.NumberOfDays));
        }
    }
}
