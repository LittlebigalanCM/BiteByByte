using BB.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BB.Core.Interfaces
{
    public interface IUnitOFWork
    {
        //Add Models/Tables here as you create them, so UnitOfWork have access
        public IGenericRepository<Category> Category { get; }
        public IGenericRepository<FoodType> FoodType { get; }
        public IGenericRepository<MenuItem> MenuItem { get; }
    }
}
