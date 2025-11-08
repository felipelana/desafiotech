using Microsoft.AspNetCore.Mvc;
using PayFlow.Models;
using PayFlow.Services;

var builder = WebApplication.CreateBuilder(args);

// configuracoes e swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<PaymentService>();
builder.Services.AddSingleton<FastPayService>();
builder.Services.AddSingleton<SecurePayService>();

var app = builder.Build();

// ler token do arq de config
var apiToken = builder.Configuration.GetValue<string>("Security:ApiToken");

// autenticação simples
app.Use(async (context, next) =>
{
    var path = context.Request.Path;
  
    if (path.StartsWithSegments("/payments", StringComparison.OrdinalIgnoreCase))
    {
        if (!context.Request.Headers.TryGetValue("Authorization", out var auth) || string.IsNullOrWhiteSpace(apiToken))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Token ausente ou não configurado!");
            return;
        }

        if (!string.Equals(auth.ToString(), apiToken, StringComparison.Ordinal))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Token inválido!");
            return;
        }
    }

    await next();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

builder.WebHost.UseUrls("http://localhost:8080");

app.Run();
