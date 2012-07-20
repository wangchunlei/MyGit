using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domas.Service.Base.Common;
using Domas.Service.Print;
using Domas.Service.Print.PrintTask;
using UIFramwork.EFHelpler;
using UIFramwork.UI.UIFrom;
using System.Linq.Dynamic;
using UIFramwork.Util;

namespace WorkTest.Controllers
{
    public class DynamicPageController : Controller
    {
        //
        // GET: /DynamicPage/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create(string typeFullName)
        {
            ViewBag.PossibleState = Enum.GetValues(typeof(Domas.Service.Base.Common.OrderState)).Cast<Domas.Service.Base.Common.OrderState>();
            ViewBag.PossibleSafeLevel = Enum.GetValues(typeof(Domas.Service.Base.Common.SafeLevel)).Cast<Domas.Service.Base.Common.SafeLevel>();
            ViewBag.PossibleDocumentType = Enum.GetValues(typeof(Domas.Service.Base.Common.DocumentType)).Cast<Domas.Service.Base.Common.DocumentType>();
            ViewBag.PossibleUsageType = Enum.GetValues(typeof(Domas.Service.Base.Common.UsageType)).Cast<Domas.Service.Base.Common.UsageType>();
            ViewBag.PossibleApprovalType = Enum.GetValues(typeof(Domas.Service.Base.Common.ApprovalType)).Cast<Domas.Service.Base.Common.ApprovalType>();
            dynamic po = EfHelper.CreateInstance(typeFullName);
            po.CreatedOn = DateTime.Now;
            var qs = Request.QueryString;
            ViewBag.Pop = qs["pop"];
            ViewBag.ParentId = qs["ParentId"];
            ViewBag.TypeFullName = typeFullName;
            ViewBag.FormParameter = new FormParameter
                {
                    TypeFullName = typeFullName,
                    FormType = FormTypeEnum.Create
                };
            return View("Index", po);
        }
        public ActionResult Update(string typeFullName, string id)
        {
            ViewBag.PossibleState = Enum.GetValues(typeof(Domas.Service.Base.Common.OrderState))
                .Cast<Domas.Service.Base.Common.OrderState>()
                .Select(option => new SelectListItem
                {
                    Text = option.ToString(),
                    Value = ((int)option).ToString()
                });
            ViewBag.PossibleSafeLevel = Enum.GetValues(typeof(Domas.Service.Base.Common.SafeLevel)).
                Cast<Domas.Service.Base.Common.SafeLevel>().
                Select(option => new SelectListItem
            {
                Text = option.ToString(),
                Value = ((int)option).ToString()
            });
            ViewBag.PossibleDocumentType = Enum.GetValues(typeof(Domas.Service.Base.Common.DocumentType)).
                Cast<Domas.Service.Base.Common.DocumentType>().
                Select(option => new SelectListItem
            {
                Text = option.ToString(),
                Value = ((int)option).ToString()
            });
            ViewBag.PossibleUsageType = Enum.GetValues(typeof(Domas.Service.Base.Common.UsageType)).
                Cast<Domas.Service.Base.Common.UsageType>().
                Select(option => new SelectListItem
            {
                Text = option.ToString(),
                Value = ((int)option).ToString()
            });
            ViewBag.PossibleApprovalType = Enum.GetValues(typeof(Domas.Service.Base.Common.ApprovalType)).
                Cast<Domas.Service.Base.Common.ApprovalType>().
                Select(option => new SelectListItem
            {
                Text = option.ToString(),
                Value = ((int)option).ToString()
            });
            dynamic po = EfHelper.FindById(typeFullName, new Guid(id));
            var qs = Request.QueryString;
            ViewBag.Pop = qs["pop"];
            ViewBag.TypeFullName = typeFullName;
            ViewBag.FormParameter = new FormParameter
            {
                TypeFullName = typeFullName,
                FormType = FormTypeEnum.Update
            };

            var subs = MetadataExtensions.FindSubEntitiesByOwner(typeFullName);

            ViewBag.Subs = subs;
            return View("Index", po);
        }
        public ActionResult List(string typeFullName)
        {
            var qs = Request.QueryString;
            ViewBag.Pop = qs["pop"];

            ViewBag.FormParameter = new FormParameter
            {
                Pop = qs["pop"] != null ? int.Parse(qs["pop"]) : 1,
                TypeFullName = typeFullName,
                FormType = FormTypeEnum.List
            };

            return View();
        }

        [HttpPost]
        public JsonResult List(int page, int rows, string sort, string order)
        {
            try
            {
                NameValueCollection nvc = Request.Form;
                var entity = Request.QueryString["entity"];
                var value = EfHelper.GetList(page, rows, sort, order, entity, nvc);

                return
                    Json(
                       value);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public JavaScriptResult Save()
        {
            var typeFullName = Request["TypeFullName"];
            string id = Request["ID"];

            EfHelper.SaveContextChanges(typeFullName, id, Request.Params);
            return JavaScript("alert('ok');");
        }

        [HttpPost]
        public JavaScriptResult Delete(string entity, string[] rows)
        {
            EfHelper.DeleteEntity(entity, rows);
            return JavaScript("");
        }
        public void Refresh(DbContext dbContext, dynamic instance)
        {
            dbContext.Entry<dynamic>(instance).Reload();
        }

    }
}
