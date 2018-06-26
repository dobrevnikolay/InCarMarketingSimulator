using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

namespace CoreTest
{
    public class MockGeoProvider :IGeoInfoProvider, IExportGeoInfoData
    {
        private bool _returnVal;
        private string _result;

        public MockGeoProvider(bool returnVal)
        {
            _returnVal = returnVal;
            _result = string.Empty;
        }

        public bool IsNearby(string patternName, string GPS, out string Result)
        {
            Result = _result;
            return _returnVal;
        }

        public void SetResult(string result)
        {
            _result = result;
        }

        public string getResult()
        {
            return _result;
        }

        public void SetReturnValue(bool returnVal)
        {
            _returnVal = returnVal;
        }

        public bool getReturnValue()
        {
            return _returnVal;
        }
    }
}
