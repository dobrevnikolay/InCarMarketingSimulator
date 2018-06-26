using System.Threading.Tasks;
using System.Windows.Input;

namespace InCarGUI
{
    public interface IAsyncCommand: ICommand
    {
        Task ExecuteAsync();
    }
}
