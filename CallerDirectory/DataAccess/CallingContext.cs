using CallerDirectory.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CallerDirectory.DataAccess
{
    internal class CallingContext : DbContext
    {
        private readonly IConfiguration _configuration;

        internal DbSet<CallRecord> CallRecords { get; set; }

        public CallingContext(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            string? connectionString = this._configuration.GetConnectionString("Default");

            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }
}
