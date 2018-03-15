using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Data.Entity;
using System;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace FYstudentMgr.Models
{
    public class ApplicationUserLogin : IdentityUserLogin<string> { }
    public class ApplicationUserClaim : IdentityUserClaim<string> { }
    public class ApplicationUserRole : IdentityUserRole<string> { }
    public class ApplicationRole : IdentityRole<string, ApplicationUserRole>, IRole<string>
    {
        public string Description { get; set; }
        public string Label { get; set; }

        public ApplicationRole() { }
        public ApplicationRole(string name)
            : this()
        {
            this.Name = name;
        }

        public ApplicationRole(string name, string description, string label)
            : this(name)
        {
            this.Description = description;
            this.Label = label;
        }
    }
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    //public class ApplicationUser : IdentityUser
    //{
    //    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
    //    {
    //        // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
    //        var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
    //        // Add custom user claims here
    //        return userIdentity;
    //    }
    //    public bool IsUploaImg { get; set; }
    //    public string Name { get; set; }
    //    public string Img { get; set; }
    //    public virtual ICollection<PostUser> PostUsers { get; set; }
    //}
    public class ApplicationUser : IdentityUser<string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>, IUser<string>
    {
        public async Task<ClaimsIdentity>
            GenerateUserIdentityAsync(UserManager<ApplicationUser, string> manager, string authenticationType)
        {
            var userIdentity = await manager
                .CreateIdentityAsync(this, authenticationType);
            return userIdentity;
        }

        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int User_ID { get; set; }
        public bool IsUploaImg { get; set; }
        public string Name { get; set; }
        public string Img { get; set; }
        public virtual ICollection<PostUser> PostUsers { get; set; }
    }
    //public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    //{
    //    public ApplicationDbContext()
    //        : base("DefaultConnection", throwIfV1Schema: false)
    //    {
    //    }

    //    public DbSet<Student> Students { get; set; }
    //    public DbSet<Post> Posts { get; set; }
    //    public DbSet<Enrollment> Enrollments { get; set; }
    //    public DbSet<Product> Products { get; set; }
    //    //public DbSet<Teacher> Teachers { get; set; }
    //    public DbSet<Class> Classes { get; set; }
    //    public DbSet<Category> Categorys { get; set; }
    //    public DbSet<CouponProduct> CouponProducts { get; set; }
    //    public DbSet<ClassTeacher> ClassTeachers { get; set; }
    //    public DbSet<PostUser> PostUsers { get; set; }
    //    public DbSet<District> Districts { get; set; }
    //    public DbSet<Service> Services { get; set; }
    //    public DbSet<School> Schools { get; set; }
    //    public DbSet<Campus> Campuses { get; set; }
    //    public DbSet<Spot> Spots { get; set; }
    //    public DbSet<Subject> Subjects { get; set; }
    //    public DbSet<Receipt> Receipts { get; set; }
    //    public DbSet<Order> Orders { get; set; }
    //    //public DbSet<Worker> Workers { get; set; }
    //    public DbSet<Diploma> Diplomas { get; set; }
    //    public DbSet<Coupon> Coupons { get; set; }
    //    public DbSet<CampusCoupon> CampusCoupons { get; set; }
    //    public DbSet<StudentDiploma> StudentDiplomas { get; set; }
    //    public DbSet<Compensation> Compensations { get; set; }
    //    public DbSet<SmsRecord> SmsRecords { get; set; }
    //    public DbSet<ProductService> ProductServices { get; set; }
    //    public DbSet<OrderDetail> OrderDetails { get; set; }
    //    public static ApplicationDbContext Create()
    //    {
    //        return new ApplicationDbContext();
    //    }
    //}
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicationDbContext()
            : base("ApplicationDbContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        //public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Product> Products { get; set; }
        //public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<CouponProduct> CouponProducts { get; set; }
        public DbSet<ClassTeacher> ClassTeachers { get; set; }
        public DbSet<PostUser> PostUsers { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<Campus> Campuses { get; set; }
        public DbSet<Spot> Spots { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<Order> Orders { get; set; }
        //public DbSet<Worker> Workers { get; set; }
        public DbSet<Diploma> Diplomas { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<CampusCoupon> CampusCoupons { get; set; }
        public DbSet<StudentDiploma> StudentDiplomas { get; set; }
        public DbSet<Compensation> Compensations { get; set; }
        public DbSet<SmsRecord> SmsRecords { get; set; }
        public DbSet<ProductService> ProductServices { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<ApplicationUserRole> UserRoles { get; set; }
        public DbSet<EnrollService> EnrollServices { get; set; }
        //public DbSet<ProductPackage> ProductPackages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);//加上这一句不会出现EntityType 'IdentityUserLogin' has no key defined. Define the key for this EntityType.错误


            //modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            //modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            //modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
            //modelBuilder.Entity<School>().HasMany(t => t.Colleges).WithRequired(p => p.School).WillCascadeOnDelete(false);
        }
        static ApplicationDbContext()
        {
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
            Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //public System.Data.Entity.DbSet<FYstudentMgr.Models.ApplicationRole> ApplicationRoles { get; set; }

        // public System.Data.Entity.DbSet<FYstudentMgr.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
    public class ApplicationUserStore :
       UserStore<ApplicationUser, ApplicationRole, string,
       ApplicationUserLogin, ApplicationUserRole,
       ApplicationUserClaim>, IUserStore<ApplicationUser, string>,
       IDisposable
    {
        public ApplicationUserStore()
            : this(new IdentityDbContext())
        {
            base.DisposeContext = true;
        }

        public ApplicationUserStore(DbContext context)
            : base(context)
        {
        }
    }

    public class ApplicationRoleStore
       : RoleStore<ApplicationRole, string, ApplicationUserRole>,
       IQueryableRoleStore<ApplicationRole, string>,
       IRoleStore<ApplicationRole, string>, IDisposable
    {
        public ApplicationRoleStore()
            : base(new IdentityDbContext())
        {
            base.DisposeContext = true;
        }

        public ApplicationRoleStore(DbContext context)
            : base(context)
        {
        }
    }
}