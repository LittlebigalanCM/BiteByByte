using BB.Core.Interfaces;
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

            if (!_db.Categories.Any())
            {

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

            if (!_db.FoodTypes.Any())
            {
                var FoodTypes = new List<FoodType>
                {
                    new FoodType { Name = "Vegetarian" },
                    new FoodType { Name = "Vegan" },
                    new FoodType { Name = "Gluten-Free" },
                    new FoodType { Name = "Dairy-Free" },
                    new FoodType { Name = "Nut-Free" },
                    new FoodType { Name = "Halal" },
                    new FoodType { Name = "Kosher" },
                    new FoodType { Name = "Spicy" },
                    new FoodType { Name = "Low-Carb" },
                    new FoodType { Name = "High-Protein" }
                };

                foreach (var ft in FoodTypes)
                {
                    _db.FoodTypes.Add(ft);
                }
                _db.SaveChanges();
            }
        }
    }
}
