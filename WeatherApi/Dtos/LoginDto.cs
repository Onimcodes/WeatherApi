using System.ComponentModel.DataAnnotations;

namespace WeatherApi.Dtos
{
    public class LoginDto
    {
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
