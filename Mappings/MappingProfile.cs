using AutoMapper;
using OGCBackend.Models;
using OGCBackend.DTOs;

namespace OGCBackend.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Partida to PartidaDto
            CreateMap<Partida, PartidaDto>();
            
            // CreatePartidaDto to Partida
            CreateMap<CreatePartidaDto, Partida>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FechaCarga, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.ArchivoOrigen, opt => opt.Ignore());
            
            // ExcelRowDto to Partida
            CreateMap<ExcelRowDto, Partida>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Partida ?? ""))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Total ?? 0))
                .ForMember(dest => dest.Aprobado, opt => opt.MapFrom(src => src.Aprobado ?? 0))
                .ForMember(dest => dest.Pagado, opt => opt.MapFrom(src => src.Pagado ?? 0))
                .ForMember(dest => dest.PorLiquidar, opt => opt.MapFrom(src => src.PorLiquidar ?? 0))
                .ForMember(dest => dest.Actual, opt => opt.MapFrom(src => src.Actual ?? 0))
                .ForMember(dest => dest.FechaCarga, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.ArchivoOrigen, opt => opt.Ignore());
        }
    }
}

