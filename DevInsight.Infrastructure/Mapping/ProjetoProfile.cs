using AutoMapper;
using DevInsight.Core.DTOs;
using DevInsight.Core.Entities;

namespace DevInsight.Infrastructure.Mapping;

public class ProjetoProfile : Profile
{
    public ProjetoProfile()
    {
        // Mapeamento de Criação
        CreateMap<ProjetoCriacaoDTO, ProjetoConsultoria>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CriadoPor, opt => opt.Ignore())
            .ForMember(dest => dest.StakeHolders, opt => opt.Ignore())
            .ForMember(dest => dest.Funcionalidades, opt => opt.Ignore())
            .ForMember(dest => dest.Requisitos, opt => opt.Ignore())
            .ForMember(dest => dest.Documentos, opt => opt.Ignore())
            .ForMember(dest => dest.Reunioes, opt => opt.Ignore())
            .ForMember(dest => dest.Tarefas, opt => opt.Ignore())
            .ForMember(dest => dest.ValidacoesTecnicas, opt => opt.Ignore())
            .ForMember(dest => dest.Entregas, opt => opt.Ignore())
            .ForMember(dest => dest.Solucoes, opt => opt.Ignore())
            .ForMember(dest => dest.Entregaveis, opt => opt.Ignore());

        // Mapeamento de Atualização
        CreateMap<ProjetoAtualizacaoDTO, ProjetoConsultoria>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CriadoPorId, opt => opt.Ignore())
            .ForMember(dest => dest.CriadoPor, opt => opt.Ignore())
            .ForMember(dest => dest.CriadoEm, opt => opt.Ignore())
            .ForMember(dest => dest.StakeHolders, opt => opt.Ignore())
            .ForMember(dest => dest.Funcionalidades, opt => opt.Ignore())
            .ForMember(dest => dest.Requisitos, opt => opt.Ignore())
            .ForMember(dest => dest.Documentos, opt => opt.Ignore())
            .ForMember(dest => dest.Reunioes, opt => opt.Ignore())
            .ForMember(dest => dest.Tarefas, opt => opt.Ignore())
            .ForMember(dest => dest.ValidacoesTecnicas, opt => opt.Ignore())
            .ForMember(dest => dest.Entregas, opt => opt.Ignore())
            .ForMember(dest => dest.Solucoes, opt => opt.Ignore())
            .ForMember(dest => dest.Entregaveis, opt => opt.Ignore());

        // Mapeamento para ConsultaDTO
        CreateMap<ProjetoConsultoria, ProjetoConsultaDTO>()
            .ForMember(dest => dest.CriadoPorNome,
                       opt => opt.MapFrom(src => src.CriadoPor != null ? src.CriadoPor.Nome : string.Empty));

        // Mapeamento para DetalhesDTO
        CreateMap<ProjetoConsultoria, ProjetoDetalhesDTO>()
            .ForMember(dest => dest.CriadoPor,
                       opt => opt.MapFrom(src => src.CriadoPor))
            .ForMember(dest => dest.TotalStakeholders,
                       opt => opt.MapFrom(src => src.StakeHolders != null ? src.StakeHolders.Count : 0))
            .ForMember(dest => dest.TotalRequisitos,
                       opt => opt.MapFrom(src => src.Requisitos != null ? src.Requisitos.Count : 0))
            .ForMember(dest => dest.TotalTarefas,
                       opt => opt.MapFrom(src => src.Tarefas != null ? src.Tarefas.Count : 0));
    }
}