   M S B A M L   `   `   `  ����
  MyToDo&     MyToDo.Views.Dialogs.AddToDoView   5      W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      _ [MaterialDesignThemes.Wpf, Version=4.9.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962a:md1http://materialdesigninxaml.net/winfx/xaml/themes  5      $	��450��5      $	��280��5	      .��5      �   � RowDefinitions  5   
   �� 5      $
��auto �6   �� 5      �� 5      $
��auto �6   .+�5   
   �� $��10q�5      $��Center=�5      $��Center=�5      $��20(�5      $
��Bold"�5      $��添加待办��5      ]� 5   
   $��1��6   $��False��6!   .+�5      �� $��20,10q�5      $	��Top=�5       �� Orientation$ 
Horizontal=�5      .+�5      �� $��Center=�6   $��	状态：��67   �� 5      $
��10,0q�5        �� 
IsReadOnly 5!      �� *
IsReadOnly+ �� SelectedIndex 5"      �� *Model.Status+.@�5#      �� .��6#   待办6'   �� 5$      .��6#   	已完成6(   5%      5&      �� 5'      $
��20,0q�5(      )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ 请输入待办概要��5)      $	��Top=�5*      ��5+      �� *Model.Title+�� 5,      $	��100��5-      $��20,10q�5.      $ 请输入待办内容��5/       �� AcceptsReturn$
 True��50      $	��Top=�51       �� TextWrapping$
 Wrap=�53      ��52      �� *Model.Content+53      54      �� 56   
   $��2��57      $��10q�58      $��Right=�59      $ 
Horizontal=�5:      .+�5;      �� $��0,0,20,0q�5<      $��取消��5>          MaterialDesignOutlinedButton#��[  5?      ��5=      �� *CancelCommand+5?      �� 5@      $��确定��65   ��6   �� *SaveCommand+65   5A      5B      5C      