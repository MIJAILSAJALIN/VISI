using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using VISI.Entidades;
using VISI.Models;
using VISI.Servicios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var autenticacion = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();
builder.Services.AddControllersWithViews(opciones =>
{
    opciones.Filters.Add(new AuthorizeFilter(autenticacion));
});

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer("name=DefaultConnection"));

builder.Services.AddAuthentication().AddMicrosoftAccount(opciones =>
{
    //opciones.ClientId = builder.Configuration["MicrosoftClientId"];
    //opciones.ClientSecret = builder.Configuration["MicrosoftSecretId"];
    opciones.ClientId = "a0cf8cda-3135-4a5d-9fec-f26911ac11cd";
    opciones.ClientSecret = "HWn8Q~aCHadykvjn_WhVCXm11.zzqiDDq.gk-ctW";

});

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
}).AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme,
    options =>
    {
        options.LoginPath = "/usuarios/login";
        options.AccessDeniedPath = "/usuarios/login";
    });


builder.Services.AddTransient<IRepositorioClientes, RepositorioClientes>();
builder.Services.AddTransient<IRepositorioUsuarios,RepositorioUsuarios>();
builder.Services.AddTransient<IRepositorioOtros,RepositorioOtros>();
builder.Services.AddTransient<IRepositorioAlbaranes,RepositorioAlbaranes>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(typeof(Program));

//builder.Services.AddTransient<IUserStore<Usuario>, UsuarioStore>();
//builder.Services.AddTransient<SignInManager<Usuario>>();
//builder.Services.AddIdentityCore<Usuario>(opciones =>
//{
//    opciones.Password.RequireDigit = false;
//    opciones.Password.RequireLowercase = false;
//    opciones.Password.RequireUppercase = false;
//    opciones.Password.RequireNonAlphanumeric = false;
//});
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
//    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
//    options.DefaultSignOutScheme = IdentityConstants.ApplicationScheme;
//}).AddCookie(IdentityConstants.ApplicationScheme, opciones =>
//{
//    opciones.LoginPath = "/Usuarios/Login";
//});


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}
app.UseDeveloperExceptionPage();
app.UseDatabaseErrorPage();
//LAS DOS LINEAS DE ARRIBA LAS HE AÑADIDO YO

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
      pattern: "{controller=Usuarios}/{action=Login}/{id?}");
    //  pattern: "{controller=Home}/{action=Inicio}/{id?}");


app.Run();


