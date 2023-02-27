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













}
