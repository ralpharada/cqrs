using Tellus.Application.Contracts;
using Tellus.Application.Core;
using Tellus.Application.Services;
using Tellus.Application.Util;
using Tellus.Domain.Interfaces;
using Tellus.Infra.Repositories;
using System.Net;
using System.Net.Mail;

namespace Tellus.API.DependencyMap
{
    public static class RepositoryDependencyMap
    {
        public static void RepositoryMap(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<UsuarioAutenticado>();
            services.AddSingleton<IJwtService,JwtService>();
            services.AddScoped<Application.Libraries.Cookie>();
            services.AddScoped<EnviarEmail>();
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IUsuarioAdmRepository, UsuarioAdmRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<ITipoDocumentoRepository, TipoDocumentoRepository>();
            services.AddScoped<IIndiceRepository, IndiceRepository>();
            services.AddScoped<ISMTPRepository, SMTPRepository>();
            services.AddScoped<IGrupoUsuarioRepository, GrupoUsuarioRepository>();
            services.AddScoped<IDocumentoRepository, DocumentoRepository>();
            services.AddScoped<ILogDocumentoRepository, LogDocumentoRepository>();
            services.AddScoped<ILogClienteRepository, LogClienteRepository>();
            services.AddScoped<ITipoDocumentoGrupoRepository, TipoDocumentoGrupoRepository>();
            services.AddScoped<IAmazonRepository, AmazonRepository>();
            services.AddScoped<IUsuarioLogadoRepository, UsuarioLogadoRepository>();
            services.AddScoped<IPermissaoRepository, PermissaoRepository>();
            services.AddScoped<IVinculoRepository, VinculoRepository>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IContabilizaConsultaRepository, ContabilizaConsultaRepository>();
            services.AddScoped<ILogRepository, LogRepository>();

            services.AddScoped<SmtpClient>(options =>
            {
                SmtpClient smtpClient = new()
                {
                    Host = configuration.GetValue<string>("Email:ServerSMTP"),
                    Port = configuration.GetValue<int>("Email:ServerPort"),
                    UseDefaultCredentials = configuration.GetValue<bool>("Email:UseDefaultCredentials"),
                    Credentials = new NetworkCredential(configuration.GetValue<string>("Email:Username"), configuration.GetValue<string>("Email:Password")),
                    EnableSsl = configuration.GetValue<bool>("Email:EnableSsl")
                };
                return smtpClient;
            });
        }
    }
}
