using PatientMonitorDataLogger.API.Setups;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var setups = new ISetup[]
{
    new ControllerSetup(),
    new OpenApiSetup(),
    new HubSetup(),
    new CorsSetup()
};
foreach (var setup in setups)
{
    setup.Run(builder.Services, builder.Configuration);
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
