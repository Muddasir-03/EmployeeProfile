using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AspiraApp.Controllers
{
    public class HomeController : Controller
    {
        AspRegistrationEntities1 db = new AspRegistrationEntities1();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        
        [Authorize]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        public  ActionResult Create(AspStudent aspira)
        {
            AspRegistrationEntities1 db = new AspRegistrationEntities1();
            
            if(ModelState.IsValid)
            {

                db.AspStudent.Add(aspira);

                db.SaveChanges();

                return RedirectToAction("Aspirants");
            }

            return View();
        }

 

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(AspRegister reg)
        {
            AspRegistrationEntities1 db = new AspRegistrationEntities1();

            if(ModelState.IsValid)
            {
                db.AspRegister.Add(reg);

                db.SaveChanges();

                return View();
            }

            return View();

        }

        [Authorize]
        [HttpGet]
        public ActionResult Aspirants(string searchtext, string SortOrder, string SortBy,int PageNumber = 1)
        {

            AspRegistrationEntities1 db = new AspRegistrationEntities1();

            ViewBag.SortOrder = SortOrder;
            ViewBag.SortBy = SortBy;

            var list = db.AspStudent.ToList();

            if (searchtext != null)
            {
                list = db.AspStudent.Where(x => x.Name.Contains(searchtext) || x.Aspira_Id.Contains(searchtext) || x.Email_Id.Contains(searchtext) || x.Tech_stack.Contains(searchtext) || x.Education.Contains(searchtext) || x.Mobile_Number.Contains(searchtext)).ToList();
            }

            switch(SortBy)
            {
                case "Name":
                    {
                        switch (SortOrder)
                        {
                            case "Asc":
                                {
                                    list = list.OrderBy(x => x.Name).ToList();
                                    break;
                                }
                            case "Desc":
                                {
                                    list = list.OrderByDescending(x => x.Name).ToList();
                                    break;
                                }
                            default:
                                {
                                    list = list.OrderBy(x => x.Name).ToList();
                                    break;
                                }
                        }
                    }
                    break;
                case "Aspira_Id":
                    {
                        switch (SortOrder)
                        {
                            case "Asc":
                                {
                                    list = list.OrderBy(x => x.Aspira_Id).ToList();
                                    break;
                                }
                            case "Desc":
                                {
                                    list = list.OrderByDescending(x => x.Aspira_Id).ToList();
                                    break;
                                }
                            default:
                                {
                                    list = list.OrderBy(x => x.Aspira_Id).ToList();
                                    break;
                                }
                        }
                    }
                    break;
                default:
                    {
                        list = list.OrderBy(x => x.Name).ToList();
                        break;
                    }


            }


            ViewBag.TotalPages = Math.Ceiling(list.Count() / 6.0);
            ViewBag.PageNumber = PageNumber;

            list = list.Skip((PageNumber - 1) * 6).Take(6).ToList();

            return View(list);

        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            AspRegistrationEntities1 db = new AspRegistrationEntities1();

            var find = db.AspStudent.Find(id);

            return View(find);
        }


        [HttpPost]

        public ActionResult Edit(AspStudent asp)
        {
            AspRegistrationEntities1 db = new AspRegistrationEntities1();

            db.Entry(asp).State = System.Data.Entity.EntityState.Modified;

            db.SaveChanges();

            return RedirectToAction("Aspirants");
        }

        public ActionResult Delete(int Id)
        {
            AspRegistrationEntities1 db = new AspRegistrationEntities1();

            var AspId = db.AspStudent.Find(Id);

            db.AspStudent.Remove(AspId);

            db.SaveChanges();

            return RedirectToAction("Aspirants");
        }

        [HttpGet]
        public ActionResult Details(int Id)
        {
            AspRegistrationEntities1 db = new AspRegistrationEntities1();

            var AspId = db.AspStudent.Find(Id);

            return View(AspId);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(AspRegister log)
        {
            AspRegistrationEntities1 db = new AspRegistrationEntities1();

            var user = db.AspRegister.Where(x => x.User_Id == log.User_Id && x.Password == log.Password).FirstOrDefault();

            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(log.User_Id,false);
                return RedirectToAction("Index");
            }
            return View();


        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}