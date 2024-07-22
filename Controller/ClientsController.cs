using Microsoft.AspNetCore.Mvc;
using ClientManagement.Models;
using System.Collections.Generic;
using System.Linq;

namespace ClientManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientsController(AppDbContext context)
        {
            _context = context;
        }
        //-=====================Get===================================

        [HttpGet]
        public ActionResult<IEnumerable<Client>> GetClients()
        {
            return _context.Clients.ToList();
        }
        //-=====================Update&&Add===================================
        [HttpPost]
        public ActionResult<Client> CreateClient(Client client)
        {
            if (client.Id > 0)  
            {
                var existingClient = _context.Clients.Find(client.Id);
                if (existingClient == null)
                {
                    return NotFound(); 
                }

                existingClient.FName = client.FName;
                existingClient.LName = client.LName;
                existingClient.Email = client.Email;
                existingClient.Phone = client.Phone;
                existingClient.RegDate = DateTime.UtcNow;

                _context.Clients.Update(existingClient);
                _context.SaveChanges();

                return Ok(existingClient);
            }
            else
            {
                client.Id = 0;
                client.RegDate = DateTime.UtcNow;
                _context.Clients.Add(client);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetClients), new { id = client.Id }, client); 
            }
        }
        //-=====================Delete===================================

        [HttpDelete("{id}")]
        public IActionResult DeleteClient(int id)
        {
           
            var client = _context.Clients.Find(id);

            if (client == null)
            {
                return NotFound(); 
            }

            _context.Clients.Remove(client); 
            _context.SaveChanges(); 

            return NoContent(); 
        }

    }
}
