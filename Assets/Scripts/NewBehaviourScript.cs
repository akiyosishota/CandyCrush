using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NewBehaviourScript : MonoBehaviour
{

}
public class ArraySyncExample
{
    // 配列
    private int[] array = new int[10];

    // 変更を通知するためのイベント
    public event Action<int[]> OnArrayChanged;

    // 配列が変更されたときに呼ばれるメソッド
    public void ChangeArray(int[] newArray)
    {
        array = newArray;
        // イベントを発生させる
        OnArrayChanged?.Invoke(array);
    }
}

public class OtherClass
{
    private ArraySyncExample arraySyncExample;

    public void Start()
    {
        arraySyncExample = new ArraySyncExample();
        // イベントを購読する
        arraySyncExample.OnArrayChanged += OnArrayChanged;
    }

    private void OnArrayChanged(int[] newArray)
    {
        // 配列が変更されたことを受け取る
    }
}