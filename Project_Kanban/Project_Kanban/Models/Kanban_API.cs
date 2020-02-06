using Project_Kanban.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project_Kanban.Models
{
    [Table("TB_M_User")]
    public class User : BaseModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    [Table("TB_M_Role")]
    public class Role : BaseModel
    {
        public string Name { get; set; }
    }

    [Table("TB_T_UserRole")]
    public class UserRole : BaseModel
    {
        public Role Role { get; set; }
        public User User { get; set; }
    }

    [Table("TB_M_Project")]
    public class Project : BaseModel
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime Due_Date { get; set; }
        public string Requirment { get; set; }
    }

    [Table("TB_M_Actor")]
    public class Actor : BaseModel
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public User User { get; set; }
    }

    [Table("TB_T_Module")]
    public class Module : BaseModel
    {
        public string Status { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime Due_Date { get; set; }
        public string Description { get; set; }
        public Project Project { get; set; }
        public Actor Actor { get; set; }
    }
}