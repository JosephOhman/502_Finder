using System.Collections.Generic;
using System.Web.Mvc;
using _502Finder.App;
using _502Finder.Models.AJAX.AutoComplete;

namespace _502Finder.Controllers.AJAX
{
    public class StrainsController : Controller
    {
        public ActionResult AutoComplete(string query)
        {
            var strains = new List<StrainAutoComplete>();
            IStrainService service = new StrainService();

            foreach (var strain in service.StrainAutoComplete(query))
                strains.Add(new StrainAutoComplete(strain.Name));

            return Json(strains, JsonRequestBehavior.AllowGet);
        }
    }
}