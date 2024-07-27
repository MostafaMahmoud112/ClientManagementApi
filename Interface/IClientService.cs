public interface IClientService
{
    Task<bool> IsEmailUniqueAsync(string email, int clientId);
}
