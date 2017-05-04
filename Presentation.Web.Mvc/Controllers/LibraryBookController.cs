using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AkaLibrary.Logic.Sets;

namespace AkaLibrary.Presentation.Web.Mvc.Controllers
{
    public class LibraryBookController : Controller
    {
        private LibraryModel db = new LibraryModel();

        // GET: LibraryBook/5
        // where 5- is libraryId
        
        public async Task<ActionResult> List(int? id)
        {
            ViewBag.LibraryName = db.Libraries.Find(id).Name;

            var libraryBook = db.Library_Book
                .Include(l => l.BookTitle)
                .Include(l => l.Library)
                .Include(l => l.BookSignedOuts)
                .Where(l => l.LibraryId == id);
            return View(await libraryBook.ToListAsync());
        }

        // GET: LibraryBook/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Library_Book library_Book = await db.Library_Book.FindAsync(id);
            if (library_Book == null)
            {
                return HttpNotFound();
            }
            return View(library_Book);
        }

   

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
