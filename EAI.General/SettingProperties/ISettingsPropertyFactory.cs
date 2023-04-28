using System.Threading.Tasks;

namespace EAI.General.SettingProperties
{
    /// <summary>
    /// Factory to handle different type of Properties from settings file
    /// </summary>
    public interface ISettingsPropertyFactory
    {

        /// <summary>
        /// Execute the handling of properties
        /// </summary>
        /// <returns></returns>
        Task ExecuteAsync();
    }
}