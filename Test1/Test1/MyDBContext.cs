using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test1
{
    class MyDBContext : DbContext
    {
        public MyDBContext():base("MyDBContextConn")
        {
        }
        public DbSet<Employee> Employees { get; set; }
    }
}
