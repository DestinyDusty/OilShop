using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OilShop.Models.Data
{
    public class OilInStock
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ИД")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите цену закупки")]
        [Display(Name = "Цена закупки")]
        [Column("decimal(10,2)")]
        public decimal PurchasePrice { get; set; }

        [Required(ErrorMessage = "Введите дату закупки")]
        [Display(Name = "Дата закупки")]
        [DataType(DataType.Date)]
        public DateTime PurchaseDate { get; set; }

        [Required(ErrorMessage = "Введите наценку")]
        [Display(Name = "Наценка")]
        public int Margin { get; set; }

        [Required]
        [Display(Name = "Масло")]
        public int IdOil { get; set; }

        //навигационное свойство
        [ForeignKey("IdOil")]
        [Display(Name = "Масло")]
        public Oil Oil { get; set; }
    }
}
