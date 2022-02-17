using System;

namespace RulesMadeEasy.Extensions
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple =false)]
    public class ActionAttribute : Attribute
    {
        public Guid ActionId { get; }
    }

    
}