using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BooksApi.Controllers
{
    [Route("/api/users")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        
        public UsersController(AppDbContext context)
        {
            _dbContext = context;
        }

        [HttpGet]
    }
}