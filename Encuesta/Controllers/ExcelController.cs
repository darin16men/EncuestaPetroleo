using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using System.ComponentModel;
using OfficeOpenXml.Style;
using System.Data;
using System.IO;
using System.Drawing;
using Encuesta.Models;

namespace Encuesta.Controllers
{
    public class ExcelController : Controller
    {
        private EncuestaPetroleoEntities db = new EncuestaPetroleoEntities();
        // GET: Excel
        public ActionResult Index()
        {
            var result = (from res in db.EncuestaPerfilesPetroleo select res).ToList();

            List<Ocupacionescollection> myListocupacionescollection = new List<Ocupacionescollection>();

            foreach (var resOperacion in result)
            {
                Ocupacionescollection ocupacionescollection = new Ocupacionescollection();
                ocupacionescollection.id = resOperacion.Id;
                ocupacionescollection.EmpresaDiligencia = resOperacion.Empresa.Empresa1;
                ocupacionescollection.DiligenciaNombreCompleto = resOperacion.PersonaDiligencia.NombreCompleto;
                ocupacionescollection.PersonaDiligenciaCargo = resOperacion.PersonaDiligencia.Cargo;
                ocupacionescollection.PersonaDiligenciaProfesion = resOperacion.PersonaDiligencia.Profesion;
                ocupacionescollection.PersonaDiligenciaDependencia = resOperacion.PersonaDiligencia.Dependencia;
                ocupacionescollection.DiligenciaEmail = resOperacion.AspNetUsers.Email;
                ocupacionescollection.FechaDiligenciaOcupacion = resOperacion.FechaDiligencia.ToString(@"yyyy/MM/dd HH\:mm\:ss tt");;
                ocupacionescollection.EspecialidadOcupacion = resOperacion.OtraEspecialidad.OtraEspecialidad1;
                ocupacionescollection.Ocupacion = resOperacion.Cargos.Cargo;
                ocupacionescollection.NivelEducativoOcupacion = resOperacion.NivelEducativo1.NivelEducativo1;
                ocupacionescollection.CursosAdicionalesRequeridosOcupacion = resOperacion.CertificacionesRequeridas;
                if (resOperacion.EstudioRequerido != null)
                {
                    ocupacionescollection.TítuloEnOcupacion = resOperacion.EstudioRequerido;
                }                
                ocupacionescollection.ExperienciaRelacionadaOcupacion = resOperacion.ExperienciaRelacionada.ExperienciaRelacionada1;
                ocupacionescollection.RangoNoDeCargosOcupacion = resOperacion.NoDeCargos1.RangoNoDeCargos;
                ocupacionescollection.FuncionesOcupacion = resOperacion.Caracteristicas.Replace( "\r", "").Replace( "\n", "" );
                ocupacionescollection.DescripcionOcupacion = resOperacion.DescripcionOcupacion.Replace( "\r", "").Replace( "\n", "" );
                if (resOperacion.Observaciones != null) {
                    ocupacionescollection.ObservacionesOcupacion = resOperacion.Observaciones.Replace("\r", "").Replace("\n", "");
                }


                myListocupacionescollection.Add(ocupacionescollection);
            }

            DataTable dt = ConvertToDataTable(myListocupacionescollection);

            MemoryStream memoryStream = new MemoryStream();
            var package = new ExcelPackage(memoryStream);
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Ocupaciones Por especialidad");
            //Step 3 : Start loading datatable form A1 cell of worksheet.
            worksheet.Cells["A1"].LoadFromDataTable(dt, true);
            worksheet.Cells.Style.Font.SetFromFont(new System.Drawing.Font("Calibri", 10));
            worksheet.Cells.AutoFitColumns();
            //Format the header    
            using (ExcelRange objRange = worksheet.Cells["A1:XFD1"])
            {
                objRange.Style.Font.Bold = true;
                objRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                objRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                objRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                objRange.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(221, 23, 33));
            }
            //Step 4 : (Optional) Set the file properties like title, author and subject
            package.Workbook.Properties.Title = @"Consolidado de  Ocupaciones";
            package.Workbook.Properties.Author = "2015 - Unidad del Servicio Público de Empleo";
            package.Workbook.Properties.Subject = @"Definición de Perfiles Petroleros";

