﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CRM_V3.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using CRM.Models;

    public partial class SDBContext : DbContext
    {
        public SDBContext()
            : base("name=SDBContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<SecurityUser> SecurityUsers { get; set; }
        public virtual DbSet<SecurityUserAccess> SecurityUserAccesses { get; set; }
        public virtual DbSet<CompanyApplication> CompanyApplications { get; set; }
        public virtual DbSet<SecurityUserApplication> SecurityUserApplications { get; set; }
    }
}
