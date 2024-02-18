using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interface;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewerRepository _reviewerRepository;

        public ReviewController(IReviewRepository reviewRepository, IMapper mapper
            ,IPokemonRepository pokemonRepository,IReviewerRepository reviewerRepository)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _pokemonRepository = pokemonRepository;
            _reviewerRepository = reviewerRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviews()
        {
            var review=_mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviews());
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(review);
        }
        [HttpGet("{reviewId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        public IActionResult GetReview(int reviewId) {

            if (!_reviewRepository.ExistsReviews(reviewId))
                return NotFound();
            var review=_mapper.Map<ReviewDto>(_reviewRepository.GetReview(reviewId));
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(review);
        }
        [HttpGet("{pokeId}/reviews")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsByAPokemon(int pokeId)
        {
           var review=_mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviewsByPokemon(pokeId));
            if(!ModelState.IsValid)
                return BadRequest();
            return Ok(review);
        }

        [HttpPost]
        [ProducesResponseType(284)]
        [ProducesResponseType(400)]
        public IActionResult CreateReview([FromQuery] int reviewerId, [FromQuery] int pokeId, [FromBody] ReviewDto reviewCreate)
        {
            if (reviewCreate == null)
            {
                return BadRequest(ModelState);
            }
            var reviews = _reviewRepository.GetReviews().
                Where(p => p.Title.Trim().ToUpper() == reviewCreate.Title.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (reviews != null)
            {
                ModelState.AddModelError("", "Review already Exists");
                return StatusCode(422, ModelState);
            }
            var reviewMap = _mapper.Map<Review>(reviewCreate);
            reviewMap.Reviewer = _reviewerRepository.GetReviewer(reviewerId);
            reviewMap.Pokemon=_pokemonRepository.GetPokemon(pokeId);
            if (!_reviewRepository.CreateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(422, ModelState);
            }
            return Ok("Successfully Created ");
        }
        [HttpPut("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReview(int reviewId, [FromBody] ReviewDto reviewUpdate)
        {
            if (reviewUpdate == null)
                return BadRequest(ModelState);

            if (reviewId != reviewUpdate.Id)
                return BadRequest(ModelState);

            if (!_reviewRepository.ExistsReviews(reviewId))
                return NotFound();

            var reviewMap = _mapper.Map<Review>(reviewUpdate);
            if (!_reviewRepository.UpdateReview(reviewMap))
                return BadRequest(ModelState);
            return NoContent();

        }
        [HttpDelete("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteOwner(int reviewId)
        {
            if (!_reviewRepository.ExistsReviews(reviewId))
                return NotFound();
            var reviewToDelete = _reviewRepository.GetReview(reviewId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewRepository.DeleteReview(reviewToDelete))
            {
                ModelState.AddModelError("", "Something went wrong Deleting");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

    }
}
