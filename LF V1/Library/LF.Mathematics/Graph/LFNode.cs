/*──────────────────────────────────────────────────────────────
 * FileName     : LFNode
 * Created      : 2020-10-13 15:47:17
 * Author       : Xu Zhe
 * Description  : 
 * ──────────────────────────────────────────────────────────────*/

namespace LF.Mathematics
{
    /// <summary>
    /// 图
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class LFNode<T>
    {
        #region Fields
        private T _data;    // 节点数据域
        #endregion

        #region Properties
        /// <summary>
        /// 数据域属性
        /// </summary>
        public T Data { get => _data; set => _data = value; }
        #endregion

        #region Constructors
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="data"></param>
        public LFNode(T data)
        {
            _data = data;
        }
        #endregion

        #region Methods
        #endregion
    }
}
