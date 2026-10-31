using System.ComponentModel.DataAnnotations;
namespace ASP.Models
{
    public class ItemOperation(int idItem, int quantityChange, string operationType)
    {
        [Key]
     public int IdItemOperation { get; set; }
     public int IdItem { get; set; } = idItem;
     public DateTime Date { get; set; } = DateTime.Now;

     public int QuantityChange { get; set; } = quantityChange;
     public string OperationType { get; set; } = operationType;

     public ItemOperation() : this(0, 0, "") { }
    }
}