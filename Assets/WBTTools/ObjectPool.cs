using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class ObjectPool<T> where T : class, IReUseObject, new()
{
    static Queue<T> tQueue = new Queue<T>();
    static T inst = null;

    public static T GetInst()
    {
        inst = null;
        while(tQueue.Count > 0)
        {
            inst = tQueue.Dequeue();
            if (inst != null)
            {
                break;
            }
        }
        if(inst == null)
        {
            inst = new T();
        }
        inst.ResetObject();
        return inst;
    }
    public static void ReturnInst(T inst)
    {
        tQueue.Enqueue(inst);
    }
    public static void ClearPool()
    {
        tQueue.Clear();
    }
}
public interface IReUseObject
{
    void ResetObject();
}