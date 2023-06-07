using System.Diagnostics;
using System.Xml;
using System.Xml.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using EAI.General.Xml.Extensions;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Xunit.Abstractions;

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

        [Fact]
        public void TestToDynamicXml05a()
        {
            var data = ResourceHelper.ZDebmas07_SampleChemicalIndustry01;

            var idoc = data
                .ToXmlDocument()
                .ToDynamic(NodeDefaultBehavior.Default)
                .Receive
                .idocData;

            var xKNA1 = ((XmlNode)idoc.E2KNA1M005GRP).ToXElement();
            var xKnvvm = xKNA1
                .Descendants()
                .Where(x => x.Name.LocalName == "E2KNVVM007GRP");
            var sd = new Dictionary<int, string?>(xKnvvm.Count());

            var customer = 0;
            foreach (var e in xKnvvm)
            {
                var vkorg = e.Descendants().Where(x => x.Name.LocalName == "VKORG" && x.Parent?.Name.LocalName == "E2KNVVM007")?.Select(x => x.Value).SingleOrDefault();
                sd.Add(++customer, vkorg);
            }

            Assert.True(sd.Count == 1);
            Assert.True(sd.ContainsKey(1));
            Assert.Equal("0001", sd[1]);
        }

        [Fact]
        public void TestToDynamicXml05b()
        {
            var data = ResourceHelper.ZDebmas07_SampleChemicalIndustry01;

            var idoc = data
                .ToXmlDocument()
                .ToDynamic(NodeDefaultBehavior.Default)
                .Receive
                .idocData;

            
            var sd = new Dictionary<int, string?>(1);

            var customer = 0;
            foreach (var e in idoc.E2KNA1M005GRP.E2KNVVM007GRP.E2KNVVM007)
            {
                sd.Add(++customer, (string)e.VKORG);
            }

            Assert.True(sd.Count == 1);
            Assert.True(sd.ContainsKey(1));
            Assert.Equal("0001", sd[1]);
        }

        [Fact]
        public void TestToDynamicXml05c()
        {
            var xIdoc = ResourceHelper.ZDebmas07_SampleChemicalIndustry01;

            Assert.NotNull(xIdoc);

            var xKnvvm = xIdoc
                .Descendants()
                .Where(x => x.Name.LocalName == "E2KNVVM007");

            var sd = new Dictionary<int, string?>(1);

            var customer = 0;
            foreach (var e in xKnvvm)
            {
                var vkorg = e.Descendants().Where(x => x.Name.LocalName == "VKORG")?.Select(x => x.Value).SingleOrDefault();

                sd.Add(++customer, vkorg);
            }

            Assert.True(sd.Count == 1);
            Assert.True(sd.ContainsKey(1));
            Assert.Equal("0001", sd[1]);
        }
    }

    public class XmlMemoryTest
    {
        // xunit output (Detail summary window)
        private readonly ITestOutputHelper output;
        
        public XmlMemoryTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Benchmark()
        {
            var logger = new AccumulationLogger();

            var config = ManualConfig.Create(DefaultConfig.Instance)
                .AddLogger(logger)
                .WithOptions(ConfigOptions.DisableOptimizationsValidator);

            BenchmarkRunner.Run<XmlBenchmarks>(config);

            var logs = logger.GetLog();
            output.WriteLine(logs);
        }
    }

    [MemoryDiagnoser]
    public class XmlBenchmarks
    {
        [Benchmark]
        public static void TestToDynamicXml05_DynamicNodeToXElement()
        {
            var data = ResourceHelper.Xml_ZDebmas07_SampleChemicalIndustry01;

            var idoc = data
                .ToDynamic(NodeDefaultBehavior.Default)
                .Receive
                .idocData;

            var xKNA1 = ((XmlNode)idoc.E2KNA1M005GRP).ToXElement();
            var xKnvvm = xKNA1
                .Descendants()
                .Where(x => x.Name.LocalName == "E2KNVVM007GRP");
            var sd = new Dictionary<int, string?>(xKnvvm.Count());

            var customer = 0;
            foreach (var e in xKnvvm)
            {
                var vkorg = e.Descendants().Where(x => x.Name.LocalName == "VKORG" && x.Parent?.Name.LocalName == "E2KNVVM007")?.Select(x => x.Value).SingleOrDefault();
                sd.Add(++customer, vkorg);
            }

            Assert.True(sd.Count == 1);
            Assert.True(sd.ContainsKey(1));
            Assert.Equal("0001", sd[1]);
        }

        [Benchmark]
        public static void TestToDynamicXml05_DynamicNode()
        {
            var data = ResourceHelper.Xml_ZDebmas07_SampleChemicalIndustry01;

            var idoc = data
                .ToDynamic(NodeDefaultBehavior.Default)
                .Receive
                .idocData;


            var sd = new Dictionary<int, string?>(1);

            var customer = 0;
            foreach (var e in idoc.E2KNA1M005GRP.E2KNVVM007GRP.E2KNVVM007)
            {
                sd.Add(++customer, (string)e.VKORG);
            }

            Assert.True(sd.Count == 1);
            Assert.True(sd.ContainsKey(1));
            Assert.Equal("0001", sd[1]);
        }

        [Benchmark]
        public static void TestToDynamicXml05_XElementLinq()
        {
            var xIdoc = ResourceHelper.ZDebmas07_SampleChemicalIndustry01;

            Assert.NotNull(xIdoc);

            var xKnvvm = xIdoc
                .Descendants()
                .Where(x => x.Name.LocalName == "E2KNVVM007");

            var sd = new Dictionary<int, string?>(1);

            var customer = 0;
            foreach (var e in xKnvvm)
            {
                var vkorg = e.Descendants().Where(x => x.Name.LocalName == "VKORG")?.Select(x => x.Value).SingleOrDefault();

                sd.Add(++customer, vkorg);
            }

            Assert.True(sd.Count == 1);
            Assert.True(sd.ContainsKey(1));
            Assert.Equal("0001", sd[1]);
        }

        [Benchmark]
        public static void TestToDynamicXml05_XElementLoad()
        {
            var xIdoc = ResourceHelper.ZDebmas07_SampleChemicalIndustry01;

            Assert.NotNull(xIdoc);
        }

        [Benchmark]
        public static void TestToDynamicXml05_XmlLoad()
        {
            var xIdoc = ResourceHelper.Xml_ZDebmas07_SampleChemicalIndustry01;

            Assert.NotNull(xIdoc);
        }

        [Benchmark]
        public static void TestToDynamicXml05_DynamicNodeLoad()
        {
            var data = ResourceHelper.Xml_ZDebmas07_SampleChemicalIndustry01;

            Assert.NotNull(data);

            var dynNode = data
                .ToDynamic(NodeDefaultBehavior.Default);

            Assert.NotNull(dynNode);
        }
    }
}