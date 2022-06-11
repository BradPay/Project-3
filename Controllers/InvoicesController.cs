using Project_3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_3.Controllers
{
    public class InvoicesController : Controller
    {
        // GET: invoices
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sortBy">0 = id, 1 = custoemr id, 2 = invoice date, 3 = producttotal, 4 = sales tax, 5 = shipping, 6 = invoice total</param>
        /// <param name="isDesc"></param>
        /// <returns></returns>

        public ActionResult All(int sortBy = 0, bool isDesc = false)
        {
            var context = new BooksEntities1();
            List<Invoice> invoices = context.Invoices.ToList();
            switch (sortBy)
            {
                case 1:
                    {
                        if (isDesc)
                            invoices = context.Invoices.OrderByDescending(c => c.InvoiceID).ToList();
                        else
                            invoices = context.Invoices.OrderBy(c => c.InvoiceID).ToList();
                        break;
                    }
                case 2:
                    {
                        if (isDesc)
                            invoices = context.Invoices.OrderByDescending(c => c.CustomerID).ToList();
                        else
                            invoices = context.Invoices.OrderBy(c => c.CustomerID).ToList();
                        break;
                    }
                case 3:
                    {
                        if (isDesc)
                            invoices = context.Invoices.OrderByDescending(c => c.InvoiceDate).ToList();
                        else
                            invoices = context.Invoices.OrderBy(c => c.InvoiceDate).ToList();
                        break;
                    }
                case 4:
                    {
                        if (isDesc)
                            invoices = context.Invoices.OrderByDescending(c => c.ProductTotal).ToList();
                        else
                            invoices = context.Invoices.OrderBy(c => c.ProductTotal).ToList();
                        break;
                    }
                case 5:
                    {
                        if (isDesc)
                            invoices = context.Invoices.OrderByDescending(c => c.SalesTax).ToList();
                        else
                            invoices = context.Invoices.OrderBy(c => c.SalesTax).ToList();
                        break;
                    }
                case 6:
                    {
                        if (isDesc)
                            invoices = context.Invoices.OrderByDescending(c => c.Shipping).ToList();
                        else
                            invoices = context.Invoices.OrderBy(c => c.Shipping).ToList();
                        break;
                    }
                case 0:
                default:
                    {
                        if (isDesc)
                            invoices = context.Invoices.OrderByDescending(c => c.InvoiceTotal).ToList();
                        else
                            invoices = context.Invoices.OrderBy(c => c.InvoiceTotal).ToList();
                        break;
                    }
            }

            return View(invoices);
        }

        [HttpGet]
        public ActionResult Upsert(int? id)
        {
            BooksEntities1 context = new BooksEntities1();
            Invoice invoice = context.Invoices.Where(s => s.InvoiceID == id).FirstOrDefault();
            //List<Department> departments = context.Departments.ToList();

            //UpsertEmployeeModel viewModel = new UpsertEmployeeModel()
            //{
            //Employee = employee,
            //Departments = departments
            //};

            return View(invoice);
        }

        [HttpPost]
        public ActionResult Upsert(Invoice newInvoice)
        {
            BooksEntities1 context = new BooksEntities1();

            try
            {
                if (context.Invoices.Where(s => s.InvoiceID == newInvoice.InvoiceID).Count() > 0)
                {
                    var invoiceToSave = context.Invoices.Where(s => s.InvoiceID == newInvoice.InvoiceID).FirstOrDefault();

                    invoiceToSave.CustomerID = newInvoice.CustomerID;
                    invoiceToSave.InvoiceDate = newInvoice.InvoiceDate;
                    invoiceToSave.ProductTotal = newInvoice.ProductTotal;
                    invoiceToSave.SalesTax = newInvoice.SalesTax;
                    invoiceToSave.Shipping = newInvoice.Shipping;
                    invoiceToSave.InvoiceTotal = newInvoice.InvoiceTotal;
                }
                else
                {
                    context.Invoices.Add(newInvoice);
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
            int invoiceID = 0;

            if (int.TryParse(id, out invoiceID))
            {
                try
                {
                    Invoice invoice = context.Invoices.Where(c => c.InvoiceID == invoiceID).FirstOrDefault();
                    context.Invoices.Remove(invoice);

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