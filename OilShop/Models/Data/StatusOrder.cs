using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OilShop.Models.Data
{
    public class StatusOrder
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ИД")]
        public short Id { get; set; }

        [Display(Name = "Статус заказа")]
        public string Status { get; set; }

        //навигационное свойство
        [Required]
        public ICollection<Order> Orders { get; set; }
    }
}
