using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

/* �l�b�g�̕������������Ă��ĘM���������Ȃ̂ŗ���������ĂȂ��|�C���g���B
 * �R�����g�����\�G�����ǋ����Ăق���
 */

// Enum�̗v�f�ɕt����Attribute
// PropertyAttribute�ł͂Ȃ�System.Attribute���p������
[AttributeUsage(System.AttributeTargets.Field | AttributeTargets.Enum, AllowMultiple = false)]
public class EnumLabelAttribute : Attribute
{
    public string _displayName { get; private set; }        //�\�������Enum�̗v�f�����i�[����

    public string _labelName { get; private set; }          //�\�������Enum�̖��O���i�[����

    public EnumLabelAttribute(string newDisplayName, string displayName)
    {
        _labelName = newDisplayName;    //��������Inspector�ɕ\�����閼�O���󂯎��

        _displayName = displayName;     //��������v�f�����󂯎��
    }
}


[AttributeUsage(AttributeTargets.Field | AttributeTargets.Enum, AllowMultiple = false)]
public class EnumElements : PropertyAttribute
{
    public Type _elementName { get; set; }     //�Ăяo�����X�N���v�g����Enum�̗v�f���擾

    public EnumElements(Type enumLabel)
    {
        _elementName = enumLabel;   //��������v�f�����擾
    }
}

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(EnumElements))]
public class EnumAttributeDrawer : PropertyDrawer
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
        EnumLabelAttribute elementAttribute = default;            //��LEnumElementAttribute�i�[�p
        EnumElements enumLabelAttribute = attribute as EnumElements;      //��LEnumLabel���擾

        // FieldInfo����e�v�f��Attribute���擾�����O�𓾂�
        var names = new List<string>();
        foreach (var fi in enumLabelAttribute._elementName.GetFields())
        {
            if (fi.IsSpecialName)
            {
                // SpecialName�͔�΂�
                continue;
            }

            //EnumElementAttribute���i�[����
            elementAttribute = fi.GetCustomAttributes(typeof(EnumLabelAttribute), false).FirstOrDefault() as EnumLabelAttribute;

            //�v�f����ǉ�
            names.Add(elementAttribute == null ? fi.Name : elementAttribute._displayName);
        }

        string _labelName = elementAttribute._labelName;        //�Ăяo�����X�N���v�g����̈������i�[����B Inspector�ɕ\�����镶����B

        // �e�v�f�̒l��Enum.GetValues()�Ŏ擾����
        IEnumerable<int> values = System.Enum.GetValues(enumLabelAttribute._elementName).Cast<int>();

        // �`��
        property.intValue = EditorGUI.IntPopup(position, _labelName, property.intValue, names.ToArray(), values.ToArray());
    }
}

#endif
