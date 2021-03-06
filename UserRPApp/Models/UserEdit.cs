﻿using System.ComponentModel.DataAnnotations;

namespace UserRPApp.Models
{
    public class UserEdit
    {
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Используйте только латинские буквы")]
        public string Login { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^\d{1}\-\d{3}\-\d{7}$", ErrorMessage = "Номер телефона должен быть в формате: X-XXX-XXXXXXX")]
        public string Telephone { get; set; }

        [Required]
        public bool IsAdmin { get; set; }
    }
}
