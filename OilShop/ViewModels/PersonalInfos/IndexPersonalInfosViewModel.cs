using System.ComponentModel.DataAnnotations;

namespace OilShop.ViewModels.PersonalInfos
{
    public class IndexPersonalInfosViewModel
    {
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }

        public string Code { get; set; }
    }
}
