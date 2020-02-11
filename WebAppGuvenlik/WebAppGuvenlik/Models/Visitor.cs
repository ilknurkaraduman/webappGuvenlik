using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace WebAppGuvenlik.Models
{
    [Table("Visitor")]
    public class Visitor
    {
        [Key]
        public int ID { get; set; }
        [DisplayName("Name")]
        [Required(ErrorMessage = "Enter name")]
        public string Name { get; set; }
        [DisplayName("Surname")]
        [Required(ErrorMessage = "Enter surname")]
        public string Surname { get; set; }
        [DisplayName("Phone")]
        [Phone(ErrorMessage = "Please make sure you enter your phone correctly.")]
        [Required(ErrorMessage = "Enter phone number")]
        public string Phone { get; set; }
        [DisplayName("Date")]
        public DateTime? Date { get; set; }
        [DisplayName("Description")]
        [Required(ErrorMessage = "Enter Reason for Visit")]
        public string Description { get; set; }
        [DisplayName("Employee to Visit")]
        [Required(ErrorMessage = "Enter the person you want to visit")]
        public int EmployeeID { get; set; }
        [DisplayName("who adds to the system")]
        public int SecurityID { get; set; }

        public bool? Click { get; set; }

    }
}