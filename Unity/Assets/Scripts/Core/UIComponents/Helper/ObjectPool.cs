using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ET
{
    public class ObjectPool<T> where T : new()    
    {
        private readonly Stack<T> objStack = new Stack<T>();

        private readonly UnityAction<T> onGet, onReturn;

        public int Count { get; private set; }

        public int UnuseCount => objStack.Count;

        public int ActiveCount => Count - UnuseCount;

        public ObjectPool(int num = 1, UnityAction<T> onGet = null, UnityAction<T> onReturn = null)
        {
            this.onGet = onGet;
            this.onReturn = onReturn;

            AddObjects(num);
        }

        public void AddObjects(int num)
        {
            for (int i=0;i< num;i++)
            {
                T obj = new T();
                objStack.Push(obj);
            }
        }

        public T NextObject()
        {
            T obj = default;
            if (objStack.Count > 0)
            {
                obj = objStack.Pop();
            }
            else
            {
                obj = new T();
                Count++;
            }
            if (onGet != null) onGet(obj);
            return obj;
        }

        public void ReturnObject(T obj)
        {
            if (objStack.Count > 0 && ReferenceEquals(objStack.Peek(), obj))
            {
#if UNITY_EDITOR
                Debug.LogWarning("is already in pool. Why are you trying to return it again? Check usage.");
#endif
            }

            if (onReturn != null) onReturn(obj);
            objStack.Push(obj);
        }

        public void DisposePool()
        {
            objStack.Clear();
            Count = 0;
        }
    }
}
