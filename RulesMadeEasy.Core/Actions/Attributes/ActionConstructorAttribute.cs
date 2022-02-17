using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesMadeEasy.Core
{
    [AttributeUsage(AttributeTargets.Constructor)]
    public class ActionConstructorAttribute : Attribute
    {
        /// <summary>
        /// The priority of the decorated constructor. The lower the priority, the higher it is. 
        /// </summary>
        public uint Priority { get; }

        public ActionConstructorAttribute(uint priority = uint.MaxValue)
        {
            Priority = priority;
        }
    }
}
