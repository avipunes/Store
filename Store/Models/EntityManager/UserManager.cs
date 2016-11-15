using System;
using System.Collections.Generic;
using Store.Models.ViewModel;
using Store.Models.DB;
using System.Linq;
using System.Web;

namespace Store.Models.EntityManager
{
    public class UserManager
    {
        public void AddUserAccount(UserSignUpView user)
        {

            using (mydbEntities db = new mydbEntities())
            {

                SYSUser SU = new SYSUser();
                SU.LoginName = user.LoginName;
                SU.PasswordEncryptedText = user.Password;
                SU.RowCreatedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                SU.RowModifiedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1; ;
                SU.RowCreatedDateTime = DateTime.Now;
                SU.RowMOdifiedDateTime = DateTime.Now;

                db.SYSUsers.Add(SU);
                db.SaveChanges();

                SYSUserProfile SUP = new SYSUserProfile();
                SUP.SYSUserID = SU.SYSUserID;
                SUP.FirstName = user.FirstName;
                SUP.LastName = user.LastName;
                SUP.Gender = user.Gender;
                SUP.RowCreatedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                SUP.RowModifiedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                SUP.RowCreatedDateTime = DateTime.Now;
                SUP.RowModifiedDateTime = DateTime.Now;

                db.SYSUserProfiles.Add(SUP);
                db.SaveChanges();


                if (user.LOOKUPRoleID > 0)
                {
                    SYSUserRole SUR = new SYSUserRole();
                    SUR.LOOKUPRoleID = user.LOOKUPRoleID;
                    SUR.SYSUserID = SU.SYSUserID;
                    SUR.IsActive = true;
                    SUR.RowCreatedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                    SUR.RowModifiedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                    SUR.RowCreatedDateTime = DateTime.Now;
                    SUR.RowModifiedDateTime = DateTime.Now;

                    db.SYSUserRoles.Add(SUR);
                    db.SaveChanges();
                }
            }
        }

        public void AddProduct(ProductDataView product)
        {

            using (mydbEntities db = new mydbEntities())
            {

                var intIdt = db.Products.Max(u => (int)u.product_id);

                Product PR = new Product();
               
                PR.product_id = intIdt+1;

                PR.product_name = product.product_name;
                PR.product_price = product.product_price;
                PR.product_address = product.product_address;

                db.Products.Add(PR);
                db.SaveChanges();
            }
        }

        public void AddSupllier(SupllierDataView supllier)
        {

            using (mydbEntities db = new mydbEntities())
            {

                var intIdt = db.Suplliers.Max(u => (int)u.SupllierID);

                Supllier SR = new Supllier();

                SR.SupllierID = intIdt + 1;

                SR.SupllierName = supllier.SupllierName;
                SR.SupllierPhone = supllier.SupllierPhone;
                SR.SupllierAddress = supllier.SupllierAddress;

                db.Suplliers.Add(SR);
                db.SaveChanges();
            }
        }

        public bool IsLoginNameExist(string loginName)
        {
            using (mydbEntities db = new mydbEntities())
            {
                return db.SYSUsers.Where(o => o.LoginName.Equals(loginName)).Any();
            }
        }
        public string GetUserPassword(string loginName)
        {
            using (mydbEntities db = new mydbEntities())
            {

                var user = db.SYSUsers.Where(o => o.LoginName.ToLower().Equals(loginName));
                if (user.Any())
                    return user.FirstOrDefault().PasswordEncryptedText;
                else
                    return string.Empty;
            }
        }
        public bool IsUserInRole(string loginName, string roleName)
        {
            using (mydbEntities db = new mydbEntities())
            {
                SYSUser SU = db.SYSUsers.Where(o => o.LoginName.ToLower().Equals(loginName))?.FirstOrDefault();
                if (SU != null)
                {
                    var roles = from q in db.SYSUserRoles
                                join r in db.LOOKUPRoles on q.LOOKUPRoleID equals r.LOOKUPRoleID
                                where r.RoleName.Equals(roleName) && q.SYSUserID.Equals(SU.SYSUserID)
                                select r.RoleName;

                    if (roles != null)
                    {
                        return roles.Any();
                    }
                }

                return false;
            }
        }
        public List<LOOKUPAvailableRole> GetAllRoles()
        {
            using (mydbEntities db = new mydbEntities())
            {
                var roles = db.LOOKUPRoles.Select(o => new LOOKUPAvailableRole
                {
                    LOOKUPRoleID = o.LOOKUPRoleID,
                    RoleName = o.RoleName,
                    RoleDescription = o.RoleDescription
                }).ToList();

                return roles;
            }
        }

