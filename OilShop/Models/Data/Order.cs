using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OilShop.Models.Data
{
    public class Order
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ИД")]
        public int Id { get; set; }

        [Display(Name = "Пользователь")]
        public string IdUser { get; set; }

        [Required]
        [Display(Name = "Масло")]
        public int IdOil { get; set; }

        [Required(ErrorMessage = "Введите кол-во")]
        [Display(Name = "Кол-во")]
        public short Quantity { get; set; }

        [Required]
        [Display(Name = "Адрес доставки")]
        public short IdAddressDelivery { get; set; }

        [Required(ErrorMessage = "Введите дату заказа")]
        [Display(Name = "Дата заказа")]
        [DataType(DataType.Date)]
        public DateTime DateOrder { get; set; }

        [Required(ErrorMessage = "Введите дату статуса")]
        [Display(Name = "Дата статуса")]
        [DataType(DataType.Date)]
        public DateTime DateStatus { get; set; }

        [Required]
        [Display(Name = "Статус заказа")]
        public short IdStatusOrder { get; set; }

        
        //навигационные свойства
        [ForeignKey("IdUser")]
        public User User { get; set; }

        [ForeignKey("IdOil")]
        public Oil Oil { get; set; }

        [ForeignKey("IdAddressDelivery")]
        public AddressDelivery AddressDelivery  { get; set; }

        [ForeignKey("IdStatusOrder")]
        public StatusOrder StatusOrder { get; set; }

    }
}
