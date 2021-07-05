﻿using Movies.Data.DataAccess.Interfaces;
using Movies.Data.Models;
using MoviesDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Movies.Data.DataAccess.Repositiories
{
    public class ReviewerRepositoty: GenericRepository<Reviewer>, IReviewerRepository
    {

        public ReviewerRepositoty(MoviesDBContext moviesDBContext): base(moviesDBContext)
        {

        }

        public async Task<IEnumerable<Reviewer>> GetAllReviewersWithAllAsync()
        {
            var reviewers = await context.Reviewers
                .Include(x => x.Reviews)
                .Include(x => x.Movies)
                .ToListAsync();

            return reviewers;
        }

        public async Task<Reviewer> GetReviewerWithAllAsync(int reviewerId)
        {
            var reviewer = await context.Reviewers.FindAsync(reviewerId);

            if (reviewer == null)
            {
                throw new InvalidOperationException($"Reviewer not found in database with id: {reviewerId}");
            }

            await context.Entry(reviewer).Collection(x => x.Movies).LoadAsync();
            await context.Entry(reviewer).Collection(x => x.Reviews).LoadAsync();

            return reviewer;
        }

        public async Task<Reviewer> GetReviewerWithMoviesAsync(int reviewerId)
        {
            var reviewer = await context.Reviewers.FindAsync(reviewerId);
            await context.Entry(reviewer).Collection(x => x.Movies).LoadAsync();            

            return reviewer;
        }

        public async Task<Reviewer> GetReviewerWithReviewsAsync(int reviewerId)
        {
            var reviewer = await context.Reviewers.FindAsync(reviewerId);           
            await context.Entry(reviewer).Collection(x => x.Reviews).LoadAsync();

            return reviewer;
        }
    }
}
