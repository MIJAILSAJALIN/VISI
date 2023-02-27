using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VISI.Entidades;
using VISI.Servicios;

namespace VISI.Controllers
{

   // [ApiController]

    [Route("api/facturasAPI")]
    public class FacturasAPIController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IRepositorioUsuarios repositorioUsuarios;

        public FacturasAPIController(ApplicationDbContext context, IRepositorioUsuarios repositorioUsuarios)
        {
            this.context = context;
            this.repositorioUsuarios = repositorioUsuarios;
        }
        //[AllowAnonymous]
        //[HttpPost(Name = "api/facturas")]        
        [HttpPost]
        public async Task<ActionResult<Factura>> Post([FromBody] string misdatos)
        {
            
            var usuarioId = repositorioUsuarios.ObtenerUsuarioId();
            var lastNumFactura = await context.VISI_Facturas.Select(n => n.Numero).MaxAsync(); //obtenemos el último número de factura

            var nuevaFactura = new Factura();
            nuevaFactura.Numero = lastNumFactura + 1;
            //crear la factura en memoria con los datos del Post...

            context.Add(nuevaFactura);
            await context.SaveChangesAsync();
            return nuevaFactura;
        }
    }





}




