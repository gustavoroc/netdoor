using ChatService.Infrastructure;

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