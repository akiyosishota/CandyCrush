using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NewBehaviourScript : MonoBehaviour
{

}
public class ArraySyncExample
{
    // �z��
    private int[] array = new int[10];

    // �ύX��ʒm���邽�߂̃C�x���g
    public event Action<int[]> OnArrayChanged;

    // �z�񂪕ύX���ꂽ�Ƃ��ɌĂ΂�郁�\�b�h
    public void ChangeArray(int[] newArray)
    {
        array = newArray;
        // �C�x���g�𔭐�������
        OnArrayChanged?.Invoke(array);
    }
}

public class OtherClass
{
    private ArraySyncExample arraySyncExample;

    public void Start()
    {
        arraySyncExample = new ArraySyncExample();
        // �C�x���g���w�ǂ���
        arraySyncExample.OnArrayChanged += OnArrayChanged;
    }

    private void OnArrayChanged(int[] newArray)
    {
        // �z�񂪕ύX���ꂽ���Ƃ��󂯎��
    }
}