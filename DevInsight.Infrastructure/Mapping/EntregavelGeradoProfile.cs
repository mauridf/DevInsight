using AutoMapper;
using DevInsight.Core.DTOs;
using DevInsight.Core.Entities;

namespace DevInsight.Infrastructure.Mapping;

public class EntregavelGeradoProfile : Profile
{
    public EntregavelGeradoProfile()
    {
        CreateMap<EntregavelGeradoCriacaoDTO, EntregavelGerado>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Projeto, opt => opt.Ignore())
            .ForMember(dest => dest.CriadoEm, opt => opt.Ignore());

        CreateMap<EntregavelGerado, EntregavelGeradoConsultaDTO>()
            .ForMember(dest => dest.DataGeracao, opt => opt.MapFrom(src => src.CriadoEm));
    }
}