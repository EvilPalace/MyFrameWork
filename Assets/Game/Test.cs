/**
 * Test
 *
 * Created by ZhangHuaGuang on 2019年11月25日
 */

using System;
using ABLoad;
using Game.Generator.UI;
using UIFramework;
using UnityEngine;

namespace Game
{
    public class Test : MonoBehaviour
    {
        private void Awake()
        {
            ABLoader.Initialize();
            UIManager.Initialize();
            UIManagerRegister.RegisterUIAB();
            
        }

        private void Start()
        {
            UIManager.CreateUIView<UIAlertContainer>(UILayer.center);
        }
    }
}