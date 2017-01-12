using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RapidSoft.Loaders.KLADR.Utils
{
    public interface ISqlUtils
    {
        void Execute(string sqlCommandText);

        object ExecuteScalar(string sqlCommandText);
    }
}
