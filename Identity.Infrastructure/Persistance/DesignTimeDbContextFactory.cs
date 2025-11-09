using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.Persistance
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
    {
        public IdentityDbContext CreateDbContext(string[] args)
        {
            // هنا بتحددي الconnection string اللي هيستخدمها EF وقت الـ migration فقط
            var optionsBuilder = new DbContextOptionsBuilder<IdentityDbContext>();

            // لو بتستخدمي SQL المحلي
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=Fitness_IdentityDb;Trusted_Connection=True;TrustServerCertificate=True;");

            return new IdentityDbContext(optionsBuilder.Options);
        }
    }
}
