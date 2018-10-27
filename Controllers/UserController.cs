using Nomaddoors.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Nomaddoors.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/
        DB_A095E6_nomaddoorsEntities nomadDB = new DB_A095E6_nomaddoorsEntities();

        public ActionResult Index()
        {
            List<VMUser> users = new List<VMUser>();
            foreach (var us in nomadDB.Users)
            {
                VMUser u = new VMUser();
                u.ID = us.ID;
                u.Name = us.Name;
                u.Surname = us.Surname;
                u.Email = us.Email;
                u.Telephone = us.Telephone;
                u.Password = us.Password;

                u.Image = us.Image;

                users.Add(u);
            }

            return View(users);
        }

        //
        // GET: /User/Details/5

        public ActionResult Details(int id)
        {

            VMUser user = new VMUser();
            User u = new User();
            u=nomadDB.Users.Find(id);

            user.ID=u.ID;
            user.Name = u.Name;
            user.Surname = u.Surname;
            user.Email = u.Email;
            user.Telephone = u.Telephone;
            user.Password = u.Password;
            user.Type = u.Type;
            user.Info = u.Info;
            user.Whatsapp = u.Whatsapp;
            user.Language = u.Language;

            user.Gender = u.Gender;
            user.Birth = u.Birth?? DateTime.Now;

            DateTime today = DateTime.Today;

            int age = today.Year - user.Birth.Year;

            if (user.Birth > today.AddYears(-age))
                age--;
            user.Age = age;

            user.Image = u.Image;

            List<VMFestival> guiding = new List<VMFestival>();
            
            foreach (var fest in nomadDB.Festivals.Where(e => e.Guide ==id))
            {
                VMFestival f = new VMFestival();
                f.ID = fest.ID;
                f.Name = fest.Name;
                DateTime d1 = fest.Date1 ?? DateTime.Now;
                DateTime d2 = fest.Date2 ?? DateTime.Now;
                if (d1.Month == d2.Month)
                {
                    if (d1.Day == d2.Day)
                    {
                        f.Date = d1.ToString("MMMM, dd");
                    }
                    else
                    {
                        f.Date = d1.ToString("MMMM, dd") + "-" + d2.ToString("dd"); ;
                    }

                }
                else
                {
                    f.Date = d1.ToString("MMMM, dd") + "-" + d2.ToString("MMMM, dd"); ;
                }
                f.Img = fest.Image;
                guiding.Add(f);
            }

            user.guiding = guiding;
            List<VMFestival> going = new List<VMFestival>();
            foreach (var joined in nomadDB.Joins.Where(q => q.Us == user.ID))
            {
                VMFestival fa = new VMFestival();
                Festival f = new Festival();
                f = nomadDB.Festivals.Find(joined.Fest);

                fa.ID = f.ID;
                fa.Name = f.Name;
                DateTime d1 = f.Date1 ?? DateTime.Now;
                DateTime d2 = f.Date2 ?? DateTime.Now;
                if (d1.Month == d2.Month)
                {
                    if (d1.Day == d2.Day)
                    {
                        fa.Date = d1.ToString("MMMM, dd");
                    }
                    else
                    {
                        fa.Date = d1.ToString("MMMM, dd") + "-" + d2.ToString("dd"); ;
                    }

                }
                else
                {
                    fa.Date = d1.ToString("MMMM, dd") + "-" + d2.ToString("MMMM, dd"); ;
                }
                fa.Img = f.Image;
                going.Add(fa);

            }
            user.going = going;
            
            return View(user);
        }

        //
        // GET: /User/Create

       

        //
        // GET: /User/Edit/5
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User u = nomadDB.Users.Find(id);
            if (u == null)
            {
                return HttpNotFound();
            }

            return View(u);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userToUpdate = nomadDB.Users.Find(id);
            if (TryUpdateModel(userToUpdate, "",
               new string[] { "Name", "Surname", "Info", "Language", "GenderID", "Telephone", "Whatsapp", "Country" }))
            {
                try
                {
                    nomadDB.SaveChanges();

                    return RedirectToAction("Details", new { id = userToUpdate.ID });
                }
                catch (Exception /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(userToUpdate);
        }


        //
        // GET: /User/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /User/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        [Authorize]
        [HttpGet]
        public ActionResult EditImage()
        {
          
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditImage(ImageType img)
        {
            var identity = (System.Web.HttpContext.Current.User as Nomaddoors.MyPrincipal).Identity as Nomaddoors.MyIdentity;

            string fileName = "user"+identity.User.ID.ToString()+".jpg";

            if (img.File!=null)
            {
                var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Images/User/"), fileName);
                img.File.SaveAs(path);
            }

            User u = nomadDB.Users.Find(identity.User.ID);
            u.Image = fileName;

            nomadDB.SaveChanges();

            return RedirectToAction("Details", new { id = identity.User.ID });
        }

        

        [Authorize]
        public ActionResult DeleteImage()
        {
            var identity = (System.Web.HttpContext.Current.User as Nomaddoors.MyPrincipal).Identity as Nomaddoors.MyIdentity;
            User u = nomadDB.Users.Find(identity.User.ID);
            if (u.Gender == "Male")
            {
                u.Image = "male.jpg";
            }
            else
            {
                u.Image = "female.jpg";
            }
            nomadDB.SaveChanges();

            return RedirectToAction("Details", new { id = identity.User.ID });
        }


    }
}
