using ClientManagement;
using Microsoft.EntityFrameworkCore;

public class ClientService : IClientService
{
    private readonly AppDbContext _context;

    public ClientService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsEmailUniqueAsync(string email, int clientId)
    {
        return !await _context.Clients.AnyAsync(c => c.Email == email && c.Id != clientId);
    }
}
