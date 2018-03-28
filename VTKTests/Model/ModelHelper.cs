using Kitware.VTK;
using System;

namespace SciVis.Model
{
    public static class ModelHelper
    {
        /// <summary>
        /// does stupid vtk work for you. you give him a variant and it returns you the value (unfortunaly as object. Don't like it? Well, life is hard.)
        /// </summary>
        /// <param name="var"></param>
        /// <returns></returns>
        public static object Variant2Value(vtkVariant var)
        {
            object ret = null;
            if (var.IsDouble())
            {
                ret = var.ToDouble();
            }
            else if (var.IsFloat())
            {
                ret = var.ToFloat();
            }
            else if (var.IsInt())
            {
                ret = var.ToInt();
            }
            else if (var.IsLong())
            {
                ret = var.ToLong();
            }
            else if (var.IsLongLong())
            {
                ret = var.ToLongLong();
            }
            else if (var.IsShort())
            {
                ret = var.ToShort();
            }
            else if (var.Is__Int64())
            {
                ret = var.ToTypeInt64();
            }

            else if (var.IsSignedChar())
            {
                ret = var.ToSignedChar();
            }
            else if (var.IsString())
            {
                ret = var.ToString();
            }

            else if (var.IsUnsignedInt())
            {
                ret = var.ToUnsignedInt();
            }
            else if (var.IsUnsignedLong())
            {
                ret = var.ToUnsignedLong();
            }
            else if (var.IsUnsignedLongLong())
            {
                ret = var.ToUnsignedLongLong();
            }
            else if (var.IsUnsignedShort())
            {
                ret = var.ToUnsignedShort();
            }
            else if (var.IsUnsigned__Int64())
            {
                ret = var.ToTypeUInt64();
            }
            else if (var.IsUnsignedChar())
            {
                ret = var.ToUnsignedChar();
            }
            else if (var.IsArray())
            {
                ret = var.ToArray();
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
            return ret;
        }

        /// <summary>
        /// does stupid vtk work for you. you give him a variant and it tries to figure out what type this is. works correct in nearly 60%.
        /// </summary>
        /// <param name="var"></param>
        /// <returns></returns>
        public static Type Variant2Type(vtkVariant var)
        {
            if (var.IsDouble())
            {
                return typeof(Double);
            }
            if (var.IsFloat())
            {
                return typeof(Single);
            }
            if (var.IsInt())
            {
                return typeof(Int32);
            }
            if (var.IsLong())
            {
                return typeof(Int32);
            }
            if (var.IsLongLong())
            {
                return typeof(Int64);
            }
            if (var.IsShort())
            {
                return typeof(Int16);
            }
            if (var.Is__Int64())
            {
                return typeof(Int64);
            }

            if (var.IsSignedChar())
            {
                return typeof(SByte);
            }
            if (var.IsString())
            {
                return typeof(String);
            }

            if (var.IsUnsignedInt())
            {
                return typeof(UInt32);
            }
            if (var.IsUnsignedLong())
            {
                return typeof(UInt32);
            }
            if (var.IsUnsignedLongLong())
            {
                return typeof(UInt64);
            }
            if (var.IsUnsignedShort())
            {
                return typeof(UInt16);
            }
            if (var.IsUnsigned__Int64())
            {
                return typeof(UInt64);
            }
            if (var.IsUnsignedChar())
            {
                return typeof(Byte);
            }
            if (var.IsArray())
            {
                return typeof(Array);
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }
    }
}
