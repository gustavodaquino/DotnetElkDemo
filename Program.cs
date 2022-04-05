using DotnetElkDemo.Options;
using Elasticsearch.Net;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace DotnetElkDemo;

public static class Program
{
    public static int Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        try
        {
            Log.Information("Starting web host");
            CreateHostBuilder(args).Build().Run();
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .UseSerilog((context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .Enrich.WithEnvironmentName()
                .Enrich.WithMachineName()
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
                .WriteTo.Elasticsearch(
                    BuildElasticSearchSinkOptions(context.Configuration)
                )
            )
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }

    /// <summary>
    ///     Configura o sink para enviar os dados para um cluster configurado do Elastic Cloud.
    /// </summary>
    /// <param name="configuration">Fontes de configurações do .NET.</param>
    private static ElasticsearchSinkOptions BuildElasticSearchSinkOptions(IConfiguration configuration)
    {
        var elasticCloudOptions = configuration.GetSection("ElasticCloud").Get<ElasticCloudOptions>();

        return new ElasticsearchSinkOptions(new CloudConnectionPool(elasticCloudOptions.CloudId,
            new BasicAuthenticationCredentials(elasticCloudOptions.UserName, elasticCloudOptions.Password)))
        {
            // Defina o nome do índice.
            IndexFormat = "dotnetelkdemo",
            // Configuração necessária para data streams de logs.
            BatchAction = ElasticOpType.Create,
            // É necessária essa configuração para o ElasticSearch > 8.0.0.
            TypeName = null
        };
    }
}