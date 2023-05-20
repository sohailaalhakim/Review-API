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
    public class ReviewerController : ControllerBase
    {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;


        public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper)
        {
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }

        //get all reviewers
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        [ProducesResponseType(400)]

        public IActionResult GetReviewers()
        {
            var reviewers = _mapper.Map<List<ReviewerDTO>>(_reviewerRepository.GetAllReviewer());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(reviewers);
        }
        //get reviewer by id
        [HttpGet("{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewer(int reviewerId)
        {
            if (!_reviewerRepository.IsReviewerExist(reviewerId))
                return NotFound();

            var reviewer = _mapper.Map<ReviewerDTO>(_reviewerRepository.GetReviewer(reviewerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviewer);
        }

        //get reviews by reviewer
        [HttpGet("{reviewerId}/reviews")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsByReviewer(int reviewerId)
        {
            if (!_reviewerRepository.IsReviewerExist(reviewerId))
                return NotFound();

            var reviews = _mapper.Map<List<ReviewDTO>>(
                 _reviewerRepository.GetReviewsByReviewer(reviewerId));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviews);
        }

        //create reviewer
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReviewer([FromBody] ReviewerDTO reviewerToCreate)
        {
            if (reviewerToCreate == null)
                return BadRequest(ModelState);

            var review = _reviewerRepository.GetAllReviewer()
                .Where(c => c.LastName.Trim().ToUpper() == reviewerToCreate.LastName.Trim().ToUpper())
                .FirstOrDefault();

            if (review != null)
            {
                ModelState.AddModelError("", $"Review {reviewerToCreate.LastName} already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewerObj = _mapper.Map<Reviewer>(reviewerToCreate);

            if (!_reviewerRepository.CreateReviewer(reviewerObj))
            {
                ModelState.AddModelError("", $"Something went wrong saving the review " +
                                                              $"{reviewerObj.LastName}");
                return StatusCode(500, ModelState);
            }
            return Ok("Review added Successfully ");
        }

    }
}
