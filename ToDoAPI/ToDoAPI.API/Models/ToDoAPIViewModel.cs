using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ToDoAPI.API.Models
{
    //public class ToDoAPIViewModel
    //{
    //}
    public class ToDoViewModel
    {
        [Key]
        public int TodoId { get; set; }

        [Required]
        public string Task { get; set; }

        public bool Done { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public int CategoryId { get; set; }

        public virtual CategoryViewModel Category { get; set; }
    }

    public class CategoryViewModel
    {
        [Key]
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }





}//end namespace