/*──────────────────────────────────────────────────────────────
 * FileName     : LF
 * Created      : 2020-10-20 20:11:36
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


namespace LF.Mathematics
{
    public class LF
    {
        #region 数组
        /// <summary>
        /// 求最小值
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        /// <param name="min"></param>
        public static void GetMin(double[] array, out int index, out double min)
        {
            index = 0;
            min = array[0];

            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] < min)
                {
                    min = array[i];
                    index = i;
                }
            }
        }

        /// <summary>
        /// 求最大
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        /// <param name="min"></param>
        public static void GetMax(double[] array, out int index, out double max)
        {
            index = 0;
            max = array[0];

            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] > max)
                {
                    max = array[i];
                    index = i;
                }
            }
        }

        public static double[] ArraySwap(double[] array, int index1, int index2)
        {
            double tmp = array[index1];
            array[index1] = array[index2];
            array[index2] = tmp;
            return array;
        }

        /// <summary>
        /// 素组交换
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        /// <returns></returns>
        public static int[] ArraySwap(int[] array, int index1, int index2)
        {
            int[] r = (int[])array.Clone();
            r[index1] = array[index2];
            r[index2] = array[index1];
            return r;
        }

        /// <summary>
        /// 数组翻转
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        /// <returns></returns>
        public static int[] ArrayFlip(int[] array, int index1, int index2)
        {
            int[] r = (int[])array.Clone();

            int tmp = 0;
            if (index1 > index2)
            {
                tmp = index1;
                index1 = index2;
                index2 = tmp;
            }
            else if (index1 == index2)
            {
                return r;
            }

            for (int i = index1; i <= index2; i++)
            {
                r[i] = array[index2 + index1 - i];
            }

            return r;
        }

        /// <summary>
        /// 数组滑移
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        /// <returns></returns>
        public static int[] ArraySlide(int[] array, int index1, int index2)
        {
            int[] r = (int[])array.Clone();
            int tmp = 0;
            if (index1 > index2)
            {
                tmp = index1;
                index1 = index2;
                index2 = tmp;
            }
            else if (index1 == index2)
            {
                return r;
            }

            for (int i = index1; i < index2; i++)
            {
                r[i] = array[i + 1];
            }
            r[index2] = array[index1];

            return r;
        }

        /// <summary>
        /// 随机数组
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int[] RandArray(int n)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < n; i++)
            {
                list.Add(i);
            }

            int[] arr = new int[n];

            for (int i = 0; i < n; i++)
            {
                int temp = new Random().Next(0, list.Count - 1);
                arr[i] = list[temp];
                list.RemoveAt(temp);
            }

            return arr;
        }

        public static int[] SubArray(int[] array, int start, int end)
        {
            int tmp = 0;
            if (start > end)
            {
                tmp = start;
                start = end;
                end = tmp;
            }
            else if (start == end)
            {
                return new int[] { array[start] };
            }
            int[] subArr = new int[end - start + 1];
            for (int i = 0; i <= end - start; i++)
            {
                subArr[i] = array[start + i];
            }
            return subArr;
        }


        #endregion

    }
}
