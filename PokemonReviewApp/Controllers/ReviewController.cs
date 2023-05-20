using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pokemon_Review_System.Models;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Repository;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;


        public ReviewController(IReviewRepository reviewRepository,
            IReviewerRepository reviewerRepository,
            IPokemonRepository pokemonRepository,
            IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _pokemonRepository = pokemonRepository;
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }
        //get all reviews
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]

        public IActionResult GetReviews()
        {
            var reviews = _mapper.Map<List<ReviewDTO>>(_reviewRepository.GetAllReviews());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(reviews);
        }
        //get review by id
        [HttpGet("{reviewId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReview(int reviewId)
        {
            if (!_reviewRepository.IsReviewExist(reviewId))
                return NotFound();

            var review = _mapper.Map<ReviewDTO>(_reviewRepository.GetReview(reviewId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(review);
        }

        [HttpGet("/pokemon/{reviewId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]

        public IActionResult GetReviewsOfPokemon (int reviewId)
        {
            var reviews = _mapper.Map<List<ReviewDTO>>(
                _reviewRepository.GetReviewsOfPokemon(reviewId)
                );
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviews);
        }

        //create review
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReview([FromQuery] int reviewerId, 
            [FromQuery] int pokeId,[FromBody] ReviewDTO reviewToCreate)
        {
            if (reviewToCreate == null)
                return BadRequest(ModelState);

            var review = _reviewRepository.GetAllReviews()
                .Where(c => c.Title.Trim().ToUpper() == reviewToCreate.Title.Trim().ToUpper())
                .FirstOrDefault();

            if (review != null)
            {
                ModelState.AddModelError("", $"Review {reviewToCreate.Title} already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewObj = _mapper.Map<Review>(reviewToCreate);

            reviewObj.Reviewer = _reviewerRepository.GetReviewer(reviewerId);
            reviewObj.Pokemon = _pokemonRepository.GetPokemonById(pokeId);

            if (!_reviewRepository.CreateReview(reviewObj))
            {
                ModelState.AddModelError("", $"Something went wrong saving the review " +
                                                              $"{reviewObj.Title}");
                return StatusCode(500, ModelState);
            }
            return Ok("Review added Successfully ");
        }

        //update review
        [HttpPut("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReview(int reviewId, [FromBody] ReviewDTO reviewToUpdate)
        {
            if (reviewToUpdate == null)
                return BadRequest(ModelState);

            if (reviewId != reviewToUpdate.Id)
                return BadRequest(ModelState);

            if (!_reviewRepository.IsReviewExist(reviewId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var reviewObj = _mapper.Map<Review>(reviewToUpdate);

            if (!_reviewRepository.UpdateReview(reviewObj))
            {
                ModelState.AddModelError("", $"Something went wrong updating the review " +
                                    $"{reviewObj.Title}");
                return StatusCode(500, ModelState);
            }

            return Ok($"{reviewObj.Title} updated successfully");
        }

        //delete review
        [HttpDelete("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteReview(int reviewId)
        {
            if (!_reviewRepository.IsReviewExist(reviewId))
            {
                return NotFound();
            }

            var reviewToDelete = _reviewRepository.GetReview(reviewId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewRepository.DeleteReview(reviewToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong deleting the review " +
                                    $"{reviewToDelete.Title}");
                return StatusCode(500, ModelState);
            }

            return Ok($"{reviewToDelete.Title} deleted successfully");
        }
        //delete reviews by reviewer
        [HttpDelete("/DeleteReviewsByReviewer/{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteReviewsByReviewer(int reviewerId)
        {
            if (!_reviewerRepository.IsReviewerExist(reviewerId))
                return NotFound();

            var reviewsToDelete = _reviewerRepository.GetReviewsByReviewer(reviewerId).ToList();
            if (!ModelState.IsValid)
                return BadRequest();

            if (!_reviewRepository.DeleteReviews(reviewsToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong deleting these reviews " +
                                    $"{reviewsToDelete.ToList()}");
                return StatusCode(500, ModelState);
            }

            return Ok($"{reviewsToDelete.ToList()} deleted successfully");
        }

    }

}

