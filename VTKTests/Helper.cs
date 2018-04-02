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
        /// <summary>
        /// redirection to Console.WriteLine
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void Display(string format, params object[] args)
        {
            Console.WriteLine(String.Format(format, args));
        }
        /// <summary>
        /// redirection to Console.WriteLine
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        public static void Display(string msg, Exception ex)
        {
            Console.WriteLine(msg + ex.Message + " " + ex.StackTrace);
        }

        /// <summary>
        /// overrides the last console line with v if z != 0
        /// </summary>
        /// <param name="v"></param>
        /// <param name="z"></param>
        public static void DisplayProgress(string v, int z)
        {
            if (z != 0)
            {
                Console.CursorTop--;
            }
            Display(v + z);
        }

        /// <summary>
        /// removes the given lines of console output
        /// </summary>
        /// <param name="Count"></param>
        public static void DisplayRemoveLines(int Count = 1)
        {
            for (int i = 0; i < Count; i++)
            {
                Console.CursorTop--;
                Console.WriteLine(String.Format("{0," + (Console.BufferWidth-1 )+ "}", ' '));
                Console.CursorTop--;
            }
        }

        /// <summary>
        /// break if attached
        /// </summary>
        public static void DebuggerBreak()
        {
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
        }
    }
}
