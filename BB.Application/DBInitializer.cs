using BB.Core.Interfaces;
using BB.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BB.Application

{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
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


            //create roles if they are not created
            //SD is a “Static Details” class we will create in Utility to hold constant strings for Roles

            _roleManager.CreateAsync(new IdentityRole(SD.AdminRole)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.DriverRole)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.KitchenRole)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.CustomerRole)).GetAwaiter().GetResult();

            //Create at least one "Super Admin" or “Admin”.  Repeat the process for other users you want to seed

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "rfry@weber.edu",
                Email = "rfry@weber.edu",
                FirstName = "Richard",
                LastName = "Fry",
                PhoneNumber = "8015556919",
            }, "Admin123*").GetAwaiter().GetResult();

            ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "rfry@weber.edu");

            _userManager.AddToRoleAsync(user, SD.AdminRole).GetAwaiter().GetResult();

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "christianmartin@mail.weber.edu",
                Email = "christianmartin@mail.weber.edu",
                FirstName = "Christian",
                LastName = "Martin",
                PhoneNumber = "5555555555",
            }, "Admin123*").GetAwaiter().GetResult();

            ApplicationUser cmartin = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "christianmartin@mail.weber.edu");
            _userManager.AddToRoleAsync(cmartin, SD.AdminRole).GetAwaiter().GetResult();

            var Categories = new List<Category>
             {
             new Category { Name = "Appetizers", DisplayOrder = 1 },
             new Category { Name = "Soups", DisplayOrder = 2 },
             new Category { Name = "Salads", DisplayOrder = 3 },
             new Category { Name = "Entrees", DisplayOrder = 4 },
             new Category { Name = "Desserts", DisplayOrder = 5 },
             new Category { Name = "Beverages", DisplayOrder = 6 }
             };

            foreach (var c in Categories)
            {
                _db.Categories.Add(c);
            }
            _db.SaveChanges();

            var FoodTypes = new List<FoodType>
             {
             new FoodType { Name = "Beef" },
             new FoodType { Name = "Chicken"},
             new FoodType { Name = "Seafood" },
             new FoodType { Name = "Vegetarian" },
             new FoodType { Name = "Vegan" },
             new FoodType { Name = "Sugar Free" },
             new FoodType { Name = "Carbonated" },
             new FoodType { Name = "Gluten Free" }
             };

            foreach (var f in FoodTypes)
            {
                _db.FoodTypes.Add(f);
            }
            _db.SaveChanges();

            var MenuItems = new List<MenuItem>
            {
                new MenuItem {
                    Name = "Chicken Satay Skewers",
                    Description = "Grilled marinated chicken skewers served with a spicy peanut dipping sauce.",
                    Price = 7.99f,
                    Image = "/images/menuitems/Chicken-Satay-Skewers.jpg",
                    CategoryId = 1,
                    FoodTypeId = 2
                },
                new MenuItem {
                    Name = "Shrimp Cocktail",
                    Description = "Chilled jumbo shrimp served with a tangy cocktail sauce.",
                    Price = 8.99f,
                    Image = "/images/menuitems/ShrimpCocktail.jpeg",
                    CategoryId = 1,
                    FoodTypeId = 3
                },
                new MenuItem {
                    Name = "Classic Tomato Basil Soup",
                    Description = "A creamy blend of ripe tomatoes and fresh basil, served with a hint of garlic.",
                    Price = 4.99f,
                    Image = "/images/menuitems/TomatoSoup.jpeg",
                    CategoryId = 2,
                    FoodTypeId = 4
                },
                new MenuItem {
                    Name = "Chicken Enchilada Chili",
                    Description = "Chili and bold Mexican flavors all in one bowl. Made with shredded chicken breast, black beans, corn, and a handful of bold spices.",
                    Price = 6.49f,
                    Image = "/images/menuitems/ChickenEnchiladaChili.jpg",
                    CategoryId = 2,
                    FoodTypeId = 2
                },
                new MenuItem {
                    Name = "Lobster Bisque",
                    Description = "A thick blended French soup made with lobster meat, cream, and broth or stock.",
                    Price = 7.99f,
                    Image = "/images/menuitems/LobsterSoup.jpeg",
                    CategoryId = 2,
                    FoodTypeId = 3
                },
                new MenuItem {
                    Name = "Mango Berry Salad",
                    Description = "Packed with your favorite fresh berries and juicy mango chunks, topped with a delicious honey-orange glaze",
                    Price = 7.49f,
                    Image = "/images/menuitems/MangoBerry.jpg",
                    CategoryId = 3,
                    FoodTypeId = 4
                },
                new MenuItem {
                    Name = "Avacado Chicken Salad",
                    Description = "Fresh greens topped with grilled chicken breast, cherry tomatoes, avacados, and shredded cheese.",
                    Price = 8.99f,
                    Image = "/images/menuitems/ACC_Salad.jpg",
                    CategoryId = 3,
                    FoodTypeId = 2
                },
                new MenuItem {
                    Name = "Chipotle Steak Sandwich",
                    Description = "Made with thinly sliced salted dry brined steak strips grilled to juicy perfection tucked inside crusty thick cut bread smeared with house-made chipotle aioli layered with sweet caramelized onions, arugula, and melted cheddar cheese.",
                    Price = 12.99f,
                    Image = "/images/menuitems/ChipotleSteak.jpg",
                    CategoryId = 4,
                    FoodTypeId = 1
                },
                new MenuItem {
                    Name = "Chicken Avocado Club",
                    Description = "Turkey, bacon, gouda, avocado, and spicy sweet sriracha sauce layered on toasty seared sourdough bread.",
                    Price = 9.99f,
                    Image = "/images/menuitems/ChickenAvacado.jpg",
                    CategoryId = 4,
                    FoodTypeId = 2
                },
                new MenuItem {
                    Name = "Seared Salmon Fillet",
                    Description = "Pan-seared salmon fillet with a dill cream sauce, served with steamed vegetables.",
                    Price = 14.99f,
                    Image = "/images/menuitems/Salmon.jpeg",
                    CategoryId = 4,
                    FoodTypeId = 3
                },
                new MenuItem {
                    Name = "Vegetable Stir-Fry",
                    Description = "Assorted fresh vegetables stir-fried in a savory sauce, served over jasmine rice.",
                    Price = 12.99f,
                    Image = "/images/menuitems/StirFry.jpeg",
                    CategoryId = 4,
                    FoodTypeId = 4
                },
                new MenuItem {
                    Name = "Caramel Brownie",
                    Description = "Gooey caramel brownies with layers of brownie, caramel, chocolate, and walnuts.",
                    Price = 6.49f,
                    Image = "/images/menuitems/CaramelBrownie.jpg",
                    CategoryId = 5,
                    FoodTypeId = 8
                },
                new MenuItem {
                    Name = "Vegan Creme Brulee",
                    Description = "Made with coconut milk and vanilla bean! It has the most creamy, smooth vanilla custard topped with a crunchy caramel layer.",
                    Price = 5.99f,
                    Image = "/images/menuitems/CremeBrulee.jpg",
                    CategoryId = 5,
                    FoodTypeId = 5
                },
                new MenuItem {
                    Name = "Zero Lemonade",
                    Description = "Lemonade made without the sugar.",
                    Price = 2.49f,
                    Image = "/images/menuitems/ZeroWater.jpg",
                    CategoryId = 6,
                    FoodTypeId = 6
                }
                ,
                new MenuItem {
                    Name = "Coke Classic",
                    Description = "12 ounce can",
                    Price = 2.49f,
                    Image = "/images/menuitems/coke.jpg",
                    CategoryId = 6,
                    FoodTypeId = 7
                }
                ,
                new MenuItem {
                    Name = "Sprite",
                    Description = "Lemonade made without the sugar.",
                    Price = 2.49f,
                    Image = "/images/menuitems/sprite.jpg",
                    CategoryId = 6,
                    FoodTypeId = 7 }
                };

            foreach (var m in MenuItems)
            {
                _db.MenuItems.Add(m);
            }
            _db.SaveChanges();

        }
    }
}