/**
 * 代码产生器产生
 *
 * Created on 2019-11-22 17:31:52Z
 */


using System;
using UIFramework;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Generator.UI
{
    public partial class TestUIView : UIView
    {
        #region 变量

		private Image Red;
		private Image WhiteBtn;
		private Text Name;
		private GameObject SonObj;


        #endregion
        
        // 初始化
        protected void Awake()
        {
			transform.Find("Red").GetComponent<Image>();
			transform.Find("WhiteBtn").GetComponent<Image>();
			transform.Find("Name").GetComponent<Text>();
			transform.Find("Son/SonObj");

        }
    }
}