using CRUD.AWDB.WEBAPI.Contexts;
using CRUD.AWDB.WEBAPI.Dtos;
using CRUD.AWDB.WEBAPI.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CRUD.AWDB.WEBAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdventureWorksController : ControllerBase
    {

        private readonly AdventureWorksContext _context;
        public AdventureWorksController(AdventureWorksContext context)
        {
            _context = context;
        }

       
        [HttpGet(Name = "GetPersons")]
        public List<ListDto> GetPersons()
        {
            var result = (from data in _context.Employees
                          join p in _context.People on data.BusinessEntityId equals p.BusinessEntityId
                          join b in _context.BusinessEntityAddresses on p.BusinessEntityId equals b.BusinessEntityId
                          join a in _context.Addresses on b.AddressId equals a.AddressId
                          join s in _context.StateProvinces on a.StateProvinceId equals s.StateProvinceId
                          join c in _context.CountryRegions on s.CountryRegionCode equals c.CountryRegionCode

                          join pp in _context.PersonPhones on p.BusinessEntityId equals pp.BusinessEntityId into pp2
                          from pp in pp2.DefaultIfEmpty()
                          join ppt in _context.PhoneNumberTypes on pp.PhoneNumberTypeId equals ppt.PhoneNumberTypeId into ppt2
                          from ppt in ppt2.DefaultIfEmpty()
                          join email in _context.EmailAddresses on p.BusinessEntityId equals email.BusinessEntityId into email2
                          from email in email2.DefaultIfEmpty()
                          select new ListDto
                          {
                              BusinessEntityID = data.BusinessEntityId,
                              FirstName = p.FirstName,
                              JobTitle = data.JobTitle,
                              LastName = p.LastName,
                              MiddleName = p.MiddleName,
                              Title = p.Title,
                              Suffix = p.Suffix,
                              PhoneNumber = pp.PhoneNumber,
                              EmailAddress = email.EmailAddress1,
                              PhoneNumberType = ppt.Name

                          }).ToList();
            return result;

        }



    }
}