        public int GetUserRule(int ID)
        {
            using (mydbEntities db = new mydbEntities())
            {
              
                
                var Roles = db.SYSUserRoles.Where(o => o.SYSUserID.Equals(ID));

                if (Roles.Any())
                    return Roles.FirstOrDefault().SYSUserRoleID;
                else
                    return -1;
            }
        }
        public int GetUserID(string loginName)
        {
            using (mydbEntities db = new mydbEntities())
            {
                var user = db.SYSUsers.Where(o => o.LoginName.Equals(loginName));
                if (user.Any())
                    return user.FirstOrDefault().SYSUserID;
                            }
            return 0;
        }

        public List<ProductDataView> GetAllProducts()
        {
            List <ProductDataView> products = new List<ProductDataView>();

            using(mydbEntities db =new mydbEntities())
            {
                ProductDataView PR;
                var pros = db.Products.ToList();
                if (pros != null)
                {
                    foreach (Product p in pros)
                    {
                        PR = new ProductDataView();
                        PR = GetProductData(p.product_id);
                        products.Add(PR);
                    }
                }

            }

            return products;
        }

        public List<SupllierDataView> GetAllSuplliers()
        {
            List<SupllierDataView> supllier = new List<SupllierDataView>();

            using (mydbEntities db = new mydbEntities())
            {
                SupllierDataView SR;
                var sups = db.Suplliers.ToList();
                if (sups != null)
                {
                    foreach (Supllier s in sups)
                    {
                        SR = new SupllierDataView();

                        SR.SupllierID = s.SupllierID;
                        SR.SupllierName = s.SupllierName;
                        SR.SupllierPhone = s.SupllierPhone;
                        SR.SupllierAddress = s.SupllierAddress;

                        supllier.Add(SR);
                    }
                }

            }

            return supllier;
        }


        public List<UserProfileView> GetAllUserProfiles()
        {
            List<UserProfileView> profiles = new List<UserProfileView>();
            using (mydbEntities db = new mydbEntities())
            {
                UserProfileView UPV;
                var users = db.SYSUsers.ToList();

                foreach (SYSUser u in db.SYSUsers)
                {
                    UPV = new UserProfileView();
                    UPV.SYSUserID = u.SYSUserID;
                    UPV.LoginName = u.LoginName;
                    UPV.Password = u.PasswordEncryptedText;

                    var SUP = db.SYSUserProfiles.Find(u.SYSUserID);
                    if (SUP != null)
                    {
                        UPV.FirstName = SUP.FirstName;
                        UPV.LastName = SUP.LastName;
                        UPV.Gender = SUP.Gender;
                    }

                    var SUR = db.SYSUserRoles.Where(o => o.SYSUserID.Equals(u.SYSUserID));
                    if (SUR.Any())
                    {
                        var userRole = SUR.FirstOrDefault();
                        UPV.LOOKUPRoleID = userRole.LOOKUPRoleID;
                        UPV.RoleName = userRole.LOOKUPRole.RoleName;
                        UPV.IsRoleActive = userRole.IsActive;
                    }

                    profiles.Add(UPV);
                }
            }

            return profiles;
        }

        public ProductsDataView GetProductDataView()
        {
            ProductsDataView PDC = new ProductsDataView();
            List<ProductDataView> products = GetAllProducts();
            PDC.Products = products;

            return PDC;
        }

        public SuplliersDataView GetSupllierDataView()
        {
            SuplliersDataView SDC = new SuplliersDataView();
            List<SupllierDataView> Suplliers = GetAllSuplliers();
            SDC.suplliers = Suplliers;

            return SDC;
        }

