using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AQRWebApp.DAL;
using AQRWebApp.Model.Entity;
using AQRWebApp.Model.EntityDto;

namespace AQRWebApp.BL
{
    public class PersonService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        public IEnumerable<Person> GetAll()
        {
            return unitOfWork.PersonRepository.Get(m => m.IsDisabled == false);
        }

        public int GetUserIdByPersonId(int personId)
        {
            var query = unitOfWork.PersonAccountRepository.Get(m => m.Person.Id == personId).FirstOrDefault();
            return query != null ? query.UserId : 0;
        }

        public Person GetPersonByUserId(int userId)
        {
            Person result = null;

            var query = unitOfWork.PersonAccountRepository.Get(m => m.UserId == userId).FirstOrDefault();

            if (query != null && query.Person != null)
                result = query.Person;

            return result;
        }

        public int GetPersonIdByUserId(int userId)
        {
            Person p = GetPersonByUserId(userId);
            return p != null ? p.Id : 0;
        }

        public IEnumerable<PersonAccount> GetAllPersonAccount()
        {
            return unitOfWork.PersonAccountRepository.Get();
        }

        public IEnumerable<PersonDto> GetListOnWorkingOfPersonDto(System.DateTime endedDate, int? departmentId, int? personId)
        {
            return (from c in GetListOnWorkingOfPerson(endedDate, departmentId, personId)
                    select new PersonDto
                    {
                        Id = c.Id,
                        Name = c.Lastname + " " + c.Firstname,
                        Lastname = c.Lastname,
                        Firstname = c.Firstname,
                        GenderId = c.Gender != null ? c.Gender.Id : -1,
                        GenderName = c.Gender != null ? c.Gender.Name : string.Empty,                        
                        Birthday = c.Birthday != null ? c.Birthday.Value.ToString("dd/MM/yyyy") : string.Empty,
                        RecruitedDate = c.RecruitedDate != null ? c.RecruitedDate.Value.ToString("dd/MM/yyyy") : string.Empty,
                        NgayNghi = c.NgayNghi != null ? c.NgayNghi.Value.ToString("dd/MM/yyyy") : string.Empty,
                        ChucDanhId = c.ChucDanh != null ? c.ChucDanh.Id : -1,
                        ChucDanhName = c.ChucDanh != null ? c.ChucDanh.Name : string.Empty,
                        DepartmentId = c.Department != null ? c.Department.Id : -1,
                        DepartmentName = c.Department != null ? c.Department.Name : string.Empty,
                        PhoneNumber = c.PhoneNumber,
                        Email = c.Email,
                        Note = c.Note
                    });
        }

        public IEnumerable<Person> GetListOnWorkingOfPerson(System.DateTime endedDate, int? departmentId, int? personId)
        {
            IEnumerable<Person> result = GetAll().Where(m => m.RecruitedDate != null
                                                        && m.RecruitedDate.Value <= endedDate
                                                        && (m.NgayNghi == null
                                                            || (m.NgayNghi != null
                                                                && m.NgayNghi.Value >= endedDate)));

            if (departmentId != null)
                result = result.Where(m => m.Department != null
                                            && m.Department.Id == departmentId.Value);


            if (personId != null)
                result = result.Where(m => m.Id == personId.Value);

            return result;
        }

        public IEnumerable<Person> GetListAllOfReleasedPerson(System.DateTime endedDate, int? departmentId, int? personId)
        {
            IEnumerable<Person> result = GetAll().Where(m => m.NgayNghi != null
                                                    && m.NgayNghi <= endedDate);

            if (departmentId != null)
                result = result.Where(m =>m.Department != null
                                            && m.Department.Id == departmentId.Value);


            if (personId != null)
                result = result.Where(m => m.Id == personId.Value);

            return result;
        }

        public IEnumerable<PersonDto> GetListAllOfReleasedPersonDto(System.DateTime endedDate, int? departmentId, int? personId)
        {
            return from c in GetListAllOfReleasedPerson(endedDate, departmentId, personId)
                   select new PersonDto
                   {
                       Id = c.Id,
                       Name = c.Lastname + " " + c.Firstname,
                       Lastname = c.Lastname,
                       Firstname = c.Firstname,
                       GenderId = c.Gender != null ? c.Gender.Id : -1,
                       GenderName = c.Gender != null ? c.Gender.Name : string.Empty,
                       Birthday = c.Birthday != null ? c.Birthday.Value.ToString("dd/MM/yyyy") : string.Empty,
                       RecruitedDate = c.RecruitedDate != null ? c.RecruitedDate.Value.ToString("dd/MM/yyyy") : string.Empty,
                       NgayNghi = c.NgayNghi != null ? c.NgayNghi.Value.ToString("dd/MM/yyyy") : string.Empty,
                       ChucDanhId = c.ChucDanh != null ? c.ChucDanh.Id : -1,
                       ChucDanhName = c.ChucDanh != null ? c.ChucDanh.Name : string.Empty,
                       DepartmentId = c.Department != null ? c.Department.Id : -1,
                       DepartmentName = c.Department != null ? c.Department.Name : string.Empty,
                       PhoneNumber = c.PhoneNumber,
                       Email = c.Email,
                       Note = c.Note
                   };
        }
    }
}
