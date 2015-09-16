using BikeTracker.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace BikeTracker.Controllers
{
    [Authorize(Roles = "IMEIAdmin,GeneralAdmin")]
    public class IMEIController : Controller
    {
        private ApplicationDbContext dbContext = new ApplicationDbContext();

        // GET: IMEI
        public ActionResult Index()
        {
            return View(dbContext.IMEIToCallsigns.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var imeiToCallsign = dbContext.IMEIToCallsigns.Find(id);

            if (imeiToCallsign == null)
                return HttpNotFound();

            return View(imeiToCallsign);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IMEI, CallSign, Type")]IMEIToCallsign imeiToCallsign)
        {
            if (ModelState.IsValid)
            {
                dbContext.IMEIToCallsigns.Add(imeiToCallsign);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(imeiToCallsign);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var imeiToCallsign = dbContext.IMEIToCallsigns.Find(id);
            if (imeiToCallsign == null)
                return HttpNotFound();

            return View(imeiToCallsign);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id, IMEI, CallSign, Type")]IMEIToCallsign imeiToCallsign)
        {
            if (ModelState.IsValid)
            {
                dbContext.Entry(imeiToCallsign).State = EntityState.Modified;
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(imeiToCallsign);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var iMEIToCallsign = dbContext.IMEIToCallsigns.Find(id);
            if (iMEIToCallsign == null)
            {
                return HttpNotFound();
            }
            return View(iMEIToCallsign);
        }

        // POST: LocationTracker/IMEIToCallsigns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var iMEIToCallsign = dbContext.IMEIToCallsigns.Find(id);
            dbContext.IMEIToCallsigns.Remove(iMEIToCallsign);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                dbContext.Dispose();

            base.Dispose(disposing);
        }
    }
}