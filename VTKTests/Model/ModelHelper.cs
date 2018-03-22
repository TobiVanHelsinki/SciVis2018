using Kitware.VTK;
using System;

namespace SciVis.Model
{
    public static class ModelHelper
    {
        public static object Variant2Value(vtkVariant var)
        {
            if (var.IsDouble())
            {
                return var.ToDouble();
            }
            if (var.IsFloat())
            {
                return var.ToFloat();
            }
            if (var.IsInt())
            {
                return var.ToInt();
            }
            if (var.IsLong())
            {
                return var.ToLong();
            }
            if (var.IsLongLong())
            {
                return var.ToLongLong();
            }
            if (var.IsShort())
            {
                return var.ToShort();
            }
            if (var.Is__Int64())
            {
                return var.ToTypeInt64();
            }

            if (var.IsSignedChar())
            {
                return var.ToSignedChar();
            }
            if (var.IsString())
            {
                return var.ToString();
            }

            if (var.IsUnsignedInt())
            {
                return var.ToUnsignedInt();
            }
            if (var.IsUnsignedLong())
            {
                return var.ToUnsignedLong();
            }
            if (var.IsUnsignedLongLong())
            {
                return var.ToUnsignedLongLong();
            }
            if (var.IsUnsignedShort())
            {
                return var.ToUnsignedShort();
            }
            if (var.IsUnsigned__Int64())
            {
                return var.ToTypeUInt64();
            }
            if (var.IsUnsignedChar())
            {
                return var.ToUnsignedChar();
            }
            if (var.IsArray())
            {
                return var.ToArray();
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

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
