   M S B A M L   `   `   `  ����O  KLF.SimSchool.Project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:LF.SimSchool.ProjectLF.SimSchool.Project  5      %    LF.SimSchool.Project.MainWindow   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      4local"clr-namespace:LF.SimSchool.Project     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      -   5        $��window��5        � Title$  
MainWindow��5	      $	��800��5
      $	��450��5         	BackColor#��[  5       � 
ResizeMode$ NoResize=�5       � UseLayoutRounding .  � WindowStartupLocation$ CenterScreen=�5       � WindowState$ 	Maximized=�5       � WindowStyle$
 None=�5      .��5      �  � RowDefinitions 5   
   �� 5      $��60 �6   �� 5      $��1* �6   �� 5      $��30 �6   .+�5   
   �� $��0��5        
StripColor#��[ 5        	LineColor#��[ 5      $��0,0,0,1q�5      ._�5      �  5       �� 5!      $��1* �6$   �� 5"      $��1* �6$   .+�5$      �  � ColumnDefinitions 5%      �� 5&      $
��Auto �6+   �� 5'      $��1* �6+   �� 5(      $
��Auto �6+   .+�5*      �� $��	10,0,10,0q�6    $��模拟校园��63   �� 5+      $��2��5,      $��	10,0,10,0q�5-      $��Right=�5.       �� Orientation$ 
Horizontal=�5/      .+�50      �� $��60��51      $��设置��54        
FlatButton#��[ 55      ��52      �� *
SettingCmd+��53      �� 	 �� ElementName$	 window��55      56      57      � 58      $��1��6    59      �� 5:      $
��Auto �6+   �� 5;      $��1* �6+   .+�5=      �� $��10,0,0,0q�6!   $ 
Horizontal=�63   .+�5>      	� $��60��5?      $��
CampusPage��5A      $��校  园��5B      
 d� 	IsChecked$

 TrueY�5C        MenuRadioButton#��[ 5D      ��5@      �� *NavigationCmd+5D      	� 5E      $��60��5F      $��	StaffPage��5H      $��人  员��5I      #��[ 5J      ��5G      �� *NavigationCmd+5J      	� 5K      $��60��5L      $��	ClassPage��5N      $��班  级��5O      #��[ 5P      ��5M      �� *NavigationCmd+5P      	� 5Q      $��60��5R      $��
CoursePage��5T      $��课  程��5U      #��[ 5V      ��5S      �� *NavigationCmd+5V      5W      5Y      5Z      5[      �� 5\   
   $��1��6   ��6&   �� *MainContent+5]      5^      