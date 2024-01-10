using ConvenienceStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Configuration;

namespace ConvenienceStore
{
    public class ConvenienceStoreContextFactory : IDesignTimeDbContextFactory<ConvenienceStoreContext>
    {
        public ConvenienceStoreContext CreateDbContext(string[] args = null)
        {
            var options = new DbContextOptionsBuilder<ConvenienceStoreContext>();
            options.UseSqlServer(@ConfigurationManager.ConnectionStrings["Default"].ToString());
            return new ConvenienceStoreContext(options.Options);
        }
    }
}