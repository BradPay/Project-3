using Project_3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_3.Controllers
{
    public class InvoiceLineItemsController : Controller
    {
        // GET: Invoice line items
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sortBy">0 = id, 1 = product code, 2 = unit price, 3 = quantity, 4 = item total</param>
        /// <param name="isDesc"></param>
        /// <returns></returns>

        public ActionResult All(int sortBy = 0, bool isDesc = false)
        {
            var context = new BooksEntities1();
            List<InvoiceLineItem> invoiceLineItems = context.InvoiceLineItems.ToList();
            switch (sortBy)
            {
                case 1:
                    {
                        if (isDesc)
                            invoiceLineItems = context.InvoiceLineItems.OrderByDescending(c => c.InvoiceID).ToList();
                        else
                            invoiceLineItems = context.InvoiceLineItems.OrderBy(c => c.InvoiceID).ToList();
                        break;
                    }
                case 2:
                    {
                        if (isDesc)
                            invoiceLineItems = context.InvoiceLineItems.OrderByDescending(c => c.ProductCode).ToList();
                        else
                            invoiceLineItems = context.InvoiceLineItems.OrderBy(c => c.ProductCode).ToList();
                        break;
                    }
                case 3:
                    {
                        if (isDesc)
                            invoiceLineItems = context.InvoiceLineItems.OrderByDescending(c => c.UnitPrice).ToList();
                        else
                            invoiceLineItems = context.InvoiceLineItems.OrderBy(c => c.UnitPrice).ToList();
                        break;
                    }
                case 4:
                    {
                        if (isDesc)
                            invoiceLineItems = context.InvoiceLineItems.OrderByDescending(c => c.Quantity).ToList();
                        else
                            invoiceLineItems = context.InvoiceLineItems.OrderBy(c => c.Quantity).ToList();
                        break;
                    }
                case 0:
                default:
                    {
                        if (isDesc)
                            invoiceLineItems = context.InvoiceLineItems.OrderByDescending(c => c.ItemTotal).ToList();
                        else
                            invoiceLineItems = context.InvoiceLineItems.OrderBy(c => c.ItemTotal).ToList();
                        break;
                    }
            }

            return View(invoiceLineItems);
        }

        [HttpGet]
        public ActionResult Upsert(int? id)
        {
            BooksEntities1 context = new BooksEntities1();
            InvoiceLineItem invoiceLineItem = context.InvoiceLineItems.Where(s => s.InvoiceID == id).FirstOrDefault();

            return View(invoiceLineItem);
        }

        [HttpPost]
        public ActionResult Upsert(InvoiceLineItem newInvoiceLineItem)
        {
            BooksEntities1 context = new BooksEntities1();

            try
            {
                if (context.InvoiceLineItems.Where(s => s.InvoiceID == newInvoiceLineItem.InvoiceID).Count() > 0)
                {
                    var invoiceLineItemsToSave = context.InvoiceLineItems.Where(s => s.InvoiceID == newInvoiceLineItem.InvoiceID).FirstOrDefault();

                    invoiceLineItemsToSave.ProductCode = newInvoiceLineItem.ProductCode;
                    invoiceLineItemsToSave.UnitPrice = newInvoiceLineItem.UnitPrice;
                    invoiceLineItemsToSave.Quantity = newInvoiceLineItem.Quantity;
                    invoiceLineItemsToSave.ItemTotal = newInvoiceLineItem.ItemTotal;
                }
                else
                {
                    context.InvoiceLineItems.Add(newInvoiceLineItem);
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
            int invoiceId = 0;

            if (int.TryParse(id, out invoiceId))
            {
                try
                {
                    InvoiceLineItem invoiceLineItem = context.InvoiceLineItems.Where(c => c.InvoiceID == invoiceId).FirstOrDefault();
                    context.InvoiceLineItems.Remove(invoiceLineItem);

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