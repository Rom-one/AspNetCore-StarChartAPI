﻿using System;
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

        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();

            return CreatedAtRoute("GetById", new { id = celestialObject.Id }, celestialObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
            var currentObject = _context.CelestialObjects.FirstOrDefault(c => c.Id == id);

            if (currentObject == null)
                return NotFound();

            currentObject.Name = celestialObject.Name;
            currentObject.OrbitalPeriod = celestialObject.OrbitalPeriod;
            currentObject.OrbitedObjectId = celestialObject.OrbitedObjectId;

            _context.CelestialObjects.Update(currentObject);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var currentObject = _context.CelestialObjects.FirstOrDefault(c => c.Id == id);

            if (currentObject == null)
                return NotFound();

            currentObject.Name = name;

            _context.CelestialObjects.Update(currentObject);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var objects = _context.CelestialObjects.Where(c => c.Id == id || c.OrbitedObjectId == id);

            if (objects.Count() == 0)
                return NotFound();

            _context.CelestialObjects.RemoveRange(objects);
            _context.SaveChanges();

            return NoContent();
        }
    }

    
}
