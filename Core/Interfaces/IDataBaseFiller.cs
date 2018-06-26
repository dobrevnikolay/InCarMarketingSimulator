using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{

    /// <summary>
    /// Defines behaviour, which has to be external, but due to the reason that is simulator 
    /// it is implemented here
    /// </summary>
    public interface IDataBaseFiller
    {
        /// <summary>
        /// Adds patterns in the database
        /// </summary>
        /// <param name="patterns"></param>
        void AddPatterns(List<Pattern> patterns);
    }
}
