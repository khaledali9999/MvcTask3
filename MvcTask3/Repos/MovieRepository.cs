using MvcTask3.DataAccses;
using MvcTask3.Models;
using System.Linq.Expressions;

namespace MvcTask3.Repos
{
    
      
        public class MovieRepository 
        {
            private readonly ApplicationDbContext _context;

            public MovieRepository(ApplicationDbContext context)
            {
                _context = context;
            }

          
        }
    }

