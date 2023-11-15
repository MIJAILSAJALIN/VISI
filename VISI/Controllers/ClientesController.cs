using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using System.Diagnostics;
using VISI.Models;
using VISI.Servicios;
//using VISI.Entidades;

namespace VISI.Controllers
{
    public class ClientesController : Controller
    {
        //private readonly ApplicationDbContext context;
        private readonly ILogger<HomeController> _logger;
        private readonly IRepositorioClientes repositorioClientes;
        private readonly IRepositorioOtros repositorioOtros;

        //public ClientesController(ApplicationDbContext context, ILogger<HomeController> logger, IRepositorioClientes repositorioClientes,
        public ClientesController(ILogger<HomeController> logger, IRepositorioClientes repositorioClientes,
              IRepositorioOtros repositorioOtros)
        {
        //    this.context = context;
            _logger = logger;
            this.repositorioClientes = repositorioClientes;
            this.repositorioOtros = repositorioOtros;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index(string buscador)
        {
            IEnumerable<Clientes> clientes;
            if (string.IsNullOrWhiteSpace(buscador))
            {
                clientes =  await repositorioClientes.ListaClientes();
                @ViewBag.filtro = "completo";
            }
            else
            {
                @ViewBag.filtro = $"filtrado por '{buscador}'";
                clientes = await repositorioClientes.FiltraClientes(buscador);
            }
            return View(clientes);
        }

        [HttpGet]
        public async Task<IActionResult> existeCliente(Clientes clientes)
        {
            //var miuser = usuarios.Actual(); 
            bool yaexiste = await repositorioClientes.Yaexiste(clientes.Nombre, clientes.Nif);
            if (yaexiste)
            {
                ModelState.AddModelError("", "Ya existe un cliente con ese nombre o ese Nif");
                return Json($"El nombre ya existe en la base de datos.");
            }
            return Json(true);
        }
        

        [HttpGet]
        public async Task<IActionResult> Alta_Cliente()
        {
            var todaslasformasdepago = await repositorioOtros.ListaFpg();
            var modelo = new ClientesViewModel();
            modelo.FormasPago = todaslasformasdepago.Select(x => new SelectListItem(x.Descripcion, x.Id));
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Alta_Cliente(Clientes clientes)
        {
            if (!ModelState.IsValid) { return View(clientes); }
            bool yaexiste = await repositorioClientes.Yaexiste(clientes.Nombre, clientes.Nif);
            if (yaexiste)
            {
                ModelState.AddModelError("", "Ya existe un cliente con ese nombre o ese Nif");
                return View(clientes);
            }
            //var miuser = usuarios.Actual();
            await repositorioClientes.Crear(clientes);
            return RedirectToAction("Index");
        }

      
        public IActionResult Clientes()
        {            
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> EditarCliente(int id)       
        {
            ////var miuser = usuarios.Actual();
            var cliente = await repositorioClientes.Busca(id);
            if (cliente is null) return RedirectToAction("NoEncontrado", "Home");           
            return View(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> EditarCliente(Clientes cliente)
        {
            //var miuser = usuarios.Actual();
            var exitecliente = await repositorioClientes.Busca(cliente.Id);
            if (cliente is null) return RedirectToAction("NoEncontrado", "Home");
            //
            //             Revisar que el nuevo nombre y el nif no corresponden a otros clientes...
            //
            bool yaexiste = await repositorioClientes.Distintode(cliente.Nombre, cliente.Nif, cliente.Id);
            if (yaexiste)
                {
                    ModelState.AddModelError("", "Ya existe un cliente con ese nombre o ese Nif");
                    return View(cliente);
                }
            await repositorioClientes.Actualizar(cliente);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            //var miuser = usuarios.Actual();
            var cliente = await repositorioClientes.Busca(id);
            if (cliente is null) return RedirectToAction("NoEncontrado", "Home");
            return View(cliente);
        }
        [HttpPost]
        public async Task<IActionResult> borraCliente(int id)
        {
            var cliente = await repositorioClientes.Busca(id);
            if (cliente is null) return RedirectToAction("NoEncontrado", "Home");
            await repositorioClientes.Delete(id);
            return RedirectToAction("Index");
        }

      


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}