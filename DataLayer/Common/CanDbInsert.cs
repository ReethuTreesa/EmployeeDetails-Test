using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Common
{
    /// <summary>
    /// Allows you to mark a property as insertable (ostensibly for building a bulk insert 
    /// datatable, but can be used elsewhere (in overrides to the DBObject int ExecXXXX methods).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class CanDbInsertAttribute : Attribute
    {
        public string Name { get; set; }
        public string Argument { get; set; }
    }
}
