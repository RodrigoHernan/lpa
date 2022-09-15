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

        bool DishExists(Guid id);
    }

    public class DishService : IDishService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILoggerService _logger;

        private readonly IWebHostEnvironment _IWebHostEnvironment;

        public DishService(ApplicationDbContext context, ILoggerService logger, IWebHostEnvironment IWebHostEnvironment) {
            _context = context;
            _logger = logger;
            _IWebHostEnvironment = IWebHostEnvironment;
        }

        public async Task<Dish[]> GetDishes(ApplicationUser user)
        {
            var items = await _context.Dishes
                .Where(x => x.UserId == user.Id)
                .ToArrayAsync();
            return items;
        }

        public async Task<Dish[]> GetDishes()
        {
            var items = await _context.Dishes
                .ToArrayAsync();
            return items;
        }

        public async Task<bool> AddDishAsync(Dish newDish, ApplicationUser user)
        {
            await saveImage(newDish);
            await _logger.Log(LogLevel.Debug, $"Adding dish {newDish.Title}", user);
            newDish.Id = Guid.NewGuid();
            newDish.Created = DateTimeOffset.Now;
            newDish.UserId = user.Id;

            _context.Dishes.Add(newDish);

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }

        private async Task<bool> saveImage(Dish newDish)
        {
            //save image to wwwroot/image
            string wwwRootPath = _IWebHostEnvironment.WebRootPath;
            string filename = Path.GetFileNameWithoutExtension(newDish.ImageFile.FileName);
            string extension = Path.GetExtension(newDish.ImageFile.FileName);

            filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
            newDish.ImageName = filename;
            string path = Path.Combine(wwwRootPath + "/images/", filename);

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await newDish.ImageFile.CopyToAsync(fileStream);
            }
            return true;
        }

        public async Task<Dish> GetDish(Guid id){

            var item = await _context.Dishes.FirstOrDefaultAsync(x => x.Id == id);
            return item;
        }

        public async Task<bool> RemoveDishAsync(Guid id)
        {
            var item = await _context.Dishes
                .FirstOrDefaultAsync(x => x.Id == id);
            if (item == null)
            {
                return false;
            }
            _context.Dishes.Remove(item);
            var saveResult = await _context.SaveChangesAsync();
            // delete image from wwwroot/image
            var imagePath = Path.Combine(_IWebHostEnvironment.WebRootPath, "images", item.ImageName);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);
            return saveResult == 1;
        }

        public async Task<bool> UpdateDishAsync(Dish dish)
        {
            var item = await _context.Dishes
                .FirstOrDefaultAsync(x => x.Id == dish.Id);
            if (item == null)
            {
                return false;
            }
            if (dish.ImageFile != null)
            {
                await saveImage(dish);
                item.ImageName = dish.ImageName;
            } else
            {
                dish.ImageName = item.ImageName;
            }
            item.Title = dish.Title;
            item.Description = dish.Description;
            item.Price = dish.Price;

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }

        public bool DishExists(Guid id)
        {
            var Dish = GetDish(id);
            if (Dish == null)
            {
                return false;
            }
            return true;
        }
    }
}
