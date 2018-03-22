using Kitware.VTK;
using System;
using System.Collections;
using System.Collections.Generic;
using static SciVis.Model.ModelHelper;
using static SciVis.Helper;

namespace SciVis.Model
{
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

}
