using System.Text.Json;
using System.Collections.Generic;
using DevInsight.Core.DTOs;
using DevInsight.Core.Entities;

namespace DevInsight.Core.Extensions
{
    public static class DadosRelatorioConsultoriaExtensions
    {
        public static Dictionary<string, string> ToDictionary(this DadosRelatorioConsultoria dados)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return new Dictionary<string, string>
            {
                // Informações básicas do projeto
                ["NomeProjeto"] = dados.NomeProjeto ?? string.Empty,
                ["Cliente"] = dados.Cliente ?? string.Empty,
                ["Consultor"] = dados.Consultor ?? string.Empty,
                ["DataEntrega"] = dados.DataEntrega.ToString("dd/MM/yyyy"),
                ["Proposito"] = dados.Proposito ?? string.Empty,
                ["SituacaoAtual"] = dados.SituacaoAtual ?? string.Empty,

                // Seção 1 - Diagnóstico e Levantamento
                ["RequisitosFuncionais"] = JsonSerializer.Serialize(dados.RequisitosFuncionais, options),
                ["RequisitosNaoFuncionais"] = JsonSerializer.Serialize(dados.RequisitosNaoFuncionais, options),
                ["PersonasChaves"] = JsonSerializer.Serialize(dados.PersonasChaves, options),
                ["StakeHolders"] = JsonSerializer.Serialize(dados.StakeHolders, options),
                ["Funcionalidades"] = JsonSerializer.Serialize(dados.Funcionalidades, options),
                ["Documentos"] = JsonSerializer.Serialize(dados.Documentos, options),

                // Seção 2 - Proposta de Solução
                ["SolucaoProposta"] = dados.SolucaoProposta != null ? JsonSerializer.Serialize(dados.SolucaoProposta, options) : string.Empty,
                ["DiagramaSolucao"] = dados.DiagramaSolucao != null ? JsonSerializer.Serialize(dados.DiagramaSolucao, options) : string.Empty,
                ["PrototipoTelas"] = dados.PrototipoTelas != null ? JsonSerializer.Serialize(dados.PrototipoTelas, options) : string.Empty,
                ["Arquitetura"] = dados.SolucaoProposta?.Arquitetura ?? string.Empty,
                ["ComponentesSistema"] = dados.SolucaoProposta?.ComponentesSistema ?? string.Empty,
                ["PontosIntegracao"] = dados.SolucaoProposta?.PontosIntegracao ?? string.Empty,
                ["Riscos"] = dados.SolucaoProposta?.Riscos ?? string.Empty,
                ["RecomendacoesTecnicas"] = dados.SolucaoProposta?.RecomendacoesTecnicas ?? string.Empty,

                // Seção 3 - Planejamento e Roadmap
                ["FasesProjeto"] = JsonSerializer.Serialize(dados.FasesProjeto, options),
                ["EstimativasCusto"] = JsonSerializer.Serialize(dados.EstimativasCusto, options),
                ["TotalEstimado"] = dados.TotalEstimado.ToString("C"),

                // Seção 4 - Validações Técnicas
                ["ValidacoesTecnicas"] = JsonSerializer.Serialize(dados.ValidacoesTecnicas, options),

                // Seção 5 - Documentação Técnica
                ["EntregasFinais"] = JsonSerializer.Serialize(dados.EntregasFinais, options),
                ["EntregaveisGerados"] = JsonSerializer.Serialize(dados.EntregaveisGerados, options),

                // Seção 6 - Checklist Final
                ["Tarefas"] = JsonSerializer.Serialize(dados.Tarefas, options),

                // Seção 7 - Plano de Ação Pós-Consultoria
                ["Recomendacoes"] = dados.RecomendacoesTecnicas ?? string.Empty,

                // Seção 8 - Contato
                ["EmailConsultor"] = dados.EmailConsultor ?? string.Empty
            };
        }
    }
}