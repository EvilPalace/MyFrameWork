/**
 * 代码产生器产生
 *
 * Created on 2019-12-06 18:36:00Z
 */


using System;
using UIFramework;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Generator.UI
{
    public partial class TestTableViewItem : UIItem
    {
        #region 变量

		private Text mNum;


        #endregion
        
        // 初始化
        protected void Awake()
        {
			mNum = transform.Find("mNum").GetComponent<Text>();

        }
    }
}