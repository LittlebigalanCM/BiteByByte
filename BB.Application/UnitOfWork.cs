using BB.Core.Interfaces;
using BB.Core.Models;
using BB.Infrastructure.Data;
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
    }
}
