using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreTest
{
    /// <summary>
    /// Used to export the private data of MockGeoProvider
    /// </summary>
    interface IExportGeoInfoData
    {
        void SetResult(string result);

        string getResult();

        void SetReturnValue(bool returnVal);

        bool getReturnValue();
    }
}
