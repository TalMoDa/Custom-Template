using My.Custom.Template;
using My.Custom.Template.Build.Logger;
using My.Custom.Template.DependencyInjection;
using My.Custom.Template.RequestPipeline;
using My.Custom.Template.Services.Implementations;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices();
builder.Host.UseSerilog((context, configuration) => { configuration.ReadFrom.Configuration(context.Configuration); });
builder.Services.UseProblemDetails();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.ConfigureSerilogLogger();

app.UseGlobalErrorHandling();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