        public UserDataView GetUserDataView(string loginName)
        {
            UserDataView UDV = new UserDataView();
            List<UserProfileView> profiles = GetAllUserProfiles();
            List<LOOKUPAvailableRole> roles = GetAllRoles();

            int? userAssignedRoleID = 0, userID = 0;
            string userGender = string.Empty;

            userID = GetUserID(loginName);
            using (mydbEntities db = new mydbEntities())
            {
                userAssignedRoleID = db.SYSUserRoles.Where(o => o.SYSUserID == userID)?.FirstOrDefault().LOOKUPRoleID;
                userGender = db.SYSUserProfiles.Where(o => o.SYSUserID == userID)?.FirstOrDefault().Gender;
            }

            List<Gender> genders = new List<Gender>();
            genders.Add(new Gender { Text = "Male", Value = "M" });
            genders.Add(new Gender { Text = "Female", Value = "F" });

            UDV.UserProfile = profiles;
            UDV.UserRoles = new UserRoles { SelectedRoleID = userAssignedRoleID, UserRoleList = roles };
            UDV.UserGender = new UserGender { SelectedGender = userGender, Gender = genders };
            return UDV;
        }

        public void UpdateUserAccount(UserProfileView user)
        {

            using (mydbEntities db = new mydbEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {

                        SYSUser SU = db.SYSUsers.Find(user.SYSUserID);
                        SU.LoginName = user.LoginName;
                        SU.PasswordEncryptedText = user.Password;
                        SU.RowCreatedSYSUserID = user.SYSUserID;
                        SU.RowModifiedSYSUserID = user.SYSUserID;
                        SU.RowCreatedDateTime = DateTime.Now;
                        SU.RowMOdifiedDateTime = DateTime.Now;

                        db.SaveChanges();

                        var userProfile = db.SYSUserProfiles.Where(o => o.SYSUserID == user.SYSUserID);
                        if (userProfile.Any())
                        {
                            SYSUserProfile SUP = userProfile.FirstOrDefault();
                            SUP.SYSUserID = SU.SYSUserID;
                            SUP.FirstName = user.FirstName;
                            SUP.LastName = user.LastName;
                            SUP.Gender = user.Gender;
                            SUP.RowCreatedSYSUserID = user.SYSUserID;
                            SUP.RowModifiedSYSUserID = user.SYSUserID;
                            SUP.RowCreatedDateTime = DateTime.Now;
                            SUP.RowModifiedDateTime = DateTime.Now;

                            db.SaveChanges();
                        }

                        if (user.LOOKUPRoleID > 0)
                        {
                            var userRole = db.SYSUserRoles.Where(o => o.SYSUserID == user.SYSUserID);
                            SYSUserRole SUR = null;
                            if (userRole.Any())
                            {
                                SUR = userRole.FirstOrDefault();
                                SUR.LOOKUPRoleID = user.LOOKUPRoleID;
                                SUR.SYSUserID = user.SYSUserID;
                                SUR.IsActive = true;
                                SUR.RowCreatedSYSUserID = user.SYSUserID;
                                SUR.RowModifiedSYSUserID = user.SYSUserID;
                                SUR.RowCreatedDateTime = DateTime.Now;
                                SUR.RowModifiedDateTime = DateTime.Now;
                            }
                            else
                            {
                                SUR = new SYSUserRole();
                                SUR.LOOKUPRoleID = user.LOOKUPRoleID;
                                SUR.SYSUserID = user.SYSUserID;
                                SUR.IsActive = true;
                                SUR.RowCreatedSYSUserID = user.SYSUserID;
                                SUR.RowModifiedSYSUserID = user.SYSUserID;
                                SUR.RowCreatedDateTime = DateTime.Now;
                                SUR.RowModifiedDateTime = DateTime.Now;
                                db.SYSUserRoles.Add(SUR);
                            }

                            db.SaveChanges();
                        }
                        dbContextTransaction.Commit();
                    }
                    catch
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }
        public void UpdateProduct(ProductDataView product)
        {

            using (mydbEntities db = new mydbEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {

                        Product PU = db.Products.Find(product.product_id);
                        PU.product_name= product.product_name;
                        PU.product_price = product.product_price;
                        PU.product_address = product.product_address;

                        db.SaveChanges();

                        dbContextTransaction.Commit();
                    }
                    catch
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }

        public void UpdateSupllier(SupllierDataView supllier)
        {

            using (mydbEntities db = new mydbEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {

                        Supllier SU = db.Suplliers.Find(supllier.SupllierID);
                        SU.SupllierName = supllier.SupllierName;
                        SU.SupllierPhone = supllier.SupllierPhone;
                        SU.SupllierAddress = supllier.SupllierAddress;

                        db.SaveChanges();

                        dbContextTransaction.Commit();
                    }
                    catch
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }


        public void DeleteUser(int userID)
        {
            using (mydbEntities db = new mydbEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {

                        var SUR = db.SYSUserRoles.Where(o => o.SYSUserID == userID);
                        if (SUR.Any())
                        {
                            db.SYSUserRoles.Remove(SUR.FirstOrDefault());
                            db.SaveChanges();
                        }

                        var SUP = db.SYSUserProfiles.Where(o => o.SYSUserID == userID);
                        if (SUP.Any())
                        {
                            db.SYSUserProfiles.Remove(SUP.FirstOrDefault());
                            db.SaveChanges();
                        }

                        var SU = db.SYSUsers.Where(o => o.SYSUserID == userID);
                        if (SU.Any())
                        {
                            db.SYSUsers.Remove(SU.FirstOrDefault());
                            db.SaveChanges();
                        }

                        dbContextTransaction.Commit();
                    }
                    catch
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }

        public void DeleteProduct(int productID)
        {
            using (mydbEntities db = new mydbEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var PR = db.Products.Where(o => o.product_id == productID);
                        if (PR.Any())
                        {
                            db.Products.Remove(PR.FirstOrDefault());
                            db.SaveChanges();
                        }
                        dbContextTransaction.Commit();
                    }
                    catch
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }

        public void DeleteSupllier(int supllierID)
        {
            using (mydbEntities db = new mydbEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var PR = db.Suplliers.Where(o => o.SupllierID == supllierID);
                        if (PR.Any())
                        {
                            db.Suplliers.Remove(PR.FirstOrDefault());
                            db.SaveChanges();
                        }
                        dbContextTransaction.Commit();
                    }
                    catch
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }

        public ProductDataView GetProductData(int productID)
        {
            ProductDataView PD = new ProductDataView();

            using (mydbEntities db = new mydbEntities())
            {
                var product = db.Products.Find(productID);
                if (product != null)
                {
                    PD.product_id = product.product_id;
                    PD.product_name = product.product_name;
                    PD.product_price = product.product_price;
                    PD.product_address = product.product_address;
                }
            }

            return PD;

        }



        public UserProfileView GetUserProfile(int userID)
        {
            UserProfileView UPV = new UserProfileView();
            using (mydbEntities db = new mydbEntities())
            {
                var user = db.SYSUsers.Find(userID);
                if (user != null)
                {
                    UPV.SYSUserID = user.SYSUserID;
                    UPV.LoginName = user.LoginName;
                    UPV.Password = user.PasswordEncryptedText;

                    var SUP = db.SYSUserProfiles.Find(userID);
                    if (SUP != null)
                    {
                        UPV.FirstName = SUP.FirstName;
                        UPV.LastName = SUP.LastName;
                        UPV.Gender = SUP.Gender;
                    }

                    var SUR = db.SYSUserRoles.Find(userID);
                    if (SUR != null)
                    {
                        UPV.LOOKUPRoleID = SUR.LOOKUPRoleID;
                        UPV.RoleName = SUR.LOOKUPRole.RoleName;
                        UPV.IsRoleActive = SUR.IsActive;
                    }
                }
            }
            return UPV;
        }
    }
}