using System;

namespace EAI.General.SettingJson
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
    public class SingletonAttribute : Attribute
    {
    }
}