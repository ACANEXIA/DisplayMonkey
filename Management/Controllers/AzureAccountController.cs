using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DisplayMonkey.Models;
using System.Text.RegularExpressions;
using System.Net;
using DisplayMonkey.Language;
using DisplayMonkey.AzureUtil;

namespace DisplayMonkey.Controllers
{
    public class AzureAccountController : BaseController
    {
        private DisplayMonkeyEntities db = new DisplayMonkeyEntities();

        #region Account Validation

        private static Regex _emailRgx = new Regex(Models.Constants.EmailMask);

        private void GetToken(AzureAccount az)
        {
            try
            {
                Match lnk = _emailRgx.Match(az.User);
                az.User = lnk.Success ? lnk.Value : "";

                if (!az.PasswordSet)
                    throw new ApplicationException(Resources.ProvideAccountPassword);
                
                TokenInfo ti = Token.GetGrantTypePassword(
                    az.Resource, 
                    az.ClientId, 
                    az.ClientSecret, 
                    az.User,
                    RsaUtil.Decrypt(az.Password), 
                    az.TenantId
                    );

                az.RefreshToken = ti.RefreshToken;
                az.AccessToken = ti.AccessToken;
                az.ExpiresOn = ti.ExpiresOn;
                az.IdToken = ti.IdToken;

                db.Entry(az).State = EntityState.Modified;
                //var errors = ModelState.Values.SelectMany(v => v.Errors).ToArray();
            }

            catch (AzureTokenException ex)
            {
                ModelState.AddModelError("User", ex.Details);
            }

            catch (ApplicationException ex)
            {
                ModelState.AddModelError("PasswordUnmasked", ex.Message);
            }
        }

        #endregion

        //
        // GET: /AzureAccount/

        public ActionResult Index()
        {
            return View(
                db.AzureAccounts
                    .OrderBy(x => x.Name)
                    .ToList()
                );
        }

        //
        // GET: /AzureAccount/Create

        public ActionResult Create()
        {
            AzureAccount az = new AzureAccount();
            az.init(db);
            FillResourceSelectList();
            return View(az);
        }

        //
        // POST: /AzureAccount/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Resource,ClientId,ClientSecret,TenantId,User,PasswordUnmasked")] AzureAccount az)
        {
            GetToken(az);

            if (ModelState.IsValid)
            {
                db.AzureAccounts.Add(az);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            FillResourceSelectList(az.Resource);
            return View(az);
        }

        //
        // GET: /AzureAccount/Edit/5

        public ActionResult Edit(int id = 0)
        {
            AzureAccount az = db.AzureAccounts.Find(id);
            if (az == null)
            {
                return View("Missing", new MissingItem(id));
            }

            FillResourceSelectList(az.Resource);
            return View(az);
        }

        //
        // POST: /AzureAccount/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AccountId,Name,Resource,ClientId,ClientSecret,TenantId,User,PasswordUnmasked")] AzureAccount az)
        {
            GetToken(az);

            if (ModelState.IsValid)
            {
                db.Entry(az).State = EntityState.Modified;
                db.Entry(az).Property(l => l.Password).IsModified = az.PasswordSet;
                //db.Entry(az).Property(l => l.Url).IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            FillResourceSelectList(az.Resource);
            return View(az);
        }

        //
        // GET: /AzureAccount/Delete/5

        public ActionResult Delete(int id = 0)
        {
            AzureAccount az = db.AzureAccounts.Find(id);
            if (az == null)
            {
                return View("Missing", new MissingItem(id));
            }
            return View(az);
        }

        //
        // POST: /AzureAccount/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AzureAccount az = db.AzureAccounts.Find(id);
            db.AzureAccounts.Remove(az);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private void FillResourceSelectList(AzureResources? selected = null)
        {
            ViewBag.AzureResources = selected.TranslatedSelectList();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}