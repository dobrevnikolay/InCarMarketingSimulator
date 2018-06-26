using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InCarGUI
{
    public class AsyncRelayCommand : IAsyncCommand
    {
        #region Private Members
        /// <summary>
        /// The Action to run
        /// </summary>
        private Func<Task> _Action;
        #endregion

        #region Public Events
        /// <summary>
        /// The event thats fired when the <see cref="CanExecute(object)"/> value has changed
        /// </summary>
        public event EventHandler CanExecuteChanged = (sender, e) => { };

        public Task ExecuteAsync()
        {
            return _Action();
        }

        #endregion

        #region Constructor

        public AsyncRelayCommand(Func<Task> action)
        {
            _Action = action;
        }

        #endregion

        #region Command Methods
        /// <summary>
        /// A reley command can always execute
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        ///  Executes the command Action
        /// </summary>
        /// <param name="parameter"></param>
        public async void Execute(object parameter)
        {
            await ExecuteAsync();
        }

        #endregion
    }
}
