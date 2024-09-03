using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingAppTest.RepositoryTests
{
    [TestFixture]
    public class CategoryRepositoryTests
    {
        private ShoppingAppContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ShoppingAppContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new ShoppingAppContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        [Test]
        public async Task Add_Category_ShouldAddCategory()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new CategoryRepository(context);
            var category = new Category { Name = "Electronics" };

            // Act
            var result = await repository.Add(category);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Name, Is.EqualTo(category.Name));
        }

        [Test]
        public async Task Update_Category_ShouldUpdateCategory()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new CategoryRepository(context);
            var category = new Category { Name = "Electronics" };
            context.Categories.Add(category);
            await context.SaveChangesAsync();

            // Act
            category.Description = "Updated Description";
            var result = await repository.Update(category);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Description, Is.EqualTo("Updated Description"));
        }

        [Test]
        public async Task Delete_Category_ShouldDeleteCategory()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new CategoryRepository(context);
            var category = new Category { Name = "Electronics" };
            context.Categories.Add(category);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.Delete(category.CategoryID);

            // Assert
            Assert.NotNull(result);
            Assert.ThrowsAsync<NotFoundException>(async () => await repository.Get(category.CategoryID));
        }

        [Test]
        public async Task Get_Category_ShouldReturnCategory()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new CategoryRepository(context);
            var category = new Category { Name = "Electronics" };
            context.Categories.Add(category);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.Get(category.CategoryID);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Name, Is.EqualTo(category.Name));
        }

        [Test]
        public async Task Get_Category_ShouldThrowNotFoundException()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new CategoryRepository(context);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(async () => await repository.Get(999));
        }

        [Test]
        public async Task GetCategoryByName_ShouldReturnCategory()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new CategoryRepository(context);
            var category = new Category { Name = "Electronics" };
            context.Categories.Add(category);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetCategoryByName(category.Name);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Name, Is.EqualTo(category.Name));
        }

        [Test]
        public async Task GetCategoryByName_ShouldThrowNotFoundException()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new CategoryRepository(context);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(async () => await repository.GetCategoryByName("NonExistingCategory"));
        }

        [Test]
        public async Task GetAll_Categories_ShouldReturnAllCategories()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new CategoryRepository(context);
            context.Categories.AddRange(
                new Category { Name = "Electronics" },
                new Category { Name = "Clothing" });
            await context.SaveChangesAsync();

            // Act
            var result = await repository.Get();

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count(), Is.EqualTo(2));
        }
    }
}
