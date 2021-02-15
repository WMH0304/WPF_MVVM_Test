using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web_netCore.Models;
using Newtonsoft.Json;

using System.Linq;

namespace Web_netCore.Controllers
{
    public class Fric : Controller
    {

        Models.EasternStar_WPF_MVVMContext m = new EasternStar_WPF_MVVMContext();


        [HttpGet]
        public IActionResult Index()
        {

            base.ViewData["name"] = new Test()
            {
                name = "sdaf"
            };
            
            base.ViewBag.name = new Test()
            {

                name = m.SysRoomStages.Where(c => c.IdRoomStage == 10).SingleOrDefault().McRoomStage,
                 bt = m.SysRoomStages.Where(c => c.IdClass == 10).SingleOrDefault()

                

               

            };
           
            base.TempData["name"] = new Test()
            {
                name = "asdfsd"
            };

            //testOther

            //base.

            if (string.IsNullOrWhiteSpace(this.HttpContext.Session.GetString("TestSession")))
            {
                base.HttpContext.Session.SetString("TestSession", Newtonsoft.Json.JsonConvert.SerializeObject(new Test()
                {
                    name = "二郎"
                }));
            }
           

            return View(new Test()
            {

                name = "dasdf"
            });





        }
    }
}
