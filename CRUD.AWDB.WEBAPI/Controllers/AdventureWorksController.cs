using CRUD.AWDB.WEBAPI.Contexts;
using CRUD.AWDB.WEBAPI.Dtos;
using CRUD.AWDB.WEBAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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


        [HttpGet]
        [Route("GetPeople")]
        public List<ListDto> GetPeople()
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

                          }).OrderByDescending(x => x.BusinessEntityID).ToList();
            return result;

        }


        [HttpPost]
        [Route("AddPerson")]

        public IActionResult AddPerson(CreateDto model)
        {
            if (ModelState.IsValid)
            {
                var BusinessId = new BusinessEntity()
                {
                    ModifiedDate = DateTime.Now,
                    rowguid = Guid.NewGuid(),
                };

                // Primary Key için veri giriþinin yapýlmasý gereken tablo
                _context.BusinessEntity.Add(BusinessId);
                _context.SaveChanges();

                _context.People.Add(new Person()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    MiddleName = model.MiddleName,
                    NameStyle = true,
                    EmailPromotion = 1,
                    ModifiedDate = DateTime.Now,
                    Rowguid = Guid.NewGuid(),
                    Suffix = model.Suffix,
                    PersonType = model.PersonType,
                    Title = model.Title,
                    BusinessEntityId = BusinessId.BusinessEntityID
                });

                _context.SaveChanges();


                return Created("", model);
            }
            return BadRequest();
        }


        [HttpPut]
        [Route("UpdatePerson")]
        public IActionResult UpdatePerson(UpdateDto model)
        {
            if (ModelState.IsValid)
            {
                var getData = _context.People.First(x => x.BusinessEntityId == model.BusinessEntityId);

                if (getData is not null)
                {

                    getData.FirstName = model.FirstName;
                    getData.LastName = model.LastName;
                    getData.MiddleName = model.MiddleName;
                    getData.ModifiedDate = DateTime.Now;
                    getData.Suffix = model.Suffix;
                    getData.PersonType = model.PersonType;
                    getData.Title = model.Title;

                    _context.SaveChanges();

                    return Ok(getData);
                }

                return NotFound();
            }
            return BadRequest();
        }


        [HttpDelete]
        [Route("DeletePerson/{Id}")]
        public IActionResult DeletePerson(int Id)
        {
            if (Id != 0)
            {
                var getData = _context.People.First(x => x.BusinessEntityId == Id);
                var getDataBusinessEntity = _context.BusinessEntity.First(x => x.BusinessEntityID == Id);

                if (getData is not null && getDataBusinessEntity is not null)
                {

                    _context.People.Remove(getData);

                    _context.SaveChanges();
                    _context.BusinessEntity.Remove(getDataBusinessEntity);
                    _context.SaveChanges();
                    return Ok();
                }

                return NotFound();
            }

            return BadRequest();
        }

        [HttpGet("{Id}")]

        public IActionResult GetById(int Id)
        {
            if (Id != 0)
            {

                var getData = _context.People.First(x => x.BusinessEntityId == Id);

                if (getData is not null)
                {
                    return Ok(new UpdateDto()
                    {
                        FirstName = getData.FirstName,
                        LastName = getData.LastName,
                        MiddleName = getData.MiddleName,
                        PersonType = getData.PersonType,
                        Suffix = getData.Suffix,
                        Title = getData.Title,
                        BusinessEntityId = getData.BusinessEntityId,
                    });

                }
                return NotFound();
            }

            return BadRequest();
        }

    }
}