using DevInsight.Core.DTOs;
using DevInsight.Core.Enums;
using System.Text;
using Xceed.Document.NET;

namespace DevInsight.Infrastructure.Services;

public class RelatorioConsultoriaTemplateProcessor
{
    public RelatorioConsultoriaTemplateResult ProcessarTemplate(DadosRelatorioConsultoria dados)
    {
        var htmlBuilder = new StringBuilder();
        var rawBuilder = new StringBuilder();

        // 1. Cabeçalho
        htmlBuilder.AppendLine($@"<!DOCTYPE html>
            <html>
            <head>
                <meta charset=""UTF-8"">
                <title>Relatório de Consultoria - {dados.NomeProjeto}</title>
                <style>
                    body {{ font-family: Arial, sans-serif; line-height: 1.6; }}
                    h1, h2 {{ color: #2c3e50; }}
                    table {{ width: 100%; border-collapse: collapse; margin-bottom: 20px; }}
                    th, td {{ border: 1px solid #ddd; padding: 8px; text-align: left; }}
                    th {{ background-color: #f2f2f2; }}
                    .completed {{ color: green; }}
                    .pending {{ color: orange; }}
                </style>
            </head>
            <body>
            <h1>Relatório de Consultoria em Desenvolvimento de Sistemas - {dados.NomeProjeto}</h1>");

        rawBuilder.AppendLine($"Relatório de Consultoria em Desenvolvimento de Sistemas - {dados.NomeProjeto}");
        rawBuilder.AppendLine(new string('=', 80));

        // 2. Seção de Informações do Cliente
        htmlBuilder.AppendLine($"""
            <div>
                <h2>Cliente: {dados.Cliente}</h2>
                <p><strong>Consultor:</strong> {dados.Consultor}</p>
                <p><strong>Data da Entrega:</strong> {dados.DataEntrega:dd/MM/yyyy}</p>
            </div>
            """);

        rawBuilder.AppendLine($"## Cliente: {dados.Cliente}");
        rawBuilder.AppendLine($"**Consultor:** {dados.Consultor}");
        rawBuilder.AppendLine($"**Data da Entrega:** {dados.DataEntrega:dd/MM/yyyy}");
        rawBuilder.AppendLine("**Escopo da Consultoria:**");
        rawBuilder.AppendLine(new string('-', 80));

        // 3. Seção 1 - Diagnóstico e Levantamento
        htmlBuilder.AppendLine($"""
            <h2>1. 🔍 Diagnóstico e Levantamento</h2>
            <h3>1.1 Objetivo do Projeto</h3>
            <p>{dados.Proposito}</p>
            """);

        htmlBuilder.AppendLine($"""
            <h3>1.2 Situação Atual</h3>
            <p>{dados.SituacaoAtual}</p>
            """);

        // Requisitos Funcionais
        htmlBuilder.AppendLine($"""
            < h3>1.3 Requisitos Funcionais</h3>
            <ul>
            """);

        foreach (var req in dados.RequisitosFuncionais)
        {
            htmlBuilder.AppendLine($"<li>{req.Descricao}</li>");
        }
        htmlBuilder.AppendLine("</ul>");

        // Requisitos Não Funcionais
        htmlBuilder.AppendLine("""
            
            <h3>1.4 Requisitos Não Funcionais</h3>
            <ul>
            
            """);

        foreach (var req in dados.RequisitosNaoFuncionais)
        {
            htmlBuilder.AppendLine($"<li>{req.Descricao}</li>");
        }
        htmlBuilder.AppendLine("</ul>");

        // Personas
        htmlBuilder.AppendLine("""
            
            <h3>1.5 Personas e Usuários-Chave</h3>
            <table>
                <tr>
                    <th>Persona</th>
                    <th>Perfil</th>
                    <th>Necessidade Principal</th>
                </tr>
            
            """);

        foreach (var persona in dados.PersonasChaves)
        {
            htmlBuilder.AppendLine($"""
                <tr>
                    <td>{persona.Persona}</td>
                    <td>{persona.Perfil}</td>
                    <td>{persona.Necessidade}</td>
                </tr>
                """);
        }
        htmlBuilder.AppendLine("</table>");

        // Versão raw/texto
        rawBuilder.AppendLine("## 1. 🔍 Diagnóstico e Levantamento");
        rawBuilder.AppendLine("### 1.1 Objetivo do Projeto");
        rawBuilder.AppendLine(dados.Proposito);
        rawBuilder.AppendLine();
        rawBuilder.AppendLine("### 1.2 Situação Atual");
        rawBuilder.AppendLine(dados.SituacaoAtual);
        rawBuilder.AppendLine();
        rawBuilder.AppendLine("### 1.3 Requisitos Funcionais");
        foreach (var req in dados.RequisitosFuncionais)
        {
            rawBuilder.AppendLine($"- {req.Descricao}");
        }
        rawBuilder.AppendLine();
        rawBuilder.AppendLine("### 1.4 Requisitos Não Funcionais");
        foreach (var req in dados.RequisitosNaoFuncionais)
        {
            rawBuilder.AppendLine($"- {req.Descricao}");
        }
        rawBuilder.AppendLine();
        rawBuilder.AppendLine("### 1.5 Personas e Usuários-Chave");
        rawBuilder.AppendLine("| Persona               | Perfil                       | Necessidade Principal              |");
        rawBuilder.AppendLine("|-----------------------|------------------------------|------------------------------------|");
        foreach (var persona in dados.PersonasChaves)
        {
            rawBuilder.AppendLine($"| {persona.Persona,-21} | {persona.Perfil,-28} | {persona.Necessidade,-34} |");
        }

        // 4. Seção 2 - Proposta de Solução
        htmlBuilder.AppendLine("""
            
            <h2>2. 🧩 Proposta de Solução</h2>
            <h3>2.1 Arquitetura Sugerida</h3>
            <p>
            
            "{ dados.SolucaoProposta?.Arquitetura} "</p>" +
            """);

        if (dados.DiagramaSolucao != null)
        {
            htmlBuilder.AppendLine($"""
                <h3>2.2 Diagrama da Solução</h3>
                <p><a href="{dados.DiagramaSolucao.Url}">{dados.DiagramaSolucao.Descricao}</a></p>
                """);
        }

        if (dados.PrototipoTelas != null)
        {
            htmlBuilder.AppendLine($"""
                <h3>2.3 Protótipo de Telas</h3>
                <p><a href="{dados.PrototipoTelas.Url}">{dados.PrototipoTelas.Descricao}</a></p>
                """);
        }

        // Versão raw/texto
        rawBuilder.AppendLine();
        rawBuilder.AppendLine("## 2. 🧩 Proposta de Solução");
        rawBuilder.AppendLine("### 2.1 Arquitetura Sugerida");
        rawBuilder.AppendLine(dados.SolucaoProposta?.Arquitetura ?? "N/A");
        if (dados.DiagramaSolucao != null)
        {
            rawBuilder.AppendLine();
            rawBuilder.AppendLine($"### 2.2 Diagrama da Solução");
            rawBuilder.AppendLine($"{dados.DiagramaSolucao.Descricao} - {dados.DiagramaSolucao.Url}");
        }
        if (dados.PrototipoTelas != null)
        {
            rawBuilder.AppendLine();
            rawBuilder.AppendLine($"### 2.3 Protótipo de Telas");
            rawBuilder.AppendLine($"{dados.PrototipoTelas.Descricao} - {dados.PrototipoTelas.Url}");
        }

        // 5. Seção 3 - Planejamento e Roadmap
        htmlBuilder.AppendLine("""
            
            <h2>3. 📆 Planejamento e Roadmap</h2>
            <h3>3.1 Fases do Projeto</h3>
            <table>
                <tr>
                    <th>Fase</th>
                    <th>Objetivo</th>
                    <th>Duração Estimada</th>
                </tr>
            
            """);

        foreach (var fase in dados.FasesProjeto)
        {
            htmlBuilder.AppendLine($"""
                <tr>
                    <td>{fase.Fase}</td>
                    <td>{fase.Objetivo}</td>
                    <td>{fase.DuracaoEstimada} semana(s)</td>
                </tr>
                """);
        }
        htmlBuilder.AppendLine("</table>");

        // Estimativa de Custo
        htmlBuilder.AppendLine("""
            
            <h3>3.2 Estimativa de Esforço e Custo</h3>
            <table>
                <tr>
                    <th>Item</th>
                    <th>Estimativa Horas</th>
                    <th>Valor/Hora</th>
                    <th>Custo Estimado</th>
                </tr>
            
            """);

        foreach (var estimativa in dados.EstimativasCusto)
        {
            htmlBuilder.AppendLine($"""
                <tr>
                    <td>{estimativa.Item}</td>
                    <td>{estimativa.EstimativaHoras}h</td>
                    <td>R$ {estimativa.ValorHoras:N2}/h</td>
                    <td>R$ {estimativa.CustoEstimado:N2}</td>
                </tr>
                """);
        }
        htmlBuilder.AppendLine($"""
                <tr>
                    <td colspan="3"><strong>Total estimado:</strong></td>
                    <td><strong>R$ {dados.TotalEstimado:N2}</strong></td>
                </tr>
            </table>
            """);

        // Versão raw/texto
        rawBuilder.AppendLine();
        rawBuilder.AppendLine("## 3. 📆 Planejamento e Roadmap");
        rawBuilder.AppendLine("### 3.1 Fases do Projeto");
        rawBuilder.AppendLine("| Fase               | Objetivo                            | Duração Estimada                        |");
        rawBuilder.AppendLine("|--------------------|-------------------------------------|-----------------------------------------|");
        foreach (var fase in dados.FasesProjeto)
        {
            rawBuilder.AppendLine($"| {fase.Fase,-18} | {fase.Objetivo,-35} | {fase.DuracaoEstimada} semana(s) |");
        }

        rawBuilder.AppendLine();
        rawBuilder.AppendLine("### 3.2 Estimativa de Esforço e Custo");
        rawBuilder.AppendLine("| Item                    | Estimativa Horas                   | Valor/Hora                        | Custo Estimado                     |");
        rawBuilder.AppendLine("|-------------------------|------------------------------------|-----------------------------------|------------------------------------|");
        foreach (var estimativa in dados.EstimativasCusto)
        {
            rawBuilder.AppendLine($"| {estimativa.Item,-23} | {estimativa.EstimativaHoras}h | R$ {estimativa.ValorHoras:N2}/h | R$ {estimativa.CustoEstimado:N2} |");
        }
        rawBuilder.AppendLine($"**Total estimado:** R$ {dados.TotalEstimado:N2}");

        // 6. Seção 4 - Validações Técnicas
        if (dados.ValidacoesTecnicas.Any())
        {
            htmlBuilder.AppendLine("""
                
                <h2>4. 🧪 Validações Técnicas</h2>
                
                """);

            foreach (var validacao in dados.ValidacoesTecnicas)
            {
                htmlBuilder.AppendLine($"""
                    <h3>4.1 {validacao.Tipo}</h3>
                    <p>{validacao.Descricao}</p>
                    <p>{validacao.Observacao}</p>
                    """);
            }

            // Versão raw/texto
            rawBuilder.AppendLine();
            rawBuilder.AppendLine("## 4. 🧪 Validações Técnicas");
            foreach (var validacao in dados.ValidacoesTecnicas)
            {
                rawBuilder.AppendLine($"### 4.1 {validacao.Tipo}");
                rawBuilder.AppendLine(validacao.Descricao);
                if (!string.IsNullOrEmpty(validacao.Observacao))
                {
                    rawBuilder.AppendLine($"> {validacao.Observacao}");
                }
            }
        }

        // 7. Seção 5 - Documentação Técnica
        if (dados.EntregasFinais.Any())
        {
            htmlBuilder.AppendLine("""
                
                <h2>5. 📑 Documentação Técnica</h2>
                <ul>
                
                """);

            foreach (var entrega in dados.EntregasFinais)
            {
                htmlBuilder.AppendLine($"<li>{entrega.Titulo}</li>");
            }
            htmlBuilder.AppendLine("</ul>");

            // Versão raw/texto
            rawBuilder.AppendLine();
            rawBuilder.AppendLine("## 5. 📑 Documentação Técnica");
            foreach (var entrega in dados.EntregasFinais)
            {
                rawBuilder.AppendLine($"- {entrega.Titulo}");
            }
        }

        // 8. Seção 6 - Checklist Final
        if (dados.Tarefas.Any())
        {
            htmlBuilder.AppendLine("""
                
                <h2>6. 🗂️ Checklist Final</h2>
                <table>
                    <tr>
                        <th>Item</th>
                        <th>Status</th>
                        <th>Observações</th>
                    </tr>
                
                """);

            foreach (var tarefa in dados.Tarefas)
            {
                var statusIcon = tarefa.Status == StatusTarefa.Feito ? "✅" : "🟡";
                htmlBuilder.AppendLine($"""
                    <tr>
                        <td>{tarefa.Titulo}</td>
                        <td>{statusIcon} {tarefa.Status}</td>
                        <td>{tarefa.Observacoes}</td>
                    </tr>
                    """);
            }
            htmlBuilder.AppendLine("</table>");

            // Versão raw/texto
            rawBuilder.AppendLine();
            rawBuilder.AppendLine("## 6. 🗂️ Checklist Final");
            rawBuilder.AppendLine("| Item                         | Status                | Observações               |");
            rawBuilder.AppendLine("|------------------------------|-----------------------|---------------------------|");
            foreach (var tarefa in dados.Tarefas)
            {
                var statusIcon = tarefa.Status == StatusTarefa.Feito ? "✅" : "🟡";
                rawBuilder.AppendLine($"| {tarefa.Titulo,-28} | {statusIcon} {tarefa.Status,-18} | {tarefa.Observacoes,-25} |");
            }
        }

        // 9. Seção 7 - Plano de Ação Pós-Consultoria
        if (!string.IsNullOrEmpty(dados.RecomendacoesTecnicas))
        {
            htmlBuilder.AppendLine($"""
                <h2>7. 🎯 Plano de Ação Pós-Consultoria</h2>
                <p>Recomendações de próximos passos:</p>
                <p>{dados.RecomendacoesTecnicas}</p>
                """);

            // Versão raw/texto
            rawBuilder.AppendLine();
            rawBuilder.AppendLine("## 7. 🎯 Plano de Ação Pós-Consultoria");
            rawBuilder.AppendLine("Recomendações de próximos passos:");
            rawBuilder.AppendLine(dados.RecomendacoesTecnicas);
        }

        // 10. Seção 8 - Contato e Agradecimentos
        htmlBuilder.AppendLine($"""
            <h2>8. 📬 Contato e Agradecimentos</h2>
            <p><strong>Consultor:</strong> {dados.Consultor}</p>
            <p><strong>E-mail:</strong> {dados.EmailConsultor}</p>
            """);

        // Finalizar HTML
        htmlBuilder.AppendLine("</body></html>");

        // Versão raw/texto
        rawBuilder.AppendLine();
        rawBuilder.AppendLine("## 8. 📬 Contato e Agradecimentos");
        rawBuilder.AppendLine($"**Consultor:** {dados.Consultor}");
        rawBuilder.AppendLine($"**E-mail:** {dados.EmailConsultor}");

        return new RelatorioConsultoriaTemplateResult
        {
            HtmlContent = htmlBuilder.ToString(),
            RawContent = rawBuilder.ToString()
        };
    }
}