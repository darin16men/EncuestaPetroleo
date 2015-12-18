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
    public class PersonasReunionesPerfilesController : Controller
    {
        private EncuestaPetroleoEntities db = new EncuestaPetroleoEntities();

        // GET: PersonasReunionesPerfiles
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "EmpresaEspecialidads");
            }
            var USER_id = User.Identity.GetUserId();
             var EM = (from item in db.AspNetUsers
                      where item.Id == USER_id
                      select item.EmpresaId).First();
             ViewBag.Especilidad = db.OtraEspecialidad.Where(x => x.Id == id).First().OtraEspecialidad1;
             ViewBag.EspecilidadId = id;

             var NoPersonasReunionesPerfiles = db.PersonasReunionesPerfiles.Where(x => x.EmpresaId == EM).Where(x => x.EspecialidadId == id).ToList().Count();
             var NoEncuestaPerfilesPetroleo = db.EncuestaPerfilesPetroleo.Where(x => x.Empresa_id == EM).Where(x => x.OtraEspecialidad_id == id).ToList().Count();
             ViewBag.no = "noCulminar"; 
            if (NoPersonasReunionesPerfiles > 0)
             {
                 if (NoEncuestaPerfilesPetroleo > 0)
                 {
                     ViewBag.no = "Culminar";
                 }               
             }

            return View(db.PersonasReunionesPerfiles.Where(x => x.EmpresaId == EM).Where(x => x.EspecialidadId == id).ToList());
        }

        // GET: PersonasReunionesPerfiles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "EmpresaEspecialidads");
            }
            PersonasReunionesPerfiles personasReunionesPerfiles = db.PersonasReunionesPerfiles.Find(id);
            if (personasReunionesPerfiles == null)
            {
                return HttpNotFound();
            }
            ViewBag.EspecilidadId = personasReunionesPerfiles.EspecialidadId;
            return View(personasReunionesPerfiles);
        }

        // GET: PersonasReunionesPerfiles/Create
        public ActionResult Create(int id)
        {
            PersonasReunionesPerfiles personasReunionesPerfiles = new PersonasReunionesPerfiles();
            ViewBag.Especilidad = db.OtraEspecialidad.Where(x => x.Id == id).First().OtraEspecialidad1;
            ViewBag.EspecilidadId = id;
            personasReunionesPerfiles.EspecialidadId = id;
            return View(personasReunionesPerfiles);
        }

        // POST: PersonasReunionesPerfiles/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,EspecialidadId,Nombre,Profesion,CargoDependencia,CorreoElectronico,TelefonoCelular")] PersonasReunionesPerfiles personasReunionesPerfiles, int? id)
        {
            if (ModelState.IsValid)
            {
                var USER_id = User.Identity.GetUserId();
                var EM = (from item in db.AspNetUsers
                          where item.Id == USER_id
                          select item.EmpresaId).First();
                personasReunionesPerfiles.EmpresaId = EM;
                personasReunionesPerfiles.UserId = USER_id;
                db.PersonasReunionesPerfiles.Add(personasReunionesPerfiles);
                db.SaveChanges();
                return RedirectToAction("Index",new{id = personasReunionesPerfiles.EspecialidadId });
            }
            ViewBag.Especilidad = db.OtraEspecialidad.Where(x => x.Id == personasReunionesPerfiles.EspecialidadId).First().OtraEspecialidad1;
            ViewBag.EspecilidadId = id;
            return View(personasReunionesPerfiles);
        }

        // GET: PersonasReunionesPerfiles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "EmpresaEspecialidads");
            }
            PersonasReunionesPerfiles personasReunionesPerfiles = db.PersonasReunionesPerfiles.Find(id);
            if (personasReunionesPerfiles == null)
            {
                return HttpNotFound();
            }
            ViewBag.EspecilidadId = personasReunionesPerfiles.EspecialidadId;
            return View(personasReunionesPerfiles);
        }

        // POST: PersonasReunionesPerfiles/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,EspecialidadId,Nombre,Profesion,CargoDependencia,CorreoElectronico,TelefonoCelular")] PersonasReunionesPerfiles personasReunionesPerfiles)
        {
            if (ModelState.IsValid)
            {
                var USER_id = User.Identity.GetUserId();
                var EM = (from item in db.AspNetUsers
                          where item.Id == USER_id
                          select item.EmpresaId).First();
                personasReunionesPerfiles.EmpresaId = EM;
                personasReunionesPerfiles.UserId = USER_id;
                db.Entry(personasReunionesPerfiles).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = personasReunionesPerfiles.EspecialidadId });
            }
            return View(personasReunionesPerfiles);
        }

        // GET: PersonasReunionesPerfiles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "EmpresaEspecialidads");
            }
            PersonasReunionesPerfiles personasReunionesPerfiles = db.PersonasReunionesPerfiles.Find(id);
            if (personasReunionesPerfiles == null)
            {
                return HttpNotFound();
            }
            ViewBag.Especilidad = db.OtraEspecialidad.Where(x => x.Id == personasReunionesPerfiles.EspecialidadId).First().OtraEspecialidad1;
            ViewBag.EspecilidadId = personasReunionesPerfiles.EspecialidadId;
            return View(personasReunionesPerfiles);
        }

        // POST: PersonasReunionesPerfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PersonasReunionesPerfiles personasReunionesPerfiles = db.PersonasReunionesPerfiles.Find(id);
            db.PersonasReunionesPerfiles.Remove(personasReunionesPerfiles);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = personasReunionesPerfiles.EspecialidadId });
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
