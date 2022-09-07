using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using app.Data;
using app.Models;
using Microsoft.AspNetCore.Identity;

namespace app.Services
{
    public interface IDishService
    {
        Task<Dish[]> GetDishes(ApplicationUser user);
        Task<Dish[]> GetDishes();

        //getbyID
        Task<Dish> GetDish(Guid id);

        Task<bool> AddDishAsync(Dish newDish, ApplicationUser user);

        Task<bool> RemoveDishAsync(Guid id);

        Task<bool> UpdateDishAsync(Dish dish);


    }

    public class DishService : IDishService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILoggerService _logger;

        public DishService(ApplicationDbContext context, ILoggerService logger) {
            _context = context;
            _logger = logger;
        }

        public async Task<Dish[]> GetDishes(ApplicationUser user)
        {
            var items = await _context.Properties
                .Where(x => x.UserId == user.Id)
                .ToArrayAsync();
            return items;
        }

        public async Task<Dish[]> GetDishes()
        {
            var items = await _context.Properties
                .ToArrayAsync();
            return items;
        }

        public async Task<bool> AddDishAsync(Dish newDish, ApplicationUser user)
        {
            await _logger.Log(LogLevel.Debug, $"Adding dish {newDish.Title}", user);
            newDish.Id = Guid.NewGuid();
            newDish.Created = DateTimeOffset.Now;
            newDish.UserId = user.Id;

            _context.Properties.Add(newDish);

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }

        public async Task<Dish> GetDish(Guid id){

            var item = await _context.Properties.FirstOrDefaultAsync(x => x.Id == id);
            return item;
        }

        public async Task<bool> RemoveDishAsync(Guid id)
        {
            var item = await _context.Properties
                .FirstOrDefaultAsync(x => x.Id == id);
            if (item == null)
            {
                return false;
            }
            _context.Properties.Remove(item);
            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }

        public async Task<bool> UpdateDishAsync(Dish dish)
        {
            var item = await _context.Properties
                .FirstOrDefaultAsync(x => x.Id == dish.Id);
            if (item == null)
            {
                return false;
            }
            item.Title = dish.Title;
            item.Description = dish.Description;
            item.Price = dish.Price;
            item.Rooms = dish.Rooms;

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }
    }
}
