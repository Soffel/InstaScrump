using System.Threading.Tasks;

namespace InstaScrump.Interface
{
    public interface ICommand
    {
        string CommandString();
        Task Execute(string[] args);
        string HelpText();
    }
}
