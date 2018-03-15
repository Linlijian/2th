using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _2th.Entities;
using _2th.Models;
using System.IO;
using Microsoft.AspNet.Identity;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Web.Helpers;


namespace _2th.Areas.Member.Controllers
{
    public class PostController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private object iTextSharp;

        // GET: Member/Post
        public ActionResult Index()
        {
            return View(db.Posts.ToList());
        }

        // GET: Member/Post/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Member/Post/Create
        public ActionResult Create()
        {
            ViewData["Auther"] = User.Identity.GetUserName();
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase file,[Bind(Include = "Id,Title,Content,Auther,TagName,Img")] Post post)
        {
            if (ModelState.IsValid)
            {
                
                    file.SaveAs(HttpContext.Server.MapPath("~/Content/img/")
                                  + file.FileName);
                    post.Img = file.FileName;


                    db.Posts.Add(post);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                

            }

            return View(post);
        }

        // GET: Member/Post/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Member/Post/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Content,Auther,TagName,Img")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Member/Post/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Member/Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult GetPDF(int? id)
        {
            Post post = db.Posts.Find(id);
            string title = post.Title.ToString();
            string tagname = post.TagName.ToString();
            string auther = post.Auther.ToString();
            string content = post.Content.ToString();

            Document pdfDoc = new Document(PageSize.A4, 25, 25, 25, 15);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();

            Chunk chunk = new Chunk("2th PDF", FontFactory.GetFont("Arial", 20,BaseColor.MAGENTA));
            pdfDoc.Add(chunk);
                        

            PdfPTable table = new PdfPTable(2);
            table.WidthPercentage = 100;
            table.HorizontalAlignment = 0;
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;


            PdfPCell cell = new PdfPCell();
            cell.Border = 0;
            Image image = Image.GetInstance(Server.MapPath("~/Content/img/") + post.Img.ToString());
            image.ScaleAbsolute(200, 150);
            cell.AddElement(image);
            table.AddCell(cell);

            BaseFont bf = BaseFont.CreateFont(Server.MapPath("~/Content/font/ITIM-REGULAR.TTF"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font font = new Font(bf, 18);
            Paragraph para = new Paragraph("Title : " + title + "\n",font);                        
            para.Add(content + "\n");
            para.Add("Tag : " + tagname + "\n");
            para.Add("By : " + auther+ "\n");
            cell = new PdfPCell();
            cell.Border = 0;
            cell.AddElement(para);
            table.AddCell(cell);
                       
            pdfDoc.Add(table);

            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=2th-pdf.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write(pdfDoc);
            Response.End();

            return View();
        }
    }
}
