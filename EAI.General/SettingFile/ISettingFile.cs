using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EAI.General.SettingFile
{
    public interface ISettingFile
    {
        bool Changed { get; }
        Task ExecuteAsync();
    }
}
