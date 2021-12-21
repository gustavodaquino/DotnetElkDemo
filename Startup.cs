using DotnetElkDemo.Models;
using DotnetElkDemo.Options;
using Elasticsearch.Net;
using Microsoft.OpenApi.Models;
using Nest;
using Serilog;

namespace DotnetElkDemo;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "DotnetElkDemo", Version = "v1"}); });

        var elasticCloudOptions = _configuration.GetSection("ElasticCloud").Get<ElasticCloudOptions>();

        // ElasticClient para consumo interno da API.
        services.AddSingleton<IElasticClient>(_ =>
        {
            var settings = new ConnectionSettings(elasticCloudOptions.CloudId,
                    new BasicAuthenticationCredentials(elasticCloudOptions.UserName, elasticCloudOptions.Password))
                .DefaultMappingFor<TraceDocument>(x => x.IndexName("dotnetelkdemo-tracedocument"));

            return new ElasticClient(settings);
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DotnetElkDemo v1"));
        }

        app.UseSerilogRequestLogging();

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}