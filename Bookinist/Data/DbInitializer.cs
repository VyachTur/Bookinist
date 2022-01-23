using Bookinist.DAL.Context;
using Bookinist.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

#nullable disable

namespace Bookinist.Data
{
    internal class DbInitializer
    {
        private readonly BookinistDB _db;
        private readonly ILogger<DbInitializer> _logger;

        public DbInitializer(BookinistDB db, ILogger<DbInitializer> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            var timer = Stopwatch.StartNew();
            _logger.LogInformation("Инициализация БД...");

            _logger.LogInformation("Удаление существующей БД...");
            await _db.Database.EnsureDeletedAsync()/*ConfigureAwait(false)*/;
            _logger.LogInformation("Удаление существующей БД выполнено за {0} мс", timer.ElapsedMilliseconds);

            //_db.Database.EnsureCreated();

            _logger.LogInformation("Миграция БД...");
            await _db.Database.MigrateAsync();
            _logger.LogInformation("Миграция БД выполнена за {0} мс", timer.ElapsedMilliseconds);

            if (await _db.Books.AnyAsync()) return;

            await InitializeCategories();
            await InitializeBooks();
            await InitializeSellers();
            await InitializeBuyers();
            await InitializeDeals();

            _logger.LogInformation("Инициализация БД выполнена за {0} с", timer.Elapsed.TotalSeconds);
        }


        private const int c_categoriesCount = 10;
        private Category[] _categories;

        private async Task InitializeCategories()
        {
            var timer = Stopwatch.StartNew();
            _logger.LogInformation("Инициализация категорий...");

            _categories = new Category[c_categoriesCount];

            for (int i = 0; i < c_categoriesCount; i++)
                _categories[i] = new Category { Name = $"Категория {i + 1}" };

            await _db.Categories.AddRangeAsync(_categories);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Инициализация категорий выполнена за {0} мс", timer.ElapsedMilliseconds);
        }

        private const int c_booksCount = 10;
        private Book[] _books;

        private async Task InitializeBooks()
        {
            var timer = Stopwatch.StartNew();
            _logger.LogInformation("Инициализация книг...");

            var rnd = new Random();
            _books = Enumerable.Range(1, c_booksCount)
                .Select(i => new Book 
                { 
                    Name = $"Книга {i}",
                    Category = rnd.NextItem(_categories)
                 })
                .ToArray();

            await _db.Books.AddRangeAsync(_books);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Инициализация книг выполнена за {0} мс", timer.ElapsedMilliseconds);
        }

        private const int c_sellersCount = 10;
        private Seller[] _sellers;

        private async Task InitializeSellers()
        {
            var timer = Stopwatch.StartNew();
            _logger.LogInformation("Инициализация продавцов...");

            _sellers = Enumerable.Range(1, c_sellersCount)
                .Select(i => new Seller
                {
                    Name = $"Продавец-Имя {i}",
                    Surname = $"Продавец-Фамилия {i}",
                    Patronymic = $"Продавец-Отчество {i}"
                })
                .ToArray();

            await _db.Sellers.AddRangeAsync(_sellers);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Инициализация продавцов выполнена за {0} мс", timer.ElapsedMilliseconds);
        }


        private const int c_buyersCount = 10;
        private Buyer[] _buyers;

        private async Task InitializeBuyers()
        {
            var timer = Stopwatch.StartNew();
            _logger.LogInformation("Инициализация покупателей...");

            _buyers = Enumerable.Range(1, c_buyersCount)
                .Select(i => new Buyer
                {
                    Name = $"Покупатель-Имя {i}",
                    Surname = $"Покупатель-Фамилия {i}",
                    Patronymic = $"Покупатель-Отчество {i}"
                })
                .ToArray();

            await _db.Buyers.AddRangeAsync(_buyers);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Инициализация покупателей выполнена за {0} мс", timer.ElapsedMilliseconds);
        }

        private const int c_dealsCount = 1000;

        private async Task InitializeDeals()
        {
            var timer = Stopwatch.StartNew();
            _logger.LogInformation("Инициализация сделок...");

            var rnd = new Random();

            var deals = Enumerable.Range(1, c_dealsCount)
            .Select(i => new Deal
            {
                Book = rnd.NextItem(_books),
                Seller = rnd.NextItem(_sellers),
                Buyer = rnd.NextItem(_buyers),
                Price = (decimal)(rnd.NextDouble() * 4000 + 700)
            });

            await _db.Deals.AddRangeAsync(deals);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Инициализация сделок выполнена за {0} мс", timer.ElapsedMilliseconds);
        }

    }
}
