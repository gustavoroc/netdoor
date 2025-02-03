using ChatService.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 1. Add SignalR:
builder.Services.AddSignalR();

// 2. Add CORS configuration:
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(builder => {
        builder
            .WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("sua-chave-secreta-muito-longa-aqui"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "http://localhost:5285", // URL do serviço de autenticação

            ValidateAudience = true,
            ValidAudience = "sua-audience",

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key, // Aqui passa a chave secreta

            ValidateLifetime = true, // Garante que o token não expirou
            ClockSkew = TimeSpan.Zero // Remove tolerância de tempo extra
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;

                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("Chat"))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// 3. Use CORS middleware:
app.UseCors();

app.MapControllers();

// 4. Add SignalR API:
app.MapHub<ChatHub>("Chat");

app.Run();