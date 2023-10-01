using System.Data;

namespace web_product.Models
{
    public class Autorize
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
        public Autorize(string email, string password, Role role)
        {
            Email = email;
            Password = password;
            Role = role;
           
        }
        
    }
    public class Role
    {
        public string Name { get; set; }
        public Role(string name) => Name = name;
    }
}

