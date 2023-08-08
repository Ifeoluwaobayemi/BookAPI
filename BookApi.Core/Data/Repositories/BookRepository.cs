using BookApi.Core.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApi.Core.Data.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BookDbContext _context;

        public BookRepository(BookDbContext context)
        {
            _context = context;
        }

        public bool Add(Book book)
        {
            _context.Add(book);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(Book book)
        {
            _context.Remove(book);
            _context.SaveChanges();
            return true;
        }

        public Book Get(int id)
        {
            var result = _context.books.FirstOrDefault(c => c.Id == id);
            if (result != null)
            {
                return result;
            }
            return new Book();

        }

        public List<Book> ListAll()
        {
            return _context.books.ToList();
        }

        public bool Update(Book book)
        {
            _context.Update(book);
            _context.SaveChanges();
            return true;
        }
    }
}
