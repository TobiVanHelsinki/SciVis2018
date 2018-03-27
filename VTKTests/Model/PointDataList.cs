using Kitware.VTK;
using System;
using System.Collections;
using System.Collections.Generic;
using static SciVis.Model.ModelHelper;

namespace SciVis.Model
{
    public class PointDataList<T> : IEnumerable<(long Index, T Value)>
    {
        public vtkAbstractArray vtkAbstractArray;
        public string Name { get => vtkAbstractArray.GetName(); set => vtkAbstractArray.SetName(value); }
        public long Count => vtkAbstractArray.GetNumberOfValues();

        public PointDataList(vtkAbstractArray vtkAbstractArray)
        {
            this.vtkAbstractArray = vtkAbstractArray ?? throw new ArgumentNullException(nameof(vtkAbstractArray));
        }
        public IEnumerator<(long Index, T Value)> GetEnumerator()
        {
            for (long i = 0; i < vtkAbstractArray.GetNumberOfValues(); i++)
            {
                yield return (i, (T)Variant2Value(vtkAbstractArray.GetVariantValue(i)));
            }
        }
        /// <summary>
        /// tested
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static (long x, long y, long z) Index2Coords(long index)
        {
            long y = index / (300 * 300);
            long z = (index - (y * 300 * 300)) / 300;
            long x = index - (z * 300) - (y * 300 * 300);
            return (x,y,z);
        }
        /// <summary>
        /// tested
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static long Coords2Index(long x, long y, long z)
        {
            return z * 300 + y * 300 * 300 + x;
        }
        public static long Coords2Index((long x, long y, long z) coords) => Coords2Index(coords.x, coords.y, coords.z);

        public (long Index, T Value) this[long i]
        {
            get
            {
                object retval;
                var variant = vtkAbstractArray.GetVariantValue(i);
                retval = Variant2Value(variant);
                variant.Dispose(); //Dennohch OutOfMemoryException
                variant = null;
                return (i, (T)retval);
            }
        }
        public (long Index, T Value) GetPoint(long x, long y, long z)
        {
            return this[Coords2Index(x,y,z)];
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            for (long i = 0; i < vtkAbstractArray.GetNumberOfValues(); i++)
            {
                yield return (i,(T)Variant2Value(vtkAbstractArray.GetVariantValue(i)));
            }
        }
    }
}
