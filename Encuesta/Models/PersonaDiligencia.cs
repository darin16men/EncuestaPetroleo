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
    
    public partial class PersonaDiligencia
    {
        public PersonaDiligencia()
        {
            this.EncuestaPerfilesPetroleo = new HashSet<EncuestaPerfilesPetroleo>();
        }
    
        public int id { get; set; }
        public string NombreCompleto { get; set; }
        public string Cargo { get; set; }
        public string Profesion { get; set; }
        public string Dependencia { get; set; }
        public int Telefono { get; set; }
        public string Celular { get; set; }
        public int Empresa_id { get; set; }
        public string UserId { get; set; }
    
        public virtual Empresa Empresa { get; set; }
        public virtual ICollection<EncuestaPerfilesPetroleo> EncuestaPerfilesPetroleo { get; set; }
    }
}
