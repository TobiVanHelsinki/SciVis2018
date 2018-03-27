using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public static int MinOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
        {
            try
            {
                return source.Min(selector);
            }
            catch (Exception)
            {
                return default;
            }
        }
        public static T MaxOrDefault<TSource, T>(this IEnumerable<TSource> source, Func<TSource, T> selector)
        {
            try
            {
                return source.Max(selector);
            }
            catch (Exception)
            {
                return default;
            }
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

        public static void DisplayProgress(string v, int z)
        {
            if (z != 0)
            {
                Console.CursorTop--;
            }
            Display(v + z);
        }
        public static void DisplayRemoveLines(int Count = 1)
        {
            for (int i = 0; i < Count; i++)
            {
                Console.CursorTop--;
                Console.WriteLine();
            }
        }
        public static void DebuggerBreak()
        {
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
        }
    }
}
