using BB.Core.Interfaces;
using BB.Core.Models;
using BB.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BB.Application
{
    public class UnitOfWork : IUnitOFWork
    {
        private readonly ApplicationDbContext _db;
        private IGenericRepository<Category>? _Category;
        private IGenericRepository<FoodType>? _FoodType;
        private IGenericRepository<MenuItem>? _MenuItem;
        private IGenericRepository<ApplicationUser>? _ApplicationUser;
        private IGenericRepository<ShoppingCart>? _ShoppingCart;
        public IGenericRepository<OrderHeader>? _OrderHeader;
        public IGenericRepository<OrderDetails>? _OrderDetails;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
        }

       public IGenericRepository<Category> Category
       {
            get
            {
                _Category ??= new GenericRepository<Category>(_db);
                return _Category;
            }
       }

        public IGenericRepository<FoodType> FoodType
        {
            get
            {
                _FoodType ??= new GenericRepository<FoodType>(_db);
                return _FoodType;
            }
        }

        public IGenericRepository<MenuItem> MenuItem
        {
            get
            {
                _MenuItem ??= new GenericRepository<MenuItem>(_db);
                return _MenuItem;
            }
        }

        public IGenericRepository<ApplicationUser> ApplicationUser
        {
            get
            {
                _ApplicationUser ??= new GenericRepository<ApplicationUser>(_db);
                return _ApplicationUser;
            }
        }

        public IGenericRepository<ShoppingCart> ShoppingCart
        {
            get
            {
                _ShoppingCart ??= new GenericRepository<ShoppingCart>(_db);
                return _ShoppingCart;
            }
        }

        public IGenericRepository<OrderHeader> OrderHeader
        {
            get
            {
                _OrderHeader ??= new GenericRepository<OrderHeader>(_db);
                return _OrderHeader;
            }
        }

        public IGenericRepository<OrderDetails> OrderDetails
        {
            get
            {
                _OrderDetails ??= new GenericRepository<OrderDetails>(_db);
                return _OrderDetails;
            }
        }
    }
}
