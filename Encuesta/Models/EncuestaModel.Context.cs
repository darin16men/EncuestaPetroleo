﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Encuesta.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class EncuestaPetroleoEntities : DbContext
    {
        public EncuestaPetroleoEntities()
            : base("name=EncuestaPetroleoEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Cargos> Cargos { get; set; }
        public virtual DbSet<Empresa> Empresa { get; set; }
        public virtual DbSet<EncuestaPerfilesPetroleo> EncuestaPerfilesPetroleo { get; set; }
        public virtual DbSet<Especialidad> Especialidad { get; set; }
        public virtual DbSet<ExperienciaRelacionada> ExperienciaRelacionada { get; set; }
        public virtual DbSet<JefeGesHum> JefeGesHum { get; set; }
        public virtual DbSet<NivelEducativo> NivelEducativo { get; set; }
        public virtual DbSet<NoDeCargos> NoDeCargos { get; set; }
        public virtual DbSet<OtraEspecialidad> OtraEspecialidad { get; set; }
        public virtual DbSet<PersonaDiligencia> PersonaDiligencia { get; set; }
        public virtual DbSet<TipoCago> TipoCago { get; set; }
        public virtual DbSet<PersonasReunionesPerfiles> PersonasReunionesPerfiles { get; set; }
        public virtual DbSet<EmpresaEspecialidad> EmpresaEspecialidad { get; set; }
        public virtual DbSet<CulminacionFinal> CulminacionFinal { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
    }
}
