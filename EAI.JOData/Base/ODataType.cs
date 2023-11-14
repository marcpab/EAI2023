namespace EAI.JOData.Base
{
    public class ODataType
    {
        public string Name { get; }
        public ODataType(string name) => (Name) = (name);

        public override string ToString()
        {
            return $"@odata.type:{Name}";
        }
    }
}
