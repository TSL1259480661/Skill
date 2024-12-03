using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ET
{
    public static class ListPoolHelper<T>
    {
        private static ObjectPool<List<T>> pool = new ObjectPool<List<T>>(1, null, l=>l.Clear());

        public static List<T> Get()
        {
            return pool.NextObject();
        }

        public static void Return(List<T> obj)
        {
            pool.ReturnObject(obj);
        }
    }

    public static class DictionaryPoolHelper<K, V>
    {
        private static ObjectPool<Dictionary<K, V>> pool = new ObjectPool<Dictionary<K, V>>(1, null, l => l.Clear());

        public static Dictionary<K, V> Get()
        {
            return pool.NextObject();
        }

        public static void Return(Dictionary<K, V> obj)
        {
            pool.ReturnObject(obj);
        }
    }

    public static class HashSetPoolHelper<T>
    {
        private static ObjectPool<HashSet<T>> pool = new ObjectPool<HashSet<T>>(1, null, l => l.Clear());

        public static HashSet<T> Get()
        {
            return pool.NextObject();
        }

        public static void Return(HashSet<T> obj)
        {
            pool.ReturnObject(obj);
        }
    }

    public static class QueuePoolHelper<T>
    {
        private static ObjectPool<Queue<T>> pool = new ObjectPool<Queue<T>>(1, null, l => l.Clear());

        public static Queue<T> Get()
        {
            return pool.NextObject();
        }

        public static void Return(Queue<T> obj)
        {
            pool.ReturnObject(obj);
        }
    }

    public static class StackPoolHelper<T>
    {
        private static ObjectPool<Stack<T>> pool = new ObjectPool<Stack<T>>(1, null, l => l.Clear());

        public static Stack<T> Get()
        {
            return pool.NextObject();
        }

        public static void Return(Stack<T> obj)
        {
            pool.ReturnObject(obj);
        }
    }

    public static class ObjectPoolHelper<T> where T : new ()
    {
        private static ObjectPool<T> pool = new ObjectPool<T>();

        public static T Get()
        {
            return pool.NextObject();
        }

        public static void Return(T obj)
        {
            pool.ReturnObject(obj);
        }
    }
}