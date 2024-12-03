using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 数据桥接组件
/// </summary>
public class ReferenceData : MonoBehaviour
{
    /// <summary>
    /// 桥接数据
    /// </summary>
    private object data;

    /// <summary>
    /// 获取数据
    /// </summary>
    public T Get<T>() where T : class
    {
        return (T)data;
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    public void Set<T>(T value) where T : class
    {
        data = value;
    }
}

