using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interface
{
    public interface IReviewRepository
    {
        ICollection<Review> GetReviews();
        Review GetReview(int reviewId);
        ICollection<Review> GetReviewsByPokemon(int pokeId);
        bool ExistsReviews(int reviewId);

        bool CreateReview( Review review);
        bool UpdateReview(Review review);
        bool DeleteReview(Review review);
        bool DeleteReviews(List<Review> reviews);
        bool Save();
    }
}
