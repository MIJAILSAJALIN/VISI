using AutoMapper;
using VISI.Entidades;
using VISI.Models;

namespace VISI.Servicios
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AlbaranesConDetalle,ListadoAlbaranesViewModel>();
            CreateMap<FacturaViewModel, Factura>();
            CreateMap<Factura, Factura>();
        }

    }
}
