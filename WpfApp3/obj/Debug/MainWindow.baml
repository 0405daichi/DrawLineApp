   M S B A M L   `   `   `  ����B  >WpfApp3, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp3WpfApp3  5   	       WpfApp3.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:WpfApp3     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)    � DataContext 5	         WpfApp3.LineLists  5
   
    � 	Resources 5         AAA �� 
TargetType �� Property �� Value F�Name �� 
StartPoint �� EndPoint	 @� RadiusX
 @� RadiusY @� Center��%d  &	        �� 5   
   " ��6   .�5      ��  � �� 5      �� 5      .R�5      �� .+�5      F� $
 Line��6#   
���   �$��5��6@   ��5   "   �� 5   &    63   �� *StartPoint.Point+ 6[   �� *EndPoint.Point+5      F� 5      $ 
StartPoint��6#   
���   ���5   "   @� 5   &   	start��$	 5Q�6i   $
 5Q�6u    6E   �� *StartPoint.Point+6u   5      F� 5      $ EndPoint��6#   
���   ���5   "   @� 5   &   end��$	 5Q�6e   $
 5Q�6q    6C   �� *EndPoint.Point+6q   5!      5"      5#      5%      5&      5'      .��5(      � .+�5)   
   �� #��[  64   ��6   �� *List+��5*      �� 5+      .R�5,         WpfApp3.DesignerCanvas  
������ 6+   5-      5/      �� 51   
   $��50��6-   $��50��68   $
��Left=�6D   $	��Top=�6_   ��6   �� *	Command+6_   52      54      