using AutoMapper;
using VISI.Models;

namespace VISI.Servicios
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AlbaranesConDetalle,ListadoAlbaranesViewModel>();
        }

    }
}
