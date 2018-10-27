using Nomaddoors.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Nomaddoors.Controllers
{
    [OutputCache(Duration = 15, VaryByParam = "None")]
    public class HomeController : Controller
    {
        //
        // GET: /Festival/
        DB_A095E6_nomaddoorsEntities nomadDB = new DB_A095E6_nomaddoorsEntities();
        public ActionResult Index()
        {
            IndexModel index = new IndexModel();
            index.upcoming = new List<VMFestival>();
            index.past = new List<VMFestival>();
            foreach (var item in nomadDB.Festivals)
            {
                VMFestival fest = new VMFestival();

                fest.ID = item.ID;
                fest.Name = item.Name;
                fest.Short = item.Short;
                fest.About = item.About;
                fest.Region = item.Region;
                fest.City = item.City;
                fest.Date1 = item.Date1;
                fest.Date2 = item.Date2;
                DateTime d1 = item.Date1 ?? DateTime.Now;
                DateTime d2 = item.Date2 ?? DateTime.Now;
                if (d1.Month == d2.Month)
                {
                    if (d1.Day == d2.Day)
                    {
                        fest.Date = d1.ToString("MMMM, dd");
                    }
                    else
                    {
                        fest.Date = d1.ToString("MMMM, dd") + "-" + d2.ToString("dd"); ;
                    }

                }
                else
                {
                    fest.Date = d1.ToString("MMMM, dd") + "-" + d2.ToString("MMMM, dd"); ;
                }


                fest.Img = item.Image;

                if (item.Date1 >= DateTime.Today)
                {
                    index.upcoming.Add(fest);
                }
                else
                {
                    index.past.Add(fest);
                }
            }


                        //foreach (var img in nomadDB.Images)
                        //{


                        //    string filePath = Url.Content("~/Images/URL/") + img.ID + ".jpg";

                        //    FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);

                        //    fs.Write(img.Data, 0, img.Data.Length);

                        //    img.Url = filePath;
                        //    nomadDB.SaveChanges();

                        //    img.Url = filePath;
                        //}
                        //nomadDB.SaveChanges();

            return View(index);
        }

        //
        // GET: /Festival/Details/5

        public ActionResult Details(int id)
        {

            VMFestival fest = new VMFestival();
            Festival item = new Festival();
            item = nomadDB.Festivals.Find(id);

            fest.ID = item.ID;
            fest.Name = item.Name;
            fest.Short = item.Short;
            fest.About = item.About;
            fest.Region = item.Region;
            fest.City = item.City;
            fest.Date1 = item.Date1;
            fest.Date2 = item.Date2;
            DateTime d1 = item.Date1 ?? DateTime.Now;
            DateTime d2 = item.Date2 ?? DateTime.Now;
            if (d1.Month == d2.Month)
            {
                if (d1.Day == d2.Day)
                {
                    fest.Date = d1.ToString("MMMM, dd");
                }
                else
                {
                    fest.Date = d1.ToString("MMMM, dd") + "-" + d2.ToString("dd"); ;
                }

            }
            else
            {
                fest.Date = d1.ToString("MMMM, dd") + "-" + d2.ToString("MMMM, dd"); ;
            }

            fest.Price = item.Price;
            fest.Province = item.Province;
            fest.Organizator = item.Organizator;
            fest.Address = item.Address;
            fest.Guide = item.Guide;


            VMUser user = new VMUser();
            User u = new User();
            u = nomadDB.Users.Find(item.Guide);

            user.ID = u.ID;
            user.Name = u.Name;
            user.Surname = u.Surname;
            user.Email = u.Email;
            user.Password = u.Password;

            Image profile = new Image();

            user.Image = u.Image;
            fest.gid = user;

            fest.Img = item.Image;

            fest.Program = nomadDB.Images.Where(i => i.Type == "Program" && i.Item == item.ID).OrderBy(um => um.ID).FirstOrDefault();

            List<Image> imgs = new List<Image>();
            foreach (var im in nomadDB.Images.Where(i => i.Type == "Festival" && i.Item == item.ID))
            {
                imgs.Add(im);
            }
            fest.Images = imgs;

            List<VMUser> goingUsers = new List<VMUser>();
            foreach (var joined in nomadDB.Joins.Where(j => j.Fest == item.ID))
            {
                VMUser userGoing = new VMUser();
                User going = new User();
                going = nomadDB.Users.Find(joined.Us);

                userGoing.ID = going.ID;
                userGoing.Name = going.Name;
                userGoing.Surname = going.Surname;
                userGoing.Email = going.Email;
                userGoing.Image = going.Image;
                goingUsers.Add(userGoing);
            }

            fest.joins = goingUsers;

            List<Subtitle> subs = new List<Subtitle>();
            foreach (var sub in nomadDB.Subtitles.Where(j => j.Festival == item.ID))
            {
                subs.Add(sub);
            }

            fest.subtitles = subs;

            List<Program> progs = new List<Program>();
            foreach (var prog in nomadDB.Programs.Where(j => j.Festival == item.ID))
            {
                progs.Add(prog);
            }
            fest.programs = progs;

            return View(fest);
        }

        //
        // GET: /Festival/Create
        [Authorize(Roles = "Guide")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Festival/Create
        [Authorize(Roles = "Guide")]
        [HttpPost]
        public ActionResult Create(VMFestival item)
        {
            try
            {
                Festival fest = new Festival();

                fest.Name = item.Name;
                fest.About = item.About;
                fest.Short = item.Short;
                fest.Region = item.Region;
                fest.City = item.City;
                fest.Price = item.Price;
                fest.Province = item.Province;
                fest.Organizator = item.Organizator;
                fest.Address = item.Address;
                fest.Date1 = item.Date1;
                fest.Date2 = item.Date2;

                var identity = (System.Web.HttpContext.Current.User as Nomaddoors.MyPrincipal).Identity as Nomaddoors.MyIdentity;
                fest.Guide = identity.User.ID;

               

                nomadDB.Festivals.Add(fest);
                nomadDB.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Festival/Edit/5
        [Authorize(Roles = "Guide")]
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Festival f = nomadDB.Festivals.Find(id);
            if (f == null)
            {
                return HttpNotFound();
            }

            return View(f);
        }

        [Authorize(Roles = "Guide")]
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var festToUpdate = nomadDB.Festivals.Find(id);
            if (TryUpdateModel(festToUpdate, "",
               new string[] { "Name", "Short", "About", "City", "Province", "Address", "Orgaizator", "Price" }))
            {
                try
                {
                    nomadDB.SaveChanges();

                    return RedirectToAction("Details", new { id = festToUpdate.ID });
                }
                catch (Exception /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(festToUpdate);
        }

        //
        // GET: /Festival/Delete/5
        [Authorize(Roles = "Guide")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Festival/Delete/5
        [Authorize(Roles = "Guide")]
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
        public ActionResult Join(int id)
        {
            Join j = new Join();
            j.Fest = id;
            var identity = (System.Web.HttpContext.Current.User as Nomaddoors.MyPrincipal).Identity as Nomaddoors.MyIdentity;
            j.Us =identity.User.ID;
            var join = nomadDB.Joins.Where(a => a.Fest == id && a.Us == j.Us).FirstOrDefault();
            if (join == null)
            {
                nomadDB.Joins.Add(j);
                nomadDB.SaveChanges();
            }
            return RedirectToAction("Details", new { id = j.Fest });
        }

        [Authorize(Roles = "Guide")]
        public ActionResult CreateDescription(int id)
        {
            Subtitle p = new Subtitle();
            p.Festival = id;
            nomadDB.Subtitles.Add(p);
            nomadDB.SaveChanges();

            Subtitle last = nomadDB.Subtitles.OrderByDescending(i => i.ID).FirstOrDefault();
            return RedirectToAction("EditDescription", new { id = last.ID });
        }

        //
        // POST: /Festival/Create



        [Authorize(Roles = "Guide")]
        [HttpGet]
        public ActionResult EditDescription(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subtitle f = nomadDB.Subtitles.Find(id);
            if (f == null)
            {
                return HttpNotFound();
            }

            return View(f);
        }
        [Authorize(Roles = "Guide")]
        [HttpPost, ActionName("EditDescription")]
        [ValidateAntiForgeryToken]
        public ActionResult EditDescriptionPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var update = nomadDB.Subtitles.Find(id);
            if (TryUpdateModel(update, "",
               new string[] { "Festival", "Description" }))
            {
                try
                {
                    nomadDB.SaveChanges();

                    return RedirectToAction("Details", new { id = update.Festival });
                }
                catch (Exception /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(update);
        }

        [Authorize(Roles = "Guide")]
        public ActionResult DeleteDescription(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            Subtitle deleted = nomadDB.Subtitles.Find(id);
            if (deleted == null)
            {
                return HttpNotFound();
            }
            return View(deleted);
        }

        [Authorize(Roles = "Guide")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDescription(int id)
        {
            Subtitle deleted = nomadDB.Subtitles.Find(id);
            try
            {

                nomadDB.Subtitles.Remove(deleted);
                nomadDB.SaveChanges();
            }
            catch (DataException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Details", new { id = deleted.Festival });
        }




        [Authorize(Roles = "Guide")]
        public ActionResult CreateProgram(int id)
        {

            Program p = new Program();
            p.Festival = id;
            nomadDB.Programs.Add(p);
            nomadDB.SaveChanges();

            Program last = nomadDB.Programs.OrderByDescending(i => i.ID).FirstOrDefault();
            return RedirectToAction("EditProgram", new { id = last.ID });
        }

        //
        // POST: /Festival/Create


        [Authorize(Roles = "Guide")]
        [HttpGet]
        public ActionResult EditProgram(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Program f = nomadDB.Programs.Find(id);
            if (f == null)
            {
                return HttpNotFound();
            }

            return View(f);
        }
        [Authorize(Roles = "Guide")]
        [HttpPost, ActionName("EditProgram")]
        [ValidateAntiForgeryToken]
        public ActionResult EditProgramPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var update = nomadDB.Programs.Find(id);
            if (TryUpdateModel(update, "",
               new string[] { "Festival", "Time", "Name", "Description" }))
            {
                try
                {
                    nomadDB.SaveChanges();

                    return RedirectToAction("Details", new { id = update.Festival });
                }
                catch (Exception /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(update);
        }

        [Authorize(Roles = "Guide")]
        public ActionResult DeleteProgram(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            Program deleted = nomadDB.Programs.Find(id);
            if (deleted == null)
            {
                return HttpNotFound();
            }
            return View(deleted);
        }

        [Authorize(Roles = "Guide")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteProgram(int id)
        {
            Program deleted = nomadDB.Programs.Find(id);
            try
            {

                nomadDB.Programs.Remove(deleted);
                nomadDB.SaveChanges();
            }
            catch (DataException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Details", new { id = deleted.Festival });
        }

        [Authorize(Roles = "Guide")]
        public ActionResult CreateCoverImage(int id)
        {
            ImageType img = new ImageType();
            return View(img);
        }

        [Authorize(Roles = "Guide")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCoverImage(ImageType model, int id)
        {
            string fileName = "post" + id.ToString() + ".jpg";

            if (model.File != null)
            {
                var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Images/postCover/"), fileName);
                model.File.SaveAs(path);
            }
            Festival fest =nomadDB.Festivals.Find(id);

            fest.Image = fileName;
            nomadDB.SaveChanges();

            return RedirectToAction("Details", new { id = id });
        }



        [Authorize(Roles = "Guide")]
        public ActionResult CreateImage(int id)
        {
            ImageType img = new ImageType();
            return View(img);
        }

        [Authorize(Roles = "Guide")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateImage(ImageType model, int id)
        {
            Image img = new Image();
            img.Type = "Festival";
            img.Item = id;

            nomadDB.Images.Add(img);
            nomadDB.SaveChanges();

            Image imag = nomadDB.Images.ToList().LastOrDefault();
            string fileName = "image" + imag.ID.ToString() + ".jpg";
            imag.Url = fileName;

            if (model.File != null)
            {
                var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Images/postImages/"), fileName);
                model.File.SaveAs(path);
            }

            nomadDB.SaveChanges();

            return RedirectToAction("Details", new { id = id });
        }



        [Authorize(Roles = "Guide")]
        public ActionResult EditImage(int id )
        {
            ImageType img = new ImageType();
            return View(img);
        }

        [Authorize(Roles = "Guide")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditImage(ImageType model, int id)
        {
           
            string fileName = "image" + id.ToString() + ".jpg";

            if (model.File != null)
            {
                var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Images/postImages/"), fileName);
                model.File.SaveAs(path);
            }


            return RedirectToAction("Details", new { id = nomadDB.Images.Find(id).Item });
        }


        [Authorize(Roles = "Guide")]
        public ActionResult DeleteImage(int id, int item)
        {

            Image img = nomadDB.Images.Find(id);
            string file = "~/Images/postImages/" + img.Url;

            if ((System.IO.File.Exists(file)))
            {
                System.IO.File.Delete(file);
            }

            nomadDB.Images.Remove(img);
            nomadDB.SaveChanges();


            return RedirectToAction("Details", new { id = item });
        }
    }
}
