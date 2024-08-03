using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using MVCWithEFCF1.Models;
namespace MVCWithEFCF1.Controllers
{
    public class ProductController : Controller
    {
        StoreDbContext dc = new StoreDbContext();

        public ViewResult DisplayProducts()
        {
            dc.Configuration.LazyLoadingEnabled = false;
            var products = dc.Products.Include(P => P.Category).Where(P => P.Discontinued == false);
            return View(products);
        }
        public ViewResult DisplayProduct(int Id)
        {
            dc.Configuration.LazyLoadingEnabled = false;
            var product = dc.Products.Include(P => P.Category).Where(P => P.Id == Id).Single();
            return View(product);
        }
        [HttpGet]
        public ViewResult AddProduct()
        {
            ViewBag.CategoryId = new SelectList(dc.Categories, "CategoryId", "CategoryName");
            return View();
        }
        [HttpPost]
        public RedirectToRouteResult AddProduct(Product product, HttpPostedFileBase selectedFile)
        {
            if (selectedFile != null)
            {
                string DirectoryPath = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(DirectoryPath))
                {
                    Directory.CreateDirectory(DirectoryPath);
                }
                selectedFile.SaveAs(DirectoryPath + selectedFile.FileName);
                product.ProductImageName = selectedFile.FileName;
                BinaryReader br = new BinaryReader(selectedFile.InputStream);
                product.ProductImage = br.ReadBytes(selectedFile.ContentLength);
            }
            dc.Products.Add(product);
            dc.SaveChanges();
            return RedirectToAction("DisplayProducts");
        }
        public ViewResult EditProduct(int Id)
        {
            var product = dc.Products.Find(Id);
            TempData["ProductImageName"] = product.ProductImageName;
            TempData["ProductImage"] = product.ProductImage;
            ViewBag.CategoryId = new SelectList(dc.Categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }
        public RedirectToRouteResult UpdateProduct(Product product, HttpPostedFileBase selectedFile)
        {
            if (selectedFile != null)
            {
                string DirectoryPath = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(DirectoryPath))
                {
                    Directory.CreateDirectory(DirectoryPath);
                }
                selectedFile.SaveAs(DirectoryPath + selectedFile.FileName);
                product.ProductImageName = selectedFile.FileName;
                BinaryReader br = new BinaryReader(selectedFile.InputStream);
                product.ProductImage = br.ReadBytes(selectedFile.ContentLength);
            }
            else if (TempData["ProductImageName"]!=null)
            {
                product.ProductImageName = TempData["ProductImageName"].ToString();
                product.ProductImage = (byte[])TempData["ProductImage"];
            }
            dc.Entry(product).State = EntityState.Modified;
            dc.SaveChanges();
            return RedirectToAction("DisplayProducts");
        }
        public RedirectToRouteResult DeleteProduct(int Id)
        {
            var product = dc.Products.Find(Id);
            product.Discontinued = true;
            dc.Entry(product).State = EntityState.Modified;
            dc.SaveChanges();
            return RedirectToAction("DisplayProducts");
        }
    }
}