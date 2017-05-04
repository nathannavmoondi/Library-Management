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
    public class BookSignedOutsController : Controller
    {
        private LibraryModel db = new LibraryModel();

        // GET: BookSignedOuts
        public async Task<ActionResult> List(int? id)
        {
            
            var bookSignedOuts = db.BookSignedOuts.Include(b => b.Member).Where(b => b.LibraryBookSId == id && b.WhenReturned == null);
            return View(await bookSignedOuts.ToListAsync());
        }

        // GET: BookSignedOuts/Create
        public ActionResult Create(int id)
        {
            var libraryBook = db.Library_Book
                    .Include(lb => lb.BookTitle)
                    .Include(lb => lb.Library)
                    .FirstAsync(lb => lb.LibraryBookSId == id);
            ViewBag.LibraryBook = string.Format("{0} of {1}", libraryBook.Result.BookTitle.Title, libraryBook.Result.Library.Name);
            ViewBag.MemberId = new SelectList(db.Members, "MemberId", "FullName");
            return View(new BookSignedOut()
            {
                LibraryBookSId = id,
                WhenSignedOut = DateTime.Today
            });
        }

        // POST: BookSignedOuts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "LibraryBookSId,MemberId,WhenSignedOut,WhenReturned")] BookSignedOut bookSignedOut)
        {

            /*
             * 5.Enforce this restriction No member can have more than two books signed out of the library at any one time.
             */

            if (db.Members
                 .Include(m => m.BookSignedOuts)
                 .First(m => m.MemberId == bookSignedOut.MemberId)
                 .BookSignedOuts.Any(so => so.WhenReturned == null)){           
                ModelState.AddModelError("", "Let the user sign out one book at a time");
            }

            if (ModelState.IsValid)
            {
                db.BookSignedOuts.Add(bookSignedOut);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Libraries");
            }

            ViewBag.MemberId = new SelectList(db.Members, "MemberId", "FullName", bookSignedOut.MemberId);
            return View(bookSignedOut);
        }

        [HttpPost]
        public async Task<ActionResult> Edit([Bind(Include = "LibraryBookSId,MemberId,WhenSignedOut,WhenReturned")] BookSignedOut bookSignedOut)
        {
            /*
             * 4. The member can return a book
             */

            db.Entry(bookSignedOut).State = EntityState.Modified;
            bookSignedOut.WhenReturned = DateTime.Now;
            await db.SaveChangesAsync();

            return RedirectToAction("List", new { id = bookSignedOut.LibraryBookSId });
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
