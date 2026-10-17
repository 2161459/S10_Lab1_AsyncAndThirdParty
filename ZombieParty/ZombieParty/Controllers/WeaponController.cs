using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZombieParty.Models;
using ZombieParty.Models.Data;
using ZombieParty.ViewModels;

namespace ZombieParty.Controllers
{
    public class WeaponController : Controller
    {
        private ZombiePartyDbContext _baseDonnees { get; set; }

        public WeaponController(ZombiePartyDbContext baseDonnees)
        {
            _baseDonnees = baseDonnees;
        }

        public async Task<IActionResult> Index()
        {
            List<Weapon> weapons = await _baseDonnees.Weapons.ToListAsync();
            return View(weapons);
        }

        //GET
        public async Task<IActionResult> Upsert(int? id)
        {
            if (id == null || id == 0)
                // create
                return View(new Weapon());
            else
                //update
                return View(await _baseDonnees.Weapons.FindAsync(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Weapon weapon)
        {
            if (ModelState.IsValid)
            {
                // Create
                if (weapon.WeaponId == 0)
                {
                    // Ajouter à la BD
                    await _baseDonnees.Weapons.AddAsync(weapon);
                    TempData["Success"] = $"{weapon.Name} weapon added";
                }
                else
                {
                    // Update
                    _baseDonnees.Weapons.Update(weapon);
                    TempData["success"] = $"{weapon.Name} weapon updated";
                }
                await _baseDonnees.SaveChangesAsync();

                return this.RedirectToAction("Index");
            }

            return this.View(weapon);
        }


        //Pour voir detail des Weapon 1. Ajouter view dans weapon apres
        // GET DETAILS
        public IActionResult Details(int id)
        {
            Weapon? weapon = _baseDonnees.Weapons.Find(id);

            if (weapon == null)
            {
                return NotFound();
            }

            return View(weapon);
        }


        //Pour que le lien delete fonctionne. Create view delete aussi
        public IActionResult Delete(int id)
        {
            Weapon? weapon = _baseDonnees.Weapons.Find(id);

            if (weapon == null)
            {
                return NotFound();
            }

            return View(weapon);
        }

        //Pour actually le delete
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int id)
        {
            Weapon? weapon = _baseDonnees.Weapons.Find(id);

            if (weapon == null)
            {
                return NotFound();
            }

            _baseDonnees.Weapons.Remove(weapon);
            _baseDonnees.SaveChanges();

            TempData["Success"] = $"{weapon.Name} weapon deleted";

            return RedirectToAction("Index");
        }
    }
}
