using Microsoft.EntityFrameworkCore;

namespace OpenTelemetry.Api.Data;

public class ApiDbContext : DbContext
{
	public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
	{

	}


	public DbSet<WeatherLog> WeatherLogs => Set<WeatherLog>();


	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		builder.Entity<WeatherLog>()
			.HasKey(q => q.WeatherLogId);
	}
}