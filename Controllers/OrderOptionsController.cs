using Project_3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_3.Controllers
{
    public class OrderOptionsController : Controller
    {
        // GET: order options
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sortBy">0 = Sales Tax Rate, 1 = First Book Ship Charge , 2 = Additional Book Ship Charge</param>
        /// <param name="isDesc"></param>
        /// <returns></returns>

        public ActionResult All(int sortBy = 0, bool isDesc = false)
        {
            var context = new BooksEntities1();
            List<OrderOption> orderOptions = context.OrderOptions.ToList();
            switch (sortBy)
            {
                case 1:
                    {
                        if (isDesc)
                            orderOptions = context.OrderOptions.OrderByDescending(c => c.SalesTaxRate).ToList();
                        else
                            orderOptions = context.OrderOptions.OrderBy(c => c.SalesTaxRate).ToList();
                        break;
                    }
                case 2:
                    {
                        if (isDesc)
                            orderOptions = context.OrderOptions.OrderByDescending(c => c.FirstBookShipCharge).ToList();
                        else
                            orderOptions = context.OrderOptions.OrderBy(c => c.FirstBookShipCharge).ToList();
                        break;
                    }
                case 0:
                default:
                    {
                        if (isDesc)
                            orderOptions = context.OrderOptions.OrderByDescending(c => c.AdditionalBookShipCharge).ToList();
                        else
                            orderOptions = context.OrderOptions.OrderBy(c => c.AdditionalBookShipCharge).ToList();
                        break;
                    }
            }

            return View(orderOptions);
        }

        [HttpGet]
        public ActionResult Upsert(int? id)
        {
            BooksEntities1 context = new BooksEntities1();
            OrderOption orderOption = context.OrderOptions.Where(s => s.SalesTaxRate == id).FirstOrDefault();
            //List<Department> departments = context.Departments.ToList();

            //UpsertEmployeeModel viewModel = new UpsertEmployeeModel()
            //{
            //Employee = employee,
            //Departments = departments
            //};

            return View(orderOption);
        }

        [HttpPost]
        public ActionResult Upsert(OrderOption newOrderOption)
        {
            BooksEntities1 context = new BooksEntities1();

            try
            {
                if (context.OrderOptions.Where(s => s.SalesTaxRate == newOrderOption.SalesTaxRate).Count() > 0)
                {
                    var orderOptionToSave = context.OrderOptions.Where(s => s.SalesTaxRate == newOrderOption.SalesTaxRate).FirstOrDefault();

                    orderOptionToSave.FirstBookShipCharge = newOrderOption.FirstBookShipCharge;
                    orderOptionToSave.AdditionalBookShipCharge = newOrderOption.AdditionalBookShipCharge;
                }
                else
                {
                    context.OrderOptions.Add(newOrderOption);
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
            int orderOptionsID = 0;

            if (int.TryParse(id, out orderOptionsID))
            {
                try
                {
                    OrderOption orderOption = context.OrderOptions.Where(c => c.SalesTaxRate == orderOptionsID).FirstOrDefault();
                    context.OrderOptions.Remove(orderOption);

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