using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Form
{
    /// <summary>
    /// UI窗体的一些辅助类
    /// </summary>
    static class FrmUtils
    {

        /// <summary>
        /// 根据名字获得一个文本控件
        /// </summary>
        /// <param name="gameObjName"></param>
        /// <returns></returns>
        public static Text GetText(string gameObjName)
        {
            var obj = GameObject.Find(gameObjName);
            if (obj == null)
                return null;

            return obj.GetComponent<Text>();
        }
    }
}
