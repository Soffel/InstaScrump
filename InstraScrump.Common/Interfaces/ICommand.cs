using System.Threading.Tasks;

namespace InstaScrump.Common.Interfaces
{
    public interface ICommand
    {
        bool CommandString(string cmd);
        Task Execute(string[] args);
        string HelpText();
    }
}
