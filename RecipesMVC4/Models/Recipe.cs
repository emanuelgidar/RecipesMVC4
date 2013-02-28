using System.ComponentModel.DataAnnotations;
using System;

namespace RecipesMVC4.Models
{
    public class Recipe
    {
        public Guid ID { get; set; }

        [Required(ErrorMessage = "Title Required")]
        [StringLength(25, ErrorMessage = "Must be less than 25 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description Required")]
        public string Description { get; set; }
        public bool isIncorrect { get; set; }

        public override bool Equals(object obj)
        {
            Recipe value = (obj as Recipe);
            if (value == null)
                return false;

            return this.ID.Equals(value.ID);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}