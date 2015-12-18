using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Encuesta.Models;
using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Encuesta.Controllers
{
            [Authorize(Roles = "Admin, JefeGH")]
    public class CulminacionesForEspacialidadsController : Controller
    {
        private EncuestaPetroleoEntities db = new EncuestaPetroleoEntities();

        // GET: CulminacionesForEspacialidads/Create
        public ActionResult Culminar(int id)
        {
            var USER_id = User.Identity.GetUserId();
            var EM = (from item in db.AspNetUsers
                      where item.Id == USER_id
                      select item.EmpresaId).First();
            EmpresaEspecialidad empresaEspecialidad = db.EmpresaEspecialidad.Where(x => x.EmpresaId.Equals(EM)).Where(x => x.Especialidadid == id).First();
            empresaEspecialidad.Especialidadid = id;
            empresaEspecialidad.EmpresaId = EM;
            empresaEspecialidad.FechaCulminacion = DateTime.Now;
            empresaEspecialidad.UserId = USER_id;
            empresaEspecialidad.EspecialidadCulminada = true;
            empresaEspecialidad.NoAplica = false;
            db.Entry(empresaEspecialidad).State = EntityState.Modified;
            db.SaveChanges();
            ViewBag.Culminacion = "Culminacion realizada con exito";
            return RedirectToAction("Index", "EmpresaEspecialidads");
        }

        public ActionResult NoAplica(int id)
        {
            var USER_id = User.Identity.GetUserId();
            var EM = (from item in db.AspNetUsers
                      where item.Id == USER_id
                      select item.EmpresaId).First();
            EmpresaEspecialidad empresaEspecialidad = db.EmpresaEspecialidad.Where(x => x.EmpresaId.Equals(EM)).Where(x => x.Especialidadid == id).First();
            empresaEspecialidad.Especialidadid = id;
            empresaEspecialidad.EmpresaId = EM;
            empresaEspecialidad.FechaCulminacion = DateTime.Now;
            empresaEspecialidad.UserId = USER_id;
            empresaEspecialidad.EspecialidadCulminada = false;
            empresaEspecialidad.NoAplica = true;
            db.Entry(empresaEspecialidad).State = EntityState.Modified;
            db.SaveChanges();
            ViewBag.Culminacion = "Culminacion realizada con exito";
            return RedirectToAction("Index", "EmpresaEspecialidads");
        }

 

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
