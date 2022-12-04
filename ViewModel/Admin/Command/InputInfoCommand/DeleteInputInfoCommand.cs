﻿using ConvenienceStore.Model.Admin;
using ConvenienceStore.ViewModel.Admin.AdminVM;
using ConvenienceStore.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.Admin.Command.InputInfoCommand
{
    public class DeleteInputInfoCommand : ICommand
    {
        InputInfoVM VM;
        public DeleteInputInfoCommand(InputInfoVM VM)
        {
            this.VM = VM;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var inputInfo = (InputInfo)parameter;
            VM.ObservableInputInfos.Remove(inputInfo);
            VM.inputInfos.Remove(inputInfo);
            DatabaseHelper.DeleteInputInfo(inputInfo.Id);
        }
    }
}
