using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PessoaAPI.Data;
using PessoaAPI.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configuração de URLs - só usa porta 80 se estiver em container Docker
var runningInDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
var portEnv = Environment.GetEnvironmentVariable("PORT");

if (runningInDocker && portEnv != null)
{
    // Rodando em Docker/Container - usa porta do ambiente
    builder.WebHost.UseUrls($"http://0.0.0.0:{portEnv}");
}
else if (runningInDocker)
{
    // Rodando em Docker sem PORT definida
    builder.WebHost.UseUrls("http://0.0.0.0:80");
}
else
{
    // Rodando localmente - sempre usa localhost:5000
    builder.WebHost.UseUrls("http://localhost:5000", "https://localhost:5001");
} 
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Pessoa API v1", 
        Version = "v1",
        Description = "API para cadastro de pessoas - Versão 1"
    });
    c.SwaggerDoc("v2", new OpenApiInfo 
    { 
        Title = "Pessoa API v2", 
        Version = "v2",
        Description = "API para cadastro de pessoas - Versão 2 (com endereço obrigatório)"
    });
 
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
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("PessoaDatabase"));

builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
});

builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

// Configuração do JWT
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

// Registro dos serviços
builder.Services.AddScoped<JWTService>();

// Configuração do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        // Projeto de estudo: liberar CORS para qualquer origem (sem credenciais)
        // Observação: AllowAnyOrigin é incompatível com AllowCredentials
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
// Habilitei o Swagger em todos os ambientes (dev e prod)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pessoa API v1");
    c.SwaggerEndpoint("/swagger/v2/swagger.json", "Pessoa API v2");
});

app.UseCors("AllowReactApp");

// UseHttpsRedirection desabilitado para desenvolvimento (evita problemas com certificados SSL)
// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
}

app.Run();
