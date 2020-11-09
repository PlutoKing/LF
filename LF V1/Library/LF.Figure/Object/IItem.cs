/*──────────────────────────────────────────────────────────────
 * FileName     : IItem
 * Created      : 2020-10-16 14:59:36
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

using System;
using System.Drawing;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LF.Figure
{
    /// <summary>
    /// 接口
    /// </summary>
    public interface IItem
    {
        Overlay Overlay { get; set; }
        DataList Datas { get; set; }
        bool IsVisible { get; set; }
        List<PointF> LocalPoints { get; set; }

        void Draw(Graphics g);
        void UpdateGraphicsPath();
    }
}
