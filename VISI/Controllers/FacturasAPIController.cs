using AutoMapper;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoreLinq.Extensions;
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
        private readonly IRepositorioUsuarios repositorioUsuarios;
        private readonly IMapper mapper;

        public FacturasAPIController(ApplicationDbContext context, IRepositorioUsuarios repositorioUsuarios, IMapper mapper)
        //public FacturasAPIController(ApplicationDbContext context)
        {
            this.context = context;
            this.repositorioUsuarios = repositorioUsuarios;
            this.mapper = mapper;
        }
        //[AllowAnonymous]
        //[HttpPost(Name = "api/facturas")]        
        [HttpPost]
        public async Task<ActionResult<Factura>> Post([FromBody] Factura factura)
        {
            var usuarioId =  repositorioUsuarios.ObtenerUsuarioId();

            factura.Subtotal = factura.LineasFacturas.Sum(x => (x != null ? x.Precio * x.Cantidad : 0));
            //si hago un campo calculado no se graba por el tema de entity, supongo...
            if (factura.Numero == 0)
            {
                uint lastNumFactura  = await context.VISI_Facturas.Select(f=>f.Numero).DefaultIfEmpty().MaxAsync();
                factura.Numero = lastNumFactura + 1;
                context.Add(factura);
            }   
            else   //la factura se está modificando...
            {
                var facturaOriginal =  await context.VISI_Facturas.FirstOrDefaultAsync(f=> f.Numero == factura.Numero) ;                
                var oldId = facturaOriginal.Id;
                //facturaOriginal.LineasFacturas = context.VISI_LineasFactura.Where(x => x.Factura.Numero == facturaOriginal.Numero).ToList();
                // no entiendo xq tengo que cargar yo los detalles de las facturas...xq no se cargan automáticamente al cargar la Factura ?
                mapper.Map(factura, facturaOriginal);
                facturaOriginal.Id= oldId;
            }
            
            await context.SaveChangesAsync();            
            return Ok((factura.Numero));
        }
    }





}




