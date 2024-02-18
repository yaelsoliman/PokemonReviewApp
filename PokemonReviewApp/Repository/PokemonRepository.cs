using PokemonReviewApp.Data;
using PokemonReviewApp.Interface;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly DataContext _context;

        public PokemonRepository(DataContext Context)
        {
            _context = Context;
        }

        public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            var pokemonOwnersEntity = _context.Owners.Where(o => o.Id == ownerId).FirstOrDefault();
            var category=_context.Categories.Where(c=>c.Id== categoryId).FirstOrDefault();

            var Pokemonowner = new PokemonOwner()
            {
                Owner=pokemonOwnersEntity,
                Pokemon=pokemon
            };
            _context.Add(Pokemonowner);

            var Pokemoncategory = new PokemonCategory()
            {
                Category=category,
                Pokemon=pokemon
            };
            _context.Add(Pokemoncategory);
            _context.Add(pokemon);
            return Save();

        }

        public bool DeletePokemon(Pokemon pokemon)
        {
            _context.Remove(pokemon);
            return Save();
        }

        public Pokemon GetPokemon(int id)
        {
            return _context.Pokemons.Where(p => p.Id == id).FirstOrDefault();
        }

        public Pokemon GetPokemon(string name)
        {
            return _context.Pokemons.Where(p => p.Name == name).FirstOrDefault();
        }

        public decimal GetPokemonRating(int pokeId)
        {
            var review = _context.Reviews.Where(p => p.Pokemon.Id == pokeId);

            if(review.Count() <= 0)  
                  return 0;
            
            return ((decimal)review.Sum(r=>r.Rating)/review.Count());
            
           
        }

        public ICollection<Pokemon> GetPokemons()
        {
          return _context.Pokemons.OrderBy(p=>p.Id).ToList();
        }

        public bool PokemonExists(int pokeId)
        {
           return _context.Pokemons.Any(p=>p.Id == pokeId);
        }

        public bool Save()
        {
            var saved=_context.SaveChanges();
            return saved>0 ? true : false;
        }

        public bool UpdatePokemon(Pokemon pokemon)
        {
            _context.Update(pokemon);
            return Save();
        }
    }
}
