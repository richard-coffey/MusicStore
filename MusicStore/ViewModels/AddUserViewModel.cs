using System.ComponentModel.DataAnnotations;

namespace MusicStore.ViewModels
{
    public class AddUserViewModel  : UserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}