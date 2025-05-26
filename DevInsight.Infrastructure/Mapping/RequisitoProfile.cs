using AutoMapper;
using DevInsight.Core.DTOs;
using DevInsight.Core.Entities;

namespace DevInsight.Infrastructure.Mapping;

public class RequisitoProfile : Profile
{
    public RequisitoProfile()
    {
        CreateMap<RequisitoCriacaoDTO, Requisito>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Projeto, opt => opt.Ignore())
            .ForMember(dest => dest.CriadoEm, opt => opt.Ignore());

        CreateMap<RequisitoAtualizacaoDTO, Requisito>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ProjetoId, opt => opt.Ignore())
            .ForMember(dest => dest.Projeto, opt => opt.Ignore())
            .ForMember(dest => dest.CriadoEm, opt => opt.Ignore());

        CreateMap<Requisito, RequisitoConsultaDTO>();
    }
}