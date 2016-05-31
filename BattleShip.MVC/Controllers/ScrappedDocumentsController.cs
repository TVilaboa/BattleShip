using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BattleShip.Common;
using BattleShip.Domain;
using BattleShip.MVC.Services;


namespace BattleShip.MVC.Controllers
{
    public class ScrappedDocumentsController : Controller
    {
        private ScrappedDocumentService service = new ScrappedDocumentService();

        // GET: ScrappedDocuments
        public async Task<ActionResult> Index(string author)
        {
            Expression<Func<ScrappedDocument,bool>> docFilter = x => x.UsedSchema.Code == "LaNacion" && x.ScrappedJson != "{}";
            if (!string.IsNullOrWhiteSpace(author))
            {
                docFilter = docFilter.AndAlso(x => x.ScrappedJson.Contains(author));
            }
            var docs = new ScrappedDocumentService().FindAll(docFilter);
            await Task.WhenAll(docs);
            return View(docs.Result);
        }

        // GET: ScrappedDocuments/Details/5
        public async Task<ActionResult> Details(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScrappedDocument scrappedDocument = await service.Find(x => x.Code == code);
            if (scrappedDocument == null)
            {
                return HttpNotFound();
            }
            return View(scrappedDocument);
        }

        // GET: ScrappedDocuments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ScrappedDocuments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Code,Id,ScrappedJson,CreationDate,UpdateDate,Uri,ScrapIdentifier,Name")] ScrappedDocument scrappedDocument)
        {
            if (ModelState.IsValid)
            {
                service.AddOrUpdate(scrappedDocument);
              
                return RedirectToAction("Index");
            }

            return View(scrappedDocument);
        }

        // GET: ScrappedDocuments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScrappedDocument scrappedDocument = await service.Find(x => x.Id == id.Value);
            if (scrappedDocument == null)
            {
                return HttpNotFound();
            }
            return View(scrappedDocument);
        }

        // POST: ScrappedDocuments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Code,Id,ScrappedJson,CreationDate,UpdateDate,Uri,ScrapIdentifier,Name")] ScrappedDocument scrappedDocument)
        {
            if (ModelState.IsValid)
            {
                //service.Entry(scrappedDocument).State = EntityState.Modified;
                //await service.SaveChangesAsync();
                service.AddOrUpdate(scrappedDocument);
                return RedirectToAction("Index");
            }
            return View(scrappedDocument);
        }

        // GET: ScrappedDocuments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScrappedDocument scrappedDocument = await service.Find(x => x.Id == id.Value);
            if (scrappedDocument == null)
            {
                return HttpNotFound();
            }
            return View(scrappedDocument);
        }

        // POST: ScrappedDocuments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScrappedDocument scrappedDocument = await service.Find(x => x.Id == id.Value);
            if (scrappedDocument == null)
            {
                return HttpNotFound();
            }
            scrappedDocument.IsDeleted = true;
            service.AddOrUpdate(scrappedDocument);
            //service.ScrappedDocuments.Remove(scrappedDocument);
            //await service.SaveChangesAsync();
            return RedirectToAction("Index");
        }

      
    }
}
