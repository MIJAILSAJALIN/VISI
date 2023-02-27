using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.IdentityModel.Tokens;
using System;
using VISI.Models;
using VISI.Servicios;

namespace VISI.Controllers
{
    
    public class OtrosController : Controller
    {
        private readonly IRepositorioOtros repositorioOtros;
        static string idform;
        static string descform;
        static string errormodelo ;
        static bool modificar = false;
        

        public OtrosController(IRepositorioOtros repositorioOtros)
        {
            this.repositorioOtros = repositorioOtros;
        }
        public IActionResult IndexOtros()
        {
            return View();
        }
        public async Task<IActionResult> FormasPago() 
        {   
            if (!string.IsNullOrEmpty(errormodelo))
            {
                ModelState.AddModelError("", errormodelo);                
                errormodelo = null;
            }
            @ViewBag.id = idform;
            @ViewBag.Desc = descform;
            //descform = null;
            //idform = null;

            if (modificar)
            //if (idform is not null)
            {
                //modificar = false;
                
                ViewBag.button = "Modificar";
                
                ViewBag.accion = "Edit_Fpg";
            }
            else
            {
                ViewBag.button = "Nueva";
                ViewBag.accion = "Alta_Fpg";
            }
            var listado = await repositorioOtros.ListaFpg();
            return View(listado);
        }
        public async Task<IActionResult> Edit_Fpg(string[] buscador)
        {
            if (idform != buscador[0] || string.IsNullOrEmpty(buscador[1]) || descform == buscador[1]
                || await repositorioOtros.Existe("", buscador[1]))
                //si modificó el código ó ingresó una descripción vacía ó si no modificó nada ó si ya existe otro registro igual
            {
                //ModelState.AddModelError("", "No se puede modificar el código");
                errormodelo = "No se puede modificar el código";
                return RedirectToAction("FormasPago");
            }
            // comprobar que la descripcion no está duplicada
            // si la descripción ha sido modificada y ya existe, te tira...
            
            ///GRABAR
            descform = null;
            idform = null;
            modificar = false;
            await repositorioOtros.update(buscador[0], buscador[1]);
            return RedirectToAction("FormasPago");
        }
        public async Task<IActionResult> BorrarFpg(string id)
        {           
            var fpg = await repositorioOtros.FindId(id);
            if (fpg is null) return RedirectToAction("NoEncontrado", "Home"); 
            return View(fpg);            
        }
        public async Task<IActionResult> deleteFpg(string id)
        {
            var fpg = await repositorioOtros.FindId(id);
            if (fpg is null) return RedirectToAction("NoEncontrado", "Home");
            await repositorioOtros.BorraId(fpg);
            return RedirectToAction("FormasPago");
        }
        public async Task<IActionResult> Alta_Fpg(string[] buscador)     
        {
            if (buscador[0] is null || buscador[1] is null)
            {
                idform = buscador[0];
                descform = buscador[1];
                return RedirectToAction("FormasPago");
            }
            var yaexiste = await repositorioOtros.Existe(buscador[0], buscador[1]);
            if (yaexiste)
            {
                //ModelState.AddModelError("", "Ya existe una forma de pago así.");
                errormodelo = "Ya existe una forma de pago así.";
                return RedirectToAction("FormasPago");
                
            }
            await repositorioOtros.Alta(buscador[0], buscador[1]);
            return RedirectToAction("FormasPago");
        }
        public async Task<IActionResult> EditarFpg(string id) 
        {
            //localizar el registro
            var fpg = await repositorioOtros.FindId(id);
            if (fpg is not null)
            {
                idform = fpg.Id;
                descform = fpg.Descripcion;
                modificar = true;
            }
            return RedirectToAction("FormasPago");
        }
        [HttpPost]
        public async Task<IActionResult> Ordenar([FromBody] string[] ids)
        {   
            var formasOrdenadas = ids.Select((valor, indice) =>
                new formasDePago() { Id = valor, orden = indice + 1 }).AsEnumerable();
            await repositorioOtros.Ordenar(formasOrdenadas);
            return Ok();
        }



    }
}
