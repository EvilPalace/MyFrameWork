/**
 * NoDrawcallRect
 *
 * Created by ZhangHuaGuang on 2019年12月3日
 */


using UnityEngine.UI;

namespace UIFramework
{
    public class NoDrawcallRect : Graphic
    {
        public NoDrawcallRect()
        {
            useLegacyMeshGeneration = false;
        }
        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            toFill.Clear();
        }
    }
}