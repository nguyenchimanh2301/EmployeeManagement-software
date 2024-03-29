using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web.Core.Atribute
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NotEmpty:Attribute
    {

    }
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKey : Attribute
    {


    }
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyName : Attribute
    {
        public string Name = string.Empty; 
        public PropertyName(string name)
        {
            Name = name;
        }
    }
    [AttributeUsage(AttributeTargets.Property)]
    public class NotMap : Attribute
    {
        
    }
    [AttributeUsage(AttributeTargets.Property)]
    public class DateValue : Attribute
    {

    }
    [AttributeUsage(AttributeTargets.Property)]
    public class NotDupplicate : Attribute
    {

    }
}
