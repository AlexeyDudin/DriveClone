using Domain;
using ISOWorker;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Commands
{
    public class HandleDrive : ICommand
    {
        private Func<object, bool> canExecute;
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            if (parameter is List<Object>)
            {
                var paramList = (List<Object>)parameter;
                if (paramList.Count > 2)
                {
                    if (paramList[0] is string && paramList[1] is string)
                    {
                        ISOHandler handler = new ISOHandler();
                        ProgressDomain progress = null;
                        if (paramList.Count > 3 && paramList[2] is ProgressDomain)
                            progress = (ProgressDomain)paramList[2];
                        handler.CreateIso((string)paramList[0], (string)paramList[1], progress);
                    }
                }
            }
        }
    }
}
