using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WLT.BusinessLogic.BAL.DBSCAN
{
    public class DbscanPoint<T>
    {
        public bool IsVisited;

        public T ClusterPoint;

        public int ClusterId;

        public DbscanPoint(T x)
        {
            ClusterPoint = x;

            IsVisited = false;

            ClusterId = (int)ClusterIds.Unclassified;
        }

    }

}
