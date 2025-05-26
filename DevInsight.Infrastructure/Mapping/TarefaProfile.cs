using AutoMapper;
using DevInsight.Core.DTOs;
using DevInsight.Core.Entities;

namespace DevInsight.Infrastructure.Mapping;

public class TarefaProfile : Profile
{
    public TarefaProfile()
    {
        CreateMap<TarefaCriacaoDTO, TarefaProjeto>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Projeto, opt => opt.Ignore())
            .ForMember(dest => dest.CriadoEm, opt => opt.Ignore());

        CreateMap<TarefaAtualizacaoDTO, TarefaProjeto>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ProjetoId, opt => opt.Ignore())
            .ForMember(dest => dest.Projeto, opt => opt.Ignore())
            .ForMember(dest => dest.CriadoEm, opt => opt.Ignore());

        CreateMap<TarefaProjeto, TarefaConsultaDTO>();
        CreateMap<TarefaProjeto, TarefaKanbanDTO>();
    }
}