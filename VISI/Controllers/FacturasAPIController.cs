using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using VISI.Entidades;
using VISI.Models;
using VISI.Servicios;

namespace VISI.Controllers
{

   // [ApiController]

    [Route("api/facturasAPI")]
    public class FacturasAPIController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        //private readonly IRepositorioUsuarios repositorioUsuarios;

        //public FacturasAPIController(ApplicationDbContext context, IRepositorioUsuarios repositorioUsuarios)
        public FacturasAPIController(ApplicationDbContext context)
        {
            this.context = context;
          //  this.repositorioUsuarios = repositorioUsuarios;
        }
        //[AllowAnonymous]
        //[HttpPost(Name = "api/facturas")]        
        [HttpPost]
        public async Task<ActionResult<Factura>> Post([FromBody] Factura factura)
        {
            Debug.WriteLine("ESTOY DENTRO.....");
            //  var usuarioId = await repositorioUsuarios.ObtenerUsuarioId();

            uint lastNumFactura = 0;
            factura.Subtotal = factura.LineasFacturas.Sum(x => (x != null ? x.Precio * x.Cantidad : 0));
            //si hago un campo calculado no se graba por el tema de entity, supongo...
            lastNumFactura = await context.VISI_Facturas.OrderByDescending(f=>f.Numero).Select(f=>f.Numero).FirstOrDefaultAsync(); //obtenemos el último número de factura
           
            factura.Numero = lastNumFactura + 1; 
            context.Add(factura);

            await context.SaveChangesAsync();
            
            return Ok((factura.Numero));
        }
    }





}




