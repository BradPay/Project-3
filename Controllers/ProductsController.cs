using Project_3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_3.Controllers
{
    public class ProductsController : Controller
    {
        // GET: products
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sortBy">0 = ProductCode, 1 = Description, 2 = UnitPrice, 3 = OnHandQuantity</param>
        /// <param name="isDesc"></param>
        /// <returns></returns>

        public ActionResult All(int sortBy = 0, bool isDesc = false)
        {
            var context = new BooksEntities1();
            List<Product> products = context.Products.ToList();
            switch (sortBy)
            {
                case 1:
                    {
                        if (isDesc)
                            products = context.Products.OrderByDescending(c => c.ProductCode).ToList();
                        else
                            products = context.Products.OrderBy(c => c.ProductCode).ToList();
                        break;
                    }
                case 2:
                    {
                        if (isDesc)
                            products = context.Products.OrderByDescending(c => c.Description).ToList();
                        else
                            products = context.Products.OrderBy(c => c.Description).ToList();
                        break;
                    }
                case 3:
                    {
                        if (isDesc)
                            products = context.Products.OrderByDescending(c => c.UnitPrice).ToList();
                        else
                            products = context.Products.OrderBy(c => c.UnitPrice).ToList();
                        break;
                    }
                case 0:
                default:
                    {
                        if (isDesc)
                            products = context.Products.OrderByDescending(c => c.OnHandQuantity).ToList();
                        else
                            products = context.Products.OrderBy(c => c.OnHandQuantity).ToList();
                        break;
                    }
            }

            return View(products);
        }

        [HttpGet]
        public ActionResult Upsert(string id)
        {
            BooksEntities1 context = new BooksEntities1();
            Product product = context.Products.Where(s => s.ProductCode == id).FirstOrDefault();
            //List<Department> departments = context.Departments.ToList();

            //UpsertEmployeeModel viewModel = new UpsertEmployeeModel()
            //{
            //Employee = employee,
            //Departments = departments
            //};

            return View(product);
        }

        [HttpPost]
        public ActionResult Upsert(Product newProduct)
        {
            BooksEntities1 context = new BooksEntities1();

            try
            {
                if (context.Products.Where(s => s.ProductCode == newProduct.ProductCode).Count() > 0)
                {
                    var customerToSave = context.Products.Where(s => s.ProductCode == newProduct.ProductCode).FirstOrDefault();

                    customerToSave.Description = newProduct.Description;
                    customerToSave.UnitPrice = newProduct.UnitPrice;
                    customerToSave.OnHandQuantity = newProduct.OnHandQuantity;
                }
                else
                {
                    context.Products.Add(newProduct);
                }

                context.SaveChanges();
            }

            catch (Exception ex)
            {
                //error
            }

            return RedirectToAction("All");
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            BooksEntities1 context = new BooksEntities1();
            int productID = 0;

            if (int.TryParse(id, out productID))
            {
                try
                {
                    Product product = context.Products.Where(c => c.ProductCode == productID.ToString()).FirstOrDefault();
                    context.Products.Remove(product);

                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            else
            {
                //not succesful parsing
            }
            return RedirectToAction("All");
        }
    }
}