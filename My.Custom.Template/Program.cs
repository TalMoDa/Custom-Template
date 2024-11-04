using My.Custom.Template;
using My.Custom.Template.DependencyInjection;
using My.Custom.Template.RequestPipeline;
using My.Custom.Template.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices();
builder.Services.UseProblemDetails();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseGlobalErrorHandling();

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