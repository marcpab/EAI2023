namespace EAI.General.Xml
{
    public static class Utilities
    {
        public static string GetNodeXPath(string name)
        {
            if (IsAttribute(name))
                return $"@*[local-name() = '{GetAttributeName(name)}']";
            else
                return $"*[local-name() = '{name}']";
        }

        public static bool IsAttribute(string name)
            => name?.StartsWith("@") ?? false;

        public static string GetAttributeName(string name)
            => name?.Remove(0, 1);

        public static string GetAsAttributeName(string name)
            => $"@{name}";
    }
}
