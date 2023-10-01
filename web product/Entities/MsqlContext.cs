using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Collections.Generic;


namespace web_product.Entities
{
    public class MsqlContext : DbContext
    {
        public DbSet<Products> Products { get; set; }
        

        public MsqlContext(DbContextOptions<MsqlContext> options)
            : base(options)
        {
            

        }

    }

}


