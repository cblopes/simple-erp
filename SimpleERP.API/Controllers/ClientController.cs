﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleERP.API.Data;
using SimpleERP.API.Entities;
using SimpleERP.API.Models;

namespace SimpleERP.API.Controllers
{
    [Route("api-simple-erp/client")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ClientDbContext _context;
        public ClientController(IMapper mapper, ClientDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var clients = _context.Clients.Where(c => c.IsActive).ToList();

            var clientViewModel = _mapper.ProjectTo<ClientViewModel>(clients.AsQueryable()).ToList();

            return Ok(clientViewModel);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        { 
            var client = _context.Clients.SingleOrDefault(c => c.Id == id);

            if (client == null)
            {
                return NotFound();
            }

            var clientViewModel = _mapper.Map<ClientViewModel>(client);

            return Ok(clientViewModel);
        }

        [HttpPost]
        public IActionResult Post(ClientInputModel model)
        {
            var client = _mapper.Map<Client>(model);

            _context.Clients.Add(client);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = client.Id }, model);
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, ClientInputModel model)
        {
            var client = _mapper.Map<Client>(model);

            client = _context.Clients.SingleOrDefault(c => c.Id == id);

            if (client == null)
            {
                return NotFound();
            }

            client.Update(model.Name);

            _context.Clients.Update(client);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var client = _context.Clients.SingleOrDefault(c => c.Id == id);

            if (client == null)
            {
                return NotFound();
            }

            client.Delete();

            _context.Clients.Update(client);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
