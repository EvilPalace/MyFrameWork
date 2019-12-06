/**
 * TestTableView
 *
 * Created by ZhangHuaGuang on 2019年12月6日
 */


using System.Collections.Generic;
using UIFramework;
using UnityEngine;

namespace Game.Generator.UI
{

    public class TestTableViewModel : ITableViewModel
    {
        public Color color = Color.black;
        public static TestTableViewModel CreateRand()
        {
            TestTableViewModel ret = new TestTableViewModel();
            ret.color = new Color(Random.Range(0, 1.0f), Random.Range(0, 1.0f), Random.Range(0, 1f), Random.Range(0.5f, 1f));
            return ret;
        }
    }
    public partial class TestTableView
    {
        private void Start()
        {
            mTableView.Initialize<TestTableViewItem, TestTableViewModel>();
            List<ITableViewModel> datas = new List<ITableViewModel>();
            for (int i = 0; i < 20; i++)
            {
                datas.Add(TestTableViewModel.CreateRand());
            }
            mTableView.SetDatas(datas);
        }
    }
}