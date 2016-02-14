using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSST.Agile.FileStorage.Interfaces
{
    public interface IFileStorage
    {
        Task<string> AddFile(int projectId, int taskId, string fileName, MemoryStream ms);
    }
}
