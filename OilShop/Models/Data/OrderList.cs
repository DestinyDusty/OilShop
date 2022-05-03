using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OilShop.Models.Data
{
    public class OrderList
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ИД")]
        public int Id { get; set; }

        [Display(Name = "Заказ")]
        public int IdOrder { get; set; }

        [Display(Name = "ИД масла на складе")]
        public int IdOilInStock { get; set; }

        [Required(ErrorMessage = "Введите количество")]
        [Display(Name = "Количество")]
        public int Amount { get; set; }

        //навигационные свойства
        [ForeignKey("IdOrder")]
        public Order Order { get; set; }

        [ForeignKey("IdOilInStock")]
        public OilInStock OilInStock { get; set; }

        [Required]
        public ICollection<OilInStock> OilsInStock { get; set; }



    }
}
