   M S B A M L   `   `   `  ����O  KLF.SimSchool.Project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:LF.SimSchool.Project.ViewLF.SimSchool.Project.View  5      -    'LF.SimSchool.Project.View.SettingDialog   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      9local'clr-namespace:LF.SimSchool.Project.View     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      -   5        $��window��5        � Title$  SettingDialog��5	      $	��600��5
      $	��400��5       � AllowsTransparency . 
������  � 
ResizeMode$ NoResize=�5       � UseLayoutRounding .  � WindowStartupLocation$ CenterScreen=�5       � WindowState$ Normal=�5       � WindowStyle$
 None=�5      .��5      �� -   5        $��5q�5   	      	BackColor#��[  5   	   
�������$��1q�5   	    J� Effect 5   
   3  -System.Windows.Media.Effects.DropShadowEffect  5         
BlurRadius$ 6Q�5      	   	Direction$	 0Q�5      
   Opacity$	
 0.3Q�5         ShadowDepth$ 0Q�5         Color$
 Gray��5       ._�5"   
   �  � RowDefinitions 5#      �� 5$      $
��Auto �6    �� 5%      $��1* �6    �� 5&      $
��Auto �6    .+�5(      ��   
StripColor#��[ 5)        	LineColor#��[ 5*      $��0,0,0,1q�5+      ._�5,      �� $��6q�5-      $��Center=�5.        	MainColor#��[ 5/      $��游戏设置��50      51      �� 52      $��1��53      $��6q�54        LeftTabItem#��[ 55        LeftTabControl#��[ 56      .@�57      �� $��界面设置��6   58      �� 59      $��2��5:      #��[ 5;      #��[ 5<      $��0,1,0,0q�5=      ._�5>      �� $��3q�5?      $��Right=�5@       �� Orientation$ 
Horizontal=�5A      .+�5B      �� $��60��5C      $��3q�5D      $��退出游戏��5G      ��5E      �� *CloseMainWindowCmd+��5F      ��  �� ElementName$ window��5G      �� 5H      $��60��5I      $��3q�5J      $��关闭设置��5M        SingleButton#��[ 5N      ��5K      �� *CloseWindowCmd+��5L      �� $ window��5N      5O      5P      5Q      5R      5S      