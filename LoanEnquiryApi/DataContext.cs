using LoanEnquiryApi.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LoanEnquiryApi
{
    public class DataContext : IdentityDbContext<UserEntity, UserRoleEntity, string>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        //TODO: Remove when deploy
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseMySQL("Server=mortgage-loan-enquiry.cz8giayekgdf.ap-southeast-1.rds.amazonaws.com; Port=3306; User Id=admin; Password=Wie95AnvsgC4J1FwRPll; Database=loanenquiry");
        //}

        public DbSet<BankEntity> Banks { get; set; }
        public DbSet<BankRateEntity> BankRates { get; set; }
        public DbSet<EnquiryEntity> Enquiries { get; set; }
        public DbSet<SoraRateEntity> SoraRates { get; set; }
    }
}