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

namespace AQRWebApp.Controllers
{
    [Authorize]
    public class ChucDanhController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        public ActionResult Index()
        {
            return View();
        }

        #region "Ajax data provider"

        public ActionResult GetList(JQueryDataTableParamModel param)
        {
            ChucDanhService chucDanhService = new ChucDanhService();

            IEnumerable<ChucDanhDto> allChucDanhs = chucDanhService.GetAllExposeDto();

            IEnumerable<ChucDanhDto> filteredChucDanhs;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var nameFilter = Convert.ToString(Request["sSearch_1"]);

                //Optionally check whether the columns are searchable at all 
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);

                filteredChucDanhs = allChucDanhs
                   .Where(c => isNameSearchable && c.Name != null && c.Name.ToString().ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredChucDanhs = allChucDanhs;
            }

            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<ChucDanhDto, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.Name.ToString() :
                                                          "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredChucDanhs = filteredChucDanhs.OrderBy(orderingFunction);
            else
                filteredChucDanhs = filteredChucDanhs.OrderByDescending(orderingFunction);

            IEnumerable<ChucDanhDto> displayedChucDanhs;

            if (param.iDisplayLength != -1)
                displayedChucDanhs = filteredChucDanhs.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            else
                displayedChucDanhs = filteredChucDanhs;

            var result = from c in displayedChucDanhs
                         select new[]
                              {
                                  c.Id.ToString(),
                                  c.Name
                              };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = allChucDanhs.Count(),
                iTotalDisplayRecords = filteredChucDanhs.Count(),
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
                    unitOfWork.ChucDanhRepository.Insert(new ChucDanh() { Name = name });
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
            ChucDanh query = unitOfWork.ChucDanhRepository.GetByID(id); 

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
                unitOfWork.ChucDanhRepository.Delete(unitOfWork.ChucDanhRepository.GetByID(id));
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
            return unitOfWork.ChucDanhRepository.Get(m => m.Name.ToLower().Equals(name.Trim().ToLower())).FirstOrDefault() != null;
        }
    }
}
