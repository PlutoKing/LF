# WPF控件样式库 

## 一、基本信息

-   **库名称**：`LF.Controls`
-   **版　本**：V1.0
-   **时　间**：2021年6月10日
-   **备　注**：WPF自定义空间库(.NET Framework)



## 二、样式内容

### 1. 样式规范

#### 关于尺寸

标准字体：12号字

控件高度：20

控件间隔：8

### 2. 颜色管理

- <font color='#007ACC'>**主题颜色**</font>`MainColor`

- <font color='#1D6BFF'>**突出颜色**</font> `NoteMainColor`

- <font color='#F0F0F0'>**背景颜色**</font> `BackColor`

- <font color='LightGray'>**线框颜色**</font> `LineColor`

### 3. 常用控件样式

-   `Button`
    -   扁平按钮 `FlatButton`
    -   简单按钮 `SingleButton`


## 三、调用方法

在需要调用该样式库的项目的`App.xaml`文件中添加：

``` xaml
<Application.Resources>
	<ResourceDictionary>
		<ResourceDictionary.MergedDictionaries>
			<ResourceDictionary Source="/LF.Controls;component/Styles/Color.xaml" />
			<ResourceDictionary Source="/LF.Controls;component/Styles/Button.xaml" />
			<ResourceDictionary Source="/LF.Controls;component/Styles/DataGrid.xaml" />
			<ResourceDictionary Source="/LF.Controls;component/Styles/GroupBox.xaml" />
			<ResourceDictionary Source="/LF.Controls;component/Styles/ListBox.xaml" />
			<ResourceDictionary Source="/LF.Controls;component/Styles/TabControl.xaml" />
			<ResourceDictionary Source="/LF.Controls;component/Styles/TreeView.xaml" />
		</ResourceDictionary.MergedDictionaries>
	</ResourceDictionary>
</Application.Resources>
```

