using BookApi.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApi.Core.Data.Repositories
{
    public interface IBookRepository
    {
        bool Add(Book book);
        bool Delete(Book book);
        Book Get(int id);
        List<Book> ListAll();
        bool Update(Book book);
    }
}
