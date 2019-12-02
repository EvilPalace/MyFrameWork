/**
 * EffectContainer
 *
 * Created by ZhangHuaGuang on 2019年11月19日
 */


using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UIFramework
{
    public class EffectContainer : IDisposable
    {
        private GameObject mPaticleObj;
        private Queue<GameObject> mPools;
        private float mDuration;

        private List<ParticleSystem> mParticles;
        private IDisposable mTimer;
        
        public EffectContainer(GameObject particleObj, Queue<GameObject> particlaPools = null)
        {
            this.mPaticleObj = particleObj;
            this.mPools = particlaPools;

            if (mPaticleObj != null)
            {
                mParticles = new List<ParticleSystem>(mPaticleObj.GetComponentsInChildren<ParticleSystem>());
            }
        }

        /// <summary>
        /// duration <= 0 代表特效常驻
        /// </summary>
        /// <param name="duration"></param>
        public void Play(float duration = 0, bool restart = true)
        {
            if (mParticles == null || mParticles.Count == 0)
                return;
            if (mTimer != null)
                mTimer.Dispose();
            foreach (var item in mParticles)
            {
                if (item != null)
                {
                    item.Simulate(0, false, restart);
                }
            }

            if (duration > 0)
                mTimer = Observable.Timer(TimeSpan.FromSeconds(duration)).Subscribe(_ => Stop());

        }

        /// <summary>
        /// 强制中断播放
        /// </summary>
        public void Stop()
        {
            if (mParticles == null || mParticles.Count == 0)
                return;
            foreach (var item in mParticles)
            {
                if (item != null && !item.isStopped)
                {
                    item.Stop(false);
                }
            }
        }

        public void Pause()
        {
            if (mParticles == null || mParticles.Count == 0)
                return;
            foreach (var item in mParticles)
            {
                if (item != null && !item.isPaused)
                {
                    item.Pause(false);
                }
            }
        }

        public void Dispose()
        {
            if (mTimer != null) mTimer.Dispose();
            Stop();
            if (mPools != null)
                mPools.Enqueue(mPaticleObj);
            else
                GameObject.Destroy(mPaticleObj);
            mPools = null;
            mPaticleObj = null;
            mParticles = null;
        }
    }
}