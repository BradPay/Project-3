using Project_3.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Project_3.Controllers
{
    public class CustomersController : Controller
    {
        // GET: Customers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sortBy">0 = id, 1 = name, 2 = address, 3 = city, 4 = state, 5 = zip code</param>
        /// <param name="isDesc"></param>
        /// <returns></returns>

        public ActionResult All(string id, int sortBy = 0, bool isDesc = false)
        {
            BooksEntities1 context = new BooksEntities1();
            List<Customer> customers;
            switch (sortBy)
            {
                case 1:
                    {
                        if (isDesc)
                            customers = context.Customers.OrderByDescending(c => c.CustomerID).ToList();
                        else
                            customers = context.Customers.OrderBy(c => c.CustomerID).ToList();
                        break;
                    }
                case 2:
                    {
                        if (isDesc)
                            customers = context.Customers.OrderByDescending(c => c.Name).ToList();
                        else
                            customers = context.Customers.OrderBy(c => c.Name).ToList();
                        break;
                    }
                case 3:
                    {
                        if (isDesc)
                            customers = context.Customers.OrderByDescending(c => c.Address).ToList();
                        else
                            customers = context.Customers.OrderBy(c => c.Address).ToList();
                        break;
                    }
                case 4:
                    {
                        if (isDesc)
                            customers = context.Customers.OrderByDescending(c => c.City).ToList();
                        else
                            customers = context.Customers.OrderBy(c => c.City).ToList();
                        break;
                    }
                case 5:
                    {
                        if (isDesc)
                            customers = context.Customers.OrderByDescending(c => c.State).ToList();
                        else
                            customers = context.Customers.OrderBy(c => c.State).ToList();
                        break;
                    }
                case 0:
                default:
                    {
                        if (isDesc)
                            customers = context.Customers.OrderByDescending(c => c.ZipCode).ToList();
                        else
                            customers = context.Customers.OrderBy(c => c.ZipCode).ToList();
                        break;
                    }
            }

            if (!string.IsNullOrWhiteSpace(id))
            {
                id = id.Trim().ToLower();

                customers = customers.Where(c =>
                                c.Name.ToLower().Contains(id) ||
                                c.Address.ToLower().Contains(id) ||
                                c.City.ToLower().Contains(id) ||
                                c.State.ToLower().Contains(id) ||
                                c.ZipCode.ToLower().Contains(id)
                                ).ToList();
            }

            return View(customers);
        }

        [HttpGet]
        public ActionResult Upsert(int? id)
        {
            BooksEntities1 context = new BooksEntities1();
            Customer customer = context.Customers.Where(s => s.CustomerID == id).FirstOrDefault();
            if (customer == null)
            {
                customer = new Customer();
            }
            List<State> states = context.States.ToList();

            UpsertCustomerModel viewModel = new UpsertCustomerModel()
            {
                Customer = customer,
                States = states
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Upsert(string id, UpsertCustomerModel model)
        {
            Customer newCustomer = model.Customer;

            Regex r = new Regex("[(][A-Za-z][A-Za-z][)]$");
            Match m = r.Match(newCustomer.State);
            newCustomer.State = m.Value.Replace("(", "").Replace(")", "");


            BooksEntities1 context = new BooksEntities1();

            bool isNewCustomer = false;

            try
            {
                if (context.Customers.Where(s => s.CustomerID == newCustomer.CustomerID).Count() > 0)
                {
                    var customerToSave = context.Customers.Where(s => s.CustomerID == newCustomer.CustomerID).FirstOrDefault();

                    customerToSave.Name = newCustomer.Name;
                    customerToSave.Address = newCustomer.Address;
                    customerToSave.City = newCustomer.City;
                    customerToSave.State = newCustomer.State;
                    customerToSave.ZipCode = newCustomer.ZipCode;
                }
                else
                {
                    context.Customers.Add(newCustomer);
                    isNewCustomer = true;
                }

                context.SaveChanges();

                if (isNewCustomer)
                {
                    string fromEmail = ConfigurationManager.AppSettings.Get("fromEmail");
                    string fromPassword = ConfigurationManager.AppSettings.Get("fromPassword");
                    string fromName = ConfigurationManager.AppSettings.Get("fromName");

                    int fromPort = Convert.ToInt32(ConfigurationManager.AppSettings.Get("fromPort"));

                    string smtpHost = ConfigurationManager.AppSettings.Get("smtpHost");

                    //send welcome email
                    MailAddress fromAddress = new MailAddress(fromEmail, fromName);
                    var toAddress = new MailAddress(newCustomer.Name, newCustomer.ZipCode);

                    string subject = "Welcome to my database programming website";
                    string body = "Love having you";

                    SmtpClient smtp = new SmtpClient
                    {
                        Host = smtpHost,
                        Port = fromPort,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                    };

                    var message = new MailMessage(fromAddress.Address, toAddress.Address, subject, body);

                    smtp.Send(message);
                }
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
            int customerId = 0;

            if (int.TryParse(id, out customerId))
            {
                try
                {
                    Customer customer = context.Customers.Where(c => c.CustomerID == customerId).FirstOrDefault();
                    context.Customers.Remove(customer);

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