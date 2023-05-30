using Xunit;
using EAI.General.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EAI.General.Settings.Tests
{
    public class SettingsHandlerTests
    {
        [Fact()]
        public async Task GetConfigurationRootTest()
        {
            var configuration = SettingsHandler.GetConfigurationRoot();

            var setting = configuration["TestSetting"];

            Assert.Equal("TestValue", setting);

            var section = configuration.GetSection("Values");

            var sectionSetting = section["SectionSetting"];

            Assert.Equal("SectionValue", sectionSetting);
        }
    }
}