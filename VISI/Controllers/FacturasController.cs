using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VISI.Entidades;
using VISI.Models;
using VISI.Servicios;

namespace VISI.Controllers
{
    public class FacturasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepositorioClientes repositorioClientes;

        public FacturasController(ApplicationDbContext context, IRepositorioClientes repositorioClientes)
        {
            _context = context;
            this.repositorioClientes = repositorioClientes;
        }
        [HttpGet]
        public async Task<IActionResult> Inicio()
        {
            var modelo = new FacturaViewModel();            
            modelo.ClientesSelector = await repositorioClientes.RellenaSelectorClientes(false);
            return View(modelo);
        }
        [HttpPost]
        public async Task<IActionResult> Inicio(FacturaViewModel facturaViewModel)
        {
            return View("borrame");
        }
        [HttpPost]
        public  IActionResult borrame(FacturaViewModel facturaViewModel)
        {
            return View();
        }

        
    }
   // [ApiController]
    [Route("api/facturas")]
        public class FacturasControllerAPI : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IRepositorioUsuarios repositorioUsuarios;

        public FacturasControllerAPI(ApplicationDbContext context, IRepositorioUsuarios repositorioUsuarios)
        {
            this.context = context;
            this.repositorioUsuarios = repositorioUsuarios;
        }

        //[HttpPost(Name = "api/facturas")]        
        [HttpPost]
        public async Task<ActionResult<Factura>> Post([FromBody] string misdatos)
        {
            var usuarioId = repositorioUsuarios.ObtenerUsuarioId();
            var lastNumFactura = (await context.VISI_Facturas.Select(n => n.Numero).MaxAsync()); //obtenemos el último número de factura

            var nuevaFactura = new Factura();
            nuevaFactura.Numero = lastNumFactura + 1;
            //crear la factura en memoria con los datos del Post...

            context.Add(nuevaFactura);
            await context.SaveChangesAsync();
            return nuevaFactura;
        }
    }

}
