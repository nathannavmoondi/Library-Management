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
    public class BookTitlesController : Controller
    {
        private LibraryModel db = new LibraryModel();

        // GET: BookTitles
        public async Task<ActionResult> Index()
        {
            return View(await db.BookTitles.ToListAsync());
        }

        // GET: BookTitles/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookTitle bookTitle = await db.BookTitles.FindAsync(id);
            if (bookTitle == null)
            {
                return HttpNotFound();
            }
            return View(bookTitle);
        }

        // GET: BookTitles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BookTitles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "BookId,Title,ISBN,DateOfPublication")] BookTitle bookTitle)
        {
            if (ModelState.IsValid)
            {
                db.BookTitles.Add(bookTitle);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(bookTitle);
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
