using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [ApiController]
    [Route("")]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            CelestialObject celestialObject = null;

            if((celestialObject = _context.CelestialObjects.FirstOrDefault(c => c.Id == id)) != null)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == id).ToList();
                _context.SaveChanges();

                return Ok(celestialObject);
            }

            return NotFound();
        }

        [HttpGet("{name}", Name = "GetByName")]
        public IActionResult GetByName(string name)
        {
            List<CelestialObject> celestialObjects = null;

            if ((celestialObjects = _context.CelestialObjects.Where(c => c.Name == name).ToList()).Count > 0)
            {
                foreach (var item in celestialObjects)
                {
                    item.Satellites = new List<CelestialObject>();
                    item.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == ((int?) item.Id)).ToList();
                }
                _context.SaveChanges();

                return Ok(celestialObjects);
            }

            return NotFound();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<CelestialObject> celestialObjects = null;

            if ((celestialObjects = _context.CelestialObjects.ToList()) != null)
            {
                foreach (var item in celestialObjects)
                {
                    item.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == ((int?)item.Id)).ToList();
                }
                _context.SaveChanges();

                return Ok(celestialObjects);
            }

            return NotFound();
        }
    }

    
}
