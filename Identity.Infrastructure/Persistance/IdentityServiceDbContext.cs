using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.Persistance
{
    public class IdentityServiceDbContext : DbContext
    {
        public IdentityServiceDbContext(DbContextOptions<IdentityServiceDbContext> options)
           : base(options) { }

        public DbSet<User> Users => Set<User>();
    }
}
