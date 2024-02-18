using PokemonReviewApp.Data;
using PokemonReviewApp.Models;

public class Seed
{
    private readonly DataContext dataContext;
    public Seed(DataContext context)
    {
        this.dataContext = context;
    }
    public void SeedDataContext()
    {
        if (!dataContext.PokemonOwners.Any())
        {
            var pokemonOwners = new List<PokemonOwner>()
            {
                    new PokemonOwner()
                    {
                        Pokemon = new Pokemon()
                        {
                            Name = "Pikachu",
                            BirthDate = new DateTime(1903,1,1),
                            PokemonCategories = new List<PokemonCategory>()
                            {
                                new PokemonCategory { Category = new Category() { Name = "Electric"}}
                            },
                            Reviews = new List<Review>()
                            {
                                new Review {Id=1, Title="Pikachu",Text = "Pickahu is the best pokemon, because it is electric",Rating=5,
                                Reviewer = new Reviewer(){ FirstName = "Teddy", LastName = "Smith" } },
                                new Review {Id=2, Title="Pikachu", Text = "Pickachu is the best a killing rocks",Rating=4,
                                Reviewer = new Reviewer(){ FirstName = "Taylor", LastName = "Jones" } },
                                new Review {Id=3, Title="Pikachu",Text = "Pickchu, pickachu, pikachu",Rating=3,
                                Reviewer = new Reviewer(){ FirstName = "Jessica", LastName = "McGregor" } },
                            }
                        },
                        Owner = new Owner()
                        {
                            FirstName = "Jack",
                            LastName = "London",
                            Gym = "Brocks Gym",
                            Country = new Country()
                            {
                                Name = "Kanto"
                            }
                        }
                    },
                    new PokemonOwner()
                    {
                        Pokemon = new Pokemon()
                        {
                            Name = "Squirtle",
                            BirthDate = new DateTime(1903,1,1),
                            PokemonCategories = new List<PokemonCategory>()
                            {
                                new PokemonCategory { Category = new Category() { Name = "Water"}}
                            },
                            Reviews = new List<Review>()
                            {
                                new Review { Id=4, Title= "Squirtle", Text = "squirtle is the best pokemon, because it is electric",Rating=5,
                                Reviewer = new Reviewer(){ FirstName = "Teddy", LastName = "Smith" } },
                                new Review { Id=5, Title= "Squirtle",Text = "Squirtle is the best a killing rocks",Rating=4,
                                Reviewer = new Reviewer(){ FirstName = "Taylor", LastName = "Jones" } },
                                new Review { Id=6, Title= "Squirtle", Text = "squirtle, squirtle, squirtle",Rating=3,
                                Reviewer = new Reviewer(){ FirstName = "Jessica", LastName = "McGregor" } },
                            }
                        },
                        Owner = new Owner()
                        {
                            FirstName = "Harry",
                            LastName = "Potter",
                            Gym = "Mistys Gym",
                            Country = new Country()
                            {
                                Name = "Saffron City"
                            }
                        }
                    },
                     new PokemonOwner()
                     {
                        Pokemon = new Pokemon()
                        {
                            Name = "Venasuar",
                            BirthDate = new DateTime(1903,1,1),
                            PokemonCategories = new List<PokemonCategory>()
                            {
                                new PokemonCategory { Category = new Category() { Name = "Leaf"}}
                            },
                            Reviews = new List<Review>()
                            {
                                new Review { Id=7, Title="Veasaur",Text = "Venasuar is the best pokemon, because it is electric",Rating=5,
                                Reviewer = new Reviewer(){ FirstName = "Teddy", LastName = "Smith" } },
                                new Review {  Id=8,Title="Veasaur",Text = "Venasuar is the best a killing rocks",Rating=4,
                                Reviewer = new Reviewer(){ FirstName = "Taylor", LastName = "Jones" } },
                                new Review { Id=9, Title="Veasaur",Text = "Venasuar, Venasuar, Venasuar",Rating=3,
                                Reviewer = new Reviewer(){ FirstName = "Jessica", LastName = "McGregor" } },
                            }
                        },
                        Owner = new Owner()
                        {
                            FirstName = "Ash",
                            LastName = "Ketchum",
                            Gym = "Ashs Gym",
                            Country = new Country()
                            {
                                Name = "Millet Town"
                            }
                        }
                     }
            };
            dataContext.PokemonOwners.AddRange(pokemonOwners);
            dataContext.SaveChanges();
        }
    }
}
