using PokemonReviewApp.Data;
using PokemonReviewApp.Interface;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext _context;

        public OwnerRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateOwner(Owner owner)
        {
            _context.Add(owner);
            return save();
        }

        public bool DeleteOwner(Owner owner)
        {
            _context.Remove(owner);
            return save();
        }

        public Owner GetOwner(int ownerId)
        {
            return _context.Owners.Where(o => o.Id == ownerId).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnerOfAPokemon(int pokeId)
        {
            return _context.PokemonOwners.Where(p => p.Pokemon.Id == pokeId).Select(p => p.Owner).ToList();
        }

        public ICollection<Owner> GetOwners()
        {
          return _context.Owners.OrderBy(o => o.Id).ToList();
        }

        public ICollection<Pokemon> GetPokemonByOwner(int ownerId)
        {
            return _context.PokemonOwners.Where(p => p.Owner.Id == ownerId).Select(p => p.Pokemon).ToList();
        }

        public bool OwnersExists(int ownerId)
        {
            return _context.Owners.Any(p=>p.Id == ownerId);
        }

        public bool save()
        {
           var saved=_context.SaveChanges();
            return saved>0 ? true: false;
        }

        public bool UpdateOwner(Owner owner)
        {
            _context.Update(owner);
            return save();
        }
    }
}
