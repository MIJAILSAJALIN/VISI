using AutoMapper;
using Azure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Abstractions;
using MoreLinq;
using System.Reflection;
using VISI.Models;
using VISI.Servicios;
using Dapper;

// linea añadida para cambiar en GitHub...
namespace VISI.Controllers
{
    public class AlbaranesController : Controller
    {
        private readonly IRepositorioAlbaranes repositorioAlbaranes;
        private readonly IRepositorioClientes repositorioClientes;
        private readonly IMapper mapper;
        private readonly ILogger<HomeController> logger;

        public AlbaranesController(IRepositorioAlbaranes repositorioAlbaranes, IRepositorioClientes repositorioClientes,
            IMapper mapper, ILogger<HomeController> logger)
        {
            this.repositorioAlbaranes = repositorioAlbaranes;
            this.repositorioClientes = repositorioClientes;
            this.mapper = mapper;
            this.logger = logger;
        }

       
        [Authorize]
        public async Task<IActionResult> Index(ListadoAlbaranesViewModel modelo)
        {

            (modelo.listado, modelo.TotaldeRegistros) = await repositorioAlbaranes.listado(modelo.ClienteId,modelo.desdeFecha, modelo.hastaFecha, modelo);


            var listacliente = (await repositorioClientes.ListaClientes()).
                                     Select(x => new SelectListItem(x.Nombre, x.Id.ToString())).ToList();
            //var xdefecto = new SelectListItem("-- Seleccione un cliente --", "0", true);
            listacliente.Insert(0, new SelectListItem("-- Seleccione un cliente --", "0", true));
            listacliente.Insert(1, new SelectListItem("-- Todos los clientes --", "-1", true));
            modelo.ClientesSelector = listacliente;


            return View(modelo);            
        }
        [HttpPost]
        public  async Task<IActionResult> Nuevo()
        {
            

            AlbaranesViewModel modelo = new AlbaranesViewModel();

            // modelo.albaranesConDetalle.ClientesSelector = (await repositorioClientes.ListaClientes()).
            //     Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
            modelo.ClientesSelector = (await repositorioClientes.ListaClientes()).
                Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));

            //return RedirectToAction("Inicio", "Home");

            return View(modelo);
        }
        //[HttpPost]
        public async Task<IActionResult> Grabar(AlbaranesViewModel albaranesViewModel)
        {

            if (!ModelState.IsValid)
            {
                return View("Nuevo");
            }

            //for (int i = 1000; i<5000; ++i )
            //{
            //    albaranesViewModel.albaranesConDetalle.Detalles[0].Descripcion = $"des {i}";
            await repositorioAlbaranes.grabar_nuevo((AlbaranesConDetalle)albaranesViewModel);

            //pantalla anunciando la grabaci´n del albarán

            return RedirectToAction("Index");
        }
        [HttpPost]        
        public async Task<IActionResult> Addlinea(AlbaranesViewModel albaranesViewModel)
        {
            int a;
            if (albaranesViewModel.Detalles == null) a = 0;
            else a = albaranesViewModel.Detalles.Max(x => x.LineaNum);            
            (albaranesViewModel.Detalles).Add(new Albaranes_detalle() { LineaNum = a + 1 });
            albaranesViewModel.ClientesSelector = (await repositorioClientes.ListaClientes()).Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
            //poner por defecto el que trae el viewmodel cliente
            
            return View("Nuevo",albaranesViewModel);            
        }
        [HttpPost]
        public async Task<IActionResult> DelLinea(AlbaranesViewModel albaranesViewModel)

        {
            int id = albaranesViewModel.numLinea;
            albaranesViewModel.Detalles.RemoveAt(id);
            int a = 1;
            if (albaranesViewModel.Detalles.Count == 0)
                (albaranesViewModel.Detalles).Add(new Albaranes_detalle() { LineaNum = 1 });
            albaranesViewModel.Detalles.ForEach(x =>
            {
                x.LineaNum = a;
                a++;
            });

           /// que tengo que hacer para que esta nueva lista se refresque en la vista ?????????????
          
            albaranesViewModel.ClientesSelector = 
                (await repositorioClientes.ListaClientes()).Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));

            return View("Nuevo",albaranesViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Modificar(int Id)
        {
            //var a = Id.ToString();  
            return View();
        }





    }
}
