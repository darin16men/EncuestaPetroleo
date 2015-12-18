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

namespace Microsoft.AspNet.Identity
{
    [Authorize]
    public class EmpresaEspecialidadsController : Controller
    {
        private EncuestaPetroleoEntities db = new EncuestaPetroleoEntities();
        [Authorize]
        // GET: EmpresaEspecialidads
        public ActionResult Index(IdentitySample.Controllers.ManageController.ManageMessageId? message)
        {
            ViewBag.StatusMessage =
            message == IdentitySample.Controllers.ManageController.ManageMessageId.ChangePasswordSuccess ? "Su contraseña ha sido cambiada."
            : message == IdentitySample.Controllers.ManageController.ManageMessageId.SetPasswordSuccess ? "Su contraseña ha sido establecido."
            : message == IdentitySample.Controllers.ManageController.ManageMessageId.SetTwoFactorSuccess ? "Su proveedor de dos factores se ha establecido."
            : message == IdentitySample.Controllers.ManageController.ManageMessageId.Error ? "Se ha producido un error."
            : message == IdentitySample.Controllers.ManageController.ManageMessageId.AddPhoneSuccess ? "Se añadió el número de teléfono."
            : message == IdentitySample.Controllers.ManageController.ManageMessageId.RemovePhoneSuccess ? "Se retiró Su número de teléfono."
            : "";
            var USER_id = User.Identity.GetUserId();
            if (!User.IsInRole("Admin"))
            {
                if (Request.IsAuthenticated)
                {
                    var ChangePassword = (from cp in db.AspNetUsers where cp.Id == USER_id select cp.PasswordChange).First();
                    if (ChangePassword)
                    {
                        return RedirectToAction("ChangePassword", "Manage");
                    }
                    if (User.IsInRole("JefeGH"))
                    {
                        var NoHaDiligenciadoEmpresa = (from item in db.Empresa
                                                       where item.UserID == USER_id
                                                       select item).ToList();
                        if (NoHaDiligenciadoEmpresa.Count() == 0)
                        {
                            return RedirectToAction("Create", "Empresas");
                        }
                        var existeJefeRH = (from item in db.JefeGesHum
                                            where item.UserId == USER_id
                                            select item).ToList();
                        if (existeJefeRH.Count() == 0)
                        {
                            return RedirectToAction("Create", "JefeGesHums");
                        }
                    }
                    var existePersonaDiligencia = (from item in db.PersonaDiligencia
                                                   where item.UserId == USER_id
                                                   select item).ToList();
                    if (existePersonaDiligencia.Count() == 0 && !User.IsInRole("JefeGH"))
                    {
                        return RedirectToAction("Create", "PersonaDiligencias");
                    }
                }
                else {
                    return RedirectToAction("Login","Account");
                }
            }
            else {
                return RedirectToAction("Index", "UsersAdmin");
            }
             var EM = (from item in db.AspNetUsers
                      where item.Id == USER_id
                      select item.EmpresaId).First();
             var empresaEspecialidad = db.EmpresaEspecialidad.Where(x => x.Empresa.Id.Equals(EM)).Include(e => e.Empresa).Include(e => e.OtraEspecialidad);
            if (User.IsInRole("JefeGH"))
            {
                ViewBag.rol = "jefe";
            }
            else {
                ViewBag.rol = "noJefe";
            }
            var Culminadas = (from item in db.CulminacionFinal
                                 where item.EmpresaId == EM
                                select item.id).ToList();
            if (User.IsInRole("JefeGH") && Culminadas.Count()==1)
            {
                ViewBag.Culminada = "si";
            }
            else if (User.IsInRole("JefeGH"))
            {
                var EspCulminadas = (from item in db.EmpresaEspecialidad.Where(x => x.EmpresaId.Equals(EM))
                                  where item.NoAplica==true || item.EspecialidadCulminada==true
                                  select item.Id).ToList();
                if (User.IsInRole("JefeGH") && EspCulminadas.Count() >= 6)
                {
                    ViewBag.Culminada = "Exito";
                }
                else {
                    ViewBag.Culminada = "no";
                }
                if (User.IsInRole("JefeGH") && EspCulminadas.Count() >= 3)
                {
                    ViewBag.Aplica = "MostrarNoaplica";
                }
            }
            return View(empresaEspecialidad.ToList());
        }

