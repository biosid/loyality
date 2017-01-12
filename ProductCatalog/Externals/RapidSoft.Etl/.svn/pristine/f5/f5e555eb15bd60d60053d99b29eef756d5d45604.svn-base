using System;
using System.Collections.Generic;

namespace RapidSoft.Etl.Console
{
    public interface IEtlConsoleCommand
    {
        #region Properties

        string CommandName
        {
            get;
        }

        string ShortDescription
        {
            get;
        }

        string Description
        {
            get;
        }

        EtlConsoleCommandOptionInfo[] OptionsInfo
        {
            get;
        }

        #endregion

        #region Methods

        void ExecuteCommand(EtlConsoleArguments options);

        #endregion
    }
}