            //Step 5 : Save all changes to ExcelPackage object which will create Excel 2007 file.
            package.Save();

            byte[] fileBytes = memoryStream.ToArray();
            string fileName = "Consolidado_" + DateTime.Now.ToString(@"yyyyMMddHHmmss") + ".xlsx";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }


        // GET: ExcelReuniones
        public ActionResult Reuniones()
        {
            var result = (from res in db.PersonasReunionesPerfiles select res).ToList();

            List<PerReunionescollection> myListPerReunionescollection = new List<PerReunionescollection>();

            foreach (var PerReunion in result)
            {
                PerReunionescollection perReunionescollection = new PerReunionescollection();
                perReunionescollection.id = PerReunion.id;
                perReunionescollection.Empresa = PerReunion.Empresa.Empresa1;
                perReunionescollection.Especialidad = PerReunion.OtraEspecialidad.OtraEspecialidad1;
                perReunionescollection.NombreReunionesPerfiles = PerReunion.Nombre;
                perReunionescollection.Profesion = PerReunion.Profesion;
                perReunionescollection.CargoDependencia = PerReunion.CargoDependencia;
                perReunionescollection.CorreoElectronico = PerReunion.CorreoElectronico;
                perReunionescollection.TelefonoCelular = PerReunion.TelefonoCelular;

                myListPerReunionescollection.Add(perReunionescollection);
            }

            DataTable dt = ConvertToDataTable(myListPerReunionescollection);

            MemoryStream memoryStream = new MemoryStream();
            var package = new ExcelPackage(memoryStream);
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Personas Reuniones Perfiles");
            //Step 3 : Start loading datatable form A1 cell of worksheet.
            worksheet.Cells["A1"].LoadFromDataTable(dt, true);
            worksheet.Cells.Style.Font.SetFromFont(new System.Drawing.Font("Calibri", 10));
            worksheet.Cells.AutoFitColumns();
            //Format the header    
            using (ExcelRange objRange = worksheet.Cells["A1:XFD1"])
            {
                objRange.Style.Font.Bold = true;
                objRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                objRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                objRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                objRange.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(221, 23, 33));
            }
            //Step 4 : (Optional) Set the file properties like title, author and subject
            package.Workbook.Properties.Title = @"Personas Reuniones Perfiles";
            package.Workbook.Properties.Author = "2015 - Unidad del Servicio Público de Empleo";
            package.Workbook.Properties.Subject = @"Definición de Perfiles Petroleros";

            //Step 5 : Save all changes to ExcelPackage object which will create Excel 2007 file.
            package.Save();

            byte[] fileBytes = memoryStream.ToArray();
            string fileName = "Personas_Reuniones_" + DateTime.Now.ToString(@"yyyyMMddHHmmss") + ".xlsx";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        public partial class Ocupacionescollection
        {
            public int id { get; set; }
            public string EmpresaDiligencia { get; set; }
            public string DiligenciaNombreCompleto { get; set; }
            public string PersonaDiligenciaCargo { get; set; }
            public string PersonaDiligenciaProfesion { get; set; }
            public string PersonaDiligenciaDependencia { get; set; }
            public string DiligenciaEmail { get; set; }
            public string FechaDiligenciaOcupacion { get; set; }
            public string EspecialidadOcupacion { get; set; }
            public string Ocupacion { get; set; }
            public string NivelEducativoOcupacion { get; set; }
            public string CursosAdicionalesRequeridosOcupacion { get; set; }
            public string TítuloEnOcupacion { get; set; }
            public string ExperienciaRelacionadaOcupacion { get; set; }
            public string RangoNoDeCargosOcupacion { get; set; }
            public string FuncionesOcupacion { get; set; }
            public string DescripcionOcupacion { get; set; }
            public string ObservacionesOcupacion { get; set; }
        }

        public partial class PerReunionescollection
        {
            public int id { get; set; }
            public string Empresa { get; set; }
            public string Especialidad { get; set; }
            public string NombreReunionesPerfiles { get; set; }
            public string Profesion { get; set; }
            public string CargoDependencia { get; set; }
            public string CorreoElectronico { get; set; }
            public string TelefonoCelular { get; set; }
        }
        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;

        }
    }
}