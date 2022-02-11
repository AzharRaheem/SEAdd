using SEAdd.Models;
using SEAdd.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SEAdd.Controllers
{
    [HandleError]
    [Authorize(Roles = "SuperAdmin")]
    public class BoardController : Controller
    {
        private readonly ApplicationDbContext db;
        public BoardController()
        {
            db = new ApplicationDbContext();
        }
        // GET: Board
        public ActionResult Index()
        {
            var boards = db.Boards.ToList();
            return View(boards);
        }

        public ActionResult AddNewBoard()
        {
            Board board = new Board();
            return View(board);
        }
        [HttpPost]
        public ActionResult AddNewBoard(Board model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var alreadyExist = db.Boards.Where(b => b.name == model.name).FirstOrDefault();
            if (alreadyExist != null)
            {
                ViewBag.ErrorMsg = "Board already exist.";
                return View(model);
            }
            db.Boards.Add(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult EditBoard(int id)
        {
            var board = db.Boards.Where(b => b.id == id).FirstOrDefault();
            return View(board);
        }
        [HttpPost]
        public ActionResult EditBoard(Board model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DeleteBoard(int id)
        {
            var board = db.Boards.Where(b => b.id == id).FirstOrDefault();
            if(board != null)
            {
                db.Boards.Remove(board);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
        }
    }
}