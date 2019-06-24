using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConfigurationTest.SqLite
{
	public class SettingsContext : DbContext
	{
		private IConfiguration Configuration { get; }

		public SettingsContext(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			var settingsConString = Configuration.GetConnectionString("Settings");

			var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = settingsConString };
			var connectionString = connectionStringBuilder.ToString();
			var connection = new SqliteConnection(connectionString);

			optionsBuilder.UseSqlite(connection);
		}
	}
}
