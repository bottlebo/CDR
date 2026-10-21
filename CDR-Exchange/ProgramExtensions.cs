using System.Text;
using CDR_Exchange.Dal;
using CDR_Exchange.Infrastructure.Configs;
using CDR_Exchange.Services;
using CDR_Exchange.Services.Cil;
using CDR_Exchange.Services.Cil.Interfaces;
using CDR_Exchange.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

namespace CDR_Exchange;

public static class ProgramExtensions
{
    private const string DbCtxName = "DefaultConnection";

    public static void AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        // builder.Services.AddSwaggerGen();
        builder.Services.AddSwaggerGen(c => 
            c.CustomOperationIds(d => d.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor 
                ? controllerActionDescriptor.MethodInfo.Name 
                : d.ActionDescriptor.AttributeRouteInfo?.Name));
    }
    
    public static void AddDbCtx(this WebApplicationBuilder builder)
    {
        var dbConnectionString = builder.Configuration.GetConnectionString(DbCtxName);
        builder.Services.AddDbContext<ExchangeDbContext>(x => x.UseNpgsql(dbConnectionString), ServiceLifetime.Transient);
    }
    
    public static void AddConfiguration(this WebApplicationBuilder builder)
    {
        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        
        var configBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile($"appsettings.{environmentName}.json", true);
        
        configBuilder.AddEnvironmentVariables();
        builder.Configuration.AddConfiguration(configBuilder.Build());
    }
    
    public static void AddHttpClients(this WebApplicationBuilder builder)
    {
        var cilWebApiOptions = builder.Configuration.GetSection("CilWebApi").Get<CilWebApiConfig>();
        var cilExplorerApiOptions = builder.Configuration.GetSection("CilExplorerApi").Get<CilExplorerApiConfig>();
        var rpcOptions = builder.Configuration.GetSection("Rpc").Get<RpcConfig>();

        builder.Services.AddHttpClient<ICilWebApi, CilWebApi>(client =>
        {
            client.BaseAddress = new Uri(cilWebApiOptions?.Url!);
            client.DefaultRequestHeaders.Add("X-CIL-SWAP", cilWebApiOptions?.XCilSwap!);
        });
        
        builder.Services.AddHttpClient<ICilExplorerApi, CilExplorerApi>(client =>
        {
            client.BaseAddress = new Uri(cilExplorerApiOptions?.Url!);
            client.DefaultRequestHeaders.Add(HeaderNames.Authorization, GetAuthHeader(rpcOptions!));
        });
    }
    
    private static string GetAuthHeader(RpcConfig config) =>
        $"Basic {Convert.ToBase64String(Encoding.UTF8.GetBytes($"{config.UserName}:{config.Password}"))}";

    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<ITransitService, TransitService>();
        builder.Services.AddTransient<IPairsService, PairsService>();
        builder.Services.AddTransient<ITransactionService, TransactionService>();
    }

    public static void AddCors(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(opt => opt.AddDefaultPolicy(
            b =>
            {
                b.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));
    }
    
    /// <summary>
    /// Migrate database
    /// </summary>
    /// <param name="app"></param>
    public static void Migrate(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        using var context = scope.ServiceProvider.GetService<ExchangeDbContext>();
        context!.Database.Migrate();
    }
}