using BookApi.Core.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApi.Core.Data
{
    public class BookDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Book> books { get; set; }
        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options)
        {
        }


    }
}
