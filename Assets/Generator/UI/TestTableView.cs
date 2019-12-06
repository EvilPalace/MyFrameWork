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
    public partial class TestTableView : UIView
    {
        #region 变量

		private Image mPost;
		private TableView mTableView;


        #endregion
        
        // 初始化
        protected void Awake()
        {
			mPost = transform.Find("mPost").GetComponent<Image>();
			mTableView = transform.Find("mTableView").GetComponent<TableView>();

        }
    }
}