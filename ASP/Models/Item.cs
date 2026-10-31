using System.ComponentModel.DataAnnotations;
namespace ASP.Models
{
    /// <summary>
    /// Класс, представляющий товар на складе.
    /// </summary>
    public class Item(string? name, string? description, int price, int quantity)
    {
        [Key]
        public int IdItem { get; set; }

        [Required(ErrorMessage = "Название товара обязательно для заполнения")]
        [Display(Name = "Название товара")]
        [StringLength(100, ErrorMessage = "Название не должно превышать 100 символов")]
        public string? Name { get; set; } = name;

        [Required(ErrorMessage = "Описание товара обязательно для заполнения")]
        [Display(Name = "Описание")]
        [StringLength(500, ErrorMessage = "Описание не должно превышать 500 символов")]
        public string? Description { get; set; } = description;
        
        [Required(ErrorMessage = "Цена обязательна для заполнения")]
        [Display(Name = "Цена")]
        [Range(0, int.MaxValue, ErrorMessage = "Цена должна быть неотрицательным числом")]
        public int Price { get; set; } = price;
        
        [Required(ErrorMessage = "Количество обязательно для заполнения")]
        [Display(Name = "Количество")]
        [Range(0, int.MaxValue, ErrorMessage = "Количество должно быть неотрицательным числом")]
        public int Quantity { get; set; } = quantity;

        [Display(Name = "Последнее изменение")]
        public DateTime LastModified { get; set; } = DateTime.Now; 

        // Конструктор по умолчанию необходим для работы Entity Framework Core
        public Item() : this(null, null, 0, 0) { }
    }
}