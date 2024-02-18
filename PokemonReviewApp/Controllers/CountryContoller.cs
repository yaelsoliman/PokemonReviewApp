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
    public class CountryContoller : ControllerBase
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CountryContoller(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]

        public IActionResult GetCountries()
        {
            var Countries = _mapper.Map<List<CountryDto>>(_countryRepository.GetCountries());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(Countries);
        }

        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
         public IActionResult GetCountry(int countryId)
         {
            if (!_countryRepository.CountryExisits(countryId))
                return NotFound();

            var country=_mapper.Map<CountryDto>(_countryRepository.GetCountry(countryId));
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(country);
         }

        [HttpGet("/owners/{ownerId}")]
        [ProducesResponseType(200,Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountryByOwner(int ownerId)
        {
            var country = _mapper.Map<CountryDto>(_countryRepository.GetCountryByOwner(ownerId));
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(country);
        }
        [HttpPost]
        [ProducesResponseType(284)]
        [ProducesResponseType(400)]
        public IActionResult CreateCountry([FromBody] CountryDto countryCreate)
        {
            if(countryCreate==null)
                return BadRequest(ModelState);
            var countries=_countryRepository.GetCountries()
                .Where(c=>c.Name.Trim().ToUpper()==countryCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (countries != null)
            {
                ModelState.AddModelError("", "Country already Exists");
                return StatusCode(422, ModelState);
            }

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var countryMap = _mapper.Map<Country>(countryCreate);
            if (!_countryRepository.CreateCountry(countryMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(422, ModelState);
            }   
            return Ok("Created Successfully");
        }
        
        [HttpPut("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int countryId, [FromBody] CountryDto countryUpdate)
        {
            if (countryUpdate == null)
                return BadRequest(ModelState);

            if (countryId != countryUpdate.Id)
                return BadRequest(ModelState);

            if (!_countryRepository.CountryExisits(countryId))
                return NotFound();

            var countryMap = _mapper.Map<Country>(countryUpdate);
            if (!_countryRepository.UpdateCountry(countryMap))
                return BadRequest(ModelState);
            return NoContent();

        }
        [HttpDelete("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCountry(int countryId)
        {
            if (!_countryRepository.CountryExisits(countryId))
                return NotFound();
            var countryToDelete = _countryRepository.GetCountry(countryId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_countryRepository.DeleteCountry(countryToDelete))
            {
                ModelState.AddModelError("", "Something went wrong Deleting");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

    }
}
