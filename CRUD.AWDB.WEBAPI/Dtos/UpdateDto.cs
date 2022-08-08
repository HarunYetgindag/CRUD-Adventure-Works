using System.ComponentModel.DataAnnotations;

namespace CRUD.AWDB.WEBAPI.Dtos
{
    public class UpdateDto
    {


        [MaxLength(8, ErrorMessage = "{0} 8 karakterden fazla girilemez")]
        public string Title { get; set; }

        [Required(ErrorMessage = "{0} Zorunlu Alan")]
        [MaxLength(50, ErrorMessage = "{0} karakterden fazla girilemez")]
        public string FirstName { get; set; }


        [MaxLength(50, ErrorMessage = "{0} karakterden fazla girilemez")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "{0} Zorunlu Alan")]
        [MaxLength(50, ErrorMessage = "{0} karakterden fazla girilemez")]
        public string LastName { get; set; }

        [MaxLength(10, ErrorMessage = "{0} karakterden fazla girilemez")]
        public string Suffix { get; set; }

        [Required(ErrorMessage = "{0} Zorunlu Alan")]
        public string PersonType { get; set; }

        public int BusinessEntityId { get; set; }
    }
}
