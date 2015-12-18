using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Encuesta.Models;

namespace Encuesta.Views
{
    public class OtraEspecialidadsController : Controller
    {
        private EncuestaPetroleoEntities db = new EncuestaPetroleoEntities();

        // GET: OtraEspecialidads
        public ActionResult Index()
        {
            return View(db.OtraEspecialidad.ToList());
        }

        // GET: OtraEspecialidads/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OtraEspecialidad otraEspecialidad = db.OtraEspecialidad.Find(id);
            if (otraEspecialidad == null)
            {
                return HttpNotFound();
            }
            return View(otraEspecialidad);
        }

        // GET: OtraEspecialidads/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OtraEspecialidads/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,OtraEspecialidad1")] OtraEspecialidad otraEspecialidad)
        {
            if (ModelState.IsValid)
            {
                db.OtraEspecialidad.Add(otraEspecialidad);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(otraEspecialidad);
        }

        // GET: OtraEspecialidads/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OtraEspecialidad otraEspecialidad = db.OtraEspecialidad.Find(id);
            if (otraEspecialidad == null)
            {
                return HttpNotFound();
            }
            return View(otraEspecialidad);
        }

        // POST: OtraEspecialidads/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,OtraEspecialidad1")] OtraEspecialidad otraEspecialidad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(otraEspecialidad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(otraEspecialidad);
        }

        // GET: OtraEspecialidads/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OtraEspecialidad otraEspecialidad = db.OtraEspecialidad.Find(id);
            if (otraEspecialidad == null)
            {
                return HttpNotFound();
            }
            return View(otraEspecialidad);
        }

        // POST: OtraEspecialidads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OtraEspecialidad otraEspecialidad = db.OtraEspecialidad.Find(id);
            db.OtraEspecialidad.Remove(otraEspecialidad);
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
