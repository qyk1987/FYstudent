using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FYstudentMgr.Models
{
    public enum Major
    {
        经济类, 工科类, 管理类, 理科类, 教育类, 医学类

    }
    public enum Education
    {
        高中, 专科, 本科, 研究生, 博士

    }
    public enum Nation
    {
        汉族, 蒙古族, 回族, 藏族, 维吾尔族,
        苗族, 彝族, 壮族, 布依族, 朝鲜族, 满族,
        侗族, 瑶族, 白族, 土家族, 哈尼族, 哈萨克族,
        傣族, 黎族, 僳僳族, 佤族, 畲族, 高山族, 拉祜族,
        水族, 东乡族, 纳西族, 景颇族, 柯尔克孜族, 土族, 达斡尔族,
        仫佬族, 羌族, 布朗族, 撒拉族, 毛南族, 仡佬族, 锡伯族,
        阿昌族, 普米族, 塔吉克族, 怒族, 乌孜别克族, 俄罗斯族,
        鄂温克族, 德昂族, 保安族, 裕固族, 京族, 塔塔尔族, 独龙族,
        鄂伦春族, 赫哲族, 门巴族, 珞巴族, 基诺族
    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }//学生姓名
        // [RegularExpression(@"^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{4}$", ErrorMessage = "身份证号格式不正确")]
        [Index("IdCardNOIndex", IsUnique = true)]
        public string IdCardNO { get; set; }//学生身份证号
        public string Grade { get; set; }//学生身年级
        public Nation Nation { get; set; }//民族
        public Education Education { get; set; }//学历
        public Major Major { get; set; }//学生专业
        public int SchoolID { get; set; }//学生所在学校id
        public int SignerId { get; set; }
        public DateTime SignDate { get; set; }
        public string QQ { get; set; }//学生的qq
        public string ClassName { get; set; }//学生的班级
        public string MobilePhoneNO { get; set; }//学生的手机号
        public string Province { get; set; }//省份
        public string City { get; set; }
        public string District { get; set; }
       // [RegularExpression(@"^[0|1]{21}$", ErrorMessage = "日程格式不正确")]
        public string Schedule { get; set; }//学生空余时间
        public string WorkPlace { get; set; }//学生工作单位
        public Boolean IsUploaImg { get; set; }//学生是否上传证件照
        public string CardPath { get; set; }//学生身份证照片路径
        public Boolean IsUploaCard { get; set; }//学生是否上传身份证
        public virtual School School { get; set; }
        public virtual PostUser Signer { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual ICollection<StudentDiploma> Diploms { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}