using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SciVis.Model.ModelHelper;
using static SciVis.Helper;
namespace SciVis.Model
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
}
