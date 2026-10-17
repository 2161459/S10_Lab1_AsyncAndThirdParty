using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ZombieParty.Models;
using ZombieParty.Models.Data;
using ZombieParty.ViewModels;

//Pour utiliser ToListAsync(), FindAsync() et SaveChangesAsync().
using Microsoft.EntityFrameworkCore;

namespace ZombieParty.Controllers
{
    public class ZombieTypeController : Controller
    {
        private ZombiePartyDbContext _baseDonnees { get; set; }

        public ZombieTypeController(ZombiePartyDbContext baseDonnees)
        {
            _baseDonnees = baseDonnees;
        }

        //public IActionResult Index()
        //{
        //    List<ZombieType> zombieTypesList = _baseDonnees.ZombieTypes.ToList();

        //    return View(zombieTypesList);
        //}


        //Rendre Index Async
        public async Task<IActionResult> Index() //Remplace IActionResult.
        {
            List<ZombieType> zombieTypes =
                await _baseDonnees.ZombieTypes.ToListAsync(); //Indique que la méthode est asynchrone. //Attend le résultat de la requête. //Récupère la liste de manière asynchrone.

            return View(zombieTypes);
        }








        //public IActionResult Details(int id)
        //{
        //    var zombies = _baseDonnees.Zombies.Where(z => z.ZombieTypeId == id);

        //    ZombieTypeVM zombieTypeVM = new()
        //    {
        //        ZombieType = new(),
        //        ZombiesList = zombies.ToList(),
        //        ZombiesCount = zombies.Count(),
        //        PointsAverage = zombies.Average(p => p.Point)
        //    };

        //    zombieTypeVM.ZombieType = _baseDonnees.ZombieTypes.FirstOrDefault(zt => zt.Id == id);
        //    return View(zombieTypeVM);
        //}


        //Rendre Async
        // GET DETAILS
        public async Task<IActionResult> Details(int id) //IActionResult → async Task<IActionResult>
        {
            ZombieType? zombieType =
                await _baseDonnees.ZombieTypes
                    .FirstOrDefaultAsync(zt => zt.Id == id); //FirstOrDefault() → await FirstOrDefaultAsync()

            if (zombieType == null)
            {
                return NotFound();
            }

            List<Zombie> zombies = await _baseDonnees.Zombies
                .Where(z => z.ZombieTypeId == id)
                .ToListAsync(); //ToList() → await ToListAsync() et Count() → Count sur la liste déjà chargée.

            ZombieTypeVM zombieTypeVM = new()
            {
                ZombieType = zombieType,
                ZombiesList = zombies,
                ZombiesCount = zombies.Count,
                PointsAverage = zombies.Count > 0
                    ? zombies.Average(z => z.Point) //Average() reste identique, car on calcule maintenant la moyenne en mémoire.
                    : 0
            };

            return View(zombieTypeVM);
        }






        //GET CREATE
        //Cette méthode ne fait aucune opération asynchrone avec la base de données.
        public IActionResult Create()
        {
            return View();
        }




        ////POST
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(ZombieType zombieType)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Ajouter à la BD
        //        _baseDonnees.ZombieTypes.Add(zombieType);
        //        _baseDonnees.SaveChanges();
        //        TempData["Success"] = $"{zombieType.TypeName} zombie type added";
        //        return this.RedirectToAction("Index");
        //    }

        //    return this.View(zombieType);
        //}



        //Rendre Async
        // POST CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ZombieType zombieType) //Ajouter le Async
        {
            if (ModelState.IsValid)
            {
                // Ajouter à la BD
                _baseDonnees.ZombieTypes.Add(zombieType);

                await _baseDonnees.SaveChangesAsync(); //Ici Changer

                TempData["Success"] = $"{zombieType.TypeName} zombie type added";

                return RedirectToAction("Index");
            }

            return View(zombieType);
        }







        //public IActionResult Edit(int id)
        //{
        //    ZombieType zombieType = _baseDonnees.ZombieTypes.Find(id);

        //    return View(zombieType);
        //}


        //Rendre Async
        // GET EDIT
        public async Task<IActionResult> Edit(int id) //Async ajouter
        {
            ZombieType? zombieType =
                await _baseDonnees.ZombieTypes.FindAsync(id); //Find Changer

            if (zombieType == null) //Ajout d'une vérification si le zombie type n'existe pas.
            {
                return NotFound();
            }

            return View(zombieType);
        }





        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit(ZombieType zombieType)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _baseDonnees.ZombieTypes.Update(zombieType);
        //        _baseDonnees.SaveChanges();
        //        TempData["Success"] = $"ZombieType {zombieType.TypeName} has been modified";
        //        return this.RedirectToAction("Index");
        //    }

        //    return View(zombieType);
        //}


        //Rendre Async
        // POST EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ZombieType zombieType)
        {
            if (ModelState.IsValid)
            {
                _baseDonnees.ZombieTypes.Update(zombieType);

                await _baseDonnees.SaveChangesAsync();

                TempData["Success"] =
                    $"ZombieType {zombieType.TypeName} has been modified";

                return RedirectToAction("Index");
            }

            return View(zombieType);
        }






        //public IActionResult Delete(int id)
        //{
        //    ZombieType? zombieType = _baseDonnees.ZombieTypes.Find(id);
        //    if (zombieType == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(zombieType);
        //}

        //Rendre Async
        // GET DELETE
        public async Task<IActionResult> Delete(int id) //Ajouter Async
        {
            ZombieType? zombieType =
                await _baseDonnees.ZombieTypes.FindAsync(id); //await FindAsync()

            if (zombieType == null)
            {
                return NotFound();
            }

            return View(zombieType);
        }





        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult DeletePost(int id)
        //{
        //    ZombieType? zombieType = _baseDonnees.ZombieTypes.Find(id);
        //    if (zombieType == null)
        //    {
        //        return NotFound();
        //    }

        //    _baseDonnees.ZombieTypes.Remove(zombieType);
        //    _baseDonnees.SaveChanges();
        //    TempData["Success"] = $"ZombieType {zombieType.TypeName} has been removed";
        //    return RedirectToAction("Index");
        //}

        //Rendre async
        // POST DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int id) //Async
        {
            ZombieType? zombieType =
                await _baseDonnees.ZombieTypes.FindAsync(id); //await FindAsync().

            if (zombieType == null)
            {
                return NotFound();
            }

            _baseDonnees.ZombieTypes.Remove(zombieType); //reste identique.

            await _baseDonnees.SaveChangesAsync(); //await SaveChangesAsync().

            TempData["Success"] =
                $"ZombieType {zombieType.TypeName} has been removed";

            return RedirectToAction("Index");
        }
    }
}
