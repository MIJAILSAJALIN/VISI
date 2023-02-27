using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using VISI.Entidades;
using VISI.Models;
using VISI.Servicios;

namespace VISI.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly ApplicationDbContext applicationDbContext;

        public UsuariosController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ApplicationDbContext applicationDbContext)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.applicationDbContext = applicationDbContext;
        }

        [AllowAnonymous]
        public IActionResult Registro()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Registro(RegistroViewModel modelo)
        {
           if (!ModelState.IsValid)
            {
                return View(modelo);

            }
            var usuario = new IdentityUser() { Email = modelo.Email, UserName = modelo.Email };
            var resultado = await userManager.CreateAsync(usuario, password: modelo.Password);
            if (resultado.Succeeded )
            {
                
                await signInManager.SignInAsync(usuario, isPersistent: modelo.Rememberme);
                return RedirectToAction("Inicio", "Home");

            }
            else
            {
                foreach (var error in resultado.Errors ) 
                { 
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            
            return View(modelo);
        }
        [HttpPost]
        public async Task<IActionResult> logout()
        {
           await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
           return RedirectToAction("login","Usuarios");

        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string mensaje = null)
        {
            if (mensaje is not null)
            {
                ViewData["mensaje"] = mensaje;
            }
                return View();

        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)  return View(loginViewModel);
            //loginViewModel.User = loginViewModel.Email;
            var resultado = await signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, loginViewModel.Rememberme, lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                return RedirectToAction("Inicio","Home");
                
            } else
            {
                ModelState.AddModelError(string.Empty, "ACCESO DENEGADO");
                return View(loginViewModel);

            }
        }
        [AllowAnonymous]
        [HttpGet]
        public ChallengeResult LoginExterno(string proveedor, string urlRetorno = null)
        {
            var urlRedireccion = Url.Action("RegistroUsuarioExterno", values: new { urlRetorno });
            var propiedades = signInManager.ConfigureExternalAuthenticationProperties(proveedor, urlRedireccion);
            return new ChallengeResult(proveedor, propiedades);
        }
        [AllowAnonymous]
        public async Task<IActionResult> RegistroUsuarioExterno(string urlRetorno = null, string remoteError = null)
        {
            urlRetorno = urlRetorno ?? Url.Content("~/Home/Inicio");
            string mensaje;
            if (remoteError != null) 
            {
                mensaje = $"Error de el proveedor de autenticación: {remoteError}";
                return RedirectToAction("login", routeValues: new { mensaje });
            }
            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info is null)
            {
                mensaje = "Error en la carga de los datos...";
                return RedirectToAction("login", routeValues: new { mensaje });
            }
            var resultadoLoginExterno = await signInManager.ExternalLoginSignInAsync(info.LoginProvider,info.ProviderKey, isPersistent: true, bypassTwoFactor: true);
            if (resultadoLoginExterno.Succeeded)
            {
                //el usuario ya está dado de alta en nuestra base de datos, de antes...
                return LocalRedirect(urlRetorno);
                //return RedirectToAction("Inicio","Home");
                //return View("../Home/Inicio");

            }
            string email;
            if (info.Principal.HasClaim(a=> a.Type == ClaimTypes.Email))
            {
                email = info.Principal.FindFirstValue(ClaimTypes.Email);
            }
            else
            {
                mensaje = "Error leyendo el email del usuario del proveedor";
                return RedirectToAction("login", routeValues: new { mensaje });
            }
            var usuario = new IdentityUser { Email= email, UserName = email };
            var rdoCrearUsuario = await userManager.CreateAsync(usuario);
            if (rdoCrearUsuario.Succeeded is false)
            {
                mensaje = rdoCrearUsuario.Errors.First().Description;
                return RedirectToAction("login", routeValues: new { mensaje });
            }
            var resultadoAgregarUser = await userManager.AddLoginAsync(usuario, info);
            if (resultadoAgregarUser.Succeeded) 
            {
                await signInManager.SignInAsync(usuario,isPersistent:true, info.LoginProvider);
                return LocalRedirect(urlRetorno);
                

            }
            mensaje = "Ha ocurrido un error al guardar el nuevo usuario.";
            return RedirectToAction("login", routeValues: new { mensaje });

        }
        [Authorize(Roles = Constantes.RolAdmin)]
        [HttpGet]
        public async Task<IActionResult> Listado(string mensaje = null)
        {
            var modelo = new UsuariosListaViewModel();
            var usuarios = await applicationDbContext.Users.Select( a =>                
                    new UsuarioViewModel { Email = a.Email, Id = a.Id  }
                ).ToListAsync();
            usuarios.ForEach(u =>
            {
                u.Roles =  applicationDbContext.UserRoles.Where(a => a.UserId == u.Id).Select(u => 
                u.RoleId
                
                ).ToList();
            });
            modelo.Mensaje = mensaje;
            modelo.UsuariosLista = usuarios;
            return View(modelo);
        }
        [HttpPost]
        [Authorize(Roles = Constantes.RolAdmin)]

        public async Task<IActionResult> HacerAdmin(string email)
        {
            var usuario = await applicationDbContext.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
            if (usuario is null) return NotFound();
            await userManager.AddToRoleAsync(usuario, Constantes.RolAdmin);
            return RedirectToAction("Listado", routeValues: new { mensaje = $"Rol '{Constantes.RolAdmin}' asignado correctamente a {email}" });
        }
        [HttpPost]
        [Authorize(Roles = Constantes.RolAdmin)]

        public async Task<IActionResult> EliminarAdmin(string email)
        {
            var usuario = await applicationDbContext.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
            if (usuario is null) return NotFound();
            await userManager.RemoveFromRoleAsync(usuario, Constantes.RolAdmin);
            return RedirectToAction("Listado", routeValues: new { mensaje = $"Rol '{Constantes.RolAdmin}' eliminado correctamente a {email}" });
        }







    }
}
