using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App2
{
        public interface ISqliteDbPath
        {
            Task<string> GetDatabasePath();
        }
}
