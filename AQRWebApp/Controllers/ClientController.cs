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
    public class ClientController : Controller
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
            ClientService clientService = new ClientService();

            var allClients = clientService.GetAllExposeDto();

            IEnumerable<ClientDto> filteredClients;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var clientIdFilter = Convert.ToString(Request["sSearch_1"]);

                //Optionally check whether the columns are searchable at all 
                var isClientIdSearchable = Convert.ToBoolean(Request["bSearchable_1"]);

                filteredClients = allClients
                   .Where(c => isClientIdSearchable && c.Name != null && c.Name.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredClients = allClients;
            }

            var isClientIdSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<ClientDto, string> orderingFunction = (c => sortColumnIndex == 1 && isClientIdSortable ? c.ClientId.ToString() :
                                                          "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredClients = filteredClients.OrderBy(orderingFunction);
            else
                filteredClients = filteredClients.OrderByDescending(orderingFunction);

            IEnumerable<ClientDto> displayedClients;

            if (param.iDisplayLength != -1)
                displayedClients = filteredClients.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            else
                displayedClients = filteredClients;

            var result = from c in displayedClients
                         select new[]
                              {
                                  c.Id.ToString(),
                                  c.IsActive,
                                  c.ClientId,
                                  c.ClientName,
                                  c.ShortName,
                                  c.AddressLine1,
                                  c.AddressLine2,
                                  c.City,
                                  c.State,
                                  c.Postcode,
                                  c.EDINo,
                                  c.Mfrid,
                                  c.NextPack,
                                  c.EndPack,
                                  c.NextASN,
                                  c.SupplierNo,
                                  c.TaxNo,
                                  c.ABN,
                                  c.Phone,
                                  c.ClientContactEmail,
                                  c.AQRAMEmail,
                                  c.FreightforwarderEmail,
                                  c.Line.ToString(),
                                  c.EntryName,
                                  c.Import,
                                  c.ImportNote
                              };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = allClients.Count(),
                iTotalDisplayRecords = filteredClients.Count(),
                aaData = result
            },
                        JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region "Create Update Delete operations"

        [Authorize(Roles = "Edit")]
        [HttpPost]
        public JsonResult Add(string name, string isActive, string clientId,
                                string clientName, string shortName, string addressLine1, string addressLine2,
                                string city, string state, string postcode,
                                string eDINo, string mfrid, string nextPack,
                                string endPack, string nextASN, string supplierNo,
                                string taxNo, string aBN, string phone,
                                string clientContactEmail, string aQRAMEmail,
                                string freightforwarderEmail, string line, string entryName,
                                string import, string importNote)
        {
            string result = string.Empty;

            bool boolValue;
            int intValue;

            try
            {
                unitOfWork.ClientRepository.Insert(new Client() { 
                    IsActive = bool.TryParse(isActive, out boolValue) ? boolValue : false,
                    ClientId = clientId,
                    ClientName = clientName,
                    ShortName = shortName,
                    AddressLine1 = addressLine1,
                    AddressLine2 = addressLine2,
                    City = city, 
                    State = state,
                    Postcode = postcode,
                    EDINo = eDINo,
                    Mfrid = mfrid,
                    NextPack = Int32.TryParse(nextPack, out intValue) ? intValue : new Nullable<int>(),
                    EndPack = Int32.TryParse(endPack, out intValue) ? intValue : new Nullable<int>(),
                    NextASN = Int32.TryParse(nextASN, out intValue) ? intValue : new Nullable<int>(),
                    SupplierNo = supplierNo,
                    TaxNo = taxNo,
                    ABN = aBN,
                    Phone = phone,
                    ClientContactEmail = clientContactEmail,
                    AQRAMEmail = aQRAMEmail,
                    FreightforwarderEmail = freightforwarderEmail,
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
            Client query = unitOfWork.ClientRepository.GetByID(id);

            switch (columnPosition)
            {
                case 0:
                    query.IsActive = bool.TryParse(value.Trim(), out boolValue) ? boolValue: false; 
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 1:
                    query.ClientId = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 2:
                    query.ClientName = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;
                case 3:
                    query.ShortName = value.Trim();
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
                    query.EDINo = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 10:
                    query.Mfrid = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 11:
                    query.NextPack = Int32.TryParse(value.Trim(), out intValue) ? intValue: new Nullable<int>();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 12:
                    query.EndPack = Int32.TryParse(value.Trim(), out intValue) ? intValue: new Nullable<int>();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;
                case 13:
                    query.NextASN = Int32.TryParse(value.Trim(), out intValue) ? intValue: new Nullable<int>();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;
                case 14:
                    query.SupplierNo = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;
                case 15:
                    query.TaxNo = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;
                case 16:
                    query.ABN = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;
                case 17:
                    query.Phone = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 18:
                    query.ClientContactEmail = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;
                case 19:
                    query.AQRAMEmail = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;
                case 20:
                    query.FreightforwarderEmail = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;
                case 21:
                    query.Line = Int32.TryParse(value.Trim(), out intValue) ? intValue : 0;
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;
                case 22:
                    query.EntryName = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;
                case 23:
                    query.Import = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;
                case 24:
                    query.ImportNote = value.Trim();
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
                unitOfWork.ClientRepository.Delete(unitOfWork.ClientRepository.GetByID(id));
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
