/*──────────────────────────────────────────────────────────────
 * FileName     : IPointList
 * Created      : 2020-01-09 11:08:03
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
    public interface IPointList : ICloneable
    {
        PointPair this[int index] { get; }
        int Count { get; }
    }
}
