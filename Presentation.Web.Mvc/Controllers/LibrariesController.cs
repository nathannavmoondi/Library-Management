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
    public class LibrariesController : Controller
    {
        private LibraryModel db = new LibraryModel();

        // GET: Libraries
        public async Task<ActionResult> Index()
        {
            return View(await db.Libraries.ToListAsync());
        }

        // GET: Libraries/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Library library = await db.Libraries.FindAsync(id);
            if (library == null)
            {
                return HttpNotFound();
            }
            return View(library);
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
