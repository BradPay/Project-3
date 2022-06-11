using Project_3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_3.Controllers
{
    public class StatesController : Controller
    {
        // GET: States
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sortBy">0 = state code, 1 = state name</param>
        /// <param name="isDesc"></param>
        /// <returns></returns>

        public ActionResult All(int sortBy = 0, bool isDesc = false)
        {
            var context = new BooksEntities1();
            List<State> states = context.States.ToList();
            switch (sortBy)
            {
                case 1:
                    {
                        if (isDesc)
                            states = context.States.OrderByDescending(c => c.StateCode).ToList();
                        else
                            states = context.States.OrderBy(c => c.StateCode).ToList();
                        break;
                    }
                case 0:
                default:
                    {
                        if (isDesc)
                            states = context.States.OrderByDescending(c => c.StateName).ToList();
                        else
                            states = context.States.OrderBy(c => c.StateName).ToList();
                        break;
                    }
            }

            return View(states);
        }

        [HttpGet]
        public ActionResult Upsert(string id)
        {
            BooksEntities1 context = new BooksEntities1();
            State state = context.States.Where(s => s.StateCode== id).FirstOrDefault();
            //List<Department> departments = context.Departments.ToList();

            //UpsertEmployeeModel viewModel = new UpsertEmployeeModel()
            //{
            //Employee = employee,
            //Departments = departments
            //};

            return View(state);
        }

        [HttpPost]
        public ActionResult Upsert(State newState)
        {
            BooksEntities1 context = new BooksEntities1();

            try
            {
                if (context.States.Where(s => s.StateCode== newState.StateCode).Count() > 0)
                {
                    var stateToSave = context.States.Where(s => s.StateCode == newState.StateCode).FirstOrDefault();

                    stateToSave.StateName = newState.StateName;
                }
                else
                {
                    context.States.Add(newState);
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
            int stateId = 0;

            if (int.TryParse(id, out stateId))
            {
                try
                {
                    State state = context.States.Where(c => c.StateCode == stateId.ToString()).FirstOrDefault();
                    context.States.Remove(state);

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