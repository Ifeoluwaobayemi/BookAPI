using BookApi.Core.Data;
using BookApi.Core.Data.Entities;
using BookApi.Core.Data.Repositories;
using BookApi.Core.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookApi.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class BookController : ControllerBase
    {
        private readonly ILogger<BookController> _logger;
        private readonly IBookRepository _repo;
      
        public BookController(ILogger<BookController> logger, IBookRepository repo)
        {
            _logger = logger;
            _repo = repo;
            
        }

        [Authorize(Roles = "admin")]
        [HttpPost("add")]
        public IActionResult AddBook([FromBody] AddBookDto model)
        {
            if (ModelState.IsValid)
            {
                var addBook = new Book
                {
                    Title = model.Title,
                    Author = model.Author,
                    Description = model.Description,
                    Pages = model.Pages
                };
               if(_repo.Add(addBook))
                {
                    return Ok("Book added successfully!");
                }
            }
            return BadRequest(ModelState);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            var booksToGet = _repo.ListAll();

            var result = booksToGet.Select(x => new Book
            {
                Id= x.Id,
                Pages = x.Pages,
                Author = x.Author,
                Title = x.Title,
                Description = x.Description
            });
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("single/{id}")]
        public IActionResult GetBook(int id)
        {
            if (ModelState.IsValid)
            {
                var aBook = _repo.Get(id);

                if (aBook.Id > 0)
                {
                    var result = new ReturnBookDto
                    {
                        Id = aBook.Id,
                        Pages = aBook.Pages,
                        Author = aBook.Author,
                        Title = aBook.Title,
                        Description = aBook.Description
                    };
                    return Ok(result);
                }
            }

            return NotFound($"Book with id: {id} is not found!");
        }

        [AllowAnonymous]
        [HttpPut("update/{id}")]
        public IActionResult EditBook(int id, [FromBody] AddBookDto model)
        {
            if (ModelState.IsValid)
            {
                var book = _repo.Get(id);
                if (book != null)
                {
                    book.Pages = model.Pages;
                    book.Title = model.Title;
                    book.Description = model.Description;
                    book.Author = model.Author;

                    if (_repo.Update(book))
                    {
                        return Ok("Book Updated successfully!");
                    }

                    return BadRequest("Update failed");
                }
            }
            return BadRequest($"Failed! Book with id: {id} not updated.");
        }

        [AllowAnonymous]
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteBook(int id)
        {
            if (ModelState.IsValid)
            {
                var book = _repo.Get(id);
                if (book != null)
                {
                    if (_repo.Delete(book))
                    {
                        return Ok("Book deleted successfully!");
                    }
                    return BadRequest("Failed to delete!");
                }
            }

            return BadRequest("$\"Failed! Book with id: {id} not updated.");
        }

        [AllowAnonymous]
        [HttpPatch("single/{id}")]
        public IActionResult EditBookAuthor(int id, [FromBody] AddBookDto model)
        {
            if (ModelState.IsValid)
            {
                var book = _repo.Get(id);
                if (book != null)
                {
                    book.Author = model.Author;

                    if (_repo.Update(book))
                    {
                        return Ok("Book Updated successfully!");
                    }

                    return BadRequest("Update failed");
                }
            }
            return BadRequest(ModelState);
        }
    }
}
