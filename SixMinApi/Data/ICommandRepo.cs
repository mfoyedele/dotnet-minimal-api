using SixMinApi.Models;

namespace SixMinApi.Data
{
    public interface IcommandRepo
    {
    Task SaveChanges();
    Task<Command?> GetCommandById(int id);
    Task<IEnumerable<Command>> GetAllCommands();

    }
}