using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VersoMVC.Areas.Mobile.Models;
using VersoMVC.ws;
using VersoMVC.ws.Model;

namespace VersoMVC.Areas.Mobile.Controllers
{
    public class MobileAccountController : Controller
    {
        [HttpPost]
        public JsonResult InitilizeGuestSession()
        {            
            return Json(new AgEnzoMyAccount().InitilizeGuestSession());
        }
        [HttpPost]
        public JsonResult UserAuthenticate(MobileUser mobileUser, AgEnzoSession agEnzoSession  )
        {
            return Json(new AgEnzoMyAccount().UserAuthenticate(mobileUser.Username, mobileUser.Password, mobileUser.LibraryId, agEnzoSession));
        }
        [HttpPost]
        public JsonResult LibraryList(AgEnzoSession agEnzoSession)
        {
            return Json(new AgEnzoMyAccount().LibraryList(agEnzoSession));
        }
        [HttpPost]
        public JsonResult GetAllLostItemsUnderPatron(AgEnzoSession agEnzoSession)
        {
            return Json(new AgEnzoMyAccount().GetAllLostItemsUnderPatron(agEnzoSession));
        }
        [HttpPost]
        public JsonResult GetAllReservesUnderPatron(AgEnzoSession agEnzoSession)
        {
            return Json(new AgEnzoMyAccount().GetAllReservesUnderPatron(agEnzoSession));
        }
        [HttpPost]
        public JsonResult GetAllCheckedOutItemsUnderPatron(AgEnzoSession agEnzoSession)
        {
            return Json(new AgEnzoMyAccount().GetAllCheckedOutItemsUnderPatron(agEnzoSession));
        }
        [HttpPost]
        public JsonResult GetUserProfile(AgEnzoSession agEnzoSession)
        {
            return Json(new AgEnzoMyAccount().UserProfile(agEnzoSession));
        }
        [HttpPost]
        public JsonResult GetPickupLocation(AgEnzoSession agEnzoSession)
        {
            return Json(new AgEnzoMyAccount().GetPickupLocations(agEnzoSession));
        }
        [HttpPost]
        public JsonResult PlaceTitleReserve(AgEnzoSession agEnzoSession, int agControlId, int pickupLocationId)
        {            
            return Json(new AgEnzoMyAccount().PlaceTitleReserve(agEnzoSession, agControlId, pickupLocationId));
        }
        [HttpPost]
        public JsonResult CancelReserveItem(AgEnzoSession agEnzoSession, int reserveId)
        {
            new AgEnzoMyAccount().CancelReserveItem(agEnzoSession, reserveId);
            return Json(new AgEnzoMyAccount().GetAllReservesUnderPatron(agEnzoSession));
        }

        [HttpPost]
        public JsonResult RenewCheckoutItem(AgEnzoSession agEnzoSession, int holdingId)
        {
            new AgEnzoMyAccount().SelfRenew(agEnzoSession, holdingId);
            return Json(new AgEnzoMyAccount().GetAllCheckedOutItemsUnderPatron(agEnzoSession));
        }

    }
}
