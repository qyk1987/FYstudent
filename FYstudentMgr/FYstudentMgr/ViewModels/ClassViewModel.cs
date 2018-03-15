using FYstudentMgr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYstudentMgr.ViewModels
{
    public class ClassViewModel
    {
    }
    public class MenuVM
    {
        public string name { get; set; }
        public List<MenuItem> lists { get; set; }
    }
    public class MenuItem
    {
        public int id { get; set; }
        public string name { get; set; }
        public int count { get; set; }


    }

    public class ChargerVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class ClassVM
    {
        public int Id { get; set; }
        public string ClassName { get; set; }//班级名称
       
        public int ChargerID { get; set; }//班主任id
        
        public DateTime OverDate { get; set; }
        public string Arrange { get; set; }//班级课程安排
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public ClassState ClassState { get; set; }//班级状态 
        public int studentCount { get; set; }
        public string chargerName { get; set; }

    }
    
}