/**
 * Code Generator
 *
 * Created on 2019-11-25 20:07:23Z
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
        }
    }
}