using Microsoft.Extensions.Configuration;
using TesMandiri.Interfaces;
using TesMandiri.Models;
using TesMandiri.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

builder.Services.Configure<Setting>(config.GetSection("SQLServer"));
builder.Services.AddSingleton<IMasterInt<EmployeeBase>, EmployeeService>();
builder.Services.AddSingleton<IMasterInt<IdCardBase>, IdCardService>();
builder.Services.AddSingleton<IMasterString<DivisionBase>, DivisionService>();
builder.Services.AddSingleton<IMasterString<TaskBase>, TaskService>();
builder.Services.AddSingleton<IEmployeeCardService, EmployeeCardService>();
builder.Services.AddSingleton<IDivisionMemberService, DivisionMemberService>();
builder.Services.AddSingleton<IEmployeeTaskService, EmployeeTaskService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
