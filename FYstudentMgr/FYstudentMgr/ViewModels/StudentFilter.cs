using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYstudentMgr.ViewModels
{
    public class StudentFilter
    {
        public List<GradeModel> grades { get; set; }
        public List<IntroModel> intros { get; set; }
        public List<SchoolModel> schools { get; set; }
        
        public List<MajorModel> majors { get; set; }
        public List<EducationModel> educations { get; set; }
    }


    public class GradeModel
    {
        public string id { get; set; }
        public string gradeName { get; set; }
        public bool selected { get; set; }
    }
    public class IntroModel
    {
        public string id { get; set; }
        public string introName { get; set; }
        public bool selected { get; set; }
    }
    public class SchoolModel
    {
        public int id { get; set; }
        public string schoolName { get; set; }
        public bool selected { get; set; }
    }

    public class MajorModel
    {
        public int id { get; set; }
        public string majorName { get; set; }
        public bool selected { get; set; }
    }
    public class EducationModel
    {
        public int id { get; set; }
        public string educationName { get; set; }
        public bool selected { get; set; }
    }


    public class FilterIds
    {
        public int[] postIds { get; set; }
        public List<string> gradeIds { get; set; }
        public List<int> majorIds { get; set; }
        public List<int> eduIds { get; set; }
        public List<int> schoolIds { get; set; }
        public List<string> introIds { get; set; }

    }
}