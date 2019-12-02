UI代码生成器：

1.脚本 - UI_Code_Creator
2.关键的三个tag：UIVIew， UIItem， Export
3.目前支持的导出Component为（Image,Text,Button），如果希望支持更多去函数GetExpression去添加
4.所有子项，目前不做名字去重处理，预制体上命的名就是到处脚本的名字
5.需要的文本范例TempleteUICode.txt，需要放在Editor Default Resources文件夹下
6.其中对应占位符有5个：时间，类名，继承的父类名，变量集合，获取路径处理集合


UI的AssetBundle路径设置：

1.脚本 - UI_AB_Creator
2.关键tag：UIView，UIItem
3.只是单纯的设置路径，不会进行AB包打包，如果需要打包AB包，是另外的操作