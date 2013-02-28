using System.ComponentModel.DataAnnotations;
using System;

namespace RecipesMVC4.Models
{
    public class User
    {
        public Guid ID { get; set; }

        [Required(ErrorMessage = "Name Required")]
        [StringLength(33, ErrorMessage = "Must be less than 25 characters")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Username Required")]
        [StringLength(25, ErrorMessage = "Must be less than 25 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password Required")]
        [StringLength(25, ErrorMessage = "Must be less than 25 characters")]
        public string Password { get; set; }
        
        public override bool Equals(object obj)
        {
            User value = (obj as User);
            if (value == null)
                return false;

            return this.Username.Equals(value.Username);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}