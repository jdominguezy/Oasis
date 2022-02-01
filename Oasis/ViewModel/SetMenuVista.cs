using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;



namespace Oasis.ViewModel
{

    public class SetMenuVista
    {
        public bool MostrarCredito { get; set; }
        public bool MostrarProduccion { get; set; }
        public bool MostrarBodega { get; set; }

        //public void SetMenuVista()
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        if (HttpContext.Current.User.IsInRole("Reporters"))
        //        {
        //            ShowReports = true;
        //        }
        //        if (HttpContext.Current.User.IsInRole("Administrators"))
        //        {
        //            ShowDashboard = true;
        //            ShowAdmin = true;
        //        }
        //    }
        //}
    }
}