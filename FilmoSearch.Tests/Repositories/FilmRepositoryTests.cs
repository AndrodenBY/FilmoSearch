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

namespace FilmoSearch.Tests.Repositories
{

    public class FilmRepositoryTests
    {
        private MockApplicationContext _context;

        public FilmRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new MockApplicationContext(options);
        }

        [Fact]
        public void GetAll_ReturnAllFilms()
        {
            //Arrange
            List<FilmDto> testFilms = new List<FilmDto>
            {
                new FilmDto
                (
                    Id: Guid.NewGuid(),
                    Title: "TestTitle1",
                    Actors: new List<ActorDto>
                    {
                        new ActorDto ( Id: Guid.NewGuid(), FirstName: "Arthur", LastName: "Morgan", null )
                    },
                    Reviews: new List<ReviewDto>
                    {
                        new ReviewDto ( Id: Guid.NewGuid(), Title: "ReviewTitle", Description: "TestDescription1", Stars: 5, null )
                    }
                ),

                new FilmDto
                (
                    Id: Guid.NewGuid(),
                    Title: "TestTitle2",
                    Actors: new List<ActorDto>
                    {
                        new ActorDto ( Id: Guid.NewGuid(), FirstName: "John", LastName: "Milton", null )
                    },
                    Reviews: new List<ReviewDto>
                    {
                        new ReviewDto ( Id: Guid.NewGuid(), Title: "ReviewTitle2", Description: "TestDescription2", Stars: 1, null )
                    }
                )
            };

            FilmRepository repository = new FilmRepository(_context);

            foreach (FilmDto film in testFilms)
            {
                _context.Films.Add(new Film
                {
                    Id = film.Id,
                    Title = film.Title,
                    Actors = film.Actors
                    .Select(actor => new Actor { Id = actor.Id, FirstName = actor.FirstName, LastName = actor.LastName })
                    .ToList(),
                    Reviews = film.Reviews
                    .Select(review => new Review { Id = review.Id, Title = review.Title, Description = review.Description, Stars = review.Stars })
                    .ToList()
                });
            }
            _context.SaveChanges();

            //Act
            IEnumerable<FilmDto> result = repository.GetAll();

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetById_ReturnFilmById()
        {
            // Arrange
            Guid filmId = Guid.NewGuid();
            FilmDto testFilm = new FilmDto
                (
                    Id: filmId,
                    Title: "TestTitle1",
                    Actors: new List<ActorDto>
                    {
                        new ActorDto ( Id: Guid.NewGuid(), FirstName: "Rip", LastName: "Van Winkle", null ),
                        new ActorDto ( Id: Guid.NewGuid(), FirstName: "TestFName", LastName: "TestLName", null )
                    },
                    Reviews: new List<ReviewDto>
                    {
                        new ReviewDto ( Id: Guid.NewGuid(), Title: "TestTitle1", Description: "SuperDescription", Stars: 2, null ),
                        new ReviewDto ( Id: Guid.NewGuid(), Title: "TestTitle2", Description: "SimpleDescription", Stars: 3, null )
                    }
                );
            Film filmToGet = new Film
            {
                Id = testFilm.Id,
                Title = testFilm.Title,
                Actors = testFilm.Actors
                .Select(actorDto => new Actor { Id = actorDto.Id, FirstName = actorDto.FirstName, LastName = actorDto.LastName })
                .ToList(),
                Reviews = testFilm.Reviews
                .Select(reviewDto => new Review { Id = reviewDto.Id, Title = reviewDto.Title, Description = reviewDto.Description, Stars = reviewDto.Stars })
                .ToList()
            };
            _context.Films.Add(filmToGet);
            _context.SaveChanges();

            FilmRepository repository = new FilmRepository(_context);

            // Act
            FilmDto result = repository.GetById(filmId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<FilmDto>(result);
            Assert.Equal(filmId, result.Id);
            Assert.Equal("TestTitle1", result.Title);
            Assert.Equal("2", result.Reviews.Count.ToString());
            Assert.Equal("2", result.Actors.Count.ToString());
        }

        [Fact]
        public void Create_ReturnTrueWhenFilmIsCreated()
        {
            // Arrange
            FilmDto filmToCreate = new FilmDto(
                Id: Guid.NewGuid(),
                Title: "TestTitle1",
                Actors: new List<ActorDto>
                {
                    new ActorDto ( Id: Guid.NewGuid(), FirstName: "FName1", LastName:"LName1", null ),
                    new ActorDto ( Id: Guid.NewGuid(), FirstName: "FName2", LastName:"LName2", null )
                },
                Reviews: new List<ReviewDto>
                {
                    new ReviewDto ( Id: Guid.NewGuid(), Title: "TestT1", Description:"NewDescription", Stars: 3, null ),
                    new ReviewDto ( Id: Guid.NewGuid(), Title: "TestT2", Description : "NewNewDescription", Stars : 3, null )
                }
            );

            FilmRepository repository = new FilmRepository(_context);

            // Act
            bool result = repository.Create(filmToCreate);

            // Assert
            Assert.True(result);
            Film createdFilm = _context.Films.Find(filmToCreate.Id);
            Assert.NotNull(createdFilm);
            Assert.Equal(filmToCreate.Title, filmToCreate.Title);
            Assert.Equal("2", createdFilm.Actors.Count.ToString());
            Assert.Equal("2", createdFilm.Reviews.Count.ToString());
        }

        [Fact]
        public void AddActor_ReturnTrueWhenActorIsAdded()
        {
            // Arrange
            Guid filmId = Guid.NewGuid();
            Guid actorId = Guid.NewGuid();

            Film testFilm = new Film { Id = filmId, Actors = new List<Actor> { new Actor { Id = actorId } } };

            FilmRepository repository = new FilmRepository(_context);

            // Act            
            bool result = repository.AddActor(filmId, actorId);
            bool isActorAdded = testFilm.Actors.Any(f => f.Id == actorId);

            // Assert
            Assert.True(result);
            Assert.True(isActorAdded);

        }

        [Fact]
        public void AddReview_ReturnTrueWhenReviewIsAdded()
        {
            // Arrange
            Guid filmId = Guid.NewGuid();
            Guid reviewId = Guid.NewGuid();

            Film testFilm = new Film { Id = filmId, Reviews = new List<Review> { new Review { Id = reviewId } } };

            FilmRepository repository = new FilmRepository(_context);

            // Act            
            bool result = repository.AddReview(filmId, reviewId);
            bool isReviewAdded = testFilm.Reviews.Any(f => f.Id == reviewId);

            // Assert
            Assert.True(result);
            Assert.True(isReviewAdded);

        }

        [Fact]
        public void RemoveActor_ReturnTrueWhenActorIsRemoved()
        {
            // Arrange
            Guid filmId = Guid.NewGuid();
            Guid actorId = Guid.NewGuid();

            Film testFilm = new Film
            {
                Id = filmId,
                Title = "Title",                
                Actors = new List<Actor> { new Actor { Id = actorId } }
            };

            _context.Films.Add(testFilm);
            _context.SaveChanges();

            FilmRepository repository = new FilmRepository(_context);

            // Act            
            bool result = repository.RemoveActor(filmId, actorId);
            Film film = _context.Films.Include(a => a.Actors).FirstOrDefault(a => a.Id == filmId);

            // Assert            
            Assert.True(result);
            Assert.NotNull(film);
            Assert.DoesNotContain(film.Actors, f => f.Id == filmId);
        }

        [Fact]
        public void RemoveReview_ReturnTrueWhenReviewIsRemoved()
        {
            // Arrange
            Guid filmId = Guid.NewGuid();
            Guid reviewId = Guid.NewGuid();

            Film testFilm = new Film
            {
                Id = filmId,
                Title = "Title",                
                Reviews = new List<Review> { new Review { Id = reviewId } }
            };

            _context.Films.Add(testFilm);
            _context.SaveChanges();

            FilmRepository repository = new FilmRepository(_context);

            // Act            
            bool result = repository.RemoveReview(filmId, reviewId);
            Film film = _context.Films.Include(a => a.Reviews).FirstOrDefault(a => a.Id == filmId);

            // Assert            
            Assert.True(result);
            Assert.NotNull(film);
            Assert.DoesNotContain(film.Actors, f => f.Id == filmId);
        }

        [Fact]
        public void Delete_ReturnTrueWhenFilmIsDeleted()
        {
            // Arrange
            Guid filmIdToDelete = Guid.NewGuid();
            Film filmToDelete = new Film { Id = filmIdToDelete, Title = "Mehr", Actors = new List<Actor>(), Reviews = new List<Review>() };
            _context.Films.Add(filmToDelete);
            _context.SaveChanges();

            FilmRepository repository = new FilmRepository(_context);

            // Act
            bool result = repository.Delete(filmIdToDelete);

            // Assert
            Assert.True(result);
            Film deletedFilm = _context.Films.FirstOrDefault(a => a.Id == filmIdToDelete);
            Assert.Null(deletedFilm);
        }
    }
}
