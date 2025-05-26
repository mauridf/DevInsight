using AutoMapper;
using DevInsight.Core.DTOs;
using DevInsight.Core.Entities;

namespace DevInsight.Infrastructure.Mapping;

public class FuncionalidadeProfile : Profile
{
    public FuncionalidadeProfile()
    {
        CreateMap<FuncionalidadeCriacaoDTO, FuncionalidadeDesejada>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Projeto, opt => opt.Ignore())
            .ForMember(dest => dest.CriadoEm, opt => opt.Ignore());

        CreateMap<FuncionalidadeAtualizacaoDTO, FuncionalidadeDesejada>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ProjetoId, opt => opt.Ignore())
            .ForMember(dest => dest.Projeto, opt => opt.Ignore())
            .ForMember(dest => dest.CriadoEm, opt => opt.Ignore());

        CreateMap<FuncionalidadeDesejada, FuncionalidadeConsultaDTO>();
    }
}