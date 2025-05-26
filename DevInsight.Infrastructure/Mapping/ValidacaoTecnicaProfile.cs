using AutoMapper;
using DevInsight.Core.DTOs;
using DevInsight.Core.Entities;

namespace DevInsight.Infrastructure.Mapping;

public class ValidacaoTecnicaProfile : Profile
{
    public ValidacaoTecnicaProfile()
    {
        CreateMap<ValidacaoTecnicaCriacaoDTO, ValidacaoTecnica>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Projeto, opt => opt.Ignore())
            .ForMember(dest => dest.Validado, opt => opt.Ignore())
            .ForMember(dest => dest.CriadoEm, opt => opt.Ignore());

        CreateMap<ValidacaoTecnicaAtualizacaoDTO, ValidacaoTecnica>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ProjetoId, opt => opt.Ignore())
            .ForMember(dest => dest.Projeto, opt => opt.Ignore())
            .ForMember(dest => dest.CriadoEm, opt => opt.Ignore());

        CreateMap<ValidacaoTecnica, ValidacaoTecnicaConsultaDTO>();
    }
}