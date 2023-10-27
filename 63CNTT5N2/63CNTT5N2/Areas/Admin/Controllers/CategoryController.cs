using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _63CNTT5N2.Library;
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
                TempData["message"] = new XMessage("danger", "Không tồn tại loại sản phẩm");
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                TempData["message"] = new XMessage("danger", "Không tồn tại loại sản phẩm");
                return RedirectToAction("Index");
            }
            return View(categories);
        }

        //    // GET: Admin/Category/Create
        //    /// /////////////////////////////////////////////////////////////////////
        //    /// CREATE
        public ActionResult Create()
        {
            ViewBag.ListOrder = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            ViewBag.ListCat = new SelectList(categoriesDAO.getList("Index"),"Id","Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Categories categories)
        {
            if (ModelState.IsValid)
            {
                //Xu ly tu dong
                categories.CreatedAt = DateTime.Now;
                categories.UpdateAt = DateTime.Now;
                if(categories.ParentId == null)
                {
                    categories.ParentId = 0;
                }
                if (categories.Order == null)
                {
                    categories.Order = 1;
                }
                else
                {
                    categories.Order += 1;
                }
                categories.Slug = XString.Str_Slug(categories.Name);
                //them dong du lieu cho DB
                categoriesDAO.Insert(categories);
                //Thong bao them dong du lieu thanh cong
                TempData["message"] = TempData["message"] = new XMessage("success", "Tạo mới loại sản phẩm thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ListOrder = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            ViewBag.ListCat = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            return View(categories);
        }

        // GET: Admin/Category/Edit/5      
        /// ////////////////////////////////
        /// EDIT
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẩu tin");
                return RedirectToAction("Index");
            }
            //Tim dong DB can chinh sua
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẩu tin");
                return RedirectToAction("Index");
            }
            ViewBag.ListOrder = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            ViewBag.ListCat = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            return View(categories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Categories categories)
        {
            if (ModelState.IsValid)
            {
                //xu ly tu dong: slug
                categories.Slug = XString.Str_Slug(categories.Name);
                //Xu ly tu dong: ParentID\
                if (categories.ParentId == null)
                {
                    categories.ParentId = 0;
                }
                //Xu ly tu dong: Order
                if (categories.Order == null)
                {
                    categories.Order = 1;
                }
                else
                {
                    categories.Order += 1;
                }
                //Xu ly tu dong: UpdateAt
                categories.UpdateAt = DateTime.Now;
                //cap nhat mau tin
                categoriesDAO.Update(categories);
                //thong bao thanh cong
                TempData["message"] = TempData["message"] = new XMessage("success", "Cập nhật mẩu tin thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ListOrder = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            ViewBag.ListCat = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            return View(categories);
        }

        ///////////////////////////////////////////////////////////////////
        /// DELETE
        // GET: Admin/Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Xóa mẩu tin thất bại");
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Xóa mẩu tin thất bại");
                return RedirectToAction("Index");
            }
            return View(categories);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Categories categories = categoriesDAO.getRow(id);

            categoriesDAO.Delete(categories);

            //thong bao thanh cong
            TempData["message"] = new XMessage("success", "Xóa mẩu tin thành công");
            return RedirectToAction("Trash", "Category");
        }

        //////////////////////////////
        ///STATUS
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            //Tim dong co id == id can thay doi
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            else
            {                          
                //Kiem tra trang lhai cua Status, neu hien tai la 1 thi thanh 2, 2 thi thanh 1
                categories.Status = (categories.Status == 1) ? 2 : 1;
                //cap nhat gia tri cho UpdateAt
                categories.UpdateAt = DateTime.Now;
                //cap nhat db
                categoriesDAO.Update(categories);
                //tra ket qua ve index
                TempData["message"] = TempData["message"] = new XMessage("success", "Cập nhật trạng thái thành công");
                return RedirectToAction("Index");
            }
        }

        //////////////////////////////
        ///DelTrash
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin");
                return RedirectToAction("Index");
            }
            //Tim dong co id == id can thay doi
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin");
                return RedirectToAction("Index");
            }
            else
            {
                //Kiem tra trang lhai cua Status 1, 2 -> 0: Khong hien thi o Index
                categories.Status = 0;
                //cap nhat gia tri cho UpdateAt
                categories.UpdateAt = DateTime.Now;
                //cap nhat db
                categoriesDAO.Update(categories);
                //tra ket qua ve index
                TempData["message"] = TempData["message"] = new XMessage("success", "Xóa mẫu tin thành công");
                return RedirectToAction("Index");
            }
        }

        ///////////////////////////////////////////////////////////////////
        /// TRASH
        // GET: Admin/Category/Trash
        public ActionResult Trash()
        {
            return View(categoriesDAO.getList("Trash"));//chi hien thi cac dong co status 0
        }

        ///////////////////////////////////////////////////////////////////
        /// Recover
        // GET: Admin/Category/Recover/5
        public ActionResult Recover(int? id)
        {
            if (id == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Phục hồi mẩu tin thất bại");
                return RedirectToAction("Index");
            }
            //tim row co id == id cua loai SP can thay doi Status
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //thong bao that bai
                TempData["message"] = new XMessage("danger", "Phục hồi mẩu tin thất bại");
                return RedirectToAction("Index");
            }
            //trang thai cua status = 2
            categories.Status = 2;//truoc recover=0
            //cap nhat gia tri cho UpdateAt
            categories.UpdateAt = DateTime.Now;

            //cap nhat lai DB
            categoriesDAO.Update(categories);
            //thong bao thanh cong
            TempData["message"] = new XMessage("success", "Phục hồi mẩu tin thành công");
            //tra ket qua ve Index
            return RedirectToAction("Index");//action trong Category
        }
    }
}
