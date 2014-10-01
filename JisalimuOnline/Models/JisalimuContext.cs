using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using JisalimuLibrary.Models;

namespace JisalimuOnline.Models
{
    public class JisalimuContext: DbContext
    {
        public JisalimuContext():base("JisalimuContext")
        {

        }
        public DbSet<User> User { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
    }
}