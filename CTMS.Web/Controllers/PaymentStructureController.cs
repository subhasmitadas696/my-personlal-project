using CTMS.Core;
using CTMS.Model.Entities.AdminMaster;
using CTMS.Model.Entities.Master;
using CTMS.Model.Entities.PaymentStructure;
using CTMS.Repository.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Claims;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CTMS.Web.Controllers
{
    [Authorize]
    public class PaymentStructureController : Controller
    {
        private readonly IAdminMasterRepository _adminMasterRepository;
        private readonly IPaymentStructureRepository _paymentStructureRepository;
        public PaymentStructureController(IAdminMasterRepository adminMasterRepository, IPaymentStructureRepository paymentStructureRepository)
        {
            _paymentStructureRepository = paymentStructureRepository;
            _adminMasterRepository = adminMasterRepository;
        }
        public async Task<IActionResult> AddPaymentStructure()
        {
            var identity = (ClaimsIdentity)User.Identity!;
            var useridClaim = identity.FindFirst("Userid");
            ViewBag.UserId = useridClaim!.Value;

            ViewBag.CatList =await _adminMasterRepository.CategoryBind();
            return View();
        }
        private string SerializeToXml<T>(List<T> items)
        {
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty); 
            XmlWriterSettings settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true,
                Encoding = System.Text.Encoding.UTF8 
            };

            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings))
                {
                    serializer.Serialize(xmlWriter, items, namespaces);
                }
                return stringWriter.ToString();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPaymentStructure(List<PaymentStructureMaster> payments)
        {
            try
            {
                string xmlData = SerializeToXml(payments);
                XElement paymentXml = XElement.Parse(xmlData);
                payments[0].PaymentStr = paymentXml;
                int retMsg =await _paymentStructureRepository.InsertPaymentStructure(payments,payments.FirstOrDefault()!.ActionCode!);
                if (retMsg == 1)
                {
                    return Json(new AjaxResult { state = ResultType.success.ToString(), message = "Record Saved Successfully.", data = 200 });
                }
                else if (retMsg == 2)
                {
                    return Json(new AjaxResult { state = ResultType.success.ToString(), message = "Record Updated Successfully.", data = 200 });
                }
                else if (retMsg == 3)
                {
                    return Json(new AjaxResult { state = ResultType.success.ToString(), message = "Record Deleted Successfully.", data = 200 });
                }
                else
                {
                    return Json(new AjaxResult { state = ResultType.warning.ToString(), message = "Record Already Exist.", data = 101 });

                }

            }
            catch (Exception ex)
            {
                return Json(new AjaxResult { state = ResultType.error.ToString(), message = $"Error: {ex.Message}", data = 500 });
            }
        }
        public IActionResult ViewPaymentStructure()
        {
            return View();
        }
        public async Task<IActionResult> ViewPaymentStructureDt()
        {
            try
            {

                var data = await _paymentStructureRepository.ViewPaymentStructure();

                return Json(data);

            }
            catch (Exception ex)
            {
                return Json(new AjaxResult { state = ResultType.error.ToString(), message = $"Error: {ex.Message}", data = 500 });
            }
        }


        [HttpPost]
        public IActionResult DeletePaymentStructure(int Id)
        {
            try
            {


                int response = _paymentStructureRepository.DeletePaymentStructure(Id).Result;
                return Json(response);

            }
            catch (Exception ex)
            {
                return Json(new AjaxResult { state = ResultType.error.ToString(), message = $"Error: {ex.Message}", data = 500 });
            }
        }
        [HttpGet]
        public IActionResult Getbyid(int Id)
        {
            try
            {
                var client = _paymentStructureRepository.PaymentStructureGetById(Convert.ToInt32(Id)).Result;
                return Ok(JsonConvert.SerializeObject(client));
            }
            catch (Exception ex)
            {
                return Json(new AjaxResult { state = ResultType.error.ToString(), message = $"Error: {ex.Message}", data = 500 });
            }


        }
    }
}
