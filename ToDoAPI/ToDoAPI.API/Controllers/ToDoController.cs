using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ToDoAPI.API.Models; //access to the DTO's
using ToDoAPI.DATA.EF; //access to Data Layer (EF)
using System.Web.Http.Cors; // access to modify the CORS for this controller specifically

namespace ToDoAPI.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]  //all origins are ok, all headers and all methods are ok

    public class ToDoController : ApiController
    {
        // create an object that will connect to the db 
        ToDoEntities db = new ToDoEntities();

        // READ - GET Functionality 
        public IHttpActionResult GetToDos()
        {
            //create list to house the ToDos
            List<ToDoViewModel> toDos = db.TodoItems.Include("Category").Select(t => new ToDoViewModel()
            {

                //We have now taken all of the TodoItems from the db and need to assign each TodoItem from the db to our Data Transfer Object so we can transport the data.

                //Assign parameters of the ToDoItems to properties in the DTO
                TodoId = t.TodoId,
                Task = t.Task,
                Done = t.Done,
                DueDate = t.DueDate,
                Category = new CategoryViewModel()
                {
                    CategoryId = t.Category.CategoryId,
                    Name = t.Category.Name,
                    Description = t.Category.Description
                }
            }).ToList<ToDoViewModel>();

            // check on results and handle accordingly
            if (toDos.Count == 0)
            {
                return NotFound(); //404 error
            }//end if

            return Ok(toDos);  //Ok = 200, which is just a way to confirm the desired results

        }//end GetToDos

        public IHttpActionResult GetToDo(int id)
        {
            ToDoViewModel toDo = db.TodoItems.Include("Category").Where(t => t.TodoId == id).Select(t => new ToDoViewModel()
            {
                TodoId = t.TodoId,
                Task = t.Task,
                Done = t.Done,
                DueDate = t.DueDate,
                Category = new CategoryViewModel()
                {
                    CategoryId = t.Category.CategoryId,
                    Name = t.Category.Name,
                    Description = t.Category.Description
                }
            }).FirstOrDefault();

            if (toDo == null)   
            {
                return NotFound();
            }
            return Ok(toDo);

        }//end GetToDo


        public IHttpActionResult PostToDo(ToDoViewModel toDoItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }//end if

            TodoItem newToDo = new TodoItem()
            {
                Task = toDoItem.Task,
                Done = toDoItem.Done,
                DueDate = toDoItem.DueDate,
                CategoryId = toDoItem.CategoryId
            };

            db.TodoItems.Add(newToDo);
            db.SaveChanges();
            return Ok(newToDo);

        }//end PostToDo


        public IHttpActionResult PutToDo(ToDoViewModel toDoItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }

            TodoItem existingToDo = db.TodoItems.Where(t => t.TodoId == toDoItem.TodoId).FirstOrDefault();

            if (existingToDo != null)       
            {
                existingToDo.TodoId = toDoItem.TodoId;
                existingToDo.Done = toDoItem.Done;
                existingToDo.DueDate = toDoItem.DueDate;
                existingToDo.CategoryId = toDoItem.CategoryId;
                return Ok();
            }//end if
            else
            {
                return NotFound();
            }//end else
        }//end PutToDo

        public IHttpActionResult DeleteToDo(int id)
        {
            TodoItem todoItem = db.TodoItems.Where(t => t.TodoId == id).FirstOrDefault();

            if (todoItem != null)
            {
                db.TodoItems.Remove(todoItem);
                db.SaveChanges();
                return Ok();
            }//end if
            else
            {
                return NotFound();
            }//end else

        }//end DeleteToDo


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose(); //terminate the db object
            }
            //Below disposes of the instance of the controller
            base.Dispose(disposing);
        }

    }//end class
}//end namespace
