using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class GameContolloer : MonoBehaviour
 {
    //�t�B�[���h�̃L�����f�B���Ǘ����邽�߂�int�^�̔z��
    private int[,] _fieldNormalCandySearch = null;
    //�t�B�[���h�̃T�C�Y
    private int _fieldMaxX = 4;
    private int _fieldMaxY = 4;
    //�z��̋󂫂��m�F���邽�߂̕ϐ�(�u0�v�̓L�����f�B�w��Ŏg�p���Ă��邽�߁u99�v)
    private int _noData = 99;
    //�����̈Ⴄ�L�����f�B��Prefab���擾���邽�߂�GameObject�^�̔z��
    private GameObject[] _normalCandys;
    private GameObject[] _horizontalBoms;
    private GameObject[] _VerticalBoms;
    private GameObject[]_surroundingsBoms;
    //�t�B�[���h�ɂ���L�����f�B�̐e�I�u�W�F�N�g���擾
    [SerializeField, Label("�t�B�[���h�̐e�I�u�W�F�N�g")] GameObject _candys;
     private void Start()
     {
        //�z��̏�����
        _fieldNormalCandySearch = new int[_fieldMaxX + 1, _fieldMaxY + 1];
        FolderSearch();
        CandyArrySet();
     }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            //�L�����f�B�������܂�
            for (int Y = 0; Y <= _fieldMaxY; Y++)
            {
                for (int X = 0; X <= _fieldMaxY; X++)
                {
                    FieldCandyMatch(X, Y);
                }
            }
            EraseCheck();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            ///<summary>
            ///�z��̒��g���o�͂���e�X�g
            ///�t�B�[���h�Ɠ����`�Ŕz��\�������邽�߁A
            ///Y�̍ő�l����console�ɏo��
            ///</summary>
            print("Field------------------------------------------");
            for (int y = _fieldMaxY; y >= 0; y--)
            {
                string outPutString = "";
                for (int x = 0; x <= _fieldMaxX; x++)
                {
                    outPutString += _fieldNormalCandySearch[x, y];
                }
                print(outPutString);
            }
            print("Field------------------------------------------");
        }
    }

    private void EraseCheck()
    {
        //3�ȏセ����Ă���L�����f�B��GameObject�������܂�
        foreach (Transform Candy in _candys.transform)
        {           
            int CandyPosX = Mathf.FloorToInt(Candy.position.x);
            int CandyPosY = Mathf.FloorToInt(Candy.position.y);
            if (_fieldNormalCandySearch[CandyPosX, CandyPosY] == _noData)
            {
                Destroy(Candy.gameObject);
            }
        }
        for (int X = 0; X <= _fieldMaxX; X++)
        {
            FallCandy(X);
        }
    }
    private void FallCandy(int X)
    {
        ///<summary>
        ///�z��̋󂫂̕������������A
        ///�c�����ɘA�����ċ󂢂Ă镪�����ufallCount�v�Ƃ���
        ///�uFallMoveCandy�v���\�b�h�ɓn��
        ///</summary>
        int Y = 0;
        for(int count = Y; count <= _fieldMaxY; count++)
        {
            int fallCount = 0;
            if (_fieldNormalCandySearch[X, count] == _noData)
            {
                fallCount++;
                for (int i = count + 1; i <= _fieldMaxY && _fieldNormalCandySearch[X, i] == _noData; i++)
                {
                    fallCount++;
                }
                FallMoveCandy(X, count, fallCount);
            }
        }       
    }
    private void FallMoveCandy(int X, int Y, int fallCount)
    {
        ///<summary>
        ///�����Ώۂ̃L�����f�B���ufallCount�v�̕���������������
        ///_fieldNormalCandySearch[X, Y]��_fieldNormalCandySearch[X, Y + fallCount]��
        ///GameObject�Ɣz��̒��g���ړ������܂��B
        ///����_fieldNormalCandySearch[X, Y + fallCount]���z��O�̏ꍇ�����_����
        ///�L�����f�B�𐶐����܂�
        ///</summary>
        if (Y + fallCount <= _fieldMaxY)
        {
            GameObject dropCandy = GetFieldObject(X, Y + fallCount);
            if (dropCandy != null)
            {
                _fieldNormalCandySearch[X, Y] = _fieldNormalCandySearch[X, Y + fallCount];
                _fieldNormalCandySearch[X, Y + fallCount] = _noData;
                dropCandy.transform.position = new Vector2(X, Y);
            }
        }
        else
        {
            CreateCandy(X, Y);
        }
    }
    private void CreateCandy(int X, int Y)
    {
        ///<summary>
        ///����������Ώۂ̃L�����f�B�����݂��Ȃ��ꍇ
        ///�u_normalCandys�v�z�񂩂烉���_���ŃL�����f�B��GameObject�Ƃ��Ď��o���A
        ///�u_candys�v�̎q�I�u�W�F�N�g�Ƃ��Đ�������
        ///</summary>
        int randomCandy = Random.Range(0, _normalCandys.Length);
        GameObject newCandy = Instantiate(_normalCandys[randomCandy]);
        newCandy.transform.position = new Vector2(X, Y);
        newCandy.transform.SetParent(_candys.transform, true);
        _fieldNormalCandySearch[X, Y] = randomCandy;
    }
    private GameObject GetFieldObject(int X, int Y)
    {
        //����������L�����f�B��GameObject���擾
        foreach (Transform fieldCandy in _candys.transform)
        {
            if(fieldCandy.transform.position.x == X &&
               fieldCandy.transform.position.y == Y)
            {
                return fieldCandy.gameObject;
            }
        }
        return null;
    }
    private void FieldCandyMatch(int X, int Y)
    {
        ///<summary>
        ///���F�̃L�����f�B���c��������������3�ȏ����ł����
        ///�����ΏۂƂ��Ĕz��Ɋi�[
        ///</summary>
        int verticalConnectionCandyCount = default;
        int horizontalConnectionCandyCount = default;
        for (int i = X; i < _fieldMaxX && _fieldNormalCandySearch[i, Y] == _fieldNormalCandySearch[i + 1, Y]; i++)
        {
            verticalConnectionCandyCount++;
        }
        if (verticalConnectionCandyCount >= 2)
        {
            for (int i = 0; i <= verticalConnectionCandyCount; i++)
            {
                _fieldNormalCandySearch[X + i, Y] = _noData;
            }
        }
        for (int i = Y; i < _fieldMaxX && _fieldNormalCandySearch[X, i] == _fieldNormalCandySearch[X, i + 1]; i++)
        {
            horizontalConnectionCandyCount++;
        }
        if (horizontalConnectionCandyCount >= 2)
        {
            for (int i = 0; i <= horizontalConnectionCandyCount; i++)
            {
                _fieldNormalCandySearch[X, i + 1] = _noData;
            }
        }
    }
    private void FolderSearch()
    {
        ///<summary>
        ///Resources�t�H���_���o�R���Ė������Ƃɕ������t�H���_�֐ڑ����A
        ///���̒���GameObject(Prefab)�����ꂼ���GameObject�^�̔z��֊i�[����
        ///</summary>
        _normalCandys = Resources.LoadAll<GameObject>("NormalCandys");
        _horizontalBoms = Resources.LoadAll<GameObject>("HorizontalBoms");
        _VerticalBoms = Resources.LoadAll<GameObject>("VerticalBoms");
        _surroundingsBoms = Resources.LoadAll<GameObject>("SurroundingsBoms");
    }
    private void CandyArrySet()
    {
        ///<summary>
        ///�t�B�[���h�ɂ��镁�ʂ̃L�����f�BGameObject��
        ///foreach�Ŏ擾���A�擾�������̂�GameObject�^��
        ///�z��������Ƃɐ�����U��Aint�^�̔z��ɍ��W�Ɠ����ꏊ�Ɋi�[����
        ///</summary>
        foreach(Transform Candy in _candys.transform)
        {
            int CandyPosX = Mathf.FloorToInt(Candy.position.x);
            int CandyPosY = Mathf.FloorToInt(Candy.position.y);
            GameObject CandyObj = Candy.gameObject;
            for(int count = 0; count < _normalCandys.Length;)
            {
                if (CandyObj.GetComponent<SpriteRenderer>().sprite ==
                    _normalCandys[count].GetComponent<SpriteRenderer>().sprite)
                {
                    _fieldNormalCandySearch[CandyPosX, CandyPosY] = count;
                    break;
                }
                count++;
            }           
        }
    }
}


