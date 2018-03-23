using Kitware.VTK;
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
                try
                {
                    return (i, (T)Variant2Value(vtkAbstractArray.GetVariantValue(i)));
                }
                catch (System.Exception ex)
                {
                    try
                    {
                        var x = vtkAbstractArray.GetVariantValue(i);
                        return default;
                    }
                    catch (System.Exception ex2)
                    {
                        return default;
                    }
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
