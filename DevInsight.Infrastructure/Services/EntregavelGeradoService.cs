﻿using AutoMapper;
using DevInsight.Core.DTOs;
using DevInsight.Core.Entities;
using DevInsight.Core.Exceptions;
using DevInsight.Core.Interfaces;
using DevInsight.Core.Interfaces.Services;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.Extensions.Logging;
using Xceed.Words.NET;
using static Amazon.Runtime.Internal.Settings.SettingsCollection;
using ObjectSettings = DinkToPdf.ObjectSettings;
using OfficeOpenXml;
using System.Text;


namespace DevInsight.Infrastructure.Services;

public class EntregavelGeradoService : IEntregavelGeradoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<EntregavelGeradoService> _logger;
    private readonly IStorageService _storageService;
    private readonly IConverter _converter;

    public EntregavelGeradoService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<EntregavelGeradoService> logger,
        IStorageService storageService,
        IConverter converter)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _storageService = storageService;
        _converter = converter;
    }

    public async Task<EntregavelGeradoConsultaDTO> CriarEntregavelAsync(EntregavelGeradoCriacaoDTO entregavelDto, Guid projetoId)
    {
        try
        {
            var projeto = await _unitOfWork.Projetos.GetByIdAsync(projetoId);
            if (projeto == null)
            {
                _logger.LogWarning("Projeto não encontrado: {ProjetoId}", projetoId);
                throw new NotFoundException("Projeto não encontrado");
            }

            var entregavel = _mapper.Map<EntregavelGerado>(entregavelDto);
            entregavel.ProjetoId = projetoId;
            entregavel.CriadoEm = DateTime.UtcNow;

            await _unitOfWork.Entregaveis.AddAsync(entregavel);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Entregável criado com sucesso: {EntregavelId}", entregavel.Id);
            return _mapper.Map<EntregavelGeradoConsultaDTO>(entregavel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar entregável");
            throw;
        }
    }

    public async Task<EntregavelGeradoConsultaDTO> ObterPorIdAsync(Guid id)
    {
        try
        {
            var entregavel = await _unitOfWork.Entregaveis.GetByIdAsync(id);
            if (entregavel == null)
            {
                _logger.LogWarning("Entregável não encontrado: {EntregavelId}", id);
                throw new NotFoundException("Entregável não encontrado");
            }

            var dto = _mapper.Map<EntregavelGeradoConsultaDTO>(entregavel);
            dto.UrlDownload = await _storageService.GetFileUrlAsync($"entregaveis/{entregavel.Id}");

            return dto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter entregável por ID: {EntregavelId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<EntregavelGeradoConsultaDTO>> ListarPorProjetoAsync(Guid projetoId)
    {
        try
        {
            var projeto = await _unitOfWork.Projetos.GetByIdAsync(projetoId);
            if (projeto == null)
            {
                _logger.LogWarning("Projeto não encontrado: {ProjetoId}", projetoId);
                throw new NotFoundException("Projeto não encontrado");
            }

            var entregaveis = (await _unitOfWork.Entregaveis.GetAllAsync())
                .Where(e => e.ProjetoId == projetoId)
                .OrderByDescending(e => e.CriadoEm)
                .ToList();

            var dtos = _mapper.Map<IEnumerable<EntregavelGeradoConsultaDTO>>(entregaveis);

            // Gerar URLs de download para cada entregável
            foreach (var dto in dtos)
            {
                dto.UrlDownload = await _storageService.GetFileUrlAsync($"entregaveis/{dto.Id}");
            }

            return dtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar entregáveis por projeto: {ProjetoId}", projetoId);
            throw;
        }
    }

    public async Task<bool> ExcluirEntregavelAsync(Guid id)
    {
        try
        {
            var entregavel = await _unitOfWork.Entregaveis.GetByIdAsync(id);
            if (entregavel == null)
            {
                _logger.LogWarning("Entregável não encontrado para exclusão: {EntregavelId}", id);
                throw new NotFoundException("Entregável não encontrado");
            }

            await _unitOfWork.Entregaveis.DeleteAsync(entregavel);
            await _unitOfWork.CompleteAsync();

            // Remover arquivo associado no storage
            await _storageService.DeleteFileAsync($"entregaveis/{id}");

            _logger.LogInformation("Entregável excluído com sucesso: {EntregavelId}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir entregável: {EntregavelId}", id);
            throw;
        }
    }

    public async Task<string> GerarUrlDownloadAsync(Guid id)
    {
        try
        {
            var entregavel = await _unitOfWork.Entregaveis.GetByIdAsync(id);
            if (entregavel == null)
            {
                _logger.LogWarning("Entregável não encontrado para gerar URL: {EntregavelId}", id);
                throw new NotFoundException("Entregável não encontrado");
            }

            var url = await _storageService.GetFileUrlAsync($"entregaveis/{id}");
            return url;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar URL de download: {EntregavelId}", id);
            throw;
        }
    }

    public async Task<DadosRelatorioConsultoria> ObterDadosRelatorioConsultoriaAsync(Guid projetoId)
    {
        var projeto = await _unitOfWork.Projetos.GetByIdAsync(projetoId);
        if (projeto == null)
            throw new NotFoundException("Projeto não encontrado");

        // Implemente a lógica para obter todos os dados necessários
        return new DadosRelatorioConsultoria
        {
            NomeProjeto = projeto.Nome,
            Cliente = projeto.Cliente,
            Consultor = projeto.CriadoPor.Nome,
            DataEntrega = projeto.DataEntrega,
            Proposito = projeto.Proposito,
            SituacaoAtual = projeto.SituacaoAtual,
            // Preencha os demais campos conforme necessário
        };
    }

    public Task<byte[]> GeneratePdfFromHtmlAsync(string htmlContent)
    {
        var doc = new HtmlToPdfDocument()
        {
            GlobalSettings = {
            ColorMode = ColorMode.Color,
            Orientation = Orientation.Portrait,
            PaperSize = PaperKind.A4,
            Margins = new MarginSettings { Top = 10, Bottom = 10, Left = 10, Right = 10 }
        },
            Objects = {
            new ObjectSettings() {
                HtmlContent = htmlContent,
                WebSettings = { DefaultEncoding = "utf-8" },
                HeaderSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontSize = 9, Center = "Gerado em: [date]" }
            }
        }
        };

        var pdfBytes = _converter.Convert(doc);
        return Task.FromResult(pdfBytes);
    }

    public Task<byte[]> GenerateDocxFromHtmlAsync(string htmlContent)
    {
        using var memoryStream = new MemoryStream();
        using var document = DocX.Create(memoryStream);

        // Adiciona o conteúdo HTML como parágrafo
        var paragraph = document.InsertParagraph();
        paragraph.Append(htmlContent);

        document.Save();
        return Task.FromResult(memoryStream.ToArray());
    }

    public async Task<string> GenerateMarkdownFromHtmlAsync(string htmlContent)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(htmlContent))
                return string.Empty;

            // Implementação mais robusta de conversão HTML para Markdown
            var markdown = new StringBuilder(htmlContent);

            // Substituições básicas
            markdown.Replace("<h1>", "# ")
                   .Replace("</h1>", "\n\n")
                   .Replace("<h2>", "## ")
                   .Replace("</h2>", "\n\n")
                   .Replace("<h3>", "### ")
                   .Replace("</h3>", "\n\n")
                   .Replace("<p>", "")
                   .Replace("</p>", "\n\n")
                   .Replace("<br>", "\n")
                   .Replace("<br/>", "\n")
                   .Replace("<ul>", "")
                   .Replace("</ul>", "\n")
                   .Replace("<ol>", "")
                   .Replace("</ol>", "\n")
                   .Replace("<li>", "- ")
                   .Replace("</li>", "\n")
                   .Replace("<strong>", "**")
                   .Replace("</strong>", "**")
                   .Replace("<em>", "_")
                   .Replace("</em>", "_");

            // Remover tags HTML restantes
            markdown = new StringBuilder(System.Text.RegularExpressions.Regex.Replace(
                markdown.ToString(), "<.*?>", string.Empty));

            return await Task.FromResult(markdown.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao converter HTML para Markdown");
            throw;
        }
    }

    public async Task<byte[]> GenerateExcelFromDataAsync<T>(IEnumerable<T> data)
    {
        try
        {
            if (data == null || !data.Any())
                return Array.Empty<byte>();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Definir o contexto de licença

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Dados");

            // Obter propriedades do tipo T
            var properties = typeof(T).GetProperties();

            // Adicionar cabeçalhos
            for (int i = 0; i < properties.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = properties[i].Name;
                worksheet.Cells[1, i + 1].Style.Font.Bold = true;
            }

            // Adicionar dados
            var row = 2;
            foreach (var item in data)
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    var value = properties[i].GetValue(item);
                    worksheet.Cells[row, i + 1].Value = value?.ToString() ?? string.Empty;
                }
                row++;
            }

            // Autoajustar colunas
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            return await Task.FromResult(package.GetAsByteArray());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar arquivo Excel");
            throw;
        }
    }
}