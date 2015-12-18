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
    public class JefeGesHumsController : Controller
    {
        private EncuestaPetroleoEntities db = new EncuestaPetroleoEntities();
        [Authorize]
        // GET: JefeGesHums
        public ActionResult Index()
        {
            var jefeGesHum = db.JefeGesHum.Include(j => j.Empresa);
            return View(jefeGesHum.ToList());
        }

        // GET: JefeGesHums/Details/5
        public ActionResult Details()
        {
            var USER_id = User.Identity.GetUserId();
            var JG = (from item in db.JefeGesHum
                      where item.UserId == USER_id
                      select item).First();
            JefeGesHum jefeGesHum = JG;
              if (jefeGesHum == null)
            {
                return HttpNotFound();
            }
            return View(jefeGesHum);
        }

        // GET: JefeGesHums/Create
        public ActionResult Create()
        {
            var USER_id = User.Identity.GetUserId();
            var ChangePassword = (from cp in db.AspNetUsers where cp.Id == USER_id select cp.PasswordChange).First();
            if (ChangePassword)
            {
                return RedirectToAction("ChangePassword", "Manage");
            }
            var NoHaDiligenciadoEmpresa = (from item in db.Empresa
                                           where item.UserID == USER_id
                                           select item).ToList();
            if (NoHaDiligenciadoEmpresa.Count() == 0)
            {
              return  RedirectToAction("Create", "Empresas");
            }
            ViewBag.Empresa_id = new SelectList(db.Empresa, "Id", "Empresa1");
            return View();
        }

        // POST: JefeGesHums/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Nombre,Cargo,CedulaCiudadania,Telefono,Celular,CorreoElectronioco,Direccion")] JefeGesHum jefeGesHum , string Profesion, string Dependencia)
        {
            string message = "";
            if (ModelState.IsValid)
            {
                var existeCedula = (from item in db.JefeGesHum
                                     where item.CedulaCiudadania == jefeGesHum.CedulaCiudadania
                                     select item).ToList();
                if (existeCedula.Count() == 0)
                {
                    var USER_id = User.Identity.GetUserId();
                    var EM = (from item in db.Empresa
                              where item.UserID == USER_id
                              select item.Id).First();
                    AspNetUsers  usuario = db.AspNetUsers.Find(USER_id);
                    usuario.EmpresaId = EM;
                    db.Entry(usuario).State = EntityState.Modified;
                    db.SaveChanges();
                    jefeGesHum.UserId = User.Identity.GetUserId();
                    jefeGesHum.EmpresaId = EM;
                    db.JefeGesHum.Add(jefeGesHum);
                    db.SaveChanges();
                    PersonaDiligencia personaDiligencia = new PersonaDiligencia();
                    personaDiligencia.Cargo = jefeGesHum.Cargo;
                    personaDiligencia.Celular = jefeGesHum.Celular.ToString();
                    personaDiligencia.Telefono = jefeGesHum.Telefono;
                    personaDiligencia.Dependencia = Dependencia;
                    personaDiligencia.Empresa_id = EM;
                    personaDiligencia.NombreCompleto = jefeGesHum.Nombre;
                    personaDiligencia.Profesion = Profesion;
                    personaDiligencia.UserId = USER_id;
                    db.PersonaDiligencia.Add(personaDiligencia);
                    db.SaveChanges();
                    return RedirectToAction("Index", "EmpresaEspecialidads");
                }
                else
                {
                    message = "Cedula de ciudadanía ya registrada!!";
                }
            }
            ViewBag.Profesion = Dependencia;
            ViewBag.Dependencia = Profesion;
            ViewBag.message = message;
            return View(jefeGesHum);
        }

        // GET: JefeGesHums/Edit/5
        public ActionResult Edit()
        {
            var USER_id = User.Identity.GetUserId();
            var existeJefeGesHum = (from item in db.JefeGesHum
                                           where item.UserId == USER_id
                                           select item).ToList();
            if (existeJefeGesHum.Count() == 0)
            {
                return RedirectToAction("Create", "JefeGesHums");
            }
            var JG = (from item in db.JefeGesHum
                      where item.UserId == USER_id
                      select item).First();
            JefeGesHum jefeGesHum = JG;
            if (jefeGesHum == null)
            {
                return HttpNotFound();
            }
            ViewBag.Profesion = (from dil in db.PersonaDiligencia where dil.UserId == USER_id select dil.Profesion).First();
            ViewBag.Dependencia = (from dil in db.PersonaDiligencia where dil.UserId == USER_id select dil.Dependencia).First();
            return View(jefeGesHum);
        }

        // POST: JefeGesHums/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Nombre,Cargo,CedulaCiudadania,Telefono,Celular,CorreoElectronioco,Direccion")] JefeGesHum jefeGesHum, string Profesion, string Dependencia)
        {
            var USER_id = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {       var EM = (from item in db.AspNetUsers
                              where item.Id == USER_id
                              select item.EmpresaId).First();
                    jefeGesHum.UserId = User.Identity.GetUserId();
                    jefeGesHum.EmpresaId = EM;
                    db.Entry(jefeGesHum).State = EntityState.Modified;
                    db.SaveChanges();
                    PersonaDiligencia personaDiligencia = (from dil in db.PersonaDiligencia where dil.UserId == USER_id select dil).First();
                    personaDiligencia.Cargo = jefeGesHum.Cargo;
                    personaDiligencia.Celular = jefeGesHum.Celular.ToString();
                    personaDiligencia.Telefono = jefeGesHum.Telefono;
                    personaDiligencia.Dependencia = Dependencia;
                    personaDiligencia.Empresa_id = EM;
                    personaDiligencia.NombreCompleto = jefeGesHum.Nombre;
                    personaDiligencia.Profesion = Profesion;
                    db.Entry(personaDiligencia).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.Profesion = Dependencia;
                    ViewBag.Dependencia = Profesion;
                    return View(jefeGesHum);
            }
            ViewBag.Profesion = Dependencia;
            ViewBag.Dependencia = Profesion;
            return View(jefeGesHum);
        }

        // GET: JefeGesHums/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JefeGesHum jefeGesHum = db.JefeGesHum.Find(id);
            if (jefeGesHum == null)
            {
                return HttpNotFound();
            }
            return View(jefeGesHum);
        }

        // POST: JefeGesHums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JefeGesHum jefeGesHum = db.JefeGesHum.Find(id);
            db.JefeGesHum.Remove(jefeGesHum);
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
