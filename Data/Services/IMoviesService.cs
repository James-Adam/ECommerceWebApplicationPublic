﻿using ECommerceWebApplication.Data.ViewModels;
using ECommerceWebApplication.Data.Base;
using ECommerceWebApplication.Models;

namespace ECommerceWebApplication.Data.Services
{
    public interface IMoviesService : IEntityBaseRepository<Movie>
    {
        Task<Movie> GetMovieByIdAsync(int id);
        Task<NewMovieDropdownsVM> GetNewMovieDropdownsValues();
        Task AddNewMovieAsync(NewMovieVM data);
        Task UpdateMovieAsync(NewMovieVM data);
    }
}