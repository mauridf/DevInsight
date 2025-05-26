using AutoMapper;
using DevInsight.Core.DTOs;
using DevInsight.Core.Entities;

namespace DevInsight.Infrastructure.Mapping;

public class EntregaFinalProfile : Profile
{
    public EntregaFinalProfile()
    {
        CreateMap<EntregaFinalCriacaoDTO, EntregaFinal>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Projeto, opt => opt.Ignore())
            .ForMember(dest => dest.CriadoEm, opt => opt.Ignore());

        CreateMap<EntregaFinalAtualizacaoDTO, EntregaFinal>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ProjetoId, opt => opt.Ignore())
            .ForMember(dest => dest.Projeto, opt => opt.Ignore())
            .ForMember(dest => dest.CriadoEm, opt => opt.Ignore());

        CreateMap<EntregaFinal, EntregaFinalConsultaDTO>();
    }
}