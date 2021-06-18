# 蓝风系统库

## 一、基本信息

-   **库名称**：`LF`

-   **版　本**：V1.0
-   **时　间**：2021年6月10日

## 二、详细内容

### 1. 排序算法

#### 1.1 冒泡排序

##### 算法思想

冒泡排序要对一个列表多次重复遍历。它要比较相邻的两项，并且交换顺序排错的项。每对列表实行一次遍历，就有一个最大项排在了正确的位置。大体上讲，列表的每一个数据项都会在 其相应的位置 “冒泡”。如果列表有$n$项，第一次遍历就要比较$n-1$对数据。需要注意，一旦列表中最大（按照规定的原则定义大小）的数据是所比较的数据对中的一个，它就会沿着列表一直后移，直到这次遍历结束。

``` c#
public static void BubbleSort(int[] array)
{
    int tmp;
    bool flag = false;
    for (int i = array.Length - 1; i >= 0; i--)
    {
        for (int j = 0; j < i; j++)
        {
            if (array[j] > array[j + 1])
            {
                tmp = array[j];
                array[j] = array[j + 1];
                array[j + 1] = tmp;
                flag = true;
            }
        }
        if (!flag) break;
    }
}
```

