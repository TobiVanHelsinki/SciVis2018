using Kitware.VTK;
using System;
using System.Collections;
using System.Collections.Generic;
using static SciVis.ModelHelper;
using static SciVis.Helper;

namespace SciVis
{
    public class PointDataList<T> : IEnumerable<T>
    {
        private vtkAbstractArray vtkAbstractArray;

        public PointDataList(vtkAbstractArray vtkAbstractArray)
        {
            this.vtkAbstractArray = vtkAbstractArray;
        }
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < vtkAbstractArray.GetNumberOfValues(); i++)
            {
                yield return (T)Variant2Value(vtkAbstractArray.GetVariantValue(i));
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < vtkAbstractArray.GetNumberOfValues(); i++)
            {
                yield return (T)Variant2Value(vtkAbstractArray.GetVariantValue(i));
            }
        }
    }
    public class MeteorData
    {
        public PointDataList<int> vtkGhostType;
        public PointDataList<int> vtkValidPointMask;

        public PointDataList<Single> rho;
        public PointDataList<Single> prs;
        public PointDataList<Single> tev;
        public PointDataList<Single> xdt;
        public PointDataList<Single> ydt;
        public PointDataList<Single> zdt;
        public PointDataList<Single> snd;
        public PointDataList<Single> grd;
        public PointDataList<Single> mat;
        public PointDataList<Single> v02;
        public PointDataList<Single> v03;

        vtkPointData PointData;
        public MeteorData(vtkPointData iPointData)
        {
            PointData = iPointData;
            vtkGhostType = new PointDataList<int>(PointData.GetAbstractArray(0));
            grd = new PointDataList<Single>(PointData.GetAbstractArray(1));
            mat = new PointDataList<Single>(PointData.GetAbstractArray(2));
            prs = new PointDataList<Single>(PointData.GetAbstractArray(3));
            rho = new PointDataList<Single>(PointData.GetAbstractArray(4));
            snd = new PointDataList<Single>(PointData.GetAbstractArray(5));
            tev = new PointDataList<Single>(PointData.GetAbstractArray(6));
            v02 = new PointDataList<Single>(PointData.GetAbstractArray(7));
            v03 = new PointDataList<Single>(PointData.GetAbstractArray(8));
            xdt = new PointDataList<Single>(PointData.GetAbstractArray(9));
            ydt = new PointDataList<Single>(PointData.GetAbstractArray(10));
            zdt = new PointDataList<Single>(PointData.GetAbstractArray(11));
            vtkValidPointMask = new PointDataList<int>(PointData.GetAbstractArray(12));
        }
    }
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
