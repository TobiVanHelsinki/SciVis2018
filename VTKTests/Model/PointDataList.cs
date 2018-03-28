using Kitware.VTK;
using System;
using System.Collections;
using System.Collections.Generic;
using static SciVis.Model.ModelHelper;

namespace SciVis.Model
{
    public class PointDataList<T> : IEnumerable<(long Index, T Value)>
    {
        /// <summary>
        /// raw vtk data, this object is using
        /// </summary>
        public vtkAbstractArray vtkAbstractArray;

        /// <summary>
        /// Name of the vtk-array
        /// </summary>
        public string Name { get => vtkAbstractArray.GetName(); set => vtkAbstractArray.SetName(value); }
        /// <summary>
        /// Number of point-values in this vtk array
        /// </summary>
        public long Count => vtkAbstractArray.GetNumberOfValues();

        /// <summary>
        /// param is mandatory, get the 
        /// </summary>
        /// <param name="vtkAbstractArray">get it from PointData</param>
        public PointDataList(vtkAbstractArray vtkAbstractArray)
        {
            this.vtkAbstractArray = vtkAbstractArray ?? throw new ArgumentNullException(nameof(vtkAbstractArray));
        }

        /// <summary>
        /// Iterate throug the raw point data
        /// </summary>
        /// <returns>the index at the raw data and the value</returns>
        public IEnumerator<(long Index, T Value)> GetEnumerator()
        {
            for (long i = 0; i < vtkAbstractArray.GetNumberOfValues(); i++)
            {
                yield return (i, (T)Variant2Value(vtkAbstractArray.GetVariantValue(i)));
            }
        }
        /// <summary>
        /// tested, converts and Index to x,y,z coordinates in an 300*300*300 Space
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static (long x, long y, long z) Index2Coords(long index)
        {
            long y = index / (300 * 300);
            long z = (index - (y * 300 * 300)) / 300;
            long x = index - (z * 300) - (y * 300 * 300);
            return (x, y, z);
        }
        /// <summary>
        /// tested, convert x,y,z coordinates in an 300*300*300 Space into a continuing index
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static long Coords2Index(long x, long y, long z)
        {
            return z * 300 + y * 300 * 300 + x;
        }
        /// <summary>
        /// see Coords2Index
        /// </summary>
        /// <param name="coords"></param>
        /// <returns></returns>
        public static long Coords2Index((long x, long y, long z) coords) => Coords2Index(coords.x, coords.y, coords.z);

        /// <summary>
        /// <returns>the index i at the raw data and the value at this point</returns>
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
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

        /// <summary>
        /// returns the specified point in 300*300*300-space and its index
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public (long Index, T Value) GetPoint(long x, long y, long z)
        {
            return this[Coords2Index(x, y, z)];
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            for (long i = 0; i < vtkAbstractArray.GetNumberOfValues(); i++)
            {
                yield return (i, (T)Variant2Value(vtkAbstractArray.GetVariantValue(i)));
            }
        }
    }
}
