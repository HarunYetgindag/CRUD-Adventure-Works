namespace CRUD.AWDB.WEBAPI.Dtos
{
    public class ListDto
    {
        public int BusinessEntityID { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string JobTitle { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneNumberType { get; set; }
        public string EmailAddress { get; set; }
    }
}
