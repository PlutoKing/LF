/*──────────────────────────────────────────────────────────────
 * FileName     : IPointListEdit
 * Created      : 2020-01-09 11:11:17
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LF.Figure
{
    public interface IPointListEdit : IPointList
    {
        new PointPair this[int index] { get; set; }
        void Add(PointPair point);
        void Add(double x, double y);
        void RemoveAt(int index);
        void Clear();
    }
}
