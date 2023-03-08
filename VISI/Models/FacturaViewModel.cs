using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using VISI.Controllers;
using VISI.Entidades;
using VISI.Servicios;

namespace VISI.Models
{
    public class  FacturaViewModel : Factura
    {

        public  IEnumerable<SelectListItem> ClientesSelector { get; set; }

        //public override DateTime Fecha { get; set; } = DateTime.Now;
        //public string NombreCliente { get; set; }


    }
    //public class FacturaViewBorra
    //{
    //    public Factura Factura { get; set; }
    //    public List<LineasFactura> LineasFacturas { get; set; } = new List<LineasFactura>();
    //}
}
