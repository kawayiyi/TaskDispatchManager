﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace TaskDispatchManager.DBModels.Base
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class dbTaskContext : DbContext
    {
        public dbTaskContext()
            : base("name=dbTaskContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Express> Express { get; set; }
        public virtual DbSet<ExpressCompany> ExpressCompany { get; set; }
        public virtual DbSet<ExpressHistory> ExpressHistory { get; set; }
        public virtual DbSet<ExpressProcessDetail> ExpressProcessDetail { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<MessageHistory> MessageHistory { get; set; }
        public virtual DbSet<Proxy> Proxy { get; set; }
        public virtual DbSet<ProxyUseHistory> ProxyUseHistory { get; set; }
    }
}