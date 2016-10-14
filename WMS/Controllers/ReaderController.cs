﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WMS.Models;
using WMS.Models;
using PagedList;
using System.Text.RegularExpressions;
using WMS.Controllers.Filters;
using WMS.HelperClass;
using WMS.CustomClass;
namespace WMS.Controllers
{
    [CustomControllerAttributes]
    public class ReaderController : Controller
    {
        private TAS2013Entities db = new TAS2013Entities();

        

        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.LocationSortParm = sortOrder == "Location" ? "Location_desc" : "Location";
            ViewBag.RdrTypeSortParm = String.IsNullOrEmpty(sortOrder) ? "RdrType_desc" : "RdrType";
            ViewBag.VendorSortParm = String.IsNullOrEmpty(sortOrder) ? "Vendor_desc" : "Vendor";

            ViewBag.StatusSortParm = String.IsNullOrEmpty(sortOrder) ? "Status_desc" : "Status";
            User LoggedInUser = Session["LoggedUser"] as User;
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            List<Reader> readers = new List<Models.Reader>();
            int NoOfReaders = Convert.ToInt32(GlobalVaribales.NoOfDevices);
            if(GlobalVaribales.DeviceType=="1")
                readers = db.Readers.Where(aa => aa.ReaderType.Category == "CNS-Card").Take(NoOfReaders).ToList();
            if (GlobalVaribales.DeviceType == "2")
                readers = db.Readers.Where(aa => aa.ReaderType.Category == "CNS-FP" || aa.ReaderType.Category == "CNS-Card").Take(NoOfReaders).ToList();
            if (GlobalVaribales.DeviceType == "3")
                readers = db.Readers.Where(aa => aa.ReaderType.Category == "CNS-Face" || aa.ReaderType.Category == "CNS-FP" || aa.ReaderType.Category == "CNS-Card").Take(NoOfReaders).ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                readers = readers.Where(s => s.RdrName.ToUpper().Contains(searchString.ToUpper())
                     || s.ReaderType.RdrTypeName.ToUpper().Contains(searchString.ToUpper())
                     || s.IpAdd.ToUpper().Contains(searchString.ToUpper())).ToList();
            }

            switch (sortOrder)
            {
                case "name_desc":
                    readers = readers.OrderByDescending(s => s.RdrName).ToList();
                    break;

                case "Location_desc":
                    readers = readers.OrderByDescending(s => s.Location.LocName).ToList();
                    break;
                case "Location":
                    readers = readers.OrderBy(s => s.Location.LocName).ToList();
                    break;
                case "RdrType_desc":
                    readers = readers.OrderByDescending(s => s.ReaderType.RdrTypeName).ToList();
                    break;
                case "RdrType":
                    readers = readers.OrderBy(s => s.ReaderType.RdrTypeName).ToList();
                    break;
                
                case "Status_desc":
                    readers = readers.OrderByDescending(s => s.Status).ToList();
                    break;
                case "Status":
                    readers = readers.OrderBy(s => s.Status).ToList();
                    break;
                default:
                    readers = readers.OrderBy(s => s.RdrName).ToList();
                    break;
            }
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(readers.ToPagedList(pageNumber, pageSize));
                //return View(readers.ToList());
        }

        // GET: /Reader/Details/5
        [CustomActionAttribute]
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reader reader = db.Readers.Find(id);
            if (reader == null)
            {
                return HttpNotFound();
            }
            return View(reader);
        }

        // GET: /Reader/Create
        [CustomActionAttribute]
        public ActionResult Create()
        {
            ViewBag.LocID = new SelectList(db.Locations.OrderBy(s=>s.LocName), "LocID", "LocName");
            ViewBag.RdrDutyID = new SelectList(db.RdrDutyCodes.OrderBy(s=>s.RdrDutyName), "RdrDutyID", "RdrDutyName");
            if (GlobalVaribales.DeviceType == "1")
                ViewBag.RdrTypeID = new SelectList(db.ReaderTypes.Where(aa => aa.Category == "CNS-Card").OrderBy(s => s.RdrTypeName), "RdrTypeID", "RdrTypeName");
            if (GlobalVaribales.DeviceType == "2")
                ViewBag.RdrTypeID = new SelectList(db.ReaderTypes.Where(aa => aa.Category == "CNS-FP" || aa.Category == "CNS-Card").OrderBy(s => s.RdrTypeName), "RdrTypeID", "RdrTypeName");
            if (GlobalVaribales.DeviceType == "3")
                ViewBag.RdrTypeID = new SelectList(db.ReaderTypes.Where(aa => aa.Category == "CNS-Face" || aa.Category == "CNS-FP" || aa.Category == "CNS-Card").OrderBy(s => s.RdrTypeName), "RdrTypeID", "RdrTypeName");
            
            return View();
        }

        // POST: /Reader/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult Create([Bind(Include = "RdrID,RdrName,RdrDutyID,IpAdd,IpPort,RdrTypeID,Status,LocID,ClearRecords")] Reader reader)
        {
            // Regex for IP [0-9]+(\.[0-9][0-9]?)?
            reader.ClearRecords = (bool)ValueProvider.GetValue("ClearRecords").ConvertTo(typeof(bool));
            reader.Status = (bool)ValueProvider.GetValue("Status").ConvertTo(typeof(bool));
            if (db.Readers.Where(aa => aa.Status == true).Count() >= Convert.ToInt32(GlobalVaribales.NoOfDevices))
                ModelState.AddModelError("RdrName", "Your Readers has exceeded from License, Please upgrade your license");
            if (string.IsNullOrEmpty(reader.IpAdd))
                ModelState.AddModelError("IpAdd", "Required");
            if (string.IsNullOrEmpty(reader.RdrName))
                ModelState.AddModelError("RdrName", "Required");
            if (string.IsNullOrEmpty(reader.IpPort.ToString()))
                ModelState.AddModelError("IpPort", "Required");
            if (reader.IpAdd != null)
            {
                if (reader.IpAdd.Length > 15)
                    ModelState.AddModelError("IpAdd", "String length exceeds!");
                Match match = Regex.Match(reader.IpAdd, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
                if (!match.Success)
                {
                    ModelState.AddModelError("IpAdd", "Enter a valid IP Address");
                }
            }
            if (reader.RdrName != null)
            {
                if (reader.RdrName.Length > 50)
                    ModelState.AddModelError("RdrName", "String length exceeds!");
            }
            if (!string.IsNullOrEmpty(reader.IpPort.ToString()))
            {
                if (reader.IpPort.ToString().Length > 4)
                    ModelState.AddModelError("IpPort", "String length exceeds!");

            }

            if (ModelState.IsValid)
            {
                User LoggedInUser = Session["LoggedUser"] as User;
                db.Readers.Add(reader);
                db.SaveChanges();
                int _userID = Convert.ToInt32(Session["LogedUserID"].ToString());
                HelperClass.MyHelper.SaveAuditLog(_userID, (byte)MyEnums.FormName.Reader, (byte)MyEnums.Operation.Add, DateTime.Now);
                return RedirectToAction("Index");
            }

            ViewBag.LocID = new SelectList(db.Locations.OrderBy(s=>s.LocName), "LocID", "LocName", reader.LocID);
            ViewBag.RdrDutyID = new SelectList(db.RdrDutyCodes.OrderBy(s=>s.RdrDutyName), "RdrDutyID", "RdrDutyName", reader.RdrDutyID);
            if (GlobalVaribales.DeviceType == "1")
                ViewBag.RdrTypeID = new SelectList(db.ReaderTypes.Where(aa => aa.Category == "CNS-Card").OrderBy(s => s.RdrTypeName), "RdrTypeID", "RdrTypeName");
            if (GlobalVaribales.DeviceType == "2")
                ViewBag.RdrTypeID = new SelectList(db.ReaderTypes.Where(aa => aa.Category == "CNS-FP" || aa.Category == "CNS-Card").OrderBy(s => s.RdrTypeName), "RdrTypeID", "RdrTypeName");
            if (GlobalVaribales.DeviceType == "3")
                ViewBag.RdrTypeID = new SelectList(db.ReaderTypes.Where(aa => aa.Category == "CNS-Face" || aa.Category == "CNS-FP" || aa.Category == "CNS-Card").OrderBy(s => s.RdrTypeName), "RdrTypeID", "RdrTypeName");
            return View(reader);
        }
        // GET: /Reader/Edit/5
        [CustomActionAttribute]
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reader reader = db.Readers.Find(id);
            if (reader == null)
            {
                return HttpNotFound();
            }
            ViewBag.LocID = new SelectList(db.Locations.OrderBy(s=>s.LocName), "LocID", "LocName", reader.LocID);
            ViewBag.RdrDutyID = new SelectList(db.RdrDutyCodes.OrderBy(s=>s.RdrDutyName), "RdrDutyID", "RdrDutyName", reader.RdrDutyID);
            if (GlobalVaribales.DeviceType == "1")
                ViewBag.RdrTypeID = new SelectList(db.ReaderTypes.Where(aa => aa.Category == "CNS-Card").OrderBy(s => s.RdrTypeName), "RdrTypeID", "RdrTypeName", reader.RdrTypeID);
            if (GlobalVaribales.DeviceType == "2")
                ViewBag.RdrTypeID = new SelectList(db.ReaderTypes.Where(aa => aa.Category == "CNS-FP" || aa.Category == "CNS-Card").OrderBy(s => s.RdrTypeName), "RdrTypeID", "RdrTypeName", reader.RdrTypeID);
            if (GlobalVaribales.DeviceType == "3")
                ViewBag.RdrTypeID = new SelectList(db.ReaderTypes.Where(aa => aa.Category == "CNS-Face" || aa.Category == "CNS-FP" || aa.Category == "CNS-Card").OrderBy(s => s.RdrTypeName), "RdrTypeID", "RdrTypeName", reader.RdrTypeID);
            return View(reader);
        }

        // POST: /Reader/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult Edit([Bind(Include = "RdrID,RdrName,RdrDutyID,IpAdd,IpPort,RdrTypeID,Status,LocID,ClearRecords")] Reader reader)
        {
            if (string.IsNullOrEmpty(reader.IpAdd))
                ModelState.AddModelError("IpAdd", "Required");
            if (string.IsNullOrEmpty(reader.RdrName))
                ModelState.AddModelError("RdrName", "Required");
            if (string.IsNullOrEmpty(reader.IpPort.ToString()))
                ModelState.AddModelError("IpPort", "Required");
            if (reader.IpAdd != null)
            {
                if (reader.IpAdd.Length > 15)
                    ModelState.AddModelError("IpAdd", "Required");
                Match match = Regex.Match(reader.IpAdd, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
                if (!match.Success)
                {
                    ModelState.AddModelError("IpAdd", "Enter a valid IP Address");
                }
            }
            if (reader.RdrName != null)
            {
                if (reader.RdrName.Length > 50)
                    ModelState.AddModelError("RdrName", "Required");
            }
            if (!string.IsNullOrEmpty(reader.IpPort.ToString()))
            {
                if (reader.IpPort.ToString().Length > 4)
                    ModelState.AddModelError("IpPort", "Required");

            }
            reader.ClearRecords = (bool)ValueProvider.GetValue("ClearRecords").ConvertTo(typeof(bool));
            reader.Status = (bool)ValueProvider.GetValue("Status").ConvertTo(typeof(bool));
            if (ModelState.IsValid)
            {
                db.Entry(reader).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                int _userID = Convert.ToInt32(Session["LogedUserID"].ToString());
                HelperClass.MyHelper.SaveAuditLog(_userID, (byte)MyEnums.FormName.Reader, (byte)MyEnums.Operation.Edit, DateTime.Now);
                return RedirectToAction("Index");
            }
            ViewBag.LocID = new SelectList(db.Locations.OrderBy(s=>s.LocName), "LocID", "LocName", reader.LocID);
            ViewBag.RdrDutyID = new SelectList(db.RdrDutyCodes.OrderBy(s=>s.RdrDutyName), "RdrDutyID", "RdrDutyName", reader.RdrDutyID);
            if (GlobalVaribales.DeviceType == "1")
                ViewBag.RdrTypeID = new SelectList(db.ReaderTypes.Where(aa => aa.Category == "CNS-Card").OrderBy(s => s.RdrTypeName), "RdrTypeID", "RdrTypeName");
            if (GlobalVaribales.DeviceType == "2")
                ViewBag.RdrTypeID = new SelectList(db.ReaderTypes.Where(aa => aa.Category == "CNS-FP" || aa.Category == "CNS-Card").OrderBy(s => s.RdrTypeName), "RdrTypeID", "RdrTypeName");
            if (GlobalVaribales.DeviceType == "3")
                ViewBag.RdrTypeID = new SelectList(db.ReaderTypes.Where(aa => aa.Category == "CNS-Face" || aa.Category == "CNS-FP" || aa.Category == "CNS-Card").OrderBy(s => s.RdrTypeName), "RdrTypeID", "RdrTypeName");
            return View(reader);
        }

        // GET: /Reader/Delete/5
        [CustomActionAttribute]
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reader reader = db.Readers.Find(id);
            if (reader == null)
            {
                return HttpNotFound();
            }
            return View(reader);
        }

        // POST: /Reader/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomActionAttribute]
        public ActionResult DeleteConfirmed(short id)
        {
            Reader reader = db.Readers.Find(id);
            db.Readers.Remove(reader);
            db.SaveChanges();
            int _userID = Convert.ToInt32(Session["LogedUserID"].ToString());
            HelperClass.MyHelper.SaveAuditLog(_userID, (byte)MyEnums.FormName.Reader, (byte)MyEnums.Operation.Delete, DateTime.Now);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
