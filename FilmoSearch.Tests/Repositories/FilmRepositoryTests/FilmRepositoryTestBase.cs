using FilmoSearch.DTO;
using FilmoSearch.Models;
using FilmoSearch.Repositories.Actor;
using FilmoSearch.Repositories.Film;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmoSearch.Tests.Repositories.FilmRepositoryTests;

public class FilmRepositoryTestBase
{
        protected readonly ApplicationContext _context;
        protected readonly FilmRepository _repository;

        protected FilmRepositoryTestBase()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationContext(options);
            _repository = new FilmRepository(_context);
        }
}