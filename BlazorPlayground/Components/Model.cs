using System.ComponentModel.DataAnnotations;

namespace BlazorPlayground.Components {
    public class Model {
        [Required(ErrorMessage = "Value is required")]
        [MinLength(1, ErrorMessage = "Value is required")]
        public string Value { get; set; }
    }
}
