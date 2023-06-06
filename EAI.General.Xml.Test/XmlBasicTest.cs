using System.Xml.Linq;
using EAI.General.Xml.Extensions;

namespace EAI.General.Xml.Test
{
    public class XmlBasicTest
    {
        [Fact]
        public void TestResources01()
        {
            var data = ResourceHelper.ZDebmas07_SampleChemicalIndustry01;

            Assert.NotNull(data);
        }

        [Fact]
        public void TestToDynamicXml01()
        {
            var data = ResourceHelper.ZDebmas07_SampleChemicalIndustry01;

            var idocETN = data
                .ToXmlDocument()
                .ToDynamic(NodeDefaultBehavior.EmptyToNull)
                .Receive
                .idocData;

            var expectedSystemPartnerId = "MAN100_01";
            var systemPartnerId = (string)idocETN.EDI_DC40.SNDPRN ?? string.Empty;
            Assert.Equal(expectedSystemPartnerId, systemPartnerId);

            var standardVersion = (string)idocETN.EDI_DC40.STDVRS;
            Assert.Null(standardVersion);
        }

        [Fact]
        public void TestToDynamicXml02()
        {
            var data = ResourceHelper.ZDebmas07_SampleChemicalIndustry01;

            var idocN = data
                .ToXmlDocument()
                .ToDynamic(NodeDefaultBehavior.Default)
                .Receive
                .idocData;

            var expectedSystemPartnerId = "MAN100_01";
            var systemPartnerId = (string)idocN.EDI_DC40.SNDPRN ?? string.Empty;
            Assert.Equal(expectedSystemPartnerId, systemPartnerId);

            var standardVersion = (string)idocN.EDI_DC40.STDVRS;
            Assert.Equal(string.Empty, standardVersion);

            var notThere = (string)idocN.DUMMY01;
            Assert.Null(notThere);
        }

        [Fact]
        public void TestToDynamicXml03()
        {
            var data = ResourceHelper.ZDebmas07_SampleChemicalIndustry01;

            var idocC = data
                .ToXmlDocument()
                .ToDynamic(NodeDefaultBehavior.Create)
                .Receive
                .idocData;

            var expectedSystemPartnerId = "MAN100_01";
            var systemPartnerId = (string)idocC.EDI_DC40.SNDPRN ?? string.Empty;
            Assert.Equal(expectedSystemPartnerId, systemPartnerId);

            var standardVersion = (string)idocC.EDI_DC40.STDVRS;
            Assert.Equal(string.Empty, standardVersion);

            // will be created...
            var notThere = (string)idocC.DUMMY01;
            Assert.Equal(string.Empty, notThere);
        }

        [Fact]
        public void TestToDynamicXml04()
        {
            var data = ResourceHelper.ZDebmas07_SampleChemicalIndustry01;

            var idocE = data
                .ToXmlDocument()
                .ToDynamic(NodeDefaultBehavior.Exception)
                .Receive
                .idocData;

            var expectedSystemPartnerId = "MAN100_01";
            var systemPartnerId = (string)idocE.EDI_DC40.SNDPRN ?? string.Empty;
            Assert.Equal(expectedSystemPartnerId, systemPartnerId);

            var standardVersion = (string)idocE.EDI_DC40.STDVRS;
            Assert.Equal(string.Empty, standardVersion);

            // ex
            Assert.Throws<InvalidOperationException>(() => (string)idocE.DUMMY01);
        }
    }
}