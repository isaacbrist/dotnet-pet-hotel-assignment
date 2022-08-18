using System.Net.NetworkInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using pet_hotel.Models;
using Microsoft.EntityFrameworkCore;

namespace pet_hotel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetsController : ControllerBase
    {
        private readonly ApplicationContext _context;
        public PetsController(ApplicationContext context)
        {
            _context = context;
        }

        // This is just a stub for GET / to prevent any weird frontend errors that 
        // occur when the route is missing in this controller
        [HttpGet]
        public IEnumerable<Pet> GetPets()
        {
            //Include joins table together to grab petOwner id  
            return _context.Pets.Include(pet => pet.petOwner);

        }

        //GET selected pet
        [HttpGet("{id}")]
        public ActionResult<Pet> GetById(int id)
        {
            Pet pet = _context.Pets
                .SingleOrDefault(pet => pet.id == id);

            // Return a `404 Not Found` if the baker doesn't exist
            if (pet is null)
            {
                return NotFound();
            }

            return pet;
        }

        //post pet
        [HttpPost]
        public Pet Post(Pet pet)
        {
            _context.Add(pet);
            _context.SaveChanges();
            return pet;
        }

        //update pet
        [HttpPut("{id}")]

        public Pet Put(int id, Pet pet)
        {
            pet.id = id;
            _context.Update(pet);
            _context.SaveChanges();
            return pet;
        }

        //update check-in
        [HttpPut("/api/pets/{id}/checkin")]

        public IActionResult CheckIn(int id)
        {
            Pet petToUpdate = _context.Pets.Find(id);
            if (petToUpdate == null) return NotFound();
            petToUpdate.checkedIn();
            _context.Update(petToUpdate);
            _context.SaveChanges();
            return Ok(petToUpdate);
        }

        //update check-out
        [HttpPut("/api/pets/{id}/checkout")]

        public IActionResult CheckOut(int id)
        {
            Pet petToUpdate = _context.Pets.Find(id);
            if (petToUpdate == null) return NotFound();
            petToUpdate.checkedIn();
            _context.Update(petToUpdate);
            _context.SaveChanges();
            return Ok(petToUpdate);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            // Find the bread, by ID
            Pet pet = _context.Pets.Find(id);

            // Tell the DB that we want to remove this bread
            _context.Pets.Remove(pet);

            // ...and save the changes to the database
            _context.SaveChanges();
        }

    }
}
