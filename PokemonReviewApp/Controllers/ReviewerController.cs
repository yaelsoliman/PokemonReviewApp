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
    public class ReviewerController : ControllerBase
    {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewerController(IReviewerRepository reviewerRepository,IMapper mapper)
        {
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewer()
        {
            var reviewer=_mapper.Map<List<ReviewerDto>>(_reviewerRepository.GetReviewers());
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(reviewer);
        }
        [HttpGet("{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewer(int reviewerId)
        {
            if(!_reviewerRepository.ExistsReviewer(reviewerId))
                return NotFound();
            var reviewer=_mapper.Map<Reviewer>(_reviewerRepository.GetReviewer(reviewerId));
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(reviewer);
        }

        [HttpGet("{reviewerId}/reviews")]
        public IActionResult GetReviewsByReviewer(int reviewerId)
        {
            if(!_reviewerRepository.ExistsReviewer(reviewerId))
                return NotFound();
            var reviews = _mapper.Map<List<ReviewDto>>(_reviewerRepository.GetReviewsByReviewer(reviewerId));
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(reviews);
        }
        [HttpPost]
        [ProducesResponseType(284)]
        [ProducesResponseType(400)]
        public IActionResult CreateReviewer([FromQuery] int reviewId, [FromBody] ReviewerDto reviewerCreate)
        {
            if (reviewerCreate == null)
            {
                return BadRequest(ModelState);
            }
            var pokemons = _reviewerRepository.GetReviewers().
                Where(p => p.LastName.Trim().ToUpper() == reviewerCreate.LastName.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (pokemons != null)
            {
                ModelState.AddModelError("", "Reviewer already Exists");
                return StatusCode(422, ModelState);
            }
            var reviwerMap = _mapper.Map<Reviewer>(reviewerCreate);
            if (!_reviewerRepository.CreateReviewer(reviewId, reviwerMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(422, ModelState);
            }
            return Ok("Successfully Created ");
        }
        [HttpPut("{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReviewer(int reviewerId, [FromBody] ReviewerDto reviewerUpdate)
        {
            if (reviewerUpdate == null)
                return BadRequest(ModelState);

            if (reviewerId != reviewerUpdate.Id)
                return BadRequest(ModelState);

            if (!_reviewerRepository.ExistsReviewer(reviewerId))
                return NotFound();

            var reviewerMap = _mapper.Map<Reviewer>(reviewerUpdate);
            if (!_reviewerRepository.UpdateReviewer(reviewerMap))
                return BadRequest(ModelState);
            return NoContent();

        }
        [HttpDelete("{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ExistsReviewer(reviewerId))
                return NotFound();

            var reviwerToDelete = _reviewerRepository.GetReviewer(reviewerId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewerRepository.DeleteReviewer(reviwerToDelete))
            {
                ModelState.AddModelError("", "Something went wrong Deleting");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
