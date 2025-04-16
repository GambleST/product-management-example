using Microsoft.EntityFrameworkCore;
using ProductInformationApi.Contexts;
using ProductInformationApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ProductInformationDbContext>(options =>
    options.UseSqlite("Data Source=product-information.db"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();