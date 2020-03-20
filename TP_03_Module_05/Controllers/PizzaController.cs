using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TP_03_Module_05.BO;
using TP_03_Module_05.Models;
using TP_03_Module_05.Utils;

namespace TP_03_Module_05.Controllers
{
    public class PizzaController : Controller
    {
        // GET: Pizza
        public ActionResult Index()
        {
            return View(FakeDb.Instance.Pizzas);
        }

        // GET: Pizza/Create
        public ActionResult Create()
        {
            PizzaViewModel vm = new PizzaViewModel();

            return View(populateViewModel(vm));
        }

        // POST: Pizza/Create
        [HttpPost]
        public ActionResult Create(PizzaViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Pizza pizza = vm.Pizza;

                    List<Ingredient> ingredients = FakeDb.Instance.IngredientsDisponible.Where(
                        x => vm.IdsIngredients.Contains(x.Id))
                        .ToList();

                    if (ingredients.Count <2)
                    {
                        ModelState.AddModelError("", "Veuillez choisir 2 ingrédients au minimum.");
                        return View(populateViewModel(vm));
                    }
                    if (ingredients.Count > 5)
                    {
                        ModelState.AddModelError("", "Veuillez choisir 5 ingrédients maximum.");
                        return View(populateViewModel(vm));
                    }
                    if (FakeDb.Instance.Pizzas.Any(p => p.Nom == pizza.Nom && p.Id != pizza.Id)) {
                        ModelState.AddModelError("", "Il existe déjà pizza avec ce nom.");
                        return View(populateViewModel(vm));
                    }
                    
                    if (FakeDb.Instance.Pizzas.Any(p => CompareList(p.Ingredients, ingredients) && p.Id != pizza.Id))
                    {
                        ModelState.AddModelError("", "Il existe déjà pizza avec cette même liste d'ingrédients.");
                        return View(populateViewModel(vm));
                    }

                    pizza.Pate = FakeDb.Instance.PatesDisponible.FirstOrDefault(x => x.Id == vm.IdPate);

                    pizza.Ingredients = ingredients;

                    pizza.Id = FakeDb.Instance.Pizzas.Count == 0 ? 1 : FakeDb.Instance.Pizzas.Max(x => x.Id) + 1;

                    FakeDb.Instance.Pizzas.Add(pizza);

                    return RedirectToAction("Index");
                }
                return View(populateViewModel(vm));
            }
            catch
            {
                return View(populateViewModel(vm));
            }
        }

        // GET: Pizza/Edit/5
        public ActionResult Edit(int id)
        {
            PizzaViewModel vm = new PizzaViewModel();

            vm.Pates = FakeDb.Instance.PatesDisponible.Select(
                x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                .ToList();

            vm.Ingredients = FakeDb.Instance.IngredientsDisponible.Select(
                x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                .ToList();

            vm.Pizza = FakeDb.Instance.Pizzas.FirstOrDefault(x => x.Id == id);

            if (vm.Pizza.Pate != null)
            {
                vm.IdPate = vm.Pizza.Pate.Id;
            }

            if (vm.Pizza.Ingredients.Any())
            {
                vm.IdsIngredients = vm.Pizza.Ingredients.Select(x => x.Id).ToList();
            }

            return View(vm);
        }

        // POST: Pizza/Edit/5
        [HttpPost]
        public ActionResult Edit(PizzaViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Pizza pizza = FakeDb.Instance.Pizzas.FirstOrDefault(x => x.Id == vm.Pizza.Id);

                    pizza.Nom = vm.Pizza.Nom;

                    List<Ingredient> ingredients = FakeDb.Instance.IngredientsDisponible.Where(
                        x => vm.IdsIngredients.Contains(x.Id))
                        .ToList();

                    if (ingredients.Count < 2)
                    {
                        ModelState.AddModelError("", "Veuillez choisir 2 ingrédients au minimum.");
                        return View(populateViewModel(vm));
                    }
                    if (ingredients.Count > 5)
                    {
                        ModelState.AddModelError("", "Veuillez choisir 5 ingrédients maximum.");
                        return View(populateViewModel(vm));
                    }
                    if (FakeDb.Instance.Pizzas.Any(p => p.Nom == pizza.Nom && p.Id != pizza.Id))
                    {
                        ModelState.AddModelError("", "Il existe déjà pizza avec ce nom.");
                        return View(populateViewModel(vm));
                    }
                    if (FakeDb.Instance.Pizzas.Any(p => CompareList(p.Ingredients, ingredients) && p.Id != pizza.Id))
                    {
                        ModelState.AddModelError("", "Il existe déjà pizza avec cette même liste d'ingrédients.");
                        return View(populateViewModel(vm));
                    }

                    pizza.Pate = FakeDb.Instance.PatesDisponible.FirstOrDefault(x => x.Id == vm.IdPate);
                    pizza.Ingredients = ingredients;

                    return RedirectToAction("Index");
                }
                return View(populateViewModel(vm));
            }
            catch
            {
                return View(populateViewModel(vm));
            }
        }

        // GET: Pizza/Delete/5
        public ActionResult Delete(int id)
        {
            return View(FakeDb.Instance.Pizzas.FirstOrDefault(x => x.Id == id));
        }

        // POST: Pizza/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                Pizza pizza = FakeDb.Instance.Pizzas.FirstOrDefault(x => x.Id == id);
                FakeDb.Instance.Pizzas.Remove(pizza);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public PizzaViewModel populateViewModel(PizzaViewModel vm)
        {
            vm.Pates = FakeDb.Instance.PatesDisponible.Select(
                x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                .ToList();

            vm.Ingredients = FakeDb.Instance.IngredientsDisponible.Select(
                x => new SelectListItem { Text = x.Nom, Value = x.Id.ToString() })
                .ToList();

            return vm;
        }

        public static bool CompareList<T>(List<T> list1, List<T> list2)
        {
            bool flag = true;

            foreach (T obj in list1)
            {
                if (!obj.Equals(list2[list1.IndexOf(obj)]))
                {
                    flag = false;
                }
            }

            return flag;
        }
    }
}
