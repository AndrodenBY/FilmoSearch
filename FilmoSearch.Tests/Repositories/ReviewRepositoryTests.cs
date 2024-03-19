using FilmoSearch.DTO;
using FilmoSearch.Models;
using FilmoSearch.Repositories.Actor;
using FilmoSearch.Repositories.Review;
using FilmoSearch.Services.Film;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmoSearch.Tests.Repositories
{

    public class ReviewRepositoryTests
    {
        private MockApplicationContext _context;

        public ReviewRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new MockApplicationContext(options);
        }        

        [Fact]
        public void GetAll_ReturnAllReviews()
        {
            //Arrange
            List<ReviewDto> testReviews = new List<ReviewDto>
            {
                new ReviewDto
                (
                    Id: Guid.NewGuid(),
                    Title: "Title1",
                    Description: "Description1",
                    Stars: 5,
                    Film: new FilmDto ( Id: Guid.NewGuid(), Title: "TestTitle1", Actors: null, Reviews: null )                    
                ),

                new ReviewDto
                (
                    Id: Guid.NewGuid(),
                    Title: "Title2",
                    Description: "Description2",
                    Stars: 4,
                    Film: new FilmDto ( Id: Guid.NewGuid(), Title: "TestTitle2", Actors: null, Reviews: null )
                )
            };

            ReviewRepository repository = new ReviewRepository(_context);

            foreach (ReviewDto review in testReviews)
            {
                _context.Reviews.Add(new Review
                {
                    Id = review.Id,
                    Title = review.Title,
                    Description = review.Description,
                    Stars = review.Stars,
                    Film = new Film { Id = review.Id, Title = review.Title },
                });
            }
            _context.SaveChanges();

            //Act
            IEnumerable<ReviewDto> result = repository.GetAll();

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetById_ReturnReviewById()
        {
            // Arrange
            Guid reviewId = Guid.NewGuid();
            ReviewDto testReview = new ReviewDto
                (
                    Id: reviewId,
                    Title: "Title1",
                    Description: "Description1",
                    Stars: 4,
                    Film: new FilmDto ( Id: Guid.NewGuid(), Title: "TestFilm1", Actors: null, Reviews: null )                    
                );
            Review reviewToGet = new Review
            {
                Id = testReview.Id,
                Title = testReview.Title,
                Description = testReview.Description,
                Stars = testReview.Stars,
                Film = new Film { Id = testReview.Id, Title = testReview.Title },
            };
            _context.Reviews.Add(reviewToGet);
            _context.SaveChanges();

            ReviewRepository repository = new ReviewRepository(_context);

            // Act
            ReviewDto result = repository.GetById(reviewId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ReviewDto>(result);
            Assert.Equal(reviewId, result.Id);
            Assert.Equal("Title1", result.Title);
            Assert.Equal("Description1", result.Description);
            Assert.Equal(4, result.Stars);            
        }

        [Fact]
        public void Create_ReturnTrueWhenReviwIsCreated()
        {
            // Arrange
            ReviewDto reviewToCreate = new ReviewDto(
                Id: Guid.NewGuid(),
                Title: "Title1",
                Description: "Description1",
                Stars: 4,
                Film: new FilmDto ( Id: Guid.NewGuid(), Title: "TestFilm1", null, null )
            );

            ReviewRepository repository = new ReviewRepository(_context);

            // Act
            bool result = repository.Create(reviewToCreate);

            // Assert
            Assert.True(result);
            Review createdReview = _context.Reviews.Find(reviewToCreate.Id);
            Assert.NotNull(createdReview);
            Assert.Equal(reviewToCreate.Title, createdReview.Title);
            Assert.Equal(reviewToCreate.Description, createdReview.Description);
            Assert.Equal(4, reviewToCreate.Stars);
        }

        [Fact]
        public void AddToFilm_WhenValidReview_ReturnsTrue()
        {
            // Arrange
            Guid filmId = Guid.NewGuid();
            ReviewDto reviewToCreate = new ReviewDto
            (
                Id: Guid.NewGuid(),

                Title: "TestTitle1",
                Description: "Description1",
                Stars: 5,
                Film: new FilmDto(Id: filmId, Title: "TestFilm1", null, null)
            );            

            ReviewRepository repository = new ReviewRepository(_context);

            // Act
            bool result = repository.AddToFilm(reviewToCreate, filmId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Delete_ReturnTrueWhenReviewIsDeleted()
        {
            // Arrange
            Guid reviewIdToDelete = Guid.NewGuid();
            Review reviewToDelete = new Review { Id = reviewIdToDelete, Title = "Title1", Description = "Desc1", Stars = 1, Film = new Film { Title = "TestTitle" } };
            _context.Reviews.Add(reviewToDelete);
            _context.SaveChanges();

            ReviewRepository repository = new ReviewRepository(_context);

            // Act
            bool result = repository.Delete(reviewIdToDelete);

            // Assert
            Assert.True(result);
            Review deletedReview = _context.Reviews.FirstOrDefault(a => a.Id == reviewIdToDelete);
            Assert.Null(deletedReview);
        }
    }
}
