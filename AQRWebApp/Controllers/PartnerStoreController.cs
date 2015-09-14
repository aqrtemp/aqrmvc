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
    public class PartnerStoreController : Controller
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
            PartnerStoreService PartnerStoreService = new PartnerStoreService();

            var allPartnerStores = PartnerStoreService.GetAllExposeDto();

            IEnumerable<PartnerStoreDto> filteredPartnerStores;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var PartnerStoreIdFilter = Convert.ToString(Request["sSearch_1"]);

                //Optionally check whether the columns are searchable at all 
                var isPartnerStoreIdSearchable = Convert.ToBoolean(Request["bSearchable_1"]);

                filteredPartnerStores = allPartnerStores
                   .Where(c => isPartnerStoreIdSearchable && c.Name != null && c.Name.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredPartnerStores = allPartnerStores;
            }

            var isPartnerIdSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<PartnerStoreDto, string> orderingFunction = (c => sortColumnIndex == 1 && isPartnerIdSortable ? c.PartnerId.ToString() :
                                                          "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredPartnerStores = filteredPartnerStores.OrderBy(orderingFunction);
            else
                filteredPartnerStores = filteredPartnerStores.OrderByDescending(orderingFunction);

            IEnumerable<PartnerStoreDto> displayedPartnerStores;

            if (param.iDisplayLength != -1)
                displayedPartnerStores = filteredPartnerStores.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            else
                displayedPartnerStores = filteredPartnerStores;

            var result = from c in displayedPartnerStores
                         select new[]
                              {
                                  c.Id.ToString(),
                                  c.PartnerId,
                                  c.PartnerName,
                                  c.StoreId,
                                  c.StoreName,
                                  c.AddressLine1,
                                  c.AddressLine2,
                                  c.City,
                                  c.State,
                                  c.Postcode,
                                  c.Country,
                                  c.Comment1,
                                  c.Comment2,
                                  c.Comment3,
                                  c.Email,
                                  c.PhoneNo,
                                  c.FaxNo,
                                  c.DCI_MyerStore,
                                  c.ReceiverID2,
                                  c.Udf1,
                                  c.Udf2,
                                  c.Udf3,
                                  c.Udf4,
                                  c.Udf5,
                                  c.Udf6,
                                  c.Udf7,
                                  c.Udf8,
                                  c.Udf9,
                                  c.Udf10
                              };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = allPartnerStores.Count(),
                iTotalDisplayRecords = filteredPartnerStores.Count(),
                aaData = result
            },
                        JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region "Create Update Delete operations"

        [Authorize(Roles = "Edit")]
        [HttpPost]
        public JsonResult Add(string partnerId, string partnerName, string storeId, string storeName,
                                  string addressLine1, string addressLine2, string city, string state,
                                  string postcode, string country, string comment1, string comment2, string comment3,
                                  string email,string phoneNo,string faxNo, string dCI_MyerStore, string receiverID2,
                                  string udf1, string udf2, string udf3, string udf4, string udf5,
                                  string udf6, string udf7, string udf8, string udf9, string udf10,
                                    string line, string entryName, string import, string importNote)
        {
            string result = string.Empty;

            int intValue;

            try
            {
                PartnerService partnerService = new PartnerService();
                PartnerStore partnerStore = new PartnerStore();
                Partner queryPartner = partnerService.GetByPartnerId(partnerId);
                if(queryPartner != null)
                {
                    partnerStore.Partner = unitOfWork.PartnerRepository.GetByID(queryPartner.Id);
                }

                partnerStore.StoreId = storeId;
                partnerStore.StoreName = storeName;
                partnerStore.AddressLine1 = addressLine1;
                partnerStore.AddressLine2 = addressLine2;
                partnerStore.City = city;
                partnerStore.State = state;
                partnerStore.Postcode = postcode;
                partnerStore.Country = country;
                partnerStore.Comment1 = comment1;
                partnerStore.Comment2 = comment2;
                partnerStore.Comment3 = comment3;
                partnerStore.Email = email;
                partnerStore.PhoneNo = phoneNo;
                partnerStore.FaxNo = faxNo;
                partnerStore.DCI_MyerStore = dCI_MyerStore;
                partnerStore.ReceiverID2 = receiverID2;
                partnerStore.Udf1 = udf1;
                partnerStore.Udf2 = udf2 ;
                partnerStore.Udf3 = udf3;
                partnerStore.Udf4 = udf4;
                partnerStore.Udf5 = udf5;
                partnerStore.Udf6 = udf6 != null ? Convert.ToDouble(udf6) : new Nullable<double>();
                partnerStore.Udf7 = udf7 != null ? Convert.ToDouble(udf7) : new Nullable<double>();
                partnerStore.Udf8 = udf8 != null ? Convert.ToDouble(udf8) : new Nullable<double>();
                partnerStore.Udf9 = udf9 != null ? Convert.ToDouble(udf9) : new Nullable<double>();
                partnerStore.Udf10 = udf10 != null ? Convert.ToDouble(udf10) : new Nullable<double>();
                partnerStore.Line = Int32.TryParse(line, out intValue) ? intValue : 0;
                partnerStore.EntryName = entryName;
                partnerStore.Import = import;
                partnerStore.ImportNote = importNote;

                unitOfWork.PartnerStoreRepository.Insert(partnerStore);
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
            double doubleValue;
            PartnerStore query = unitOfWork.PartnerStoreRepository.GetByID(id);

            switch (columnPosition)
            {
                case 0:
                    query.Partner = unitOfWork.PartnerRepository.GetByID(Convert.ToInt32(value.Trim()));
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 1:
                    query.Partner = unitOfWork.PartnerRepository.GetByID(Convert.ToInt32(value.Trim()));
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 2:
                    query.StoreId = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 3:
                    query.StoreName = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 4:
                    query.AddressLine1 = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 5:
                    query.AddressLine2 = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 6:
                    query.City = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 7:
                    query.State = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 8:
                    query.Postcode = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 9:
                    query.Country = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 10:
                    query.Comment1 = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 11:
                    query.Comment2 = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 12:
                    query.Comment3 = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 13:
                    query.Email = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 14:
                    query.PhoneNo = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 15:
                    query.FaxNo = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 16:
                    query.FaxNo = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 17:
                    query.DCI_MyerStore = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 18:
                    query.ReceiverID2 = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 19:
                    query.Udf1 = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 20:
                    query.Udf1 = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 21:
                    query.Udf2 = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 22:
                    query.Udf3 = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 23:
                    query.Udf4 = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 24:
                    query.Udf5 = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 25:
                    if(double.TryParse(value.Trim(), out doubleValue))
                    {
                        query.Udf6 = doubleValue;
                        query.ModifiedDate = System.DateTime.Now;
                        unitOfWork.Save();
                    }
                    
                    break;

                case 26:
                    if(double.TryParse(value.Trim(), out doubleValue))
                    {
                        query.Udf7 = doubleValue;
                        query.ModifiedDate = System.DateTime.Now;
                        unitOfWork.Save();
                    }
                    
                    break;

                case 27:
                    if(double.TryParse(value.Trim(), out doubleValue))
                    {
                        query.Udf8 = doubleValue;
                        query.ModifiedDate = System.DateTime.Now;
                        unitOfWork.Save();
                    }
                    
                    break;

                case 28:
                    if(double.TryParse(value.Trim(), out doubleValue))
                    {
                        query.Udf9 = doubleValue;
                        query.ModifiedDate = System.DateTime.Now;
                        unitOfWork.Save();
                    }
                    
                    break;

                case 29:
                    if(double.TryParse(value.Trim(), out doubleValue))
                    {
                        query.Udf10 = doubleValue;
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
                unitOfWork.PartnerStoreRepository.Delete(unitOfWork.PartnerStoreRepository.GetByID(id));
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
