using System.Text;
using System.Text.Json;
using Amazon.S3;
using DevInsight.Core.Interfaces;
using DevInsight.Core.Interfaces.Services;
using DevInsight.Core.Services;
using DevInsight.Infrastructure.Data;
using DevInsight.Infrastructure.Services;
using DinkToPdf.Contracts;
using DinkToPdf;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

try
{
    // Configuração inicial do logger
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteTo.Console()
        .WriteTo.File("logs/startup-log.txt", rollingInterval: RollingInterval.Day)
        .CreateBootstrapLogger();

    Log.Information("Iniciando a aplicação DevInsight");

    var builder = WebApplication.CreateBuilder(args);

    // Configuração do Serilog como provedor de logging principal
    builder.Host.UseSerilog((context, services, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration));

    // Add services to the container.
    builder.Services.AddControllers();

    // Configure DbContext
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
            npgsqlOptions =>
            {
                npgsqlOptions.UseNodaTime();
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorCodesToAdd: null);
            }));

    // Configuração do Storage
    if (builder.Configuration.GetValue<bool>("UseAWSStorage"))
    {
        builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
        builder.Services.AddAWSService<IAmazonS3>();
        builder.Services.AddScoped<IStorageService, StorageService>();
    }
    else
    {
        builder.Services.AddScoped<IStorageService, LocalStorageService>();
    }

    // Registrar UnitOfWork
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

    // Registrar AutoMapper
    builder.Services.AddAutoMapper(typeof(Program).Assembly);
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    // Registrar Services
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<IProjetoService, ProjetoService>();
    builder.Services.AddScoped<IStakeHolderService, StakeHolderService>();
    builder.Services.AddScoped<IFuncionalidadeService, FuncionalidadeService>();
    builder.Services.AddScoped<IRequisitoService, RequisitoService>();
    builder.Services.AddScoped<IDocumentoLinkService, DocumentoLinkService>();
    builder.Services.AddScoped<IReuniaoService, ReuniaoService>();
    builder.Services.AddScoped<ITarefaService, TarefaService>();
    builder.Services.AddScoped<IValidacaoTecnicaService, ValidacaoTecnicaService>();
    builder.Services.AddScoped<IEntregaFinalService, EntregaFinalService>();
    builder.Services.AddScoped<ISolucaoPropostaService, SolucaoPropostaService>();
    builder.Services.AddScoped<IEntregavelGeradoService, EntregavelGeradoService>();
    builder.Services.AddScoped<IUsuarioService, UsuarioService>();
    builder.Services.AddScoped<IDocumentGeneratorService, DocumentGeneratorService>();
    builder.Services.AddScoped<IPersonaChave, PersonaChaveService>();
    builder.Services.AddScoped<IFaseProjetoService, FaseProjetoService>();
    builder.Services.AddScoped<IEstimativaCusto, EstimativaCustoService>();
    builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

    // Configuração CORS
    var corsSettings = builder.Configuration.GetSection("CorsSettings");
    var allowedOrigins = corsSettings["AllowedOrigins"]?.Split(';') ?? Array.Empty<string>();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("DevInsightPolicy", policy =>
        {
            if (builder.Environment.IsDevelopment() || allowedOrigins.Length == 0)
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            }
            else
            {
                policy.WithOrigins(allowedOrigins)
                      .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS", "PATCH")
                      .WithHeaders("Authorization", "Content-Type", "Accept", "X-Requested-With")
                      .AllowCredentials();
            }
        });
    });

    // Configurar autenticação JWT
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
        });

    // Configuração do Swagger
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "DevInsight API", Version = "v1" });

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header
                },
                new List<string>()
            }
        });
    });

    var app = builder.Build();

    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "LocalStorage")),
        RequestPath = "/storage"
    });

    // Middleware de tratamento de erros global
    app.UseExceptionHandler(exceptionHandlerApp =>
    {
        exceptionHandlerApp.Run(async context =>
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var contextFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
            if (contextFeature != null)
            {
                Log.Error(contextFeature.Error, "Erro não tratado");

                await context.Response.WriteAsync(new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "Ocorreu um erro inesperado. Tente novamente mais tarde.",
                    Detailed = app.Environment.IsDevelopment() ? contextFeature.Error.Message : null
                }.ToString());
            }
        });
    });

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseDeveloperExceptionPage();
    }

    app.UseExceptionHandler("/error");
    app.UseStatusCodePages(async context =>
    {
        if (context.HttpContext.Response.StatusCode == 403)
        {
            context.HttpContext.Response.ContentType = "application/json";
            await context.HttpContext.Response.WriteAsync(
                JsonSerializer.Serialize(new { error = "Acesso negado" }));
        }
    });

    app.UseHttpsRedirection();
    app.UseCors("DevInsightPolicy");
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    // Apply migrations automatically (only for development)
    try
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<ApplicationDbContext>();

            Log.Information("Aplicando migrações do banco de dados...");
            context.Database.Migrate();
            Log.Information("Migrações aplicadas com sucesso");
        }
    }
    catch (Exception ex)
    {
        Log.Fatal(ex, "Falha ao aplicar migrações do banco de dados");
        throw;
    }

    Log.Information("Aplicação iniciada com sucesso");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Aplicação encerrada devido a uma exceção não tratada");
}
finally
{
    Log.Information("Encerrando aplicação...");
    Log.CloseAndFlush();
}