using FilmoSearch.DTO;
using FilmoSearch.Models;
using FilmoSearch.Repositories.Actor;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmoSearch.Tests.Repositories
{
    public class ActorRepositoryTests
    {
        private readonly ApplicationContext _context;
        private readonly ActorRepository _repository;

        public ActorRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationContext(options);
            _repository = new ActorRepository(_context); 
        }

        [Fact]
        public async Task GetAll_ReturnsAllActors()
        {
            // Arrange
            var testActors = new List<Actor>
            {
                new Actor
                {
                    Id = Guid.NewGuid(),
                    FirstName = "John",
                    LastName = "Doe",
                    Films = new List<Film> { new Film { Id = Guid.NewGuid(), Title = "TestTitle1" } }
                },
                new Actor
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Jane",
                    LastName = "Smith",
                    Films = new List<Film> { new Film { Id = Guid.NewGuid(), Title = "TestTitle2" } }
                }
            };

            await _context.Actors.AddRangeAsync(testActors);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
        
        [Fact]
        public async Task GetByIdAsync_ReturnsActorById()
        {
            // Arrange
            var actorId = Guid.NewGuid();
            var testActor = new Actor
            {
                Id = actorId,
                FirstName = "John",
                LastName = "Doe",
                Films = new List<Film>
                {
                    new Film { Id = Guid.NewGuid(), Title = "TestFilm1" },
                    new Film { Id = Guid.NewGuid(), Title = "TestFilm2" }
                }
            };

            await _context.Actors.AddAsync(testActor);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(actorId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(actorId, result.Id);
            Assert.Equal("John", result.FirstName);
            Assert.Equal("Doe", result.LastName); 
            Assert.Equal(2, result.Films.Count); 
            Assert.Contains(result.Films, f => f.Title == "TestFilm1"); 
            Assert.Contains(result.Films, f => f.Title == "TestFilm2");
        }
        
        [Fact]
        public async Task CreateAsync_ReturnsTrueWhenActorIsCreated()
        {
            // Arrange
            var actorToCreate = new ActorDto
            (
                Id: Guid.NewGuid(),
                FirstName: "John",
                LastName: "Doe",
                Films: new List<FilmDto> 
                {
                    new FilmDto(Guid.NewGuid(), "TestFilm1", null, null),
                    new FilmDto(Guid.NewGuid(), "TestFilm2", null, null)
                }
            );

            // Act
            var result = await _repository.CreateAsync(actorToCreate);

            // Assert
            Assert.True(result); 
            var createdActor = await _context.Actors.FindAsync(actorToCreate.Id);
            Assert.NotNull(createdActor);
            Assert.Equal(actorToCreate.FirstName, createdActor.FirstName);
            Assert.Equal(actorToCreate.LastName, createdActor.LastName); 
            Assert.Equal(actorToCreate.Films.Count, createdActor.Films.Count); 
            Assert.Contains(createdActor.Films, f => f.Title == "TestFilm1"); 
            Assert.Contains(createdActor.Films, f => f.Title == "TestFilm2"); 
        }
        
    }
}