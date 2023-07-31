using Microsoft.AspNetCore.Authentication.Cookies;
using OdinWeb.Models.Data.Classes;
using OdinWeb.Models.Data.Interfaces;
using Rotativa.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/Auth/Login";
        option.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    });

builder.Services.AddScoped<IUserModel, UserModel>();
builder.Services.AddScoped<IServicioModel, ServicioModel>();
builder.Services.AddScoped<IBranchModel, BranchModel>();
builder.Services.AddScoped<IRolModel, RolModel>();
builder.Services.AddScoped<IClienteModel, ClienteModel>();
builder.Services.AddScoped<ISupervisorModel, SupervisorModel>();
builder.Services.AddScoped<ITicketModel, TicketModel>();
builder.Services.AddScoped<IDocumentModel, DocumentModel>();
builder.Services.AddScoped<ICommentModel, CommentModel>();
builder.Services.AddScoped<IReportModel, ReportModel>();


builder.Services.AddScoped<IStatusModel, StatusModel>();
builder.Services.AddHttpContextAccessor();

//Rotativa para PDF
var rotativaPath = "../PDF";

RotativaConfiguration.Setup((Microsoft.AspNetCore.Hosting.IHostingEnvironment)builder.Services
    .BuildServiceProvider()
    .GetService(typeof(Microsoft.AspNetCore.Hosting.IHostingEnvironment)),
    rotativaPath);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//app.UseSession();

//Configuracion para sesion
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();