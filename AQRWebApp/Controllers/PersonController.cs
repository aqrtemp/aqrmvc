using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AQRWebApp.DAL;
using AQRWebApp.Models;
using AQRWebApp.Model.Entity;
using AQRWebApp.Model.EntityDto;
using AQRWebApp.Filters;
using AQRWebApp.BL;
using System.Globalization;
using WebMatrix.WebData;
using System.Web.Security;

namespace AQRWebApp.Controllers
{
    [Authorize]
    public class PersonController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        private string datePattern = "yyyy-M-d";
        private string datePattern2 = "d/M/yyyy";

        public ActionResult Index()
        {
            PersonModel model = new PersonModel();
            GenderService genderService = new GenderService();
            DepartmentService departmentService = new DepartmentService();
            ChucDanhService chucDanhService = new ChucDanhService();
            PersonService personService = new PersonService();

            model.Departments = departmentService.GetAll().ToList();
            model.Departments.Insert(0, new Department() { Id = -1, Name = "Tất cả"});
            model.ChucDanhs = chucDanhService.GetAll().ToList();
            model.ReportDate = System.DateTime.Now.ToShortDateString();

            model.ListOfFilterNames = new List<FilterName>();
            model.ListOfFilterNames.Add(new FilterName() { Id = 1, Name = "Đang công tác" });
            model.ListOfFilterNames.Add(new FilterName() { Id = 2, Name = "Thôi việc" });
            model.ListOfFilterNames.Add(new FilterName() { Id = 3, Name = "Tất cả" });

            model.Genders = genderService.GetAll().ToList();
            return View(model);
        }

        public ActionResult GetList(JQueryDataTableParamModel param, string reportDate, int departmentId, int filter)
        {
            PersonService pservice = new PersonService();
            IEnumerable<PersonDto> allPersons = new List<PersonDto>();
            int? department = null;
            System.DateTime endedDate;

            if (!System.DateTime.TryParse(reportDate, out endedDate))
                endedDate = System.DateTime.Now;

            if (departmentId != -1)
                department = departmentId;

            switch (filter)
            {
                case 1:
                    if (Roles.IsUserInRole(WebSecurity.CurrentUserName, RoleNames.view1Role))
                        allPersons = pservice.GetListOnWorkingOfPersonDto(System.DateTime.Now, department, pservice.GetPersonIdByUserId(WebSecurity.CurrentUserId));

                    if (Roles.IsUserInRole(WebSecurity.CurrentUserName, RoleNames.view2Role))
                    {
                        Person p = pservice.GetPersonByUserId(WebSecurity.CurrentUserId);
                        if (p != null)
                        {
                            if (p.Department != null)
                            {
                                if (p.Department.Id == departmentId)
                                    allPersons = pservice.GetListOnWorkingOfPersonDto(System.DateTime.Now, departmentId, null);
                            }
                        }
                    }

                    if (Roles.IsUserInRole(WebSecurity.CurrentUserName, RoleNames.view3Role))
                        allPersons = pservice.GetListOnWorkingOfPersonDto(System.DateTime.Now, department, null);

                    break;

                case 2:
                    if (Roles.IsUserInRole(WebSecurity.CurrentUserName, RoleNames.view1Role))
                        allPersons = pservice.GetListAllOfReleasedPersonDto(System.DateTime.Now, department, pservice.GetPersonIdByUserId(WebSecurity.CurrentUserId));

                    if (Roles.IsUserInRole(WebSecurity.CurrentUserName, RoleNames.view2Role))
                    {
                        Person p = pservice.GetPersonByUserId(WebSecurity.CurrentUserId);
                        if (p != null)
                        {
                            if (p.Department != null)
                            {
                                if (p.Department.Id == departmentId)
                                    allPersons = pservice.GetListOnWorkingOfPersonDto(System.DateTime.Now, departmentId, null);
                            }
                        }
                    }

                    if (Roles.IsUserInRole(WebSecurity.CurrentUserName, RoleNames.view3Role))
                        allPersons = pservice.GetListAllOfReleasedPersonDto(System.DateTime.Now, department, null);

                    break;
            }

            IEnumerable<PersonDto> filteredPersons;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var lastnameFilter = Convert.ToString(Request["sSearch_0"]);
                var firstnameFilter = Convert.ToString(Request["sSearch_1"]);
                var phongbanFilter = Convert.ToString(Request["sSearch_6"]);

                //Optionally check whether the columns are searchable at all 
                var isLastnameSearchable = Convert.ToBoolean(Request["bSearchable_0"]);
                var isFirstnameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);
                var isPhongBanSearchable = Convert.ToBoolean(Request["bSearchable_6"]);

