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
using System.Text.RegularExpressions;

namespace Encuesta.Controllers
{
    [Authorize]
    public class EncuestaPerfilesPetroleosController : Controller
    {
        private EncuestaPetroleoEntities db = new EncuestaPetroleoEntities();

        // GET: EncuestaPerfilesPetroleos
        public ActionResult Index(int? id)
        {
            if (id == null) {
                return RedirectToAction("Index", "EmpresaEspecialidads");
            }
            var USER_id = User.Identity.GetUserId();
            var EM = (from item in db.AspNetUsers
                      where item.Id == USER_id
                      select item.EmpresaId).First();

            if (User.IsInRole("JefeGH"))
            {
                var encuestaPerfilesPetroleo = db.EncuestaPerfilesPetroleo.Where(x => x.Empresa_id.Equals(EM));
                ViewBag.Especilidad = db.OtraEspecialidad.Where(x => x.Id == id).First().OtraEspecialidad1;
                ViewBag.EspecilidadId = id;
                if (encuestaPerfilesPetroleo.Count() > 0)
                {
                    return View(encuestaPerfilesPetroleo.Where(x => x.OtraEspecialidad_id == id).Include(e => e.Cargos).Include(e => e.Empresa).Include(e => e.PersonaDiligencia).Include(e => e.Especialidad).Include(e => e.ExperienciaRelacionada).Include(e => e.NivelEducativo1).Include(e => e.NoDeCargos1).Include(e => e.OtraEspecialidad).ToList());
                }
                else
                {
                    return View(encuestaPerfilesPetroleo.ToList());
                }
            }
            else {
                var encuestaPerfilesPetroleo = db.EncuestaPerfilesPetroleo.Where(x => x.UserId.Equals(USER_id));
                ViewBag.Especilidad = db.OtraEspecialidad.Where(x => x.Id == id).First().OtraEspecialidad1;
                ViewBag.EspecilidadId = id;
                if (encuestaPerfilesPetroleo.Count()>0)
                {
                    return View(encuestaPerfilesPetroleo.Where(x => x.OtraEspecialidad_id == id).Include(e => e.Cargos).Include(e => e.Empresa).Include(e => e.PersonaDiligencia).Include(e => e.Especialidad).Include(e => e.ExperienciaRelacionada).Include(e => e.NivelEducativo1).Include(e => e.NoDeCargos1).Include(e => e.OtraEspecialidad).ToList());
                }
                else {
                    return View(encuestaPerfilesPetroleo.ToList());
                }
            }
        }

        // GET: EncuestaPerfilesPetroleos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EncuestaPerfilesPetroleo encuestaPerfilesPetroleo = db.EncuestaPerfilesPetroleo.Find(id);
            if (encuestaPerfilesPetroleo == null)
            {
                return HttpNotFound();
            }
            
            return View(encuestaPerfilesPetroleo);
        }

        // GET: EncuestaPerfilesPetroleos/Create

        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "EmpresaEspecialidads");
            }
           var USER_id = User.Identity.GetUserId();
            var existePersonaDiligencia = (from item in db.PersonaDiligencia
                                           where item.UserId == USER_id
                                           select item).ToList();
            if (existePersonaDiligencia.Count() == 0 && User.IsInRole("Diligencia"))
            {
                return RedirectToAction("Create", "PersonaDiligencias");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.ExperienciaRelacionada_id = new SelectList(db.ExperienciaRelacionada, "Id", "ExperienciaRelacionada1");
            ViewBag.NivelEducativo = new SelectList(db.NivelEducativo, "Id", "NivelEducativo1");
            ViewBag.NoDeCargos = new SelectList(db.NoDeCargos, "id", "RangoNoDeCargos");
            ViewBag.TipoCargo_id = new SelectList(db.TipoCago, "id", "Tipo");
            ViewBag.OtraEspecialidad_id = new SelectList(db.OtraEspecialidad, "Id", "OtraEspecialidad1", id);
            ViewBag.Especialidad_id = new SelectList(db.Especialidad, "Id", "Especialidad1", id);
            var EM = (from item in db.AspNetUsers
                      where item.Id == USER_id
                      select item.EmpresaId).First();
            var cargosesp = (from c in db.Cargos where !(from e in db.EncuestaPerfilesPetroleo where e.Empresa_id.Equals(EM) select e.Cargo).Contains(c.id) && c.EspecialidadId == id || c.EspecialidadId == 7 select c).ToList();
            ViewBag.Cargo = new SelectList(cargosesp, "id", "Cargo");
            ViewBag.Especilidad = db.OtraEspecialidad.Where(x => x.Id == id).First().OtraEspecialidad1;
            return View();
        }

        // POST: EncuestaPerfilesPetroleos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Cargo,CertificacionesRequeridas,ExperienciaRelacionada_id,NivelEducativo,NoDeCargos,Caracteristicas,Especialidad_id,OtraEspecialidad_id,EstudioRequerido,Observaciones, DescripcionOcupacion")] EncuestaPerfilesPetroleo encuestaPerfilesPetroleo, string OtraOcupacion)
        {
            var USER_id = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                var existePersonaDiligencia = (from item in db.PersonaDiligencia
                                               where item.UserId == USER_id
                                               select item).ToList();
                if (existePersonaDiligencia.Count() == 1)
                {
                    if (OtraOcupacion != "")
                    {
                        Cargos cargo = new Cargos();
                        cargo.EspecialidadId = encuestaPerfilesPetroleo.OtraEspecialidad_id;
                        cargo.Cargo = OtraOcupacion;
                        db.Cargos.Add(cargo);
                        db.SaveChanges();
                        encuestaPerfilesPetroleo.Cargo = cargo.id;
                    }
                    var EM = (from item in db.AspNetUsers
                              where item.Id == USER_id
                              select item.EmpresaId).First();

                    var PD = (from item in db.PersonaDiligencia
                              where item.UserId == USER_id
                              select item.id).First();
                    encuestaPerfilesPetroleo.Empresa_id = EM;
                    encuestaPerfilesPetroleo.UserId = USER_id;
                    encuestaPerfilesPetroleo.Diligencia_id = PD;
                    encuestaPerfilesPetroleo.FechaDiligencia = DateTime.Now;
                    db.EncuestaPerfilesPetroleo.Add(encuestaPerfilesPetroleo);
                    db.SaveChanges();
                    return RedirectToAction("Index/" + encuestaPerfilesPetroleo.OtraEspecialidad_id);
                }
                else {
                    return RedirectToAction("Create", "PersonaDiligencias");
                     }
            }
            ViewBag.ExperienciaRelacionada_id = new SelectList(db.ExperienciaRelacionada, "Id", "ExperienciaRelacionada1", encuestaPerfilesPetroleo.ExperienciaRelacionada_id);
            ViewBag.NivelEducativo = new SelectList(db.NivelEducativo, "Id", "NivelEducativo1", encuestaPerfilesPetroleo.NivelEducativo);
            ViewBag.NoDeCargos = new SelectList(db.NoDeCargos, "id", "RangoNoDeCargos", encuestaPerfilesPetroleo.NoDeCargos);
            ViewBag.OtraEspecialidad_id = new SelectList(db.OtraEspecialidad, "Id", "OtraEspecialidad1", encuestaPerfilesPetroleo.Especialidad_id);
            ViewBag.Especialidad_id = new SelectList(db.Especialidad, "Id", "Especialidad1", encuestaPerfilesPetroleo.Especialidad_id);
            var EMP = (from item in db.AspNetUsers
                      where item.Id == USER_id
                      select item.EmpresaId).First();
            var cargosesp = (from c in db.Cargos where !(from e in db.EncuestaPerfilesPetroleo where e.Empresa_id.Equals(EMP) select e.Cargo).Contains(c.id) && c.EspecialidadId == encuestaPerfilesPetroleo.OtraEspecialidad_id || c.EspecialidadId == 7 select c).ToList();
            ViewBag.Cargo = new SelectList(cargosesp, "id", "Cargo");
            return View(encuestaPerfilesPetroleo);
        }

        // GET: EncuestaPerfilesPetroleos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "EmpresaEspecialidads");
            }
            EncuestaPerfilesPetroleo encuestaPerfilesPetroleo = db.EncuestaPerfilesPetroleo.Find(id);
            if (encuestaPerfilesPetroleo == null)
            {
                return HttpNotFound();
            }
            ViewBag.Empresa_id = new SelectList(db.Empresa, "Id", "Empresa1", encuestaPerfilesPetroleo.Empresa_id);
            ViewBag.Cargo = new SelectList(db.Cargos.OrderBy(x => x.EspecialidadId).ThenBy(x => x.Cargo).Where(x => x.EspecialidadId == encuestaPerfilesPetroleo.OtraEspecialidad_id || x.EspecialidadId == 7), "id", "Cargo",encuestaPerfilesPetroleo.Cargo);
            ViewBag.Diligencia_id = new SelectList(db.PersonaDiligencia, "id", "NombreCompleto", encuestaPerfilesPetroleo.Diligencia_id);
            ViewBag.Especialidad_id = new SelectList(db.Especialidad, "Id", "Especialidad1", encuestaPerfilesPetroleo.Especialidad_id);
            ViewBag.ExperienciaRelacionada_id = new SelectList(db.ExperienciaRelacionada, "Id", "ExperienciaRelacionada1", encuestaPerfilesPetroleo.ExperienciaRelacionada_id);
            ViewBag.NivelEducativo = new SelectList(db.NivelEducativo, "Id", "NivelEducativo1", encuestaPerfilesPetroleo.NivelEducativo);
            ViewBag.NoDeCargos = new SelectList(db.NoDeCargos, "id", "RangoNoDeCargos", encuestaPerfilesPetroleo.NoDeCargos);
            ViewBag.OtraEspecialidad_id = new SelectList(db.OtraEspecialidad, "Id", "OtraEspecialidad1", encuestaPerfilesPetroleo.OtraEspecialidad_id);
            ViewBag.Especilidad = db.OtraEspecialidad.Where(x => x.Id == encuestaPerfilesPetroleo.OtraEspecialidad_id).First().OtraEspecialidad1;
            return View(encuestaPerfilesPetroleo);
        }

        // POST: EncuestaPerfilesPetroleos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Cargo,CertificacionesRequeridas,ExperienciaRelacionada_id,NivelEducativo,NoDeCargos,Caracteristicas,Diligencia_id,FechaDiligencia,Especialidad_id,OtraEspecialidad_id,UserId,EstudioRequerido,Observaciones, DescripcionOcupacion")] EncuestaPerfilesPetroleo encuestaPerfilesPetroleo)
        {
            if (ModelState.IsValid)
            {
                var USER_id = User.Identity.GetUserId();
                var EM = (from item in db.AspNetUsers
                          where item.Id == USER_id
                          select item.EmpresaId).First();
                var PD = (from item in db.PersonaDiligencia
                          where item.UserId == USER_id
                          select item.id).First();
                encuestaPerfilesPetroleo.Empresa_id = EM;
                encuestaPerfilesPetroleo.UserId = USER_id;
                encuestaPerfilesPetroleo.Diligencia_id = PD;
                encuestaPerfilesPetroleo.FechaDiligencia = DateTime.Now;
                db.Entry(encuestaPerfilesPetroleo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index/" + encuestaPerfilesPetroleo.OtraEspecialidad_id);
            }
            ViewBag.Cargo = new SelectList(db.Cargos.OrderBy(x => x.EspecialidadId).ThenBy(x => x.Cargo).Where(x => x.EspecialidadId == encuestaPerfilesPetroleo.OtraEspecialidad_id || x.EspecialidadId == 7), "id", "Cargo", encuestaPerfilesPetroleo.Cargo);
            ViewBag.Empresa_id = new SelectList(db.Empresa, "Id", "Empresa1", encuestaPerfilesPetroleo.Empresa_id);
            ViewBag.Diligencia_id = new SelectList(db.PersonaDiligencia, "id", "NombreCompleto", encuestaPerfilesPetroleo.Diligencia_id);
            ViewBag.Especialidad_id = new SelectList(db.Especialidad, "Id", "Especialidad1", encuestaPerfilesPetroleo.Especialidad_id);
            ViewBag.ExperienciaRelacionada_id = new SelectList(db.ExperienciaRelacionada, "Id", "ExperienciaRelacionada1", encuestaPerfilesPetroleo.ExperienciaRelacionada_id);
            ViewBag.NivelEducativo = new SelectList(db.NivelEducativo, "Id", "NivelEducativo1", encuestaPerfilesPetroleo.NivelEducativo);
            ViewBag.NoDeCargos = new SelectList(db.NoDeCargos, "id", "RangoNoDeCargos", encuestaPerfilesPetroleo.NoDeCargos);
            ViewBag.OtraEspecialidad_id = new SelectList(db.OtraEspecialidad, "Id", "OtraEspecialidad1", encuestaPerfilesPetroleo.OtraEspecialidad_id);
            return View(encuestaPerfilesPetroleo);
        }

        // GET: EncuestaPerfilesPetroleos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "EmpresaEspecialidads");
            }
            EncuestaPerfilesPetroleo encuestaPerfilesPetroleo = db.EncuestaPerfilesPetroleo.Find(id);
            if (encuestaPerfilesPetroleo == null)
            {
                return HttpNotFound();
            }
            return View(encuestaPerfilesPetroleo);
        }

        // POST: EncuestaPerfilesPetroleos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EncuestaPerfilesPetroleo encuestaPerfilesPetroleo = db.EncuestaPerfilesPetroleo.Find(id);
            db.EncuestaPerfilesPetroleo.Remove(encuestaPerfilesPetroleo);
            db.SaveChanges();

            return RedirectToAction("Index/" + encuestaPerfilesPetroleo.Especialidad_id);
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
