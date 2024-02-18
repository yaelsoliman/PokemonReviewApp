using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interface;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly DataContext _context;

        public ReviewerRepository(DataContext context)
        {
           _context = context;
        }

        public bool CreateReviewer(int reviewId, Reviewer reviewer)
        {
            var review = _context.Reviewers.Where(r => r.Id == reviewId).FirstOrDefault();
            var Rev = new Reviewer()
            {
                Reviews=review.Reviews,
            };
            _context.Add(Rev);
            _context.Add(reviewer);
            return Save();
        }

        public bool DeleteReviewer(Reviewer reviewer)
        {
            _context.Remove(reviewer);
            return Save();
        }

        public bool ExistsReviewer(int reviewerId)
        {
            return _context.Reviewers.Any(r=>r.Id == reviewerId);
        }

        public Reviewer GetReviewer(int reviewerId)
        {
            return _context.Reviewers.Where(r => r.Id == reviewerId).Include(e => e.Reviews).FirstOrDefault();
        }

        public ICollection<Reviewer> GetReviewers()
        {
            return _context.Reviewers.OrderBy(r => r.Id).ToList();
        }

        public ICollection<Review> GetReviewsByReviewer(int reviewerId)
        {
           return _context.Reviews.Where(r=>r.Reviewer.Id==reviewerId).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateReviewer(Reviewer reviewer)
        {
            _context.Update(reviewer);
            return Save();
        }
    }
}
