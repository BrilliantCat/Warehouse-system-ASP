using ASP.Models;
using ASP.Services;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Controllers
{
    /// <summary>
    /// Контроллер для управления товарами на складе и складскими операциями.
    /// </summary>
    public class ProductController : Controller
    {
        private readonly ProductContext _context;

        public ProductController(ProductContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Отображает главную страницу со списком товаров с возможностью поиска, сортировки и пагинации.
        /// </summary>
        public IActionResult Index(int? editId, string? searchString, string? sortBy, int page = 1)
        {
            ViewBag.editId = editId;
            var items = GetFilteredItems(searchString, sortBy, page);
            return View(items);
        }

        /// <summary>
        /// Страница добавления нового товара.
        /// </summary>
        public IActionResult NewItem()
        {
            return View();
        }

        /// <summary>
        /// Обработчик добавления нового товара с валидацией.
        /// </summary>
        [HttpPost]
        public IActionResult AddItem(Item item)
        {
            // Проверка валидации модели
            if (!ModelState.IsValid)
            {
                return View("NewItem", item);
            }

            // Проверка уникальности названия товара
            var tempItem = _context.Items.FirstOrDefault(x => x.Name == item.Name);
            if (tempItem != null)
            {
                ModelState.AddModelError("Name", "Товар с таким названием уже существует");
                return View("NewItem", item);
            }

            _context.Items.Add(item);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Обработчик удаления товара.
        /// </summary>
        [HttpPost]
        public IActionResult DeleteItem(int id, string? searchString, string? sortBy, int page = 1)
        {
            var item = _context.Items.Find(id);
            if (item != null)
            {
                _context.Items.Remove(item);
                _context.SaveChanges();
            }
            return RedirectToAction("Index", new { searchString, sortBy, page });
        }

        /// <summary>
        /// Обработчик сохранения изменений при редактировании товара.
        /// </summary>
        [HttpPost]
        public IActionResult EditItem(Item newItem, string? searchString, string? sortBy, int page = 1)
        {
            // Если модель невалидна, возвращаем страницу с открытой формой редактирования текущей строки
            if (!ModelState.IsValid)
            {
                ViewBag.editId = newItem.IdItem;
                return View("Index", GetFilteredItems(searchString, sortBy, page));
            }

            // Проверяем уникальность названия среди ДРУГИХ товаров (исключая текущий)
            var tempItem = _context.Items.FirstOrDefault(x => x.Name == newItem.Name && x.IdItem != newItem.IdItem);
            if (tempItem != null)
            {
                ModelState.AddModelError("Name", "Товар с таким названием уже существует");
                ViewBag.editId = newItem.IdItem;
                return View("Index", GetFilteredItems(searchString, sortBy, page));
            }

            var item = _context.Items.Find(newItem.IdItem);
            if (item != null)
            {
                item.Name = newItem.Name;
                item.Description = newItem.Description;
                item.Price = newItem.Price;
                // Количество не редактируется напрямую в этой форме для сохранения истории транзакций, 
                // но передается скрытым полем для прохождения валидации модели.
                item.Quantity = newItem.Quantity;
                item.LastModified = DateTime.Now;
                
                _context.SaveChanges();
                return RedirectToAction("Index", new { searchString, sortBy, page });
            }

            return RedirectToAction("Index", new { searchString, sortBy, page });
        }

        /// <summary>
        /// Расход товара (Экспорт со склада).
        /// </summary>
        [HttpPost]
        public IActionResult ExportItem(string Name, int? Quantity, string? searchString, string? sortBy, int page = 1)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                ModelState.AddModelError("ExportNameError", "Необходимо выбрать или указать название товара");
                return View("Index", GetFilteredItems(searchString, sortBy, page));
            }

            if (Quantity == null || Quantity <= 0)
            {
                ModelState.AddModelError("ExportQuantityError", "Количество должно быть больше нуля");
                return View("Index", GetFilteredItems(searchString, sortBy, page));
            }

            var tempItem = _context.Items.FirstOrDefault(x => x.Name == Name);
            if (tempItem == null)
            {
                ModelState.AddModelError("ExportNameError", $"Товар '{Name}' не найден на складе");
                return View("Index", GetFilteredItems(searchString, sortBy, page));
            }

            if (tempItem.Quantity < Quantity.Value)
            {
                ModelState.AddModelError("ExportQuantityError", $"Недостаточно товара на складе (в наличии: {tempItem.Quantity})");
                return View("Index", GetFilteredItems(searchString, sortBy, page));
            }

            // Уменьшаем количество товара
            tempItem.Quantity -= Quantity.Value;
            tempItem.LastModified = DateTime.Now;
            
            // Добавляем запись в лог операций (храним положительное значение изменения, тип - "Export")
            _context.ItemOperations.Add(new ItemOperation(tempItem.IdItem, Quantity.Value, "Export"));
            _context.SaveChanges();

            return RedirectToAction("Index", new { searchString, sortBy, page });
        }

        /// <summary>
        /// Приход товара (Импорт на склад).
        /// </summary>
        [HttpPost]
        public IActionResult ImportItem(string Name, int? Quantity, string? searchString, string? sortBy, int page = 1)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                ModelState.AddModelError("ImportNameError", "Необходимо выбрать или указать название товара");
                return View("Index", GetFilteredItems(searchString, sortBy, page));
            }

            if (Quantity == null || Quantity <= 0)
            {
                ModelState.AddModelError("ImportQuantityError", "Количество должно быть больше нуля");
                return View("Index", GetFilteredItems(searchString, sortBy, page));
            }

            var tempItem = _context.Items.FirstOrDefault(x => x.Name == Name);
            if (tempItem == null)
            {
                ModelState.AddModelError("ImportNameError", $"Товар '{Name}' не найден. Создайте его сначала кнопкой 'Добавить'.");
                return View("Index", GetFilteredItems(searchString, sortBy, page));
            }

            // Увеличиваем количество товара
            tempItem.Quantity += Quantity.Value;
            tempItem.LastModified = DateTime.Now;
            
            // Добавляем запись в лог операций (храним положительное значение изменения, тип - "Import")
            _context.ItemOperations.Add(new ItemOperation(tempItem.IdItem, Quantity.Value, "Import"));
            _context.SaveChanges();

            return RedirectToAction("Index", new { searchString, sortBy, page });
        }

        /// <summary>
        /// Отображает лог складских операций.
        /// </summary>
        public IActionResult Operations()
        {
            // Загружаем список товаров и список операций для сопоставления названий товаров по ID
            var operations = new TwoLists(_context.Items.ToList(), _context.ItemOperations.ToList());
            return View(operations);
        }

        /// <summary>
        /// Приватный вспомогательный метод для фильтрации, сортировки и пагинации товаров.
        /// </summary>
        private List<Item> GetFilteredItems(string? searchString, string? sortBy, int page)
        {
            ViewBag.SearchString = searchString;
            ViewBag.SortBy = sortBy;
            ViewBag.AllItems = _context.Items.OrderBy(x => x.Name).ToList();

            var itemsQuery = _context.Items.AsQueryable();

            // Поиск по подстроке в названии или описании товара
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                itemsQuery = itemsQuery.Where(x => (x.Name != null && x.Name.Contains(searchString)) || 
                                                   (x.Description != null && x.Description.Contains(searchString)));
            }

            // Выбор направления и поля сортировки
            itemsQuery = sortBy switch
            {
                "name_desc" => itemsQuery.OrderByDescending(x => x.Name),
                "price_asc" => itemsQuery.OrderBy(x => x.Price),
                "price_desc" => itemsQuery.OrderByDescending(x => x.Price),
                "quantity_asc" => itemsQuery.OrderBy(x => x.Quantity),
                "quantity_desc" => itemsQuery.OrderByDescending(x => x.Quantity),
                "date_asc" => itemsQuery.OrderBy(x => x.LastModified),
                "date_desc" => itemsQuery.OrderByDescending(x => x.LastModified),
                _ => itemsQuery.OrderBy(x => x.Name) // Сортировка по имени по умолчанию
            };

            // Параметры пагинации
            int pageSize = 10;
            int totalItems = itemsQuery.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            if (page < 1) page = 1;
            if (page > totalPages && totalPages > 0) page = totalPages;

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            // Извлечение нужной страницы данных
            return itemsQuery.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }
    }

    /// <summary>
    /// Вспомогательный класс для передачи списков товаров и операций в представление истории операций.
    /// </summary>
    public class TwoLists(List<Item> items, List<ItemOperation> operations)
    {
        public List<Item> Items { get; set; } = items;
        public List<ItemOperation> Operations { get; set; } = operations;
    }
}
