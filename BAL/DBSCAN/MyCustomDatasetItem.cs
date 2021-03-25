using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WLT.BusinessLogic.BAL.DBSCAN
{
    public class DatasetItem : DatasetItemBase

    {

        public double X;

        public double Y;

        public DatasetItem(double x, double y)

        {

            X = x;

            Y = y;

        }

    }
}
