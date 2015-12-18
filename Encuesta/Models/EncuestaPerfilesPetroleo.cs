//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Encuesta.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class EncuestaPerfilesPetroleo
    {
        public int Id { get; set; }
        public int Cargo { get; set; }
        public string CertificacionesRequeridas { get; set; }
        public int ExperienciaRelacionada_id { get; set; }
        public int NivelEducativo { get; set; }
        public int NoDeCargos { get; set; }
        public string Caracteristicas { get; set; }
        public int Empresa_id { get; set; }
        public int Diligencia_id { get; set; }
        public System.DateTime FechaDiligencia { get; set; }
        public int Especialidad_id { get; set; }
        public int OtraEspecialidad_id { get; set; }
        public string UserId { get; set; }
        public string EstudioRequerido { get; set; }
        public string Observaciones { get; set; }
        public string DescripcionOcupacion { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual Cargos Cargos { get; set; }
        public virtual Empresa Empresa { get; set; }
        public virtual PersonaDiligencia PersonaDiligencia { get; set; }
        public virtual Especialidad Especialidad { get; set; }
        public virtual ExperienciaRelacionada ExperienciaRelacionada { get; set; }
        public virtual NivelEducativo NivelEducativo1 { get; set; }
        public virtual NoDeCargos NoDeCargos1 { get; set; }
        public virtual OtraEspecialidad OtraEspecialidad { get; set; }
    }
}