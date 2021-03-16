using EFA_DEMO.Models;
using EFA_DEMO.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Data.Entity;

namespace EFA_DEMO.Controllers
{
    public class PostsController : Controller
    {
        private EFAContext db = new EFAContext();

        [UserAccess]
        public ActionResult Index()
        {
            var posts = db.Posts.OrderByDescending(p => p.CreationDate).ToList();
            var result = posts;
            var tags = "";

            if (System.Web.HttpContext.Current.Session["Tags"] != null)
            {
                tags = System.Web.HttpContext.Current.Session["Tags"].ToString();
                var array = tags.Split(' ');

                result = posts.Where(p => {
                    bool valid = true;

                    for (int i = 0; i < array.Length; i++)
                    {
                        if (p.Tags != null && !p.Tags.Contains(array[i]))
                        {
                            valid = false;
                            break;
                        }
                    }

                    return valid; 
                }).OrderByDescending(p => p.CreationDate).ToList();
            }

            ViewBag.Tags = tags.Length > 0 ? tags.Split(' ') : null;

            return View(result);
        }

        [UserAccess]
        public ActionResult Create()
        {
            return View(new PostView());
        }

        [UserAccess]
        [HttpPost]
        public ActionResult Create(PostView postView)
        {
            if (ModelState.IsValid)
            {
                UserView currentUser = OnlineUsers.GetSessionUser();
                postView.User = currentUser;
                postView.UserId = currentUser.Id;
                db.Posts.Add(postView.ToPost());
                db.SaveChanges();
                return RedirectToAction("Index", "Posts");
            }

            return View(postView);
        }

        [UserAccess]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PostView postView = db.Posts.Find(id).ToPostView();

            if (postView == null)
            {
                return HttpNotFound();
            }

            UserView currentUser = OnlineUsers.GetSessionUser();

            if (postView.UserId == currentUser.Id || currentUser.Admin)
            {
                return View(postView);
            }

            return RedirectToAction("Index");
        }

        [UserAccess]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PostView postView)
        {
            if (ModelState.IsValid)
            {
                db.Entry(postView.ToPost()).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(postView);
        }

        [UserAccess]
        public ActionResult ConfirmDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PostView postView = db.Posts.Find(id).ToPostView();

            if (postView == null)
            {
                return HttpNotFound();
            }

            UserView currentUser = OnlineUsers.GetSessionUser();

            if (postView.UserId == currentUser.Id || currentUser.Admin)
            {
                return View(postView);
            }

            return RedirectToAction("Index");
        }

        [UserAccess]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(PostView postView)
        {
            Post post = db.Posts.Find(postView.Id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [UserAccess]
        public ActionResult Tags()
        {
            ViewBag.Tags = "";

            if (System.Web.HttpContext.Current.Session["Tags"] != null)
                ViewBag.Tags = System.Web.HttpContext.Current.Session["Tags"].ToString();

            return View();
        }

        [UserAccess]
        [HttpPost]
        public ActionResult Tags(PostView postView)
        {
            string tags = postView.Tags != null ? postView.Tags.ToString() : "";
            System.Web.HttpContext.Current.Session["Tags"] = tags;

            return RedirectToAction("Index", "Posts");
        }

        [UserAccess]
        public ActionResult ClearTags()
        {
            System.Web.HttpContext.Current.Session["Tags"] = "";

            return RedirectToAction("Index", "Posts");
        }

        [UserAccess]
        public ActionResult SetTag()
        {
            if (Request.QueryString["tag"] != null)
                System.Web.HttpContext.Current.Session["Tags"] = Request.QueryString["tag"];

            return RedirectToAction("Index");
        }

        [UserAccess]
        public ActionResult Repost(int? id)
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

            UserView currentUser = OnlineUsers.GetSessionUser();

            if (post.UserId == currentUser.Id || currentUser.Admin)
            {
                post.CreationDate = DateTime.Now;
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
            }

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
    }
}