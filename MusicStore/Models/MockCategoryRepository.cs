using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStore.Models
{
    public class MockCategoryRepository : ICategoryRepository
    {
        public IEnumerable<Category> Categories
        {
            get
            {
                return new List<Category>
                {
                    new Category{CategoryId=1, CategoryName="Fruit Albums", Description="All-fruity Albums"},
                    new Category{CategoryId=2, CategoryName="Cheese cakes", Description="Cheesy all the way"},
                    new Category{CategoryId=3, CategoryName="Seasonal Albums", Description="Get in the mood for a seasonal Album"}
                };
            }
        }
    }
}
