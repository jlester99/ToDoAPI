using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ToDoAPI.API.Models;
using ToDoAPI.DATA.EF;
using System.Web.Http.Cors;

namespace ToDoAPI.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]

    public class CategoriesController : ApiController
    {
        ToDoEntities db = new ToDoEntities();
        //Step 4 - get Categories
        //api/categories
        public IHttpActionResult GetCategories()
        {
            //Create a list of categories from the db
            List<CategoryViewModel> cats = db.Categories.Select(c => new CategoryViewModel()
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
                Description = c.Description
            }).ToList<CategoryViewModel>();

            //handle if categories are found or not below
            if (cats.Count == 0)
            {
                return NotFound(); //404
            }//end if

            return Ok(cats);

        }//end GetCategories

        //Step 5 - Create a Get for one category
        //api/categories/id
        public IHttpActionResult GetCategory(int id)
        {
            //create the resource object and assign it to the results in the db
            CategoryViewModel cat = db.Categories.Where(c => c.CategoryId == id).Select(c => new CategoryViewModel()
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
                Description = c.Description
            }).FirstOrDefault();

            if (cat == null)
            {
                return NotFound();
            }//end if

            return Ok(cat);
        }//end GetCategory

        //Step 6 - Create functionaliy
        //api/categories/  (HttpPost)
        public IHttpActionResult PostCategory(CategoryViewModel cat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }

            db.Categories.Add(new Category()
            {
                Name = cat.Name,
                Description = cat.Description
            });

            db.SaveChanges();
            return Ok();

        }//end PostCategory

        //Step 7
        //api/categories (HttpPut)
        public IHttpActionResult PutCategory(CategoryViewModel cat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }//end if

            //category object to pass the info to the db
            Category existingCat = db.Categories.Where(c => c.CategoryId == cat.CategoryId).FirstOrDefault();

            if (existingCat != null)
            {
                existingCat.Name = cat.Name;
                existingCat.Description = cat.Description;
                db.SaveChanges();
                return Ok();

            }//end if
            else
            {
                return NotFound();
            }//end else


        }//end PutCategory

        //Step 8
        //api/categories/id (HttpDelete)
        public IHttpActionResult DeleteCategory(int id)
        {
            Category cat = db.Categories.Where(c => c.CategoryId == id).FirstOrDefault();

            if (cat != null)
            {
                db.Categories.Remove(cat);
                db.SaveChanges();
                return Ok();  //this return breaks you out of the method so you really don't need the else below
            }//end if
            else
            {
                return NotFound();
            }
        }//end DeleteCategory

        //Step 9
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);

        }


    }//end class
}//end namespace

