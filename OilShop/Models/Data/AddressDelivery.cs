using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OilShop.Models.Data
{
    public class AddressDelivery
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ИД")]
        public short Id { get; set; }

        [Required(ErrorMessage = "Введите улицу")]
        [Display(Name = "Поставщик масла")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Введите номер дома")]
        [Display(Name = "Номер дома")]
        public string House { get; set; }

        [Display(Name = "Пользователь")]
        public string IdUser { get; set; }

        //навигационное свойство
        [Required]
        public ICollection<Order> Orders { get; set; }

        [ForeignKey("IdUser")]
        public User User { get; set; }
    }
}
