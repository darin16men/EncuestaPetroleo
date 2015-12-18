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
    [Authorize]
    public class PersonaDiligenciasController : Controller
    {
        private EncuestaPetroleoEntities db = new EncuestaPetroleoEntities();
        [Authorize]
        // GET: PersonaDiligencias
        public ActionResult Index()
        {
            var USER_id = User.Identity.GetUserId();
            var EM = (from item in db.AspNetUsers
                      where item.Id == USER_id
                      select item.EmpresaId).First();
            var personaDiligencia = db.PersonaDiligencia.Where(x => x.Empresa_id.Equals(EM)).Include(p => p.Empresa);
            return View(personaDiligencia.ToList());
        }

        // GET: PersonaDiligencias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonaDiligencia personaDiligencia = db.PersonaDiligencia.Find(id);
            if (personaDiligencia == null)
            {
                return HttpNotFound();
            }
            return View(personaDiligencia);
        }

        // GET: PersonaDiligencias/Create
        public ActionResult Create()
        {
            var USER_id = User.Identity.GetUserId();
            var NoHaDiligenciadoEmpresa = (from item in db.AspNetUsers
                                            where item.Id == USER_id
                                            select item.EmpresaId).ToList();
            if (NoHaDiligenciadoEmpresa.Count() == 0 || NoHaDiligenciadoEmpresa.First()==0)
            {
                return RedirectToAction("Create", "Empresas");
            }
            if (User.IsInRole("JefeGH"))
            {
                var existeJefeGesHum = (from item in db.JefeGesHum
                                        where item.UserId == USER_id
                                        select item).ToList();
                if (existeJefeGesHum.Count() == 0)
                {
                    return RedirectToAction("Create", "JefeGesHums");
                }


                var Name = (from item in db.JefeGesHum
                            where item.UserId == USER_id
                            select new { item.Nombre, item.Cargo }).First();
                PersonaDiligencia personaDiligencia = new PersonaDiligencia();
                personaDiligencia.Cargo = Name.Cargo;
                personaDiligencia.NombreCompleto = Name.Nombre;
                return View(personaDiligencia);
            }
            else {
                return View();
            }
            
        }

        // POST: PersonaDiligencias/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,NombreCompleto,Cargo,Profesion,Dependencia,Telefono,Celular")] PersonaDiligencia personaDiligencia)
        {
            if (ModelState.IsValid)
            {
                personaDiligencia.UserId = User.Identity.GetUserId();
                var USER_id = User.Identity.GetUserId();
                var EM = (from item in db.AspNetUsers
                          where item.Id == USER_id
                          select item.EmpresaId).First();
                personaDiligencia.Empresa_id = EM;
                db.PersonaDiligencia.Add(personaDiligencia);
                db.SaveChanges();
                return RedirectToAction("Index", "EmpresaEspecialidads");
            }
            return View(personaDiligencia);
        }

        // GET: PersonaDiligencias/Edit/5
        public ActionResult Edit()
        {
            var USER_id = User.Identity.GetUserId();
            var existePersonaDiligencia = (from item in db.PersonaDiligencia
                                           where item.UserId == USER_id
                                           select item).ToList();
            if (existePersonaDiligencia.Count() == 0)
            {
                return RedirectToAction("Create", "PersonaDiligencias");
            }
            var PD = (from item in db.PersonaDiligencia
                      where item.UserId == USER_id
                      select item).First();
            
            PersonaDiligencia personaDiligencia = PD;
            if (personaDiligencia == null)
            {
                return HttpNotFound();
            }
            return View(personaDiligencia);
        }

        // POST: PersonaDiligencias/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,NombreCompleto,Cargo,Profesion,Dependencia,Telefono,Celular,Empresa_id,UserId")] PersonaDiligencia personaDiligencia)
        {
            if (ModelState.IsValid)
            {
                var USER_id = User.Identity.GetUserId();
                var EM = (from item in db.AspNetUsers
                          where item.Id == USER_id
                          select item.EmpresaId).First();
                personaDiligencia.UserId = USER_id;
                personaDiligencia.Empresa_id = EM;
                db.Entry(personaDiligencia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index","EmpresaEspecialidads");
            }
            ViewBag.Empresa_id = new SelectList(db.Empresa, "Id", "Empresa1", personaDiligencia.Empresa_id);
            return View(personaDiligencia);
        }

        // GET: PersonaDiligencias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PersonaDiligencia personaDiligencia = db.PersonaDiligencia.Find(id);
            if (personaDiligencia == null)
            {
                return HttpNotFound();
            }
            return View(personaDiligencia);
        }

        // POST: PersonaDiligencias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PersonaDiligencia personaDiligencia = db.PersonaDiligencia.Find(id);
            db.PersonaDiligencia.Remove(personaDiligencia);
            db.SaveChanges();
            return RedirectToAction("Index");
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
