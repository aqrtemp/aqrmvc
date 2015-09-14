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
    public class ClientPartnerController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        public ActionResult Index()
        {
            ClientService clientService = new ClientService();
            PartnerService partnerService = new PartnerService();

            ClientPartnerModel model = new ClientPartnerModel();
            model.Clients = clientService.GetAllExposeDto().ToList();
            model.Partners = partnerService.GetAllExposeDto().ToList();
            return View(model);
        }

        #region "Ajax data provider"
        [HttpPost]
        public ActionResult GetList(JQueryDataTableParamModel param)
        {
            ClientPartnerService clientPartnerService = new ClientPartnerService();

            var allClientPartners = clientPartnerService.GetAllExposeDto();

            IEnumerable<ClientPartnerDto> filteredClientPartners;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var clientIdFilter = Convert.ToString(Request["sSearch_1"]);
                var partnerIdFilter = Convert.ToString(Request["sSearch_2"]);

                //Optionally check whether the columns are searchable at all 
                var isClientIdSearchable = Convert.ToBoolean(Request["bSearchable_1"]);
                var isPartnerIdSearchable = Convert.ToBoolean(Request["bSearchable_2"]);

                filteredClientPartners = allClientPartners
                   .Where(c => isClientIdSearchable && c.ClientId != null && c.ClientId.ToLower().Contains(param.sSearch.ToLower())
                            || isPartnerIdSearchable && c.PartnerId != null && c.PartnerId.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredClientPartners = allClientPartners;
            }

            var isClientIdSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var isPartnerIdSortable = Convert.ToBoolean(Request["bSortable_2"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<ClientPartnerDto, string> orderingFunction = (c => sortColumnIndex == 1 && isClientIdSortable ? c.ClientId.ToString() :
                                                                    sortColumnIndex == 2 && isPartnerIdSortable ? c.PartnerId.ToString() :
                                                          "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredClientPartners = filteredClientPartners.OrderBy(orderingFunction);
            else
                filteredClientPartners = filteredClientPartners.OrderByDescending(orderingFunction);

            IEnumerable<ClientPartnerDto> displayedClientPartners;

            if (param.iDisplayLength != -1)
                displayedClientPartners = filteredClientPartners.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            else
                displayedClientPartners = filteredClientPartners;

            var result = from c in displayedClientPartners
                         select new[]
                              {
                                  c.Id.ToString(),
                                  c.ClientId,
                                  c.PartnerId,
                                  c.CarrierId,
                                  c.PartnerEDINo,
                                  c.EDIDocCode,
                                  c.EDIDocType,
                                  c.Directory
                              };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = allClientPartners.Count(),
                iTotalDisplayRecords = filteredClientPartners.Count(),
                aaData = result
            },
                        JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region "Create Update Delete operations"

        [Authorize(Roles = "Edit")]
        [HttpPost]
        public JsonResult Add(string clientId, string partnerId, string carrierId,
                                string partnerEDINo, string eDIDocCode, string eDIDocType, string directory,
                                string line, string entryName, string import, string importNote)
        {
            string result = string.Empty;
            int intValue;
            ClientService clientService = new ClientService();
            PartnerService partnerService = new PartnerService();
            try
            {
                ClientPartner clientPartner = new ClientPartner();

                Client queryClient = clientService.GetByClientId(clientId);
                if(queryClient != null)
                {
                    clientPartner.Client = unitOfWork.ClientRepository.GetByID(queryClient.Id);
                }

                clientPartner.CarrierId = carrierId != null ? carrierId : String.Empty;
                clientPartner.PartnerEDINo = partnerEDINo != null ? partnerEDINo : String.Empty;
                clientPartner.EDIDocCode = eDIDocCode != null ? eDIDocCode : String.Empty;
                clientPartner.EDIDocType = eDIDocType != null ? eDIDocType : String.Empty;
                clientPartner.Directory = directory != null ? directory : String.Empty;

                Partner queryPartner = partnerService.GetByPartnerId(partnerId);
                if (queryPartner != null)
                {
                    clientPartner.Partner = unitOfWork.PartnerRepository.GetByID(queryPartner.Id);
                }

                clientPartner.Line = Int32.TryParse(line, out intValue) ? intValue : 0;
                clientPartner.EntryName = entryName;
                clientPartner.Import = import;
                clientPartner.ImportNote = importNote;

                unitOfWork.ClientPartnerRepository.Insert(clientPartner);
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
            ClientPartner query = unitOfWork.ClientPartnerRepository.GetByID(id);

            switch (columnPosition)
            {
                case 0:
                    query.Client = unitOfWork.ClientRepository.GetByID(Convert.ToInt32(value));
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 1:
                    query.Partner = unitOfWork.PartnerRepository.GetByID(Convert.ToInt32(value));
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 2:
                    query.CarrierId = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 3:
                    query.PartnerEDINo = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 4:
                    query.EDIDocCode = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 5:
                    query.EDIDocType = value.Trim();
                    query.ModifiedDate = System.DateTime.Now;
                    unitOfWork.Save();

                    break;

                case 6:
                    query.Directory = value.Trim();
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
                unitOfWork.ClientPartnerRepository.Delete(unitOfWork.ClientPartnerRepository.GetByID(id));
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
