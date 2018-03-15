namespace FYstudentMgr.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using FYstudentMgr.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            //var diploms = new List<Diploma> {
            //    new Diploma{ DiplomaName="会计初级职称资格证", DiplomaState=true, CreateDate=DateTime.Parse("2017-09-12")},
            //    new Diploma{ DiplomaName="计算机二级Office", DiplomaState=true, CreateDate=DateTime.Parse("2017-09-13")},
            //    new Diploma{ DiplomaName="计算机二级C语言", DiplomaState=true, CreateDate=DateTime.Parse("2017-09-14")},
            //    new Diploma{ DiplomaName="17继续教育", DiplomaState=true, CreateDate=DateTime.Parse("2017-09-15")}
            //};
            //diploms.ForEach(s => context.Diplomas.AddOrUpdate(p => p.DiplomaName, s));
            //context.SaveChanges();


            //var teachers = new List<Worker> {
            //    new Worker{ Name="范芬", MobilePhoneNO="15307000257"},
            //    new Worker{ Name="小汇演", MobilePhoneNO="15307000257", ParentID=1},
            //    new Worker{ Name="陈文君", MobilePhoneNO="15307000257",ParentID=2},
            //    new Worker{ Name="彭玉亮", MobilePhoneNO="15307000257"}

            //};
            //teachers.ForEach(s => context.Workers.AddOrUpdate(p => p.Name, s));
            //context.SaveChanges();
            //var districts = new List<District> {
            //    new District{ DistrictName="南昌区", DistrictAddress="南昌市枫林大街970号" ,  CreateDate=DateTime.Now, DistrictState=true}



            //};
            //districts.ForEach(s => context.Districts.AddOrUpdate(p => p.DistrictName, s));
            //context.SaveChanges();

            //var campuses = new List<Campus> {
            //    new Campus{  DistrictID=1, CampusName="昌北校区", CreateDate=DateTime.Now, CampusAddress="枫林大街", CampusState=true},
            //    new Campus{  DistrictID=1, CampusName="南工校区",  CreateDate=DateTime.Now, CampusAddress="南昌工学院", CampusState=true}


            //};
            //campuses.ForEach(s => context.Campuses.AddOrUpdate(p => p.CampusName, s));
            //context.SaveChanges();

            //var spots = new List<Spot> {
            //    new Spot{  CampusID=1, SpotName="世纪新宸服务点", CreateDate=DateTime.Now, SpotState=true,SpotAddress="枫林大街世纪新宸中心"},
            //    new Spot{  CampusID=1, SpotName="蛟桥服务点",  CreateDate=DateTime.Now, SpotState=true,SpotAddress="江西财经大学创孵中心106"},
            //    new Spot{  CampusID=2, SpotName="南工服务点",  CreateDate=DateTime.Now, SpotState=true,SpotAddress="南昌工学院老商业街"},


            //};
            //spots.ForEach(s => context.Spots.AddOrUpdate(p => p.SpotName, s));
            //context.SaveChanges();

            //var categorys = new List<Category> {
            //    new Category{ CategoryName ="计算机等级考试", State=true,  Sort=1 },
            //    new Category{ CategoryName ="会计初级", State=true,  Sort=2},
            //    new Category{ CategoryName ="继续教育", State=true,  Sort=3},


            //};
            //categorys.ForEach(s => context.Categorys.AddOrUpdate(p => p.CategoryName, s));
            //context.SaveChanges();

            //var subjects = new List<Subject> {
            //    new Subject{ Name ="初级培训班", State=true, CategoryId=2, Sort=1 , CreateTime=DateTime.Now},
            //    new Subject{ Name ="计算机二级培训", State=true, CategoryId=1, Sort=1,CreateTime=DateTime.Now},
            //    new Subject{ Name ="会计继续教育", State=true, CategoryId=3, Sort=1,CreateTime=DateTime.Now}


            //};
            //subjects.ForEach(s => context.Subjects.AddOrUpdate(p => p.Name, s));
            //context.SaveChanges();

            //var courses = new List<Product> {
            //     new Product{ProductName="18初级会计实务名师班（单科）", State=true, CreateDate=DateTime.Parse("2017-09-20"),
            //        OverDate =DateTime.Parse("2018-03-20"),Price=700,  SubjectId=1,IsPackage=false, IsDiscountForOld=false,
            //        Sort =1, SaleCount=0, CoverImg="111111.jpg", Desc="产品描述"},
            //    new Product{ProductName="18经济法名师班（单科）", State=true, CreateDate=DateTime.Parse("2017-09-20"),
            //        OverDate =DateTime.Parse("2018-03-20"),Price=700,  SubjectId=1,IsPackage=false, IsDiscountForOld=false,
            //        Sort =2, SaleCount=0, CoverImg="111111.jpg", Desc="产品描述"},
            //    new Product{ProductName="18初级会计名师班", State=true, CreateDate=DateTime.Parse("2017-09-20"),
            //        OverDate =DateTime.Parse("2018-03-20"),Price=1280,  SubjectId=1,IsPackage=true,PackageIdList="1-2" , IsDiscountForOld=false,
            //        Sort =3, SaleCount=0, CoverImg="111111.jpg", Desc="产品描述"},
            //    new Product{ProductName="18计算机二级Office精讲班",State=true, CreateDate=DateTime.Parse("2017-09-20"),
            //        OverDate =DateTime.Parse("2018-03-20"),Price=480,SubjectId=2,IsPackage=false,  IsDiscountForOld=false, Sort=1,
            //        SaleCount =0,CoverImg="222222.jpg", Desc="产品描述"},
            //    new Product{ProductName="18计算机二级报考",State=true, CreateDate=DateTime.Parse("2017-09-20"),
            //        OverDate =DateTime.Parse("2018-03-20"),Price=480,SubjectId=2, IsPackage=false,  IsDiscountForOld=false, Sort=2,
            //        SaleCount =0,CoverImg="222222.jpg", Desc="产品描述"},
            //    new Product{ProductName="18年继续教育", State=true, CreateDate=DateTime.Parse("2017-09-20"),
            //        OverDate =DateTime.Parse("2018-03-20"),Price=120,SubjectId=3, IsPackage=false,  IsDiscountForOld=true, Sort=1,
            //        DiscountValue =10, AccordIdList="1,2,3",SaleCount=0,CoverImg="333333.jpg", Desc="产品描述"}

            //};
            //courses.ForEach(s => context.Products.AddOrUpdate(p => p.ProductName, s));
            //context.SaveChanges();

            //var coupons = new List<Coupon> {
            //    new Coupon{ CouponName="限时优惠200元", Rule="限定时间前享受优惠", Vlaue=200, State=true, StartDate=DateTime.Now,OverDate=DateTime.Parse("2018-05-20")},
            //   new Coupon{ CouponName="限时优惠280元", Rule="限定时间前享受优惠", Vlaue=280, State=true, StartDate=DateTime.Now,OverDate=DateTime.Parse("2018-05-20")},
            //   new Coupon{ CouponName="限时优惠50元", Rule="限定时间前享受优惠", Vlaue=50, State=true, StartDate=DateTime.Now,OverDate=DateTime.Parse("2018-05-20")},
            //   new Coupon{ CouponName="限时优惠20元", Rule="限定时间前享受优惠", Vlaue=20, State=true, StartDate=DateTime.Now,OverDate=DateTime.Parse("2018-05-20")}
            //};
            //coupons.ForEach(s => context.Coupons.AddOrUpdate(p => p.CouponName, s));
            //context.SaveChanges();

            //var campusCoupons = new List<CampusCoupon> {
            //    new CampusCoupon{ CampusID=1, CouponID=1 },
            //    new CampusCoupon{ CampusID=2, CouponID=2 },
            //    new CampusCoupon{ CampusID=1, CouponID=3 },
            //    new CampusCoupon{ CampusID=1, CouponID=4},
            //    new CampusCoupon{ CampusID=2, CouponID=3 },
            //    new CampusCoupon{ CampusID=2, CouponID=4}
            //};
            //foreach (CampusCoupon e in campusCoupons)
            //{
            //    var enrollmentInDataBase = context.CampusCoupons.Where(
            //          s => s.CampusID == e.CampusID && s.CouponID == e.CouponID).SingleOrDefault();
            //    if (enrollmentInDataBase == null)
            //    {
            //        context.CampusCoupons.Add(e);
            //    }
            //}
            //context.SaveChanges();



            //var packages = new List<Package> {
            //    new Package{ Name="套餐1", DiscountValue=20, CreateTime=DateTime.Now, State=true  }        
            //};
            //packages.ForEach(s => context.Packages.AddOrUpdate(p => p.Name, s));
            //context.SaveChanges();

            //var productPackages = new List<ProductPackage> {
            //    new ProductPackage{ PackageId=1,   ProductId=2 },
            //    new ProductPackage{ PackageId=1,   ProductId=3 }

            //};
            //foreach (ProductPackage e in productPackages)
            //{
            //    var enrollmentInDataBase = context.ProductPackages.Where(
            //          s => s.ProductId == e.ProductId && s.PackageId == e.PackageId).SingleOrDefault();
            //    if (enrollmentInDataBase == null)
            //    {
            //        context.ProductPackages.Add(e);
            //    }
            //}
            //context.SaveChanges();


            //var couponProducts = new List<CouponProduct> {
            //    new CouponProduct{ CouponId=1, ProductId=3 },
            //    new CouponProduct{ CouponId=2, ProductId=3 },
            //    new CouponProduct{ CouponId=3, ProductId=4 },
            //    new CouponProduct{ CouponId=4, ProductId=6 }
            //};
            //foreach (CouponProduct e in couponProducts)
            //{
            //    var enrollmentInDataBase = context.CouponProducts.Where(
            //          s => s.ProductId == e.ProductId && s.CouponId == e.CouponId).SingleOrDefault();
            //    if (enrollmentInDataBase == null)
            //    {
            //        context.CouponProducts.Add(e);
            //    }
            //}
            //context.SaveChanges();

            //var coupons = new List<Coupon> { 
            //    new Coupon{ CouponName="计算机学院课程折上立减30元",  OverDate=DateTime.Parse("2017-06-30"), Rule="适用平台：PC、触屏、App <br>1. 每单限用一张，一次性使用，不找零。<br>2. 不可与其它优惠券叠加使用。<br>3. 本券不能与其它优惠活动同享，不可用于预售课程。<br>4. 如报班时未使用本券，则视作自动放弃。<br>5. 本券不可退换，超过有效时间将无法使用。<br>6. 本优惠券与账户绑定，不可转让。", Type=CouponType.折上减, Vlaue=30, State=true},
            //    new Coupon{ CouponName="计算机学院课程折上立减60元", OverDate=DateTime.Parse("2017-06-30"), Rule="适用平台：PC、触屏、App <br>1. 每单限用一张，一次性使用，不找零。<br>2. 不可与其它优惠券叠加使用。<br>3. 本券不能与其它优惠活动同享，不可用于预售课程。<br>4. 如报班时未使用本券，则视作自动放弃。<br>5. 本券不可退换，超过有效时间将无法使用。<br>6. 本优惠券与账户绑定，不可转让。", Type=CouponType.折上减, Vlaue=60, State=true},

            //    new Coupon{ CouponName="会计学院课程折上立减60元", OverDate=DateTime.Parse("2017-06-30"), Rule="适用平台：PC、触屏、App <br>1. 每单限用一张，一次性使用，不找零。<br>2. 不可与其它优惠券叠加使用。<br>3. 本券不能与其它优惠活动同享，不可用于预售课程。<br>4. 如报班时未使用本券，则视作自动放弃。<br>5. 本券不可退换，超过有效时间将无法使用。<br>6. 本优惠券与账户绑定，不可转让。", Type=CouponType.折上减, Vlaue=60, State=true},
            //    new Coupon{ CouponName="会计学院课程折上立减50元",  OverDate=DateTime.Parse("2017-06-30"), Rule="适用平台：PC、触屏、App <br>1. 每单限用一张，一次性使用，不找零。<br>2. 不可与其它优惠券叠加使用。<br>3. 本券不能与其它优惠活动同享，不可用于预售课程。<br>4. 如报班时未使用本券，则视作自动放弃。<br>5. 本券不可退换，超过有效时间将无法使用。<br>6. 本优惠券与账户绑定，不可转让。", Type=CouponType.折上减, Vlaue=50, State=true},
            //    new Coupon{ CouponName="会计学院课程折上立减108元", OverDate=DateTime.Parse("2017-06-22"), Rule="适用平台：PC、触屏、App <br>1. 每单限用一张，一次性使用，不找零。<br>2. 不可与其它优惠券叠加使用。<br>3. 本券不能与其它优惠活动同享，不可用于预售课程。<br>4. 如报班时未使用本券，则视作自动放弃。<br>5. 本券不可退换，超过有效时间将无法使用。<br>6. 本优惠券与账户绑定，不可转让。", Type=CouponType.折上减, Vlaue=108, State=true}

            //};
            //coupons.ForEach(s => context.Coupons.Add(s));
            //context.SaveChanges();




            //var qusBanks = new List<QusBank> { 
            //    new QusBank{ProductID=1,BankName="二级C语言笔试真题题库", BankDescription="该题库包含历年考试笔试部分所有真题"},
            //    new QusBank{ProductID=2,BankName="二级Office笔试真题题库", BankDescription="该题库包含历年考试笔试部分所有真题"}
            //};
            //qusBanks.ForEach(s => context.QusBanks.Add(s));
            //context.SaveChanges();

            //var chapters = new List<Chapter> { 
            //    new Chapter{ ChapterName="数据类型及其运算", Sort=1, QusBankID=1},
            //    new Chapter{ ChapterName="输入与输出", Sort=2,  QusBankID=1},
            //    new Chapter{ ChapterName="选择结构", Sort=3,  QusBankID=1}

            //};
            //chapters.ForEach(s => context.Chapters.Add(s));
            //context.SaveChanges();

            //var qus = new List<Question> { 
            //    new Question{ ChapterID=1, Sort=1,   Category=QuestionCategory.单选题, QuestionBody="以下不是整型常量的是", TextAnalysis="这是题目解析"},
            //    new Question{ ChapterID=1, Sort=2, Category=QuestionCategory.单选题,  QuestionBody="以下不是实型常量的是", TextAnalysis="这是题目解析"},
            //    new Question{ ChapterID=2, Sort=1, Category=QuestionCategory.单选题,  QuestionBody="在C语言中，语句结束的标志是？ ______ 。", TextAnalysis="C语言中语句结束的标志是分号。"},
            //    new Question{ ChapterID=2,Sort=2,Category=QuestionCategory.单选题,  QuestionBody="全国计算机等级考试无纸化“通过”的条件是 ______ 。", TextAnalysis="通过条件是：只需总分高于60分即可。"},
            //     new Question{ ChapterID=3,Sort=1,Category=QuestionCategory.单选题,  QuestionBody="若变量x、y已正确定义并赋值，以下符合C语言语法的表达式是", TextAnalysis="赋值运算符“=”的左边必须是变量"},
            //    new Question{  ChapterID=3,Sort=2,Category=QuestionCategory.单选题,  QuestionBody="以下定义语句中正确的是", TextAnalysis="变量必须先定义，后使用"}



            //};
            //qus.ForEach(s => context.Questions.Add(s));
            //context.SaveChanges();


            //var qusoptions = new List<QusOption> { 
            //    new QusOption{  QuestionID=1, Content="12", IsCorrect=false},
            //    new QusOption{ QuestionID=1, Content="011", IsCorrect=false},
            //    new QusOption{ QuestionID=1, Content="0x12", IsCorrect=false},
            //    new QusOption{ QuestionID=1, Content="018", IsCorrect=true},
            //    new QusOption{ QuestionID=2, Content="12.1", IsCorrect=false},
            //    new QusOption{ QuestionID=2, Content="12e2", IsCorrect=false},
            //    new QusOption{ QuestionID=2, Content="12e0.2", IsCorrect=true},
            //    new QusOption{ QuestionID=2, Content="12.0", IsCorrect=false},
            //    new QusOption{ QuestionID=3, Content="分号“;”", IsCorrect=true},
            //    new QusOption{ QuestionID=3, Content="换行（回车）", IsCorrect=false},
            //    new QusOption{ QuestionID=4, Content="只需总分高于60分即可", IsCorrect=true},
            //    new QusOption{ QuestionID=4, Content="总分高于60分，同时操作题高于36分", IsCorrect=false},
            //    new QusOption{ QuestionID=5, Content="++x,y=x--", IsCorrect=true},
            //    new QusOption{ QuestionID=5, Content="x+1=y", IsCorrect=false},
            //    new QusOption{ QuestionID=5, Content="x=x+10=x+y", IsCorrect=false},
            //    new QusOption{ QuestionID=5, Content="double(x)/10", IsCorrect=false},
            //    new QusOption{ QuestionID=6, Content="char  A=65+1,b='b';", IsCorrect=true},
            //    new QusOption{ QuestionID=6, Content="int  a=b=0;", IsCorrect=false},
            //    new QusOption{ QuestionID=6, Content="float  a=1,*b=&a,*c=&b;", IsCorrect=false},
            //    new QusOption{ QuestionID=6, Content="double  a=0.0; b=1.1;", IsCorrect=false}

            //};
            //qusoptions.ForEach(s => context.QusOptions.Add(s));
            //context.SaveChanges();




            //if (!context.Roles.Any(r => r.Name == "Admin"))
            //{
            //    var store = new ApplicationRoleStore(context);
            //    var manager = new ApplicationRoleManager(store);
            //    var role = new ApplicationRole {Id=Guid.NewGuid().ToString(), Name = "Admin", Label = "管理员", Description = "负责对系统基础数据进行维护以及系统运行维护" };

            //    manager.Create(role);
            //}
            //if (!context.Roles.Any(r => r.Name == "Student"))
            //{
            //    var store = new ApplicationRoleStore(context);
            //    var manager = new ApplicationRoleManager(store);
            //    var role = new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "Student", Label = "学生", Description = "在本系统登记的有学习意向的学生" };

            //    manager.Create(role);
            //}
            //if (!context.Roles.Any(r => r.Name == "Teacher"))
            //{
            //    var store = new ApplicationRoleStore(context);
            //    var manager = new ApplicationRoleManager(store);
            //    var role = new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "Teacher", Label = "教师", Description = "负责教学工作" };

            //    manager.Create(role);
            //}
            //if (!context.Roles.Any(r => r.Name == "Seller"))
            //{
            //    var store = new ApplicationRoleStore(context);
            //    var manager = new ApplicationRoleManager(store);
            //    var role = new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "Seller", Label = "课程顾问", Description = "负责招生工作的一线业务人员" };

            //    manager.Create(role);
            //}
            //if (!context.Roles.Any(r => r.Name == "Manager"))
            //{
            //    var store = new ApplicationRoleStore(context);
            //    var manager = new ApplicationRoleManager(store);
            //    var role = new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "Manager", Label = "大区经理", Description = "主管一个区域的总经理" };

            //    manager.Create(role);
            //}
            //if (!context.Roles.Any(r => r.Name == " SchoolMaster"))
            //{
            //    var store = new ApplicationRoleStore(context);
            //    var manager = new ApplicationRoleManager(store);
            //    var role = new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "SchoolMaster", Label = "校长", Description = "主管一个校区的所有工作" };

            //    manager.Create(role);
            //}
            //if (!context.Roles.Any(r => r.Name == "SpotCharger"))
            //{
            //    var store = new ApplicationRoleStore(context);
            //    var manager = new ApplicationRoleManager(store);
            //    var role = new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "SpotCharger", Label = "服务点负责人", Description = "主管一个服务点的所有工作" };

            //    manager.Create(role);
            //}
            //if (!context.Roles.Any(r => r.Name == "Accounter"))
            //{
            //    var store = new ApplicationRoleStore(context);
            //    var manager = new ApplicationRoleManager(store);
            //    var role = new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "Accounter", Label = "会计", Description = "主管一个部门的所有财务工作" };

            //    manager.Create(role);
            //}
            //if (!context.Roles.Any(r => r.Name == "ClassCharger"))
            //{
            //    var store = new ApplicationRoleStore(context);
            //    var manager = new ApplicationRoleManager(store);
            //    var role = new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "ClassCharger", Label = "班主任", Description = "主管一个班级的排课考勤工作" };

            //    manager.Create(role);
            //}




            //if (!context.Users.Any(u => u.UserName == "qinyuankun"))
            //{
            //    var store = new ApplicationUserStore(context);
            //    var manager = new ApplicationUserManager(store);
            //    var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "qinyuankun", Email = "327179615@qq.com", Name = "秦元坤", Img = "avatar:svg-1" };

            //    manager.Create(user, "111aaa");
            //    manager.AddToRole(user.Id, "Admin");
            //}
            //if (!context.Users.Any(u => u.UserName == "fangyaobing"))
            //{
            //    var store = new ApplicationUserStore(context);
            //    var manager = new ApplicationUserManager(store);
            //    var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "fangyaobing", Email = "fangyaobing@qq.com", Name = "方姚兵", Img = "avatar:svg-1" };

            //    manager.Create(user, "111aaa");
            //    manager.AddToRole(user.Id, "Manager");
            //}
            //if (!context.Users.Any(u => u.UserName == "xiaohuiyan"))
            //{
            //    var store = new ApplicationUserStore(context);
            //    var manager = new ApplicationUserManager(store);
            //    var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "xiaohuiyan", Email = "xiaohuiyan@qq.com", Name = "肖辉燕", Img = "avatar:svg-1" };

            //    manager.Create(user, "111aaa");
            //    manager.AddToRole(user.Id, "SchoolMaster");
            //    manager.AddToRole(user.Id, "SpotCharger");
            //}
            //if (!context.Users.Any(u => u.UserName == "fanfen"))
            //{
            //    var store = new ApplicationUserStore(context);
            //    var manager = new ApplicationUserManager(store);
            //    var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "fanfen", Email = "fanfen@qq.com", Name = "樊芬", Img = "avatar:svg-2" };

            //    manager.Create(user, "111aaa");
            //    manager.AddToRole(user.Id, "SpotCharger");
            //}

            //if (!context.Users.Any(u => u.UserName == "chenwenjun"))
            //{
            //    var store = new ApplicationUserStore(context);
            //    var manager = new ApplicationUserManager(store);
            //    var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "chenwenjun", Email = "chenwenjun@qq.com", Name = "陈文君", Img = "avatar:svg-3" };

            //    manager.Create(user, "111aaa");
            //    manager.AddToRole(user.Id, "SchoolMaster");
            //    manager.AddToRole(user.Id, "SpotCharger");
            //}

            //if (!context.Users.Any(u => u.UserName == "huangjia"))
            //{
            //    var store = new ApplicationUserStore(context);
            //    var manager = new ApplicationUserManager(store);
            //    var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "huangjia", Email = "huangjia@qq.com", Name = "黄佳", Img = "avatar:svg-4" };

            //    manager.Create(user, "111aaa");
            //    manager.AddToRole(user.Id, "Seller");
            //}

            //if (!context.Users.Any(u => u.UserName == "zengchunling"))
            //{
            //    var store = new ApplicationUserStore(context);
            //    var manager = new ApplicationUserManager(store);
            //    var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "zengchunling", Email = "zengchunling@qq.com", Name = "曾春龄", Img = "avatar:svg-5" };

            //    manager.Create(user, "111aaa");
            //    manager.AddToRole(user.Id, "Seller");
            //}
            //if (!context.Users.Any(u => u.UserName == "wangsirui"))
            //{
            //    var store = new ApplicationUserStore(context);
            //    var manager = new ApplicationUserManager(store);
            //    var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "wangsirui", Email = "wangsirui@qq.com", Name = "王思睿", Img = "avatar:svg-6" };

            //    manager.Create(user, "111aaa");
            //    manager.AddToRole(user.Id, "Accounter");
            //}

            //if (!context.Users.Any(u => u.UserName == "zhonghuali"))
            //{
            //    var store = new ApplicationUserStore(context);
            //    var manager = new ApplicationUserManager(store);
            //    var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "zhonghuali", Email = "zhonghuali@qq.com", Name = "钟华丽", Img = "avatar:svg-7" };

            //    manager.Create(user, "111aaa");
            //    manager.AddToRole(user.Id, "Accounter");
            //}

            //if (!context.Users.Any(u => u.UserName == "zoushu"))
            //{
            //    var store = new ApplicationUserStore(context);
            //    var manager = new ApplicationUserManager(store);
            //    var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "zoushu", Email = "zoushu@qq.com", Name = "邹树", Img = "avatar:svg-8" };

            //    manager.Create(user, "111aaa");
            //    manager.AddToRole(user.Id, "Seller");
            //}

            //if (!context.Users.Any(u => u.UserName == "daidi"))
            //{
            //    var store = new ApplicationUserStore(context);
            //    var manager = new ApplicationUserManager(store);
            //    var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "daidi", Email = "daidi@qq.com", Name = "代娣", Img = "avatar:svg-8" };

            //    manager.Create(user, "111aaa");
            //    manager.AddToRole(user.Id, "Seller");
            //    manager.AddToRole(user.Id, "ClassCharger");
            //}
            //if (!context.Users.Any(u => u.UserName == "wangguilian"))
            //{
            //    var store = new ApplicationUserStore(context);
            //    var manager = new ApplicationUserManager(store);
            //    var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "wangguilian", Email = "wangguilian@qq.com", Name = "王桂莲", Img = "avatar:svg-9" };

            //    manager.Create(user, "111aaa");
            //    manager.AddToRole(user.Id, "Seller");
            //    manager.AddToRole(user.Id, "ClassCharger");
            //}

            //var posts1 = new List<Post> {
            //    new Post{  RoleId=context.Roles.FirstOrDefault(r=>r.Name=="Admin").Id, UserId=context.Users.FirstOrDefault(u=>u.UserName=="qinyuankun").Id, CreateTime=DateTime.Now, PostName="01号管理员", State=true,DistrictId=1},

            //};
            //foreach (Post e in posts1)
            //{
            //    var enrollmentInDataBase = context.Posts.Where(
            //          s => s.RoleId == e.RoleId && s.UserId == e.UserId).SingleOrDefault();
            //    if (enrollmentInDataBase == null)
            //    {
            //        context.Posts.Add(e);
            //    }
            //}
            //context.SaveChanges();

            //var postUsers1 = new List<PostUser> {
            //    new PostUser{  PostId=1, UserId=context.Users.FirstOrDefault(u=>u.UserName=="qinyuankun").Id,PostDate=DateTime.Now},

            //};
            //foreach (PostUser e in postUsers1)
            //{
            //    var enrollmentInDataBase = context.PostUsers.Where(
            //          s => s.PostId == e.PostId && s.UserId == e.UserId).SingleOrDefault();
            //    if (enrollmentInDataBase == null)
            //    {
            //        context.PostUsers.Add(e);
            //    }
            //}
            //context.SaveChanges();

            //var posts2 = new List<Post> {
            //    new Post{   RoleId=context.Roles.FirstOrDefault(r=>r.Name=="Manager").Id, UserId=context.Users.FirstOrDefault(u=>u.UserName=="fangyaobing").Id, CreaterId=1, CreateTime=DateTime.Now, PostName="01大区经理",SupperId=1, State=true,DistrictId=1,},

            //};
            //foreach (Post e in posts2)
            //{
            //    var enrollmentInDataBase = context.Posts.Where(
            //          s => s.RoleId == e.RoleId && s.UserId == e.UserId).SingleOrDefault();
            //    if (enrollmentInDataBase == null)
            //    {
            //        context.Posts.Add(e);
            //    }
            //}
            //context.SaveChanges();

            //var postUsers12 = new List<PostUser> {
            //   new PostUser{  PostId=2, UserId=context.Users.FirstOrDefault(u=>u.UserName=="fangyaobing").Id,PostDate=DateTime.Now, PosterID=1},

            //};
            //foreach (PostUser e in postUsers12)
            //{
            //    var enrollmentInDataBase = context.PostUsers.Where(
            //          s => s.PostId == e.PostId && s.UserId == e.UserId).SingleOrDefault();
            //    if (enrollmentInDataBase == null)
            //    {
            //        context.PostUsers.Add(e);
            //    }
            //}
            //context.SaveChanges();


            //var posts3 = new List<Post> {
            //    new Post{ RoleId=context.Roles.FirstOrDefault(r=>r.Name=="SchoolMaster").Id, UserId=context.Users.FirstOrDefault(u=>u.UserName=="xiaohuiyan").Id, CreaterId=2, CreateTime=DateTime.Now, PostName="01校区校长", State=true,DistrictId=1,CampusId=1,SupperId=2},
            //    new Post{  RoleId=context.Roles.FirstOrDefault(r=>r.Name=="SpotCharger").Id, UserId=context.Users.FirstOrDefault(u=>u.UserName=="xiaohuiyan").Id, CreaterId=2, CreateTime=DateTime.Now, PostName="01服务点负责人", State=true,DistrictId=1,CampusId=1,SpotId=1,SupperId=3},
            //    new Post{  RoleId=context.Roles.FirstOrDefault(r=>r.Name=="SchoolMaster").Id, UserId=context.Users.FirstOrDefault(u=>u.UserName=="chenwenjun").Id, CreaterId=2, CreateTime=DateTime.Now, PostName="02校区校长", State=true,DistrictId=1,CampusId=2,SupperId=2},
            //    new Post{ RoleId=context.Roles.FirstOrDefault(r=>r.Name=="SpotCharger").Id, UserId=context.Users.FirstOrDefault(u=>u.UserName=="chenwenjun").Id, CreaterId=2, CreateTime=DateTime.Now, PostName="03服务点负责人", State=true,DistrictId=1,CampusId=2,SpotId=3,SupperId=5},
            //    new Post{  RoleId=context.Roles.FirstOrDefault(r=>r.Name=="SpotCharger").Id, UserId=context.Users.FirstOrDefault(u=>u.UserName=="fanfen").Id, CreaterId=2, CreateTime=DateTime.Now, PostName="02服务点负责人", State=true,DistrictId=1,CampusId=1,SpotId=2,SupperId=3},
            //    new Post{ RoleId=context.Roles.FirstOrDefault(r=>r.Name=="Accounter").Id, UserId=context.Users.FirstOrDefault(u=>u.UserName=="wangsirui").Id, CreaterId=2, CreateTime=DateTime.Now, PostName="01号会计", State=true,DistrictId=1,CampusId=1,SupperId=2},
            //    new Post{  RoleId=context.Roles.FirstOrDefault(r=>r.Name=="Accounter").Id, UserId=context.Users.FirstOrDefault(u=>u.UserName=="zhonghuali").Id, CreaterId=2, CreateTime=DateTime.Now, PostName="02号会计", State=true,DistrictId=1,CampusId=2,SupperId=2},
            //    new Post{  RoleId=context.Roles.FirstOrDefault(r=>r.Name=="Seller").Id, UserId=context.Users.FirstOrDefault(u=>u.UserName=="huangjia").Id, CreaterId=2, CreateTime=DateTime.Now, PostName="01课程顾问", State=true,DistrictId=1,CampusId=1,SpotId=1,SupperId=4},
            //    new Post{   RoleId=context.Roles.FirstOrDefault(r=>r.Name=="Seller").Id, UserId=context.Users.FirstOrDefault(u=>u.UserName=="zengchunling").Id,  CreaterId=2, CreateTime=DateTime.Now, PostName="02课程顾问", State=true,DistrictId=1,CampusId=1,SpotId=1,SupperId=4},
            //    new Post{  RoleId=context.Roles.FirstOrDefault(r=>r.Name=="Seller").Id, UserId=context.Users.FirstOrDefault(u=>u.UserName=="zoushu").Id, CreaterId=2, CreateTime=DateTime.Now, PostName="03课程顾问", State=true,DistrictId=1,CampusId=1,SpotId=2,SupperId=7},
            //    new Post{   RoleId=context.Roles.FirstOrDefault(r=>r.Name=="Seller").Id, UserId=context.Users.FirstOrDefault(u=>u.UserName=="daidi").Id, CreaterId=2, CreateTime=DateTime.Now, PostName="04课程顾问", State=true,DistrictId=1,CampusId=1,SpotId=2,SupperId=7},
            //    new Post{  RoleId=context.Roles.FirstOrDefault(r=>r.Name=="Seller").Id, UserId=context.Users.FirstOrDefault(u=>u.UserName=="wangguilian").Id, CreaterId=2, CreateTime=DateTime.Now, PostName="05课程顾问", State=true,DistrictId=1,CampusId=2,SpotId=3,SupperId=6},
            //    new Post{  RoleId=context.Roles.FirstOrDefault(r=>r.Name=="ClassCharger").Id, UserId=context.Users.FirstOrDefault(u=>u.UserName=="daidi").Id, CreaterId=2, CreateTime=DateTime.Now, PostName="01班主任", State=true,DistrictId=1,CampusId=1,SupperId=3},
            //    new Post{  RoleId=context.Roles.FirstOrDefault(r=>r.Name=="ClassCharger").Id, UserId=context.Users.FirstOrDefault(u=>u.UserName=="wangguilian").Id, CreaterId=2, CreateTime=DateTime.Now, PostName="02班主任", State=true,DistrictId=1,CampusId=2,SupperId=5}
            //};
            //foreach (Post e in posts3)
            //{
            //    var enrollmentInDataBase = context.Posts.Where(
            //          s => s.RoleId == e.RoleId && s.UserId == e.UserId).SingleOrDefault();
            //    if (enrollmentInDataBase == null)
            //    {
            //        context.Posts.Add(e);
            //    }
            //}
            //context.SaveChanges();

            //var postUsers3 = new List<PostUser> {

            //    new PostUser{  PostId=3,  UserId=context.Users.FirstOrDefault(u=>u.UserName=="xiaohuiyan").Id,PostDate=DateTime.Now, PosterID=2,IsOnDuty=true},
            //    new PostUser{  PostId=4,  UserId=context.Users.FirstOrDefault(u=>u.UserName=="xiaohuiyan").Id,PostDate=DateTime.Now, PosterID=2,IsOnDuty=true},
            //    new PostUser{  PostId=5, UserId=context.Users.FirstOrDefault(u=>u.UserName=="chenwenjun").Id,PostDate=DateTime.Now, PosterID=2,IsOnDuty=true},
            //    new PostUser{  PostId=6, UserId=context.Users.FirstOrDefault(u=>u.UserName=="chenwenjun").Id,PostDate=DateTime.Now, PosterID=2,IsOnDuty=true},
            //    new PostUser{  PostId=7, UserId=context.Users.FirstOrDefault(u=>u.UserName=="fanfen").Id,PostDate=DateTime.Now, PosterID=2,IsOnDuty=true},
            //    new PostUser{  PostId=8,UserId=context.Users.FirstOrDefault(u=>u.UserName=="wangsirui").Id,PostDate=DateTime.Now, PosterID=2,IsOnDuty=true},
            //    new PostUser{  PostId=9, UserId=context.Users.FirstOrDefault(u=>u.UserName=="zhonghuali").Id,PostDate=DateTime.Now, PosterID=2,IsOnDuty=true},
            //    new PostUser{  PostId=10, UserId=context.Users.FirstOrDefault(u=>u.UserName=="huangjia").Id,PostDate=DateTime.Now, PosterID=2,IsOnDuty=true},
            //    new PostUser{  PostId=11, UserId=context.Users.FirstOrDefault(u=>u.UserName=="zengchunling").Id, PostDate=DateTime.Now, PosterID=2,IsOnDuty=true},
            //    new PostUser{  PostId=12,UserId=context.Users.FirstOrDefault(u=>u.UserName=="daidi").Id,PostDate=DateTime.Now, PosterID=2,IsOnDuty=true},
            //    new PostUser{  PostId=13,UserId=context.Users.FirstOrDefault(u=>u.UserName=="daidi").Id, PostDate=DateTime.Now, PosterID=2,IsOnDuty=true},
            //    new PostUser{  PostId=14, UserId=context.Users.FirstOrDefault(u=>u.UserName=="wangguilian").Id,PostDate=DateTime.Now, PosterID=2,IsOnDuty=true},
            //    new PostUser{  PostId=15, UserId=context.Users.FirstOrDefault(u=>u.UserName=="daidi").Id, PostDate=DateTime.Now, PosterID=2,IsOnDuty=true},
            //    new PostUser{  PostId=16, UserId=context.Users.FirstOrDefault(u=>u.UserName=="wangguilian").Id,PostDate=DateTime.Now, PosterID=2,IsOnDuty=true}
            //};
            //foreach (PostUser e in postUsers3)
            //{
            //    var enrollmentInDataBase = context.PostUsers.Where(
            //          s => s.PostId == e.PostId && s.UserId == e.UserId && s.IsOnDuty == e.IsOnDuty).SingleOrDefault();
            //    if (enrollmentInDataBase == null)
            //    {
            //        context.PostUsers.Add(e);
            //    }
            //}
            //context.SaveChanges();

            
        }
    }
}
