using System.Threading.Tasks;

namespace InstaScrump.Interface
{
    public interface ICommand
    {
        bool CommandString(string cmd);
        Task Execute(string[] args);
        string HelpText();
    }
}
