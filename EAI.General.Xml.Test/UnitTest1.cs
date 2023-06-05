namespace EAI.General.Xml.Test
{
    public class UnitTest1
    {
        
        [Fact]
        public void TestResources01()
        {
            var data = ResourceHelper.XmlChemicalTest01;

            Assert.NotNull(data);
        }
    }
}