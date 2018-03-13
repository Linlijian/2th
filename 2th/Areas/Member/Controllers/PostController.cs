using _2th.Entities;
using _2th.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace _2th.Areas.Member.Controllers
{
    public class PostController : Controller
    {
        // GET: Member/Post
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            ViewData["A"] = "Post page";
            
            return View(db.Posts.ToList());
        }

        public ActionResult Detail(int ?id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
                return HttpNotFound();
            return View(post);
        }

    }
}