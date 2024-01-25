using EmployeeManagementApp.DAL.Implementations;
using FinalProject.Models;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Dal.Implementations;
using log4net.Config;
using log4net;
using System.Reflection;
var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json");

builder.Services.AddControllers();

builder.Services.AddSingleton<IDAttendance, DAttendance>();
builder.Services.AddSingleton<IDDepartmnets, DDepartments>();
builder.Services.AddSingleton<IDUser, DUser>();
builder.Services.AddSingleton<IDEmployee, DEmployee>();
builder.Services.AddSingleton<IDLeaveRequest, DLeaveRequest>();
builder.Services.AddSingleton<IDSalaryDetail, DSalaryDetail>();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddDbContext<EmployeeManagementDbContext>((serviceProvider, options) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    options.UseMySQL(configuration.GetConnectionString("MyDatabaseConnection"));
}, ServiceLifetime.Singleton);
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
});
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
    });
}
else
{
    app.UseExceptionHandler("/Error");
}
var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();