   M S B A M L   `   `   `  ����A  =MyToDo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:MyToDo.ExtensionsMyToDo.Extensions  5      clr-namespace:MyToDoMyToDo  5      5clr-namespace:MyToDo.ViewModelsMyToDo.ViewModels  5          MyToDo.view.MainView   5      W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /extclr-namespace:MyToDo.Extensions     5      _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5       localclr-namespace:MyToDo  5      _ [MaterialDesignThemes.Wpf, Version=4.9.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5	      Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006  5
      [ WPrism.DryIoc.Wpf, Version=8.1.97.5141, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59T	 PPrism.Wpf, Version=8.1.97.5141, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59&prismhttp://prismlibrary.com/  	 5      *vmclr-namespace:MyToDo.ViewModels  	 5        � Title$  
MainWindow��5      $
��1260��5      $	��600��5      ! 	 Prism.Mvvm.ViewModelLocator   AutoWireViewModel True5       � AllowsTransparency$
 True��5         MaterialDesignPaper#���   5      $��微软雅黑)�5      $��13(�5      $��Regular"�5        MaterialDesignBody#���  5      &   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=�5         TextRenderingMode$
 Auto=�5       � WindowStartupLocation$ CenterScreen=�5       � WindowStyle$
 None=�5      .��5      )  #MaterialDesignThemes.Wpf.DialogHost  -   5          Name$ 
DialogHost��5   	      DialogTheme$ Inherit=�5   	   	   
Identifier$
	 Root��5   	   .��5!   
   )  #MaterialDesignThemes.Wpf.DrawerHost  -   5        
  Name$
 
drawerHost��5!   $      IsLeftDrawerOpen 68   ��  �� ElementName$ MenuToggleButton�� �� Path$ 	IsChecked�   LeftDrawerContent 5"      ]� 5#      $	��220��6   .+�5$      �� $
��0,20q�6!   $	��Top=�6/   .+�5%      �� $��50��5&      $��50��5'      $��/Images/user.jpg��5(       J� Clip 5)      @� 5*   "    @� Center$ 25,25&�5+   %    @� RadiusX$ 25Q�5,   %    @� RadiusY$ 25Q�5-   %   5/      �� 50      $
��0,10q�51      $��Center=�52      ��53      �� *
UserName+54      �� 56      -   5         ��Name$ menubar��57        MyListBoxItemStyle#��[ 58      ��59      �� *
MenuBars+*  $Microsoft.Xaml.Behaviors.Interaction   Triggers 5:      +  %Microsoft.Xaml.Behaviors.EventTrigger  5;         	EventName$ SelectionChanged��6-   *  $Microsoft.Xaml.Behaviors.TriggerBase   Actions. 5<   "   2  ,Microsoft.Xaml.Behaviors.InvokeCommandAction     Command 68   �� *NavigateCommand+   CommandParameter 6\   �� $ menubar��$ SelectedItem�5=      ��5?      �� 5@      .R�5A   "   �� 
������  �� Orientation$ 
Horizontal=�6F   .+�5B   &   '	  !MaterialDesignThemes.Wpf.PackIcon	  $
��15,0q�6>    	  Kind 6L   �� *icon+�� 5C   &   $
��10,0q�60   ��6>   �� *Title+5D   #   5E      5G      5H      .��5J      �  � RowDefinitions 5K      �� 5L      $
��auto �6$   �� 5M      .+�5O      '
  !MaterialDesignThemes.Wpf.Snackbar
  -   5         
 Name$ Snackbar��5P       K� ZIndex$ 1��5Q       
  MessageQueue 5R      4  .MaterialDesignThemes.Wpf.MessageQueueExtension  (  "MaterialDesignThemes.Wpf.ColorZone  5S      -   5          Name$ 	ColorZone��5T      $��16q�5U      .  (MaterialDesignThemes.Wpf.ElevationAssist    	Elevation$	  Dp4=�5V      $	��Top=�5W      !   Mode$! 
PrimaryMid=�5X      .��5Y      ]� $
��True��6    .+�5[      �� $��Right=�6%   $ 
Horizontal=�6<   .+�5\      �� -   5        " ��Name$" btnMin��5]   !   $	��—��5^   !    ! MaterialDesignFlatMidBgButton#��[ 5_   !   �� 5`      -   5        $" btnMax��5a   !   $	��❐��5b   !   #��[ 5c   !   �� 5d      -   5        $" btnClose��5e   !   $	��✕��5f   !   #��[ 5g   !   5h      '  !MaterialDesignThemes.Wpf.PopupBox  5i      $��Right=�62   #   ToggleContent# 5j      �� 5k   "   $��25��5l   %   $��25��5m   %   $��/Images/user.jpg��5n   %    5o   &   @� 5p   *   $ 	12.5,12.5&�5q   -   $
 12.5Q�5r   -   $
 12.5Q�5s   -   5u   #   $   PopupContent.$ 5x      �� .+�5y   "   �� $��个人中心��6N   ��6)   �� *AppCenterCommand+6N   �� 5z   "   $��注销当前账户��6M   ��6)   �� *LoginOutCommand+6M   5{      5|      �� 5~      $ 
Horizontal=�6%   .+�5      d� -	   5        % d�Name$% MenuToggleButton��5�   !   4  .System.Windows.Automation.AutomationProperties&   Name$& HamburgerToggleButton��5�   !   ' d� 	IsChecked$' FalseY�5�   !    ' #MaterialDesignHamburgerToggleButton#��[ 5�   !   �� 5�      $��24,0,0,0q�5�   !     MaterialDesignToolButton#��[ 5�   !   ( � ToolTip$( Previous Item��5�   !   +  %MaterialDesignThemes.Wpf.RippleAssist)   Feedback) 5�   !   �� * �� RelativeSource* �� *Self+$ 
Foreground�  BrushRoundConverter+ �� 	Converter#+ [ ��5�   !   �� *GoBackCommand+��5�   !   0  *MaterialDesignThemes.Wpf.PackIconExtension  ,   Kind$, 	ArrowLeft=�-   Size- 24��5�   !   �� * �� . �� AncestorType". �$ (TextElement.Foreground)�5�   !   �� 5�      $��16,0,0,0q�5�   !   #��[ 5�   !   $( 	Next Item��5�   !   ) 5�   !   �� * �� *Self+$ 
Foreground�#+ [ ��5�   !   �� *GoForwardCommand+��5�   !     $, 
ArrowRight=�- 24��5�   !   �� * �� ". �$ (TextElement.Foreground)�5�   !   �� 5�      $��16,0,0,0q�5�   !   $��Center=�5�   !   $��Center=�5�   !   $%& Material Design In XAML Toolkit��5�   !   $��22(�5�   !   $��猜猜我是谁��5�   !   5�      5�      5�      �� 5�      $��1��6!   $   MyToDo.Extensions.PrismManager/   MainViewRegionName! 	 Prism.Regions.RegionManager0   
RegionName#0 Z/ 6.   5�      5�      5�      5�      