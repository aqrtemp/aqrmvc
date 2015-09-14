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
    public class PartnerController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        public ActionResult Index()
        {
            return View();
        }

        #region "Ajax data provider"
        [HttpPost]
        public ActionResult GetList(JQueryDataTableParamModel param)
        {
            PartnerService PartnerService = new PartnerService();

            var allPartners = PartnerService.GetAllExposeDto();

            IEnumerable<PartnerDto> filteredPartners;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var PartnerIdFilter = Convert.ToString(Request["sSearch_2"]);
                var PartnerNameFilter = Convert.ToString(Request["sSearch_3"]);

                //Optionally check whether the columns are searchable at all 
                var isPartnerIdSearchable = Convert.ToBoolean(Request["bSearchable_2"]);
                var isPartnerNameSearchable = Convert.ToBoolean(Request["bSearchable_3"]);

                filteredPartners = allPartners
                   .Where(c => isPartnerIdSearchable && c.PartnerId != null && c.PartnerId.ToLower().Contains(param.sSearch.ToLower())
                            || isPartnerNameSearchable && c.PartnerName != null && c.PartnerName.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredPartners = allPartners;
            }

            var isPartnerIdSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<PartnerDto, string> orderingFunction = (c => sortColumnIndex == 1 && isPartnerIdSortable ? c.PartnerId.ToString() :
                                                          "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredPartners = filteredPartners.OrderBy(orderingFunction);
            else
                filteredPartners = filteredPartners.OrderByDescending(orderingFunction);

            IEnumerable<PartnerDto> displayedPartners;

            if (param.iDisplayLength != -1)
                displayedPartners = filteredPartners.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            else
                displayedPartners = filteredPartners;

            var result = from c in displayedPartners
                         select new[]
                              {
                                  c.Id.ToString(),
                                  c.IsActive,
                                  c.PartnerId,
                                  c.PartnerName,
                                  c.AddressLine1,
                                  c.AddressLine2,
                                  c.City,
                                  c.State,
                                  c.Postcode,
                                  c.Contact,
                                  c.Phone,
                                  c.Fax,
                                  c.SupplierNo
                              };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = allPartners.Count(),
                iTotalDisplayRecords = filteredPartners.Count(),
                aaData = result
            },
                        JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region "Create Update Delete operations"

        [Authorize(Roles = "Edit")]
        [HttpPost]
        public JsonResult Add(string isActive, string partnerId, string partnerName, string addressLine1, string addressLine2,
                                  string city, string state, string postcode, string contact,
                                  string phone, string fax, string supplierNo, string line, string entryName,
                                string import, string importNote)
        {
            string result = string.Empty;

            bool boolValue;
            int intValue;

            try
            {
                unitOfWork.PartnerRepository.Insert(new Partner()
                {
                    IsActive = bool.TryParse(isActive, out boolValue) ? boolValue : false,
                    PartnerId = partnerId != null ? partnerId.Trim() : String.Empty,
                    PartnerName = partnerName != null ? partnerName.Trim() : String.Empty,
                    AddressLine1 = addressLine1 != null ? addressLine1.Trim() : String.Empty,
                    AddressLine2 = addressLine2 != null ? addressLine2.Trim() : String.Empty,
                    City = city != null ? city.Trim() : String.Empty,
                    State = state != null ? state.Trim() : String.Empty,
                    Postcode = postcode != null ? postcode.Trim() : String.Empty,
                    Contact = contact != null ? contact.Trim() : String.Empty,
                    Phone = phone != null ? phone.Trim() : String.Empty,
                    Fax = fax != null ? fax.Trim() : String.Empty,
                    SupplierNo = supplierNo != null ? supplierNo.Trim() : String.Empty,
                    Line = Int32.TryParse(line, out intValue) ? intValue : 0,
                    EntryName = entryName,
                    Import = import,
                    ImportNote = importNote
                });
                unitOfWork.Save();
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return Json(result);
        }

        [Authorize(Roles = "Edit")]
        public string Update(int id, string value, int? rowId, int? columnPosition, int? columnId, string columnName)
        {
            bool boolValue;
            int intValue;
            Partner query = unitOfWork.PartnerRepository.GetByID(id);

            switch (columnPosition)
            {
                case 0:
                    query.IsActive = bool.TryParse(value.Trim(), out boolValue) ? boolValue : false;
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 1:
                    query.PartnerId = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 2:
                    query.PartnerName = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 3:
                    query.AddressLine1 = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 4:
                    query.AddressLine2 = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;
                     
                case 5:
                    query.City = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;
                     
                case 6:
                    query.State = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;
                     
                case 7:
                    query.Postcode = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 8:
                    query.Contact = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;
                     
                case 9:
                    query.Phone = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;
                     
                case 10:
                    query.Fax = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 11:
                    query.SupplierNo = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
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
                unitOfWork.PartnerRepository.Delete(unitOfWork.PartnerRepository.GetByID(id));
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
    }
}