        // GET: EmpresaEspecialidads/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "EmpresaEspecialidads");
            }
            EmpresaEspecialidad empresaEspecialidad = db.EmpresaEspecialidad.Find(id);
            if (empresaEspecialidad == null)
            {
                return HttpNotFound();
            }
            return View(empresaEspecialidad);
        }

        // GET: EmpresaEspecialidads/Create
        [Authorize]
        public ActionResult Create()
        {
            var USER_id = User.Identity.GetUserId();
            var EM = (from item in db.AspNetUsers
                     where item.Id == USER_id
                     select item.EmpresaId).First();
            var linked = from foo in db.Especialidad
                         from bar in (from o in db.EmpresaEspecialidad
                                 where o.EmpresaId == EM
                                 select o.Especialidadid)
                         where foo.Id == bar
                         select foo;
            var notLinked =  db.Especialidad.Except(linked);
            
            ViewBag.Especialidadid = new SelectList(notLinked, "Id", "Especialidad1");
            return View();
        }

        // POST: EmpresaEspecialidads/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,EmpresaId,Especialidadid")] EmpresaEspecialidad empresaEspecialidad, string OtraEspecialidad)
        {
            if (ModelState.IsValid)
            {
                var USER_id = User.Identity.GetUserId();
                var EM = (from item in db.AspNetUsers
                          where item.Id == USER_id
                          select item.EmpresaId).First();
                empresaEspecialidad.EmpresaId = EM;
                if(empresaEspecialidad.Especialidadid==7){
                    OtraEspecialidad OtraEsp = new OtraEspecialidad();
                    OtraEsp.OtraEspecialidad1 = OtraEspecialidad;
                    db.OtraEspecialidad.Add(OtraEsp);
                    db.SaveChanges();
                     empresaEspecialidad.Especialidadid=OtraEsp.Id;
                }
                empresaEspecialidad.NoAplica = false;
                empresaEspecialidad.EspecialidadCulminada = false;
                empresaEspecialidad.FechaCulminacion = DateTime.Now;
                empresaEspecialidad.UserId = USER_id;
                db.EmpresaEspecialidad.Add(empresaEspecialidad);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmpresaId = new SelectList(db.Empresa, "Id", "Empresa1", empresaEspecialidad.EmpresaId);
            ViewBag.Especialidadid = new SelectList(db.OtraEspecialidad, "Id", "OtraEspecialidad1", empresaEspecialidad.Especialidadid);
            return View(empresaEspecialidad);
        }

        // GET: EmpresaEspecialidads/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "EmpresaEspecialidads");
            }
            EmpresaEspecialidad empresaEspecialidad = db.EmpresaEspecialidad.Find(id);
            if (empresaEspecialidad == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmpresaId = new SelectList(db.Empresa, "Id", "Empresa1", empresaEspecialidad.EmpresaId);
            ViewBag.Especialidadid = new SelectList(db.OtraEspecialidad, "Id", "OtraEspecialidad1", empresaEspecialidad.Especialidadid);
            return View(empresaEspecialidad);
        }

        // POST: EmpresaEspecialidads/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EmpresaId,Especialidadid")] EmpresaEspecialidad empresaEspecialidad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(empresaEspecialidad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmpresaId = new SelectList(db.Empresa, "Id", "Empresa1", empresaEspecialidad.EmpresaId);
            ViewBag.Especialidadid = new SelectList(db.OtraEspecialidad, "Id", "OtraEspecialidad1", empresaEspecialidad.Especialidadid);
            return View(empresaEspecialidad);
        }

        // GET: EmpresaEspecialidads/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "EmpresaEspecialidads");
            }
            EmpresaEspecialidad empresaEspecialidad = db.EmpresaEspecialidad.Find(id);
            if (empresaEspecialidad == null)
            {
                return HttpNotFound();
            }
            return View(empresaEspecialidad);
        }

        // POST: EmpresaEspecialidads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EmpresaEspecialidad empresaEspecialidad = db.EmpresaEspecialidad.Find(id);
            db.EmpresaEspecialidad.Remove(empresaEspecialidad);
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
