﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebService.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class healthCenterDBEntities : DbContext
    {
        public healthCenterDBEntities()
            : base("name=healthCenterDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<admin> admin { get; set; }
        public virtual DbSet<appointment> appointment { get; set; }
        public virtual DbSet<doctor> doctor { get; set; }
        public virtual DbSet<freeterms> freeterms { get; set; }
        public virtual DbSet<nurse> nurse { get; set; }
        public virtual DbSet<patient> patient { get; set; }
        public virtual DbSet<reception> reception { get; set; }
        public virtual DbSet<specialities> specialities { get; set; }
    }
}
