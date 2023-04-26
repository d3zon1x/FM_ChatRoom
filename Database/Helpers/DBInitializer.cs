using Database.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Helpers
{
    internal static class DBInitializer
    {
        public static void SeedUser(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Credentials>().HasData(new Credentials[]
            {
                new Credentials() {Id = 1, Login = "Test", Password = "test", Email="test@gmail.com"},
            });
        }
    }
}
