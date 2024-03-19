using FilmoSearch.Controllers;
using FilmoSearch.DTO;
using FilmoSearch.Models;
using FilmoSearch.Repositories.Actor;
using FilmoSearch.Services.Actor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FilmoSearch.Tests.Repositories
{
    public class MockApplicationContext : ApplicationContext
    {
        public MockApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }

    public class ActorRepositoryTests
    {

        private MockApplicationContext _context;        

        public ActorRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new MockApplicationContext(options);
        }

        [Fact]
        public void GetAll_ReturnAllActors()
        {
            //Arrange
            List<ActorDto> testActors = new List<ActorDto>
            {
                new ActorDto
                (
                    Id: Guid.NewGuid(),
                    FirstName: "John",
                    LastName: "Doe",
                    Films: new List<FilmDto>
                    {
                        new FilmDto ( Id: Guid.NewGuid(), Title: "TestTitle1", Actors: null, Reviews: null )
                    }
                ),

                new ActorDto
                (
                    Id: Guid.NewGuid(),
                    FirstName: "John",
                    LastName: "Marston",
                    Films: new List<FilmDto>
                    {
                        new FilmDto ( Id: Guid.NewGuid(), Title: "Redemption1", Actors: null, Reviews: null )
                    }
                )
            };

            ActorRepository repository = new ActorRepository(_context);

            foreach (ActorDto actor in testActors)
            {
                _context.Actors.Add(new Actor
                {
                    Id = actor.Id,
                    FirstName = actor.FirstName,
                    LastName = actor.LastName,
                    Films = actor.Films.Select(film => new Film { Id = film.Id, Title = film.Title }).ToList()
                });
            }
            _context.SaveChanges();

            //Act
            IEnumerable<ActorDto> result = repository.GetAll();

            //Assert
            Assert.NotNull(result);            
        }
        
        [Fact]
        public void GetById_ReturnActorById()
        {
            // Arrange
            Guid actorId = Guid.NewGuid();
            ActorDto testActor = new ActorDto
                (
                    Id: actorId,
                    FirstName: "John",
                    LastName: "Smith",
                    Films: new List<FilmDto>
                    {
                        new FilmDto ( Id: Guid.NewGuid(), Title: "TestFilm1", Actors: null, Reviews: null ),
                        new FilmDto ( Id: Guid.NewGuid(), Title: "TestFilm2", Actors: null, Reviews: null )
                    }
                );
             Actor actorToGet = new Actor
                {
                    Id = testActor.Id,
                    FirstName = testActor.FirstName,
                    LastName = testActor.LastName,
                    Films = testActor.Films.Select(filmDto => new Film { Id = filmDto.Id, Title = filmDto.Title }).ToList()
                };
            _context.Actors.Add(actorToGet);
            _context.SaveChanges();

            ActorRepository repository = new ActorRepository(_context);

            // Act
            ActorDto result = repository.GetById(actorId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ActorDto>(result);
            Assert.Equal(actorId, result.Id);
            Assert.Equal("John", result.FirstName);
            Assert.Equal("Smith", result.LastName);
            Assert.Equal("2", result.Films.Count.ToString());
            Assert.Equal("TestFilm1", result.Films.First().Title);
        }

        [Fact]
        public void Create_ReturnTrueWhenActorIsCreated()
        {
            // Arrange
            ActorDto actorToCreate = new ActorDto(
                Id: Guid.NewGuid(),
                FirstName: "John",
                LastName: "Doe",
                Films: new List<FilmDto>
                {
                    new FilmDto ( Id: Guid.NewGuid(), Title: "TestFilm1", null, null ),
                    new FilmDto ( Id: Guid.NewGuid(), Title: "TestFilm2", null, null )
                }
            );

            ActorRepository repository = new ActorRepository(_context);

            // Act
            bool result = repository.Create(actorToCreate);

            // Assert
            Assert.True(result);            
            Actor createdActor = _context.Actors.Find(actorToCreate.Id);
            Assert.NotNull(createdActor);
            var expectedFilmTitle = actorToCreate.Films[0].Title;
            var actualFilmTitle = createdActor.Films.FirstOrDefault()?.Title;
            Assert.Equal(expectedFilmTitle, actualFilmTitle);
            Assert.Equal(actorToCreate.FirstName, createdActor.FirstName);
            Assert.Equal(actorToCreate.LastName, createdActor.LastName);
            Assert.Equal("2", createdActor.Films.Count.ToString());

        }

        [Fact]
        public void AddFilm_ReturnTrueWhenFilmIsAdded()
        {            
            // Arrange
            Guid actorId = Guid.NewGuid();
            Guid filmId = Guid.NewGuid();

            Actor testActor = new Actor { Id = actorId, Films = new List<Film> { new Film { Id = filmId } } };

            ActorRepository repository = new ActorRepository(_context);

            // Act            
            bool result = repository.AddFilm(actorId, filmId);
            bool isFilmAdded = testActor.Films.Any(f => f.Id == filmId);

            // Assert
            Assert.True(result);
            Assert.True(isFilmAdded);

        }

        [Fact]
        public void RemoveFilm_ReturnTrueWhenFilmIsRemoved()
        {
            // Arrange
            Guid actorId = Guid.NewGuid();
            Guid filmId = Guid.NewGuid();

            Actor testActor = new Actor 
            { 
                Id = actorId, 
                FirstName = "John", 
                LastName = "Doe", 
                Films = new List<Film> { new Film { Id = filmId } } 
            };

            _context.Actors.Add(testActor);
            _context.SaveChanges();

            ActorRepository repository = new ActorRepository(_context);

            // Act            
            bool result = repository.RemoveFilm(actorId, filmId);
            Actor actor = _context.Actors.Include(a => a.Films).FirstOrDefault(a => a.Id == actorId);

            // Assert            
            Assert.True(result);
            Assert.NotNull(actor);
            Assert.DoesNotContain(actor.Films, f => f.Id == filmId);
        }

        [Fact]
        public void Update_ReturnTrueWhenActorIsUpdated() //Test is not working
        {
            // Arrange
            ActorDto actorToUpdate = new ActorDto
            (
                Id: Guid.NewGuid(),
                FirstName: "UpdatedFirstName",
                LastName: "UpdatedLastName",
                Films: new List<FilmDto>
                {
                    new FilmDto ( Id: Guid.NewGuid(), Title: "UpdatedTestTitle", Actors: null, Reviews: null )
                }
            );

            ActorRepository repository = new ActorRepository(_context);           

            // Act
            bool result = repository.Update(actorToUpdate);

            // Assert
            Assert.True(result);
            Actor updatedActor = _context.Actors.Find(actorToUpdate.Id);
            Assert.NotNull(updatedActor);
            Assert.Equal(actorToUpdate.FirstName, updatedActor.FirstName);
            Assert.Equal(actorToUpdate.LastName, updatedActor.LastName);
            Assert.Equal(actorToUpdate.Films.Count, updatedActor.Films.Count);
        }

        [Fact]
        public void Delete_ReturnTrueWhenActorIsDeleted()
        {
            // Arrange
            Guid actorIdToDelete = Guid.NewGuid();
            Actor actorToDelete = new Actor { Id = actorIdToDelete, FirstName = "FN1", LastName = "LN1", Films = new List<Film>() };
            _context.Actors.Add(actorToDelete);
            _context.SaveChanges();

            ActorRepository repository = new ActorRepository(_context);

            // Act
            bool result = repository.Delete(actorIdToDelete);

            // Assert
            Assert.True(result);
            Actor deletedActor = _context.Actors.FirstOrDefault(a => a.Id == actorIdToDelete);
            Assert.Null(deletedActor);
        }
    }
}
