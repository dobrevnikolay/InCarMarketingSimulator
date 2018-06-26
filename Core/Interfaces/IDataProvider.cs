using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public interface IDataProvider
    {
        /// <summary>
        /// Gets all patterns from the database
        /// </summary>
        /// <returns>:List of Pattern</returns>
        Task<List<Pattern>> GetPatternsAsync();

        /// <summary>
        /// Gets all patterns from the database
        /// </summary>
        /// <returns>:List of Pattern</returns>
        List<Pattern> GetPatterns();

        /// <summary>
        /// Updates UserActivity collection, which is responsible for patterns
        /// recongnised by the system from the user screen
        /// </summary>
        /// <param name="userActivity"> Pattern name- occurance mapping</param>
        void UpdateUserActivity(Dictionary<string, bool> userActivity);

        /// <summary>
        /// Returns User Activity
        /// </summary>
        /// <returns>Dictionary, Pattern name- occurance mapping</returns>
        Dictionary<string, bool> GetUserActivity();

        /// <summary>
        /// Uploads unrecognised screen for further analisys
        /// </summary>
        /// <param name="Image"></param>
        void UploadScreen(byte[] Image);
    }
}
