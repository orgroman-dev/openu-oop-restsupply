﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RestSupplyDB;
using RestSupplyDB.Models.Ingredient;
using RestSupplyDB.Models.Supplier;
using RestSupplyMVC.Persistence;
using RestSupplyMVC.ViewModels;

namespace RestSupplyMVC.Controllers
{
    public class SupplierOrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public SupplierOrderController()
        {
            _unitOfWork = new UnitOfWork(new RestSupplyDbContext());
        }
        // GET: SupplierOrder
        public ActionResult Index()
        {
            var dbSupplierOrders = _unitOfWork.SupplierOrders.GetAll();

            var supplierOrderIndexVm = dbSupplierOrders.Select(s => new SupplierOrderViewModel
            {
                Id = s.SupplierId,
                Name = _unitOfWork.Suppliers.GetById(s.SupplierId).Name,
                Phone = _unitOfWork.Suppliers.GetById(s.SupplierId).Phone,
                Address = _unitOfWork.Suppliers.GetById(s.SupplierId).Address,
                SupplierOrderIngredientsList = s.SupplierOrderDetails.Select(i => new SupplierOrderIngredientsViewModel
                {
                    IngredientId = i.IngredientId,
                    Name = _unitOfWork.Ingredients.GetById(s.SupplierId).Name,
                    Unit = _unitOfWork.Ingredients.GetById(s.SupplierId).Unit,
                    Amount = i.Amount,
                    OrderId = i.OrderId
                })
            });
        
            return View(supplierOrderIndexVm);
        }

        // GET: SupplierOrder/Details/5
        public ActionResult Details(int id)
        {
            var dbSupplierOrder = _unitOfWork.SupplierOrders.GetById(id);
            var supplierOrderVm = new SupplierOrderViewModel
            {
                OrderDate = dbSupplierOrder.Date,
                OrderId = dbSupplierOrder.Id,
                SupplierOrderIngredientsList = dbSupplierOrder.SupplierOrderDetails.Select(i =>
                    new SupplierOrderIngredientsViewModel
                    {
                        Amount = i.Amount,
                        IngredientId = i.IngredientId,
                        OrderId = i.OrderId,
                        Name = _unitOfWork.Ingredients.GetById(i.IngredientId).Name,
                        Unit = _unitOfWork.Ingredients.GetById(i.IngredientId).Unit,
                        OrderIngredientId = i.Id
                    }),
                Id = dbSupplierOrder.Id,
                Address = _unitOfWork.Suppliers.GetById(dbSupplierOrder.SupplierId).Address,
                Name = _unitOfWork.Suppliers.GetById(dbSupplierOrder.SupplierId).Name,
                Phone = _unitOfWork.Suppliers.GetById(dbSupplierOrder.SupplierId).Phone
                
            };
        return View();
        }

        // GET: SupplierOrder/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SupplierOrder/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: SupplierOrder/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
       
        // POST: SupplierOrder/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: SupplierOrder/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SupplierOrder/Delete/5
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

        public ActionResult SaveSupplierOrder(int supplierId, SupplierOrderIngredientsViewModel[] ingredients)
        {
            string result = "Error! Saving supplier process Is Not Complete!";
            if (supplierId > 0 && ingredients != null)
            {
                var supplierOrder = new SupplierOrders
                {
                    SupplierId = supplierId
                    // TODO pass date
                };
                foreach (var ingredientItem in ingredients)
                {
                    supplierOrder.SupplierOrderDetails.Add(
                        new SupplierOrderDetails
                        {
                            IngredientId = ingredientItem.IngredientId,
                            Amount = ingredientItem.Amount
                        });
                }

                _unitOfWork.SupplierOrders.Add(supplierOrder);
                _unitOfWork.Complete();

                result = "Success! order is saved!";
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
