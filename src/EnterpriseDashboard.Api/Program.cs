using EnterpriseDashboard.Application.Interfaces;
using EnterpriseDashboard.Application.Validators.Tasks;
using EnterpriseDashboard.Infrastructure.Data;
using EnterpriseDashboard.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IIngestionBatchRepository, IngestionBatchRepository>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateTaskValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Enterprise DashBoard Api").WithTheme(ScalarTheme.DeepSpace);
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
