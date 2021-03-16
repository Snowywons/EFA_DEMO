using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EFA_DEMO.Data
{
    public class EFAContext : DbContext
    {
        public EFAContext() : base("name=DBEntities")
        {

        }

        public System.Data.Entity.DbSet<EFA_DEMO.Models.Post> Posts { get; set; }

        public System.Data.Entity.DbSet<EFA_DEMO.Models.User> Users { get; set; }
    }
}