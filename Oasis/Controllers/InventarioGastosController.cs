﻿using Oasis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Oasis.Controllers
{
    public class InventarioGastosController : Controller
    {
        // GET: InventarioGastos
        public ActionResult Index()
        {
            as2oasis oasis = new as2oasis();
            return View();
        }

        // GET: InventarioGastos/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: InventarioGastos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InventarioGastos/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: InventarioGastos/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: InventarioGastos/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: InventarioGastos/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: InventarioGastos/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
