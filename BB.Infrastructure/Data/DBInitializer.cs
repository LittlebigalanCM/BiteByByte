using BB.Application.Interfaces;
using BB.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace BB.Infrastructure.Data.DbInitializer

{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;

        public DbInitializer(ApplicationDbContext db)
        {
            _db = db;
        }


        public void Initialize()
        {
            _db.Database.EnsureCreated();

            //migrations if they are not applied
            try
            {
                if (_db.Database.GetPendingMigrations().Any())
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception)
            {

            }

            if (_db.Categories.Any())
            {
                return; //DB has been seeded
            }

            var Categories = new List<Category>
            {

            new Category { Name = "Soup", DisplayOrder = 1 },
            new Category { Name = "Salad", DisplayOrder = 2 },
            new Category { Name = "Sandwiches", DisplayOrder = 3 }
            };

            foreach (var c in Categories)
            {
                _db.Categories.Add(c);
            }
            _db.SaveChanges();
        }
    }
}