                filteredPersons = allPersons
                   .Where(c => isLastnameSearchable && c.Lastname != null && c.Lastname.ToString().ToLower().Contains(param.sSearch.ToLower())
                            ||
                            isFirstnameSearchable && c.Firstname != null && c.Firstname.ToString().ToLower().Contains(param.sSearch.ToLower())
                           );
            }
            else
            {
                filteredPersons = allPersons;
            }

            var isLastnameSortable = Convert.ToBoolean(Request["bSortable_0"]);
            var isFirstnameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<PersonDto, string> orderingFunction = (c => 
                                                          sortColumnIndex == 2 && isLastnameSortable ? c.Lastname.ToString() :
                                                          sortColumnIndex == 3 && isFirstnameSortable ? c.Firstname.ToString() :
                                                          "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredPersons = filteredPersons.OrderBy(orderingFunction);
            else
                filteredPersons = filteredPersons.OrderByDescending(orderingFunction);

            IEnumerable<PersonDto> displayedPersons;

            if (param.iDisplayLength != -1)
                displayedPersons = filteredPersons.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            else
                displayedPersons = filteredPersons;

            var result = from c in displayedPersons
                         select new string[]
                              {
                                  c.Id.ToString(),
                                  c.Lastname,
                                  c.Firstname,
                                  c.GenderName,
                                  c.Birthday,
                                  c.ChucDanhName,
                                  c.DepartmentName,
                                  c.PhoneNumber,
                                  c.Email,
                                  c.RecruitedDate,
                                  c.NgayNghi,
                                  c.Note
                              };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = allPersons.Count(),
                iTotalDisplayRecords = filteredPersons.Count(),
                aaData = result
            },
                        JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        public ActionResult Add(string lastname, string firstname, int genderId, string birthday, string recruitedDate, string ngayNghi, string phoneNumber, int chucDanhId, int departmentId, string email, string note)
        {
            string result = string.Empty;
            System.DateTime date;
            Person p = null;

            try {
                p = new Person();

                p.Lastname = lastname.Trim();
                p.Firstname = firstname.Trim();
                p.Gender = unitOfWork.GenderRepository.GetByID(genderId);

                if (System.DateTime.TryParseExact(birthday, datePattern, null, DateTimeStyles.None, out date))
                    p.Birthday = date;

                if (System.DateTime.TryParseExact(recruitedDate, datePattern, null, DateTimeStyles.None, out date))
                    p.RecruitedDate = date;
                
                if (System.DateTime.TryParseExact(ngayNghi, datePattern, null, DateTimeStyles.None, out date))
                    p.NgayNghi = date;

                p.PhoneNumber = phoneNumber;
                p.ChucDanh = unitOfWork.ChucDanhRepository.GetByID(chucDanhId);
                p.Department = unitOfWork.DepartmentRepository.GetByID(departmentId);
                p.Email = email;
                p.Note = note;

                p = unitOfWork.PersonRepository.Insert(p);
                unitOfWork.Save();
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        public string Update(int id, string value, int? rowId, int? columnPosition, int? columnId, string columnName)
        {
            System.DateTime date;
            Person p = unitOfWork.PersonRepository.GetByID(id);

            switch (columnPosition)
            {
                case 0:
                    p.Lastname = value;
                    p.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 1:
                    p.Firstname = value;
                    p.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 2:
                    p.Gender = unitOfWork.GenderRepository.GetByID(Convert.ToInt32(value));
                    p.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 3:
                    if (System.DateTime.TryParseExact(value.Trim(), datePattern2, null, DateTimeStyles.None, out date))
                    {
                        p.Birthday = date;
                        p.ModifiedDate = System.DateTime.Now;
                        unitOfWork.Save();
                    }

                    break;

                case 4:
                    p.ChucDanh = unitOfWork.ChucDanhRepository.GetByID(value); ;
                    p.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 5:
                    p.Department = unitOfWork.DepartmentRepository.GetByID(value); ;
                    p.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 6:
                    p.PhoneNumber = value.Trim();
                    p.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 7:
                    p.Email = value.Trim();
                    p.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 8:
                    if (System.DateTime.TryParseExact(value.Trim(), datePattern2, null, DateTimeStyles.None, out date))
                    {
                        p.RecruitedDate = date;
                        p.ModifiedDate = System.DateTime.Now;
                        unitOfWork.Save();
                    }

                    break;

                case 9:
                    if (System.DateTime.TryParseExact(value.Trim(), datePattern2, null, DateTimeStyles.None, out date))
                    {
                        p.NgayNghi = date;
                        p.ModifiedDate = System.DateTime.Now;
                        unitOfWork.Save();
                    }

                    break;
                case 10:
                    p.Note = value.Trim();
                    p.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;
            }

            return value;
        }

        [Authorize(Roles = "Delete")]
        public string DeleteById(int id)
        {
            string result = string.Empty;

            try
            {
                unitOfWork.PersonRepository.Delete(unitOfWork.PersonRepository.GetByID(id));
                unitOfWork.Save();
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            if (result.Equals(string.Empty))
                result = "ok";

            return result;
        }
    }
}
