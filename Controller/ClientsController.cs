using Microsoft.AspNetCore.Mvc;
using ClientManagement.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ClientManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IClientService _clientService;

        public ClientsController(AppDbContext context, IClientService clientService)
        {
            _context = context;
            _clientService = clientService;
        }

        //=====================Get===================================

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            return await _context.Clients.ToListAsync();
        }

        //=====================Update&&Add===================================
        [HttpPost]
        public async Task<ActionResult<Client>> CreateClient(Client client)
        {
            if (!await _clientService.IsEmailUniqueAsync(client.Email, client.Id))
            {
                return BadRequest("Email already exists.");
            }

            if (client.Id > 0)
            {
                var existingClient = await _context.Clients.FindAsync(client.Id);
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
                await _context.SaveChangesAsync();

                return Ok(existingClient);
            }
            else
            {
                client.Id = 0;
                client.RegDate = DateTime.UtcNow;
                _context.Clients.Add(client);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetClients), new { id = client.Id }, client);
            }
        }

        //=====================Delete===================================

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
