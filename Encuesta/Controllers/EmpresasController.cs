using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Encuesta.Models;
using System.Globalization;
using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace IdentitySample.Controllers
{
    [Authorize]
    public class EmpresasController : Controller
    {
        private EncuestaPetroleoEntities db = new EncuestaPetroleoEntities();

        // GET: Empresas
        public ActionResult Index()
        {
            return View(db.Empresa.ToList());
        }

        // GET: Empresas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Empresa empresa = db.Empresa.Find(id);
            if (empresa == null)
            {
                return HttpNotFound();
            }
            return View(empresa);
        }

        // GET: Empresas/Create
        public ActionResult Create()
        {
            if (!User.IsInRole("JefeGH"))
            {
                 return RedirectToAction("Login", "Account");
            }
             return View();
        }

        // POST: Empresas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Empresa1,NoTotalTrabajadores,Nit")] Empresa empresa)
        {
            string message = "";
            if (ModelState.IsValid)
            {
                var existeEmpresa = (from item in db.Empresa
                              where item.Empresa1 == empresa.Empresa1
                              select item).ToList();
                var existeNit = (from item in db.Empresa
                              where item.Nit == empresa.Nit
                              select item).ToList();
                if (existeEmpresa.Count() == 0)
                {
                    if (existeNit.Count() == 0)
                    {
                        empresa.UserID = User.Identity.GetUserId();
                        db.Empresa.Add(empresa);
                        db.SaveChanges(); 
                        var empresaID = (from item in db.Empresa
                                         where item.Nit == empresa.Nit
                                         select item.Id).First();
                        AspNetUsers UsurioEmpresa = db.AspNetUsers.Find(User.Identity.GetUserId());
                        UsurioEmpresa.EmpresaId = empresaID;
                        db.Entry(UsurioEmpresa).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();                       

                        return RedirectToAction("Create", "JefeGesHums");
                    }
                    else {
                        message = "El Nit de la Empresa Ya Existe!!";
                        
                         }
                 }
                 else{
                     message = "El nombre Empresa Ya Existe!!";
                     }
             }
            ViewBag.message = message;
            return View(empresa);
            
        }

        // GET: Empresas/Edit/5
        public ActionResult Edit()
        {
            var USER_id = User.Identity.GetUserId();
            var EM = (from item in db.Empresa
                      where item.UserID == USER_id
                select item).First();
            Empresa empresa = EM;
            if (empresa == null)
            {
                return HttpNotFound();
            }
            return View(empresa);
        }

        // POST: Empresas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Empresa1,NoTotalTrabajadores,Nit")] Empresa empresa)
        {
            if (ModelState.IsValid)
            {
                var USER_id = User.Identity.GetUserId();
                empresa.UserID = USER_id;
                db.Entry(empresa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(empresa);
        }

        // GET: Empresas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Empresa empresa = db.Empresa.Find(id);
            if (empresa == null)
            {
                return HttpNotFound();
            }
            return View(empresa);
        }

        // POST: Empresas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Empresa empresa = db.Empresa.Find(id);
            db.Empresa.Remove(empresa);
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
