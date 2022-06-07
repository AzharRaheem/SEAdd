using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using SEAdd.Models.DomainModels;

namespace SEAdd.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [Required]
        [StringLength(15)]
        public string Cnic { get; set; }
        public string profileImgUrl { get; set; }
        public string fatherName { get; set; }
        public string address { get; set; }
        public bool? gender { get; set; } //True for Male
        public string department { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("SEAddDBConn", throwIfV1Schema: false)
        {
        }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Fee> Fees { get; set; }
        public DbSet<Board> Boards { get; set; }
        public DbSet<Qota> Qotas { get; set; }
        public DbSet<Program> Programs { get; set; }
        public DbSet<Campus> Campuses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<AdmissionDate> AdmissionDate { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<AcademicDegree> AcademicDegrees { get; set; }
        public DbSet<Academic> Academics { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<Provience> Proviences { get; set; }
        public DbSet<ProgramSelection> ProgramSelections { get; set; }
        public DbSet<TransportRoute> TransportRoutes { get; set; }
        public DbSet<Hostel> Hostels { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<RejectionMessage> RejectionMessages { get; set; }
        public DbSet<TestCategory> TestCategories { get; set; }
        public DbSet<TestType> TestTypes { get; set; }
        public DbSet<CategoryPercentage> CategoriesPercentage { get; set; }
        public DbSet<MeritCriteria> MeritCriterias { get; set; }
        public DbSet<MeritList> MeritsLists { get; set; }
        public DbSet<TestName> TestNames { get; set; }
        public DbSet<TestQuestion> TestQuestions { get; set; }
        public DbSet<DepartmentSetting> DepartmentSettings { get; set; }
        public DbSet<EntryTestSetting> EntryTestSettings { get; set; }
        public DbSet<RollNumber> RollNumbers { get; set; }
        public DbSet<TransferApplicationForm> TransferApplicationForms { get; set; }
        public DbSet<ApplicantQualificationProgram> ApplicantQualificationPrograms { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}