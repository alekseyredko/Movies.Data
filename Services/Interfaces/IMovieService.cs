﻿using Movies.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Data.Services.Interfaces
{
    public interface IMovieService
    {
        Task AddActorAsync(int movieId, int actorId);
        Task DeleteActorAsync(int movieId, int actorId);
        Task AddMovieAsync(Movie movie);
        Task UpdateMovieAsync(Movie movie);
        Task<Movie> GetMovieAsync(int id);        
        Task<IEnumerable<Actor>> GetMovieActorsAsync(int movieId);
        Task<IEnumerable<Movie>> GetAllMoviesAsync();
        Task<IEnumerable<Movie>> GetAllMoviesWithInfoAsync();
        Task DeleteMovieAsync(int id);
    }
}
