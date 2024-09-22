using Leaderboard.Filter;
using Leaderboard.Leaderboard;
using Leaderboard.Leaderboard.Imp;
using Leaderboard.Model;
using System.Collections.Concurrent;

namespace Leaderboard
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();

            // add leaderboard service; 20 is max level,comes from log2(1000000) about 19.93
            var leaderbordSkip = new SkipList<Customer>(20);
            var customer = new ConcurrentDictionary<long, Customer>();

            builder.Services.AddSingleton(leaderbordSkip);
            builder.Services.AddSingleton(customer);
            builder.Services.AddTransient<ILeaderboard, SkipListLeaderboard>();
            // add filter
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<IActionErrorFilter>();
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
