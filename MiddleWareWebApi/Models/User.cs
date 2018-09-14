namespace MiddleWareWebApi.Models
{
    /// <summary>
    /// 1. Логин
    /// 
    /// 2. Пароль
    /// 
    /// 3. Фамилия
    /// 
    /// 4. Имя
    /// 
    /// 5. Отчество
    /// 
    /// 6. Телефон
    /// 
    /// 7. Роль
    /// </summary>
    public class User
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Telephone { get; set; }
        
        public bool IsAdmin { get; set; }

    }
}
