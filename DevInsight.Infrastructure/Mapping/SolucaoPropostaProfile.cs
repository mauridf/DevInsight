using AutoMapper;
using DevInsight.Core.DTOs;
using DevInsight.Core.Entities;

namespace DevInsight.Core.Mappings;

public class SolucaoPropostaProfile : Profile
{
    public SolucaoPropostaProfile()
    {
        CreateMap<SolucaoPropostaCriacaoDTO, SolucaoProposta>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Projeto, opt => opt.Ignore())
            .ForMember(dest => dest.CriadoEm, opt => opt.Ignore());

        CreateMap<SolucaoPropostaAtualizacaoDTO, SolucaoProposta>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ProjetoId, opt => opt.Ignore())
            .ForMember(dest => dest.Projeto, opt => opt.Ignore())
            .ForMember(dest => dest.CriadoEm, opt => opt.Ignore());

        CreateMap<SolucaoProposta, SolucaoPropostaConsultaDTO>();
    }
}