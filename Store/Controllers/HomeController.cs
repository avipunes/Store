using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Store.Security;
using System.Web.Security;
using Store.Models.ViewModel;
using Store.Models.EntityManager;

namespace Store.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Welcome()
        {
            return View();
        }
        public ActionResult Maps()
        {
            return Suplliers();
        }


        [Authorize]
        public ActionResult IndexAfterLogin(UserLoginView ULV)
        {
            var Userid = ULV.SYSUserID;
            
            UserManager um = new UserManager();
            if(um.GetUserRule(Userid)==1)
                return View(ULV);
            else 
                return View();
        }


        [AuthorizeRoles("Admin")]
        public ActionResult AdminOnly()
        {
            return View();
        }

        public ActionResult UnAuthorized()
        {
            return View();
        }

        public ActionResult Products(string status = "")
        {

            UserManager UM = new UserManager();
            ProductsDataView UDV = UM.GetProductDataView();

            string message = string.Empty;
            if (status.Equals("update"))
                message = "Update Successful";
            else if (status.Equals("delete"))
                message = "Delete Successful";

            ViewBag.Message = message;

            return PartialView(UDV);

        }

        public ActionResult Suplliers(string status = "")
        {

            UserManager UM = new UserManager();
            SuplliersDataView SDV = new SuplliersDataView();
            SDV.suplliers= UM.GetAllSuplliers();

            string message = string.Empty;
            if (status.Equals("update"))
                message = "Update Successful";
            else if (status.Equals("delete"))
                message = "Delete Successful";

            ViewBag.Message = message;

            return PartialView(SDV);

        }

        public ActionResult ProductsToSupllier(string status = "")
        {
            UserManager UM = new UserManager();
            ProductsToSupllier PTS = new ProductsToSupllier();
            PTS= UM.GetSuplliersAndProducts();

            return PartialView(PTS);

        }

        [AuthorizeRoles("Admin")]
        public ActionResult ManageUserPartial(string status = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                string loginName = User.Identity.Name;
                UserManager UM = new UserManager();
                UserDataView UDV = UM.GetUserDataView(loginName);

                string message = string.Empty;
                if (status.Equals("update"))
                    message = "Update Successful";
                else if (status.Equals("delete"))
                    message = "Delete Successful";

                ViewBag.Message = message;

                return PartialView(UDV);
            }

            return RedirectToAction("Index", "Home");
        }

        [AuthorizeRoles("Admin")]
        public ActionResult AddUser(int userID, string loginName, string password, string firstName, string lastName, string gender, int roleID = 0)
        {
            UserSignUpView UPV = new UserSignUpView();
            UPV.SYSUserID = userID;
            UPV.LoginName = loginName;
            UPV.Password = password;
            UPV.FirstName = firstName;
            UPV.LastName = lastName;
            UPV.Gender = gender;

            if (roleID >= 0)
                UPV.LOOKUPRoleID = roleID;

            UserManager UM = new UserManager();
            UM.AddUserAccount(UPV);

            return Json(new { success = true });
        }

        [AuthorizeRoles("Admin")]
        public ActionResult AddProduct(string pName, int pPrice,string address, int pSid)
        {
            ProductDataView UDV = new ProductDataView();
            UDV.product_id = 0;
            UDV.product_name = pName;
            UDV.product_price = pPrice;
            UDV.product_address = address;
            UDV.SupllierID = pSid;

            UserManager UM = new UserManager();
            UM.AddProduct(UDV);

            return Json(new { success = true });
        }

        public ActionResult AddSupllier(string sName, string sPhone, string address)
        {
            SupllierDataView SDV = new SupllierDataView();
            SDV.SupllierID = 0;
            SDV.SupllierName = sName;
            SDV.SupllierPhone = sPhone;
            SDV.SupllierAddress = address;

            UserManager UM = new UserManager();
            UM.AddSupllier(SDV);

            return Json(new { success = true });
        }

        [AuthorizeRoles("Admin")]
        public ActionResult UpdateUserData(int userID, string loginName, string password, string firstName, string lastName, string gender, int roleID = 0)
        {
            UserProfileView UPV = new UserProfileView();
            UPV.SYSUserID = userID;
            UPV.LoginName = loginName;
            UPV.Password = password;
            UPV.FirstName = firstName;
            UPV.LastName = lastName;
            UPV.Gender = gender;

            if (roleID > 0)
                UPV.LOOKUPRoleID = roleID;

            UserManager UM = new UserManager();
            UM.UpdateUserAccount(UPV);

            return Json(new { success = true });
        }

        public ActionResult UpdateProductData(int id , string  pName,int  pPrice, string address)
        {
            ProductDataView PDV = new ProductDataView();
            PDV.product_id=id;
            PDV.product_name = pName;
            PDV.product_price = pPrice;
            PDV.product_address = address;

            UserManager UM = new UserManager();
            UM.UpdateProduct(PDV);

            return Json(new { success = true });
        }

        public ActionResult UpdateSupllierData(int id, string sName, string sPhone, string address)
        {
            SupllierDataView SDV = new SupllierDataView();
            SDV.SupllierID = id;
            SDV.SupllierName = sName;
            SDV.SupllierPhone = sPhone;
            SDV.SupllierAddress = address;

            UserManager UM = new UserManager();
            UM.UpdateSupllier(SDV);

            return Json(new { success = true });
        }

        [AuthorizeRoles("Admin")]
        public ActionResult DeleteUser(int userID)
        {
            UserManager UM = new UserManager();
            UM.DeleteUser(userID);
            return Json(new { success = true });
        }

        [AuthorizeRoles("Admin")]
        public ActionResult DeleteProduct(int productID)
        {
            UserManager UM = new UserManager();
            UM.DeleteProduct(productID);
            return Json(new { success = true });
        }
        public ActionResult DeleteSupllier(int supllierID)
        {
            UserManager UM = new UserManager();
            UM.DeleteSupllier(supllierID);
            return Json(new { success = true });
        }


        [Authorize]
        public ActionResult EditProfile()
        {
            string loginName = User.Identity.Name;
            UserManager UM = new UserManager();
            UserProfileView UPV = UM.GetUserProfile(UM.GetUserID(loginName));
            return View(UPV);
        }


        [HttpPost]
        [Authorize]
        public ActionResult EditProfile(UserProfileView profile)
        {
            if (ModelState.IsValid)
            {
                UserManager UM = new UserManager();
                UM.UpdateUserAccount(profile);

                ViewBag.Status = "Update Sucessful!";
            }
            return View(profile);
        }
    }
}