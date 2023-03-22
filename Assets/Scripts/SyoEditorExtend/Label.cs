using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Label : PropertyAttribute
{
    public readonly string _label;      //Inspector�ɕ\������镶������i�[����(�ǂݎ���p)

    /// <summary>
    /// <para>Label</para>
    /// <para>�Ăяo���ꂽ�ۂ̈�����_label�ɑ������</para>
    /// </summary>
    /// <param name="label">Inspector�ɕ\������镶���� [Label("����")]</param>
    public Label(string label)
    {
        _label = label;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(Label))]
public class LabelAttributeDrawer : PropertyDrawer
{

    /// <summary>
    /// <para>OnGUI</para>
    /// <para>Unity��GUI�����B �p�����Ēǉ��̏��������ď㏑������</para>
    /// </summary>
    /// <param name="position">Inspector��ɕ\�������v�f�̉�ʏ���W</param>
    /// <param name="property">Inspector�̗v�f int�̓��͗��Ƃ�[SerializeField]�ŕ\�������z�B�v�����Inspector�ɏo�Ă��S��</param>
    /// <param name="label">Inspector�ɕ\�����镶����</param>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Label newLabel = attribute as Label;                        //��LLabel�N���X���擾
        label.text = newLabel._label;                               //��LLabel�N���X�Ɉ�����string��n��
        EditorGUI.PropertyField(position, property, label, true);   //Inspector��ł̕\�����㏑������
                                                                    //(Inspector��̍��W, �㏑������v�f, �\����, �\���̗L��)
    }

    /// <summary>
    /// <para>GetPropertyHeight</para>
    /// <para>Unity��GUI�����B �����ł͕ϐ��̕\���ʒu��Ԃ������ɏ㏑��</para>
    /// </summary>
    /// <param name="property">Inspector�̗v�f</param>
    /// <param name="label">Inspector�ɕ\�����镶����</param>
    /// <returns></returns>
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, true);         //�Ăяo�����ϐ���Inspector���W��Ԃ�
    }
}
#endif
