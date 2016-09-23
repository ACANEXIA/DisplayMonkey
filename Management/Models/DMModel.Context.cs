﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DisplayMonkey.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DisplayMonkeyEntities : DbContext
    {
        public DisplayMonkeyEntities()
            : base("name=DisplayMonkeyEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Canvas> Canvases { get; set; }
        public DbSet<Clock> Clocks { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<Display> Displays { get; set; }
        public DbSet<Frame> Frames { get; set; }
        public DbSet<FullScreen> FullScreens { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Memo> Memos { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Panel> Panels { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Weather> Weathers { get; set; }
        public DbSet<ReportServer> ReportServers { get; set; }
        public DbSet<Html> Html { get; set; }
        public DbSet<Youtube> Youtube { get; set; }
        public DbSet<ExchangeAccount> ExchangeAccounts { get; set; }
        public DbSet<Outlook> Outlooks { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<AzureAccount> AzureAccounts { get; set; }
    }
}
