using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ExemploAspNetMvc.Models;

namespace ExemploAspNetMvc.Controllers
{
    public class ProfissoesController : Controller
    {
        private Cadastro db = new Cadastro();

        // GET: Profissoes
        public ActionResult Index()
        {
            return View(db.Cad_Profissoes.ToList());
        }

        // GET: Profissoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cad_Profissoes cad_Profissoes = db.Cad_Profissoes.Find(id);
            if (cad_Profissoes == null)
            {
                return HttpNotFound();
            }
            return View(cad_Profissoes);
        }

        // GET: Profissoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Profissoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Pro_ID,Pro_Nome,Pro_Descricao")] Cad_Profissoes cad_Profissoes)
        {
            if (ModelState.IsValid)
            {
                cad_Profissoes.Pro_DtInc = DateTime.Now;
                cad_Profissoes.Pro_DtAlt = DateTime.Now;

                db.Cad_Profissoes.Add(cad_Profissoes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cad_Profissoes);
        }

        // GET: Profissoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cad_Profissoes cad_Profissoes = db.Cad_Profissoes.Find(id);
            if (cad_Profissoes == null)
            {
                return HttpNotFound();
            }
            return View(cad_Profissoes);
        }

        // POST: Profissoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Pro_ID,Pro_Nome,Pro_Descricao,Pro_DtInc")] Cad_Profissoes cad_Profissoes)
        {
            if (ModelState.IsValid)
            {
                cad_Profissoes.Pro_DtAlt = DateTime.Now;

                db.Entry(cad_Profissoes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cad_Profissoes);
        }

        // GET: Profissoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cad_Profissoes cad_Profissoes = db.Cad_Profissoes.Find(id);
            if (cad_Profissoes == null)
            {
                return HttpNotFound();
            }
            return View(cad_Profissoes);
        }

        // POST: Profissoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cad_Profissoes cad_Profissoes = db.Cad_Profissoes.Find(id);
            db.Cad_Profissoes.Remove(cad_Profissoes);
            db.SaveChanges();
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
