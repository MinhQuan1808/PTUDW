﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyClass.DAO;
using MyClass.Model;

namespace _63CNTT5N2.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {

        CategoriesDAO categoriesDAO = new CategoriesDAO();
        /// /////////////////////////////////////////////////////////////////////
        /// INDEX
       
        

        // GET: Admin/Category
        public ActionResult Index()
        {
            return View(categoriesDAO.getList("Index"));//chi hien thi cac dong co status = 1, 2
        }

    //    /// /////////////////////////////////////////////////////////////////////
    //    /// DETAILS

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    }
    Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                return HttpNotFound();
}
return View(categories);
        }

        //    // GET: Admin/Category/Create
        //    /// /////////////////////////////////////////////////////////////////////
        //    /// CREATE
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Categories categories)
        {
            if (ModelState.IsValid)
            {
                categoriesDAO.Insert(categories);
                return RedirectToAction("Index");
            }

            return View(categories);
        }

        // GET: Admin/Category/Edit/5      
        /// ////////////////////////////////
        /// EDIT
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Tim dong DB can chinh sua
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                return HttpNotFound();
            }
            return View(categories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Categories categories)
        {
            if (ModelState.IsValid)
            {
                categoriesDAO.Update(categories);
                return RedirectToAction("Index");
            }
            return View(categories);
        }

        // GET: Admin/Category/Delete/5
        /// /////////////////////////
        /// DELETE
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                return HttpNotFound();
            }
            return View(categories);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Categories categories = categoriesDAO.getRow(id);
            categoriesDAO.Detele(categories);
            return RedirectToAction("Index");
        }
    }
}
