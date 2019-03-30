﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using RestSupplyDB;
using RestSupplyDB.Models.Ingredient;
using RestSupplyDB.Models.Kitchen;
using RestSupplyDB.Models.Supplier;
using RestSupplyMVC.Persistence;
using RestSupplyMVC.ViewModels;

namespace RestSupplyMVC.Controllers
{
    public class KitchensController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private RestSupplyDbContext db = new RestSupplyDbContext();

        public KitchensController()
        {
            _unitOfWork = new UnitOfWork(new RestSupplyDbContext());
        }
        // GET: Kitchens
        public ActionResult Index(string controllerRedirect = null)
        {
            var dbUsers = _unitOfWork.Users.GetAll();
            var dbKitchens = _unitOfWork.Kitchens.GetKitchensByUserId(User.Identity.GetUserId());
            var vm = new KitchenIndexViewModel
            {
                KitchenToCreate = new CreateKitchenViewModel
                {
                    AllUsersList = dbUsers.Select(
                        u => new UserViewModel
                        {
                            Id = u.Id,
                            Email = u.Email,
                            LastName = u.LastName,
                            PrivateName = u.FirstName,
                            UserRolesList = _unitOfWork.Roles.GetRolesByUserId(u.Id).Select(r => new RoleViewModel
                            {
                                RoleId = r.RoleId,
                                RoleName = _unitOfWork.Roles.GetById(r.RoleId).Name
                            }).ToList()

                        }).ToList()

                },
                KitchensList = dbKitchens.Select(k => new KitchenViewModel
                {
                    KitchenName = k.Name,
                    KitchenAddress = k.Address,
                    KitchenId = k.Id,
                    
                }).ToList(),
                ControllerRedirect = controllerRedirect
            };
            return View(vm);
        }

        // GET: Kitchens/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var allUsers = _unitOfWork.Users.GetAll();
            var kitchen = _unitOfWork.Kitchens.GetById(id.Value);
            // TODO add user role
            var dbRoles = _unitOfWork.Roles.GetAll();
            var kitchenVm = new CreateKitchenViewModel
            {
                KitchenUsersList = kitchen.KitchenUsers.Select(u => new UserViewModel
                {
                    Id = u.UserId,
                    Email = u.AppUser.Email,
                    RoleName = dbRoles.FirstOrDefault(r => r.Id == u.AppUser.Roles.FirstOrDefault()?.RoleId)?.Name
                }).ToList(),
                AllUsersList = allUsers.Select(us => new UserViewModel
                {
                    Id = us.Id,
                    Email = us.Email
                }).ToList(),
                KitchenName = kitchen.Name,
                KitchenAddress = kitchen.Address,
                KitchenId = kitchen.Id

            };

            return View(kitchenVm);
        }

        public ActionResult GetIngredientsByKitchenId(int kitchenId)
        {
            var ingredients = _unitOfWork.Ingredients.GetIngredientsByKitchenId(kitchenId);
            var response = ingredients.Select(i => new IngredientViewModel
            {
                IngredientId = i.Id,
                Name = i.Name
            }).ToList();

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveKitchen(string kitchenName, string kitchenAddress, UserViewModel[] users)
        {
            string result = "Error! Saving Kitchen Process Is Not Complete!";
            if (kitchenName != null)
            {
                var kitchen = new Kitchens()
                {
                    Name = kitchenName,
                    Address = kitchenAddress
                };
                if (users != null && users.Length > 0)
                {
                    foreach (var user in users)
                    {
                        kitchen.KitchenUsers.Add(
                            new KitchenUsers
                            {
                                UserId = user.Id,
                            });
                    }
                }

                _unitOfWork.Kitchens.Add(kitchen);
                _unitOfWork.Complete();
                result = "Success! Kitchen is saved!";
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddUsersToKitchen(int kitchenId, UserViewModel[] users)
        {
            string result = "Error! Unable to add ingredients!";
            if (kitchenId > 0 && users.Any())
            {
                var userIds = users.Select(u => u.Id).ToList();
                _unitOfWork.Kitchens.AddUsersToKitchen(kitchenId, userIds);
                _unitOfWork.Complete();

                result = "Success! Users were added!";
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        // GET: Kitchens/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Kitchens/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Address")] Kitchens kitchens)
        {
            if (ModelState.IsValid)
            {
                db.KitchensSet.Add(kitchens);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(kitchens);
        }

        // GET: Kitchens/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kitchens kitchens = db.KitchensSet.Find(id);
            if (kitchens == null)
            {
                return HttpNotFound();
            }
            return View(kitchens);
        }

        // POST: Kitchens/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Address")] Kitchens kitchens)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kitchens).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kitchens);
        }

        // GET: Kitchens/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kitchens kitchens = db.KitchensSet.Find(id);
            if (kitchens == null)
            {
                return HttpNotFound();
            }
            return View(kitchens);
        }

        // POST: Kitchens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Kitchens kitchens = db.KitchensSet.Find(id);
            db.KitchensSet.Remove(kitchens);
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
    }
}
