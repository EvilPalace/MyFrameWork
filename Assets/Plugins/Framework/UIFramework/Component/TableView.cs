/**
 * TableView
 *
 * Created by ZhangHuaGuang on 2019年12月3日
 */


using System;
using System.Collections.Generic;
using UniRx.Toolkit;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIFramework
{
    public interface ITableViewModel
    {
        
    }

    public interface ITableViewItem
    {
        void Bind(ITableViewModel data);
    }

    class ItemPool : ObjectPool<UIItem>
    {
        private Transform mParent;
        private Type mType;
        public ItemPool(Type type, Transform parent)
        {
            this.mParent = parent;
            this.mType = type;
        }
        
        protected override UIItem CreateInstance()
        {
            return UIManager.CreateUIItem(mType, mParent);
        }
    }
    
    [RequireComponent(typeof(RectMask2D), typeof(NoDrawcallRect))]
    public class TableView : ScrollRect 
    {

        #region 方便访问值

        private RectTransform rectTransform;
        private GridLayoutGroup grid;

        #endregion
        
        #region Type

        private System.Type ItemCell;
        private System.Type ItemModel;

        #endregion

        private ItemPool mItemPool;
        private List<RectTransform> mCellContainers = new List<RectTransform>();
        private Dictionary<int, UIItem> mItems = new Dictionary<int, UIItem>();
        private Vector2Int indexRange;

        #region Initialize

        void Awake()
        {
            base.Awake();
            if (content != null) return;
            
            rectTransform = GetComponent<RectTransform>();
            // content的初始化
            content = new GameObject("Content", typeof(RectTransform)).GetComponent<RectTransform>();
            content.SetParent(transform);
            content.anchorMin = content.anchorMax = content.pivot = Vector2.up;
            content.localPosition = Vector3.zero;
            content.localScale = Vector3.one;
            grid = content.gameObject.AddComponent<GridLayoutGroup>();
            var fitter = content.gameObject.AddComponent<ContentSizeFitter>();
            fitter.horizontalFit = fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        }

        public void Initialize<ItemCell, ItemModel>()
            where  ItemCell : UIItem, ITableViewItem
            where  ItemModel : ITableViewModel
        {
            this.ItemCell = typeof(ItemCell);
            this.ItemModel = typeof(ItemModel);
            
            mItemPool = new ItemPool(this.ItemCell, transform);
        }

#if UNITY_EDITOR
        [MenuItem("Reset")]
        public void Reset()
        {
            if (grid != null)
                DestroyImmediate(grid.gameObject);
            content = null;
            Awake();
        }
#endif

        #endregion
        

        private List<ITableViewModel> Datas { get; set; }

        public void SetDatas(List<ITableViewModel> datas)
        {
            this.Datas = datas;
            // 回收所有item
            HideRange(0, mCellContainers.Count);
            // 处理对应container
            HandleContainer(datas.Count);
            ShowRange(indexRange.x, indexRange.y);
        }

        public override void OnScroll(PointerEventData data)
        {
            base.OnScroll(data);

            Vector2Int tempIndexRange = GetIndex();
            if (tempIndexRange != indexRange)
            {
                // 整个缩小
                if (InRange(tempIndexRange.x, indexRange) && InRange(tempIndexRange.y, indexRange))
                {
                    // 把多余的部分挪去
                    HideRange(indexRange.x, tempIndexRange.x - 1);
                    HideRange(tempIndexRange.y + 1, indexRange.y);
                }
                // 整个变大
                else if (InRange(indexRange.x, tempIndexRange) && InRange(indexRange.y, tempIndexRange))
                {
                    // 把不足的部分补上
                    ShowRange(tempIndexRange.x, indexRange.x);
                    ShowRange(indexRange.y, tempIndexRange.y);
                }
                // 上滑（展示靠下的部分）
                else if (InRange(tempIndexRange.x, indexRange))
                {
                    // 补上一部分
                    ShowRange(indexRange.y, tempIndexRange.y);
                    // 挪去多余的部分
                    HideRange(indexRange.x, tempIndexRange.x);
                }
                // 下滑（展示靠上的部分）
                else if (InRange(tempIndexRange.y, indexRange))
                {
                    // 补上一部分
                    ShowRange(tempIndexRange.x, indexRange.x);
                    // 挪去多余的部分
                    HideRange(tempIndexRange.y, indexRange.y);
                }

                indexRange = tempIndexRange;
            }

        }

        // 将container数量弄为count
        void HandleContainer(int count)
        {
            if (mCellContainers.Count < count)
            {
                for (int i = 0, num = count - mCellContainers.Count; i < num; i++)
                {
                    var rectT = new GameObject("Container", typeof(RectTransform)).GetComponent<RectTransform>();
                    rectT.SetParent(content);
                    rectT.localScale = Vector3.one;
                    mCellContainers.Add(rectT);
                }
            }
            else if (mCellContainers.Count > count)
            {
                for (int i = count, num = mCellContainers.Count; i < num; i++)
                {
                    Destroy(mCellContainers[i].gameObject);
                }
            }
        }

        // 将 [from,to] 的部分加进来 
        void ShowRange(int from, int to)
        {
            for (int i = from; i <= to; i++)
            {
                if (i < 0 || i >= mCellContainers.Count) 
                    continue;
                UIItem item = null;
                if (!mItems.ContainsKey(i) || mItems[i] == null)
                {
                    item = mItemPool.Rent();
                    item.transform.SetParent(mCellContainers[i]);
                    (item as ITableViewItem).Bind(Datas[i]);
                }
                mItems[i] = item;
            }
        }

        // 将 [from,to] 的部分隐藏起来
        void HideRange(int from, int to)
        {
            for (int i = from; i <= to; i++)
            {
                if (i < 0 || i >= mCellContainers.Count) 
                    continue;
                if (mItems.ContainsKey(i) && mItems[i] != null)
                    mItemPool.Return(mItems[i]);
                mItems[i] = null;
            }
        }

        // 判定target是否在（range.x, range.y)中
        bool InRange(int target, Vector2Int range)
        {
            return target > range.x && target < range.y;
        }

        // 获取展示出来的index范围
        Vector2Int GetIndex()
        {
            if (mCellContainers.Count == 0) return Vector2Int.one * -1;
            
            Vector2Int ret = new Vector2Int(-1, -1);
            Rect rect = rectTransform.rect;
            
            for (int i = 0, count = mCellContainers.Count; i < count; i++)
            {
                if (ret.x == -1)
                {
                    if (mCellContainers[i].rect.Overlaps(rect))
                    {
                        ret.x = i;
                    }
                }
                else
                {
                    if (!mCellContainers[i].rect.Overlaps(rect))
                    {
                        ret.y = i;
                        break;
                    }
                }
            }

            if (ret.y == -1) ret.y = ret.x;

            return ret;
        }
    }
}