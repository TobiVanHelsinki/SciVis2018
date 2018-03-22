using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SciVis
{
    public static partial class Helper
    {
        public static IEnumerable<PropertyInfo> GetProperties(object obj, Type type)
        {
            return obj.GetType().GetProperties().Where(p => p.CustomAttributes.Any(c => c.AttributeType == type));
        }
    }
    public static partial class Helper
    {
        public static void Display(string format, params object[] args)
        {
            Console.WriteLine(String.Format(format, args));
        }
        public static void Display(string msg, Exception ex)
        {
            Console.WriteLine(msg + ex.Message + " " + ex.StackTrace);
        }
    }
}
