﻿using System;
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

namespace AQRWebApp.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        public ActionResult Index()
        {
            return View();
        }

        #region "Ajax data provider"

        public ActionResult GetList(JQueryDataTableParamModel param)
        {
            DepartmentService DepartmentService = new DepartmentService();

            IEnumerable<DepartmentDto> allDepartments = DepartmentService.GetAllExposeDto();

            IEnumerable<DepartmentDto> filteredDepartments;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var nameFilter = Convert.ToString(Request["sSearch_1"]);

                //Optionally check whether the columns are searchable at all 
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);

                filteredDepartments = allDepartments
                   .Where(c => isNameSearchable && c.Name != null && c.Name.ToString().ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredDepartments = allDepartments;
            }

            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<DepartmentDto, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.Name.ToString() :
                                                          "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredDepartments = filteredDepartments.OrderBy(orderingFunction);
            else
                filteredDepartments = filteredDepartments.OrderByDescending(orderingFunction);

            IEnumerable<DepartmentDto> displayedDepartments;

            if (param.iDisplayLength != -1)
                displayedDepartments = filteredDepartments.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            else
                displayedDepartments = filteredDepartments;

            var result = from c in displayedDepartments
                         select new[]
                              {
                                  c.Id.ToString(),
                                  c.Name
                              };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = allDepartments.Count(),
                iTotalDisplayRecords = filteredDepartments.Count(),
                aaData = result
            },
                        JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region "Create Update Delete operations"

        [Authorize(Roles = "Edit")]
        public ActionResult Add(string name)
        {
            string result = string.Empty;

            if (IsExistingByName(name))
            {
                result = "Tên này đã có trong cơ sở dữ liệu.";
            }
            else
                try
                {
                    unitOfWork.DepartmentRepository.Insert(new Department() { Name = name });
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
            Department query = unitOfWork.DepartmentRepository.GetByID(id);

            switch (columnPosition)
            {
                case 0:
                    if (!IsExistingByName(value.Trim()))
                    {
                        query.Name = value.Trim();
                        query.ModifiedDate = System.DateTime.Now;
                        unitOfWork.Save();
                    }

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
                unitOfWork.DepartmentRepository.Delete(unitOfWork.DepartmentRepository.GetByID(id));
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

        #endregion

        private bool IsExistingByName(string name)
        {
            return unitOfWork.DepartmentRepository.Get(m => m.Name.ToLower().Equals(name.Trim().ToLower())).FirstOrDefault() != null;
        }
    }
}
