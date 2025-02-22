using Microsoft.AspNetCore.HttpOverrides;
using PatientMonitorDataLogger.API.Hubs;
using PatientMonitorDataLogger.API.Setups;

var builder = WebApplication.CreateBuilder(args);

var setups = new ISetup[]
{
    new ControllerSetup(),
    new OpenApiSetup(),
    new HubSetup(),
    new CorsSetup(),
    new WorkflowSetup()
};
foreach (var setup in setups)
{
    setup.Run(builder.Services, builder.Configuration);
}
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

// -------------------------

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    TypescriptGeneratorRunner.Run();
    app.UseDeveloperExceptionPage();
}

app.UseForwardedHeaders();
//app.UseHttpsRedirection(); // API is hosted by reverse proxy, which handles HTTPS

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();
if (app.Environment.IsDevelopment())
{
    app.UseCors();
}

app.MapControllers();
app.MapHub<DataHub>("/hubs/data");

app.Run();
