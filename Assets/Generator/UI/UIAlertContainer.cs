/**
 * 代码产生器产生
 *
 * Created on 2019-11-22 18:27:54Z
 */


using System;
using UIFramework;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Generator.UI
{
    public partial class UIAlertContainer : UIView
    {
        #region 变量

		private GameObject Content;


        #endregion
        
        // 初始化
        protected void Awake()
        {
			transform.Find("Content");

        }
    }
}