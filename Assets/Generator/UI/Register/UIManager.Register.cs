/**
 * Code Generator
 *
 * Created on 2019-12-06 18:21:56Z
 */

using UIFramework;

namespace Game.Generator.UI
{
    public static partial class UIManagerRegister
    {
        /// <summary>
        /// 注册UI的AB路径
        /// </summary>
        public static void RegisterUIAB()
        {
			UIManager.Register<UIAlertContainer>("UI/UIAlertContainer");
			UIManager.Register<TestTableView>("UI/Test/TestTableView");
			UIManager.Register<TestTableViewItem>("UI/Test/TestTableViewItem");

        }
    }
}