using FYstudentMgr.Models;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FYstudentMgr.ViewModels;
using FYstudentMgr.Manager;
using System.Threading.Tasks;
using FYstudentMgr.Helper;
using System.Web.OData;

namespace FYstudentMgr.Manager
{
    public class StudentManager
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// 获取所有学生
        /// </summary>
        /// <returns></returns>
        public IQueryable<Student> GetStudents()
        {
            return db.Students.Include(s=>s.Signer);
        }
        /// <summary>
        /// 根据id获取一个学生
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Student> GetStudent(int id)
        {
            return await db.Students.FindAsync(id); 
        }
        /// <summary>
        /// 根据岗位id查找该岗位及其下属岗位经手的学生信息
        /// </summary>
        /// <param name="postId">岗位id</param>
        /// <returns></returns>
        public IQueryable<Student> GetStudentsByPostIds(int[] idList)
        {
            
            List<int> ids = idList.ToList();
            if (ids == null)
            {
                return null;
            }
            var students = db.Students.Include(s => s.Signer)
                            .Where(s => ids.Contains(s.Signer.PostId));
           
            return students;
        }
        /// <summary>
        /// 根据关键字搜索学生
        /// </summary>
        /// <param name="key">搜索关键字</param>
        /// <returns></returns>
        public IQueryable<Student> GetStudentsByKey(string key)
        {

            return db.Students.Include(s=>s.Signer)
                .Where(s => s.IdCardNO == key || s.Name == key || s.QQ == key || s.MobilePhoneNO == key);
        }
        /// <summary>
        /// 根据关键字模糊查询
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IQueryable<SimpleStudentData> GetStudentsByLike(string key)
        {
            key = key.ToLower();
            return db.Students.Include(s => s.Signer)
                .Where(s => s.IdCardNO.ToLower().IndexOf(key)>-1 || s.Name.ToLower().IndexOf(key) > -1 || s.QQ.ToLower().IndexOf(key) > -1 || s.MobilePhoneNO.ToLower().IndexOf(key) > -1)
                .Select(s=>new SimpleStudentData() {
                     Id=s.Id,
                     Name=s.Name,
                     IdCardNO=s.IdCardNO
                });
        }
        /// <summary>
        /// 根据筛选器筛选学生
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<ViewModels.PageResult<Student>> GetStudentsByFilter(FilterIds filter, string key,int pageSize, int page, string order, bool asc)
        {

            ViewModels.PageResult<Student> result = new ViewModels.PageResult<Student>();
            var students = db.Students.Include(s => s.Signer);
            if (key == "" || key == null)
            {
                students= students.Where(s => filter.postIds.Contains(s.Signer.PostId));
            }else
            {
                students = students.Where(s => s.IdCardNO == key || s.Name == key || s.QQ == key || s.MobilePhoneNO == key);
            }
                            
                            
            if (students.Count()==0)
            {
                result.Data = null;
                result.Count = 0;
                result.CurrentPage = 1;
                result.Order = order;
                result.IsAsc = asc;

                return result;
            }
            if (filter.gradeIds.Count > 0)
            {
                students = students.Where(s => filter.gradeIds.Contains(s.Grade));
            }
            if (filter.introIds.Count > 0)
            {
                students = students.Where(s => filter.introIds.Contains(s.Signer.UserId));
            }
            if (filter.majorIds.Count > 0)
            {
                students = students.Where(s => filter.majorIds.Contains((int)s.Major));
            }
            if (filter.eduIds.Count > 0)
            {
                students = students.Where(s => filter.eduIds.Contains((int)s.Education));
            }
            if (filter.schoolIds.Count > 0)
            {
                students = students.Where(s => filter.schoolIds.Contains(s.SchoolID));
            }
            result.Count = students.Count();
            var stu = asc ? LinqOrder.DataSort(students, order, "asc") : LinqOrder.DataSort(students, order, "desc");
            result.Data =await stu.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            result.CurrentPage = page;
            result.Order = order;
            result.IsAsc = asc;
            result.PageSize = pageSize;
            return result;
        }

        /// <summary>
        /// 根据postid查询筛选标签
        /// </summary>
        /// <param name="postId">岗位id</param>
        /// <returns></returns>
        public async Task<StudentFilter> GetFilterByPostIds(int[] idList)
        {

            var students = GetStudentsByPostIds(idList);
            StudentFilter filter = new StudentFilter();
            filter.schools =await students.Select(s => new SchoolModel()
            {
                id = s.SchoolID,
                schoolName = s.School.SchoolName,
                selected = false
            }).Distinct().ToListAsync();
            filter.grades = await students.Select(s => new GradeModel()
            {
                id = s.Grade,
                gradeName = s.Grade + "级",
                selected = false
            }).Distinct().ToListAsync();
            filter.majors = await students.Select(s => new MajorModel()
            {
                id = (int)s.Major,
                majorName = s.Major.ToString(),
                selected = false
            }).Distinct().ToListAsync();
            filter.educations = await students.Select(s => new EducationModel()
            {
                id = (int)s.Education,
                educationName = s.Education.ToString(),
                selected = false
            }).Distinct().ToListAsync();
            filter.intros = await students.Select(s => 
            new IntroModel()
            {
                id = s.Signer.UserId,
                introName = s.Signer.User.Name,
                selected = false
            }).Distinct().ToListAsync();
            return filter;
        }
        /// <summary>
        /// 更新学生
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        public async Task<int> Update(Student student)
        {
            db.Entry(student).State = EntityState.Modified;
            return await db.SaveChangesAsync();
        }
        /// <summary>
        /// 增加学生
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        public async Task<Student> Add(Student student)
        {
            var stu = await db.Students.Where(s => s.IdCardNO == student.IdCardNO).FirstOrDefaultAsync();
            if (stu != null)
            {
                return null;
            }
            db.Students.Add(student);
            await db.SaveChangesAsync();
            return student;
        }
        /// <summary>
        /// 增加学生
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        public async Task<Student> modify(int id,Delta<Student> student)
        {
            var stu = await db.Students.FindAsync(id);
            if (stu == null)
            {
                return null;
            }
            student.Patch(stu);
            await db.SaveChangesAsync();
            return stu;
        }

        /// <summary>
        /// 删除学生
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Student> Delete(int id)
        {
            Student student = await db.Students.FindAsync(id);
            if (student == null)
            {
                return null;
            }
            db.Students.Remove(student);
            await db.SaveChangesAsync();
            return student;
        }
        /// <summary>
        /// 检查学生是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool StudentExists(int id)
        {
            return db.Students.Count(e => e.Id == id) > 0;
        }

        public void Dispose()
        {
            db.Dispose();
        }

        #region utils工具
        /// <summary>
        /// 根据postid返回该岗位下属的所有岗位id
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        private List<int> getPostIdsByPostId(int postId)
        {
            PostManager postManager = new PostManager(); 
            var intorIds = postManager.GetSons(postId).ToList().Select(s => s.Id).ToList();
            return intorIds;
        }
        /// <summary>
        /// 根据postid获取下属的所有指定角色的岗位id
        /// </summary>
        /// <param name="postId">岗位id</param>
        /// <param name="roleName">角色名称</param>
        /// <returns></returns>
        private IEnumerable<int> getPostIdsByPostId(int postId,string roleName)
        {
            var postManager = new PostManager();
            var intorIds = postManager.GetSons(postId).Where(s=>s.Role.Name== roleName).Select(s => s.Id);
            return intorIds;
        }

        //private IQueryable<Student> getStudent
        #endregion
    }
}