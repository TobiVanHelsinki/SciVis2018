using Kitware.VTK;
using System;
using System.Collections;
using System.Collections.Generic;
using static SciVis.Model.ModelHelper;

namespace SciVis.Model
{
    public class PointDataList<T> : IEnumerable<(int Index, T Value)>
    {
        private vtkAbstractArray vtkAbstractArray;

        public int Count => vtkAbstractArray.GetNumberOfValues();

        public PointDataList(vtkAbstractArray vtkAbstractArray)
        {
            this.vtkAbstractArray = vtkAbstractArray;
        }
        public IEnumerator<(int Index, T Value)> GetEnumerator()
        {
            for (int i = 0; i < vtkAbstractArray.GetNumberOfValues(); i++)
            {
                yield return (i, (T)Variant2Value(vtkAbstractArray.GetVariantValue(i)));
            }
        }

        public (int Index, T Value) this[int i]
        {
            get
            {
                //return default;
                try
                {
                    object retval;
                    using (var variant = vtkAbstractArray.GetVariantValue(i))
                    {
                        retval = Variant2Value(variant);
                        //variant.Dispose(); //Dennohch OutOfMemoryException
                    }
                    return (i, (T)retval);
                }
                catch (OutOfMemoryException ex)
                {
                    return default;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < vtkAbstractArray.GetNumberOfValues(); i++)
            {
                yield return (i,(T)Variant2Value(vtkAbstractArray.GetVariantValue(i)));
            }
        }
    }
}
