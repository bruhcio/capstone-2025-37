using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PriorityList<T>
where T : class
{
    // priority 오름차순 정렬됨.
    List<PriorityNode<T>> dataList;

    public T this[int i]
    {
        get
        {
            return dataList[i].GetTargetValue();
        }
    }

    public int Count
    {
        get
        {
            return dataList.Count;
        }
    }

    Queue<PriorityNode<T>> nodePooler;
    PriorityNode<T> tempNode;

    public PriorityList()
    {
        dataList = new List<PriorityNode<T>>();
        nodePooler = new Queue<PriorityNode<T>>();
    }

    public void Enqueue(double priority, T targetValue)
    {
        if (nodePooler.Count > 0)
        {
            tempNode = nodePooler.Dequeue();
            tempNode.InitNode(priority, targetValue);
            dataList.Insert(GetPriorityIndex(priority), tempNode);
        }
        else
        {
            dataList.Insert(GetPriorityIndex(priority), new PriorityNode<T>(priority, targetValue));
        }
    }

    public T Dequeue()
    {
        if(dataList.Count <= 0){
            return null;
        }

        tempNode = dataList[0];
        nodePooler.Enqueue(tempNode);
        dataList.RemoveAt(0);
        return tempNode.GetTargetValue();
    }

    public PriorityNode<T> GetTargetNode(int index){
        return dataList[index];
    }

    public void Clear()
    {
        dataList.Clear();
    }

    public void RemoveValue(T targetValue)
    {
        int found = -1;
        found = dataList.FindIndex(item => item.GetTargetValue().Equals(targetValue));
        dataList.RemoveAt(found);
    }

    public T PeekFirst(){
        if(dataList.Count <= 0){
            return null;
        }

        return dataList[0].GetTargetValue();
    }
    public T PeekLast(){
        if(dataList.Count <= 0){
            return null;
        }

        return dataList[dataList.Count - 1].GetTargetValue();
    }

    int GetPriorityIndex(double priority)
    {
        int seek = 0;
        int low = 0, high = dataList.Count;

        if (high <= 0)
        {
            return 0;
        }

        while (true)
        {
            seek = (high + low) / 2;
            if (dataList[seek].GetPriority() > priority)
            {
                high = seek;
            }
            else if (dataList[seek].GetPriority() <= priority)
            {
                low = seek + 1;
            }
            else
            {
                return seek;
            }

            if (low >= high)
            {
                return low;
            }
        }
    }
}

public class PriorityNode<T>
{
    T targetValue;
    double priority;

    public PriorityNode(double priority, T targetValue)
    {
        InitNode(priority, targetValue);
    }

    public void InitNode(double priority, T targetValue)
    {
        this.priority = priority;
        this.targetValue = targetValue;
    }

    public double GetPriority()
    {
        return priority;
    }

    public void UpdatePriority(double priority)
    {
        this.priority = priority;
    }

    public T GetTargetValue()
    {
        return targetValue;
    }
}
