﻿using IronBarCode;
using Sprint33.Areas.Pharmacist.Models;
using Sprint33.Extensions;
using Sprint33.Models;
using Sprint33.PharmacyEntities;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sprint33.Areas.Pharmacist.Controllers
{
    public class CouponsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Pharmacist/Coupons
        public ActionResult Index()
        {
            var model = new CouponVM();

            model.Coupons = db.Coupons
                .ToArray()
                .OrderByDescending(c => c.StartDate)
                .Select(c => new CouponItem(c))
                .ToList();

            model.CategoryDropdown = new SelectList(db.Categories.ToList(), "Id", "Name");

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> AddCoupon(CouponVM model)
        {
            if (!ModelState.IsValid)
            {
                model.CategoryDropdown = new SelectList(db.Categories.ToList(), "Id", "Name");
                TempData["Error"] = "Something happened!";
                return RedirectToAction("index");
            }

            var code_upper = model.Code.ToUpper();

            if (!model.IsDatesValid())
            {
                TempData["Error"] = "Dates were invalid!";
                return RedirectToAction("index");
            }

            var isCodeValid = db.Coupons.IsCodeUnique(code_upper);
            if (!isCodeValid)
            {
                TempData["Error"] = "Coupon Code already exists";
                return RedirectToAction("index");
            }

            var catId_is_null = model.IsCategoryIdEmpty();

            Category category;
            if (model.CategoryId != null)
            {
                category = db.Categories.Find(model.CategoryId);
            }

            #region Generating QR
            string _path = Path.Combine(Server
                            .MapPath("~/Files/Coupons"), code_upper + ".png");
            try
            {
                //using IronBarCode;
                //using System.Drawing;
                // Styling a QR code and adding annotation text
                var MyBarCode = IronBarCode.BarcodeWriter.CreateBarcode(code_upper, BarcodeWriterEncoding.QRCode);
                MyBarCode.AddAnnotationTextAboveBarcode("Coupon");
                MyBarCode.AddBarcodeValueTextBelowBarcode();
                MyBarCode.SetMargins(100);
                MyBarCode.ChangeBarCodeColor(Color.Purple);
                // Save as HTML
                MyBarCode.SaveAsPng(_path);
            }
            catch (Exception e)
            {
                throw;
            }

            #endregion

            db.Coupons.Add(new Coupon
            {
                Code = code_upper,
                Description = model.Description,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                DiscountRate = model.DiscountRate,
                QRcodeURL = "/Files/Coupons/" + code_upper + ".png",
                CategoryId = catId_is_null ? null : model.CategoryId
            });

            await db.SaveChangesAsync();

            TempData["Success"] = "Coupon was successfully created";
            return RedirectToAction("index");
        }
    }
}