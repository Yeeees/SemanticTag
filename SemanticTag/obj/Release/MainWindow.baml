   M S B A M L   `   `   `  ���� F  BSemanticTag, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:SemanticTagSemanticTag      SemanticTag.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     +localclr-namespace:SemanticTag       � Title$  
MainWindow��$��655.332��$��1016.395��.��(�  (� Stretch$
 Fill=�.���� $	��963��$	��559��.+��� -    ��Name$ sheetComboBox��$
��Left=�$	��Top=�$	��123��$��21�� �� 
IsEditable .  �� Left$	 327��
 �� Top$ 10���� -    ��Name$ 
sheetLabel��$��Sheet:��$
��Left=�$	��Top=�$��29��$��52��$	 275��$ 7��&   System.Windows.Controls.DataGrid  -     Name$ 	showTable��$
��Left=�$	��Top=�$	��460��$	��947��$ 10��$ 41�� J� RenderTransformOrigin$ 0.5,0.5&�	 %� IsVirtualizing	 .  
   
IsReadOnly
 .    SelectionUnit$
 Cell=�   CanUserSortColumns .   %� VirtualizationMode$ Standard=�����  �� IsAsync . �� -   $ tagComboBox��$
��Left=�$	��Top=�$	��120�� . $	 255��$	 506���� -    ��Name$ 
wordsLabel��$
��Left=�$	��Top=�$	��134��$��31��$��Right=�$	 116��$	 506��$��0q��� -    ��Name$ 
confirmBtn��$��确定��$
��Left=�$	��Top=�$��82��$��21��$	 380��$	 506���� -   $ addItemToTagBtn��$��添加新标签��$
��Left=�$	��Top=�$	��120��$	 255��$	 532���� -   $ processLabel��$�� ��$
��Left=�$	��Top=�$��82��$	 871��$ 11���� -	   $ openExcelBtn��$��打开Excel��$
��Left=�$��	10,10,0,0q�$	��Top=�$��75���� -
   $ 
openTxtBtn��$��	打开TXT��$
��Left=�$	��Top=�$��75��$	 101��$ 10���� -   $ readBtn��$��读取��$
��Left=�$	��Top=�$��75��$	 195��$ 10���� -   $ startBtn��$��查找下一个词��$	 463��$ 10��$��80���� -   $ setColumnBtn��$��标记整列��$	 551��$ 9��$��80��$��21��