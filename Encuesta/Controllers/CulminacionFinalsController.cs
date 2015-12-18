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
      [Authorize(Roles = "JefeGH")]
    public class CulminacionFinalsController : Controller
    {
        private EncuestaPetroleoEntities db = new EncuestaPetroleoEntities();


        // GET: CulminacionFinals/Create
        public ActionResult Culminar()
        {
            var USER_id = User.Identity.GetUserId();
            var EM = (from item in db.AspNetUsers
                      where item.Id == USER_id
                      select item.EmpresaId).First();
            CulminacionFinal culminacionFinal = new CulminacionFinal();
            culminacionFinal.EmpresaId = EM;
            culminacionFinal.FechaCulminacion = DateTime.Now;
            culminacionFinal.UserId = USER_id;
            db.CulminacionFinal.Add(culminacionFinal);
            db.SaveChanges();
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
