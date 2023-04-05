using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameContolloer : MonoBehaviour
 {
    //�t�B�[���h�̃L�����f�B���Ǘ����邽�߂̔z��
    private int[,] _fieldNormalCandySearch = null;
    private int[,] _fieldConnectionSearch = null;
    //��������{���̏����Ǘ�����z��
    private int[,] _fieldVerticalBoms = null;
    private int[,] _fieldHorizontalBoms = null;
    //�t�B�[���h�̃T�C�Y
    private int _fieldMaxX = 4;
    private int _fieldMaxY = 4;
    private int _cursorDisplayPosX = 99;
    private int _cursorDisplayPosY = 99;
    //�z��̋󂫂��m�F���邽�߂̕ϐ�(�u0�v�̓L�����f�B�w��Ŏg�p���Ă��邽�߁u99�v)
    private int _noData = 99;
    //����������������ēx�L�����f�B�������邩�m�F���邽�߂̕ϐ�
    private bool _reFallCandyCheck = false;

    private bool _cursorNotAcceptable = false;
    //�����̈Ⴄ�L�����f�B��Prefab���擾���邽�߂�GameObject�^�̔z��
    private GameObject[] _normalCandys;
    private GameObject[] _horizontalBoms;
    private GameObject[] _verticalBoms;
    private GameObject[] _surroundingsBoms;

    [SerializeField]  public  TextMeshProUGUI _text;
    //�t�B�[���h�ɂ���L�����f�B�̐e�I�u�W�F�N�g
    private GameObject _candys;
    //�N���b�N�������\��������J�[�\���̐e�I�u�W�F�N�g
    private GameObject _cursors;
    //�N���b�N���ɔ�΂������C�ƐڐG�����I�u�W�F�N�g���擾
    private GameObject _clickedCandy;
    //�\�����Ă���J�[�\���I�u�W�F�N�g
    private void Start()
     {
        //�z��̏�����
        _fieldNormalCandySearch = new int[_fieldMaxX + 1, _fieldMaxY + 1];
        _fieldVerticalBoms = new int[_fieldMaxX + 1, _fieldMaxY + 1];
        _fieldHorizontalBoms = new int[_fieldMaxX + 1, _fieldMaxY + 1];
        //�t�B�[���h�ɂ���L�����f�B�̐e�I�u�W�F�N�g���擾
        _candys = GameObject.Find("CandyObjects");
        //�N���b�N�������\��������J�[�\���̐e�I�u�W�F�N�g���擾
        _cursors = GameObject.Find("Cursors");
        FolderSearch();
        ArrySet();
     }
    private void Update()
    {
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
        if (Input.GetKeyDown(KeyCode.J))
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
                    outPutString += _fieldConnectionSearch[x, y];
                }
                print(outPutString);
            }
            print("Field------------------------------------------");
        }
        if (Input.GetMouseButtonDown(0))
        {
            //�L�����f�B�������Ă���Ƃ��ɂ͑���ł��Ȃ��悤�ɂ��܂�
            if (_cursorNotAcceptable == false)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);
                if (hit)
                {
                    _clickedCandy = hit.transform.gameObject;
                    CursorDisplay();
                }
                else
                {
                    CursorNotDisplay();
                }
            }
        }
        {
            string aaa = default;

            string a = default;

            //�t�B�[���h��`��
            for (int y = _fieldMaxY; y >= 0; y--)
            {
                for (int x = 0; x <= _fieldMaxX; x++)
                {

                    a = _fieldHorizontalBoms[x, y].ToString();

                    aaa = String.Concat(aaa, a);

                    _text.text = aaa;

                    aaa = String.Concat(aaa, "  ");
                }
                aaa = String.Concat(aaa, "\n");
            }
        }
    }
    private void CursorNotDisplay()
    {
        foreach (Transform cursor in _cursors.transform)
        {
            int cursorX = Mathf.FloorToInt(cursor.position.x);
            int cursorY = Mathf.FloorToInt(cursor.position.y);
            if (_cursorDisplayPosX == cursorX &&
                _cursorDisplayPosY == cursorY)
            {
                _cursorDisplayPosX = _noData;
                _cursorDisplayPosY = _noData;
                cursor.gameObject.SetActive(false);
            }
        }
    }
    private void CursorDisplay()
    {
        foreach (Transform cursor in _cursors.transform)
        {
            int cursorX = Mathf.FloorToInt(cursor.position.x);
            int cursorY = Mathf.FloorToInt(cursor.position.y);
            if (_clickedCandy.transform.position.x == _cursorDisplayPosX &&
                _clickedCandy.transform.position.y == _cursorDisplayPosY)
            {
                if (cursorX == _clickedCandy.transform.position.x &&
                    cursorY == _clickedCandy.transform.position.y)
                {
                    _cursorDisplayPosX = _noData;
                    _cursorDisplayPosY = _noData;
                    cursor.gameObject.SetActive(false);
                    break;
                }
            }
            if (_cursorDisplayPosX != _noData &&
                _cursorDisplayPosY != _noData)
            {
                if (cursorX == _cursorDisplayPosX &&
                    cursorY == _cursorDisplayPosY)
                {
                    int secondClickCandyPosX = Mathf.FloorToInt(_clickedCandy.transform.position.x);
                    int secondClickCandyPosY = Mathf.FloorToInt(_clickedCandy.transform.position.y);
                    cursor.gameObject.SetActive(false);
                    CandyChange(_cursorDisplayPosX, _cursorDisplayPosY, secondClickCandyPosX, secondClickCandyPosY);
                    _cursorDisplayPosX = _noData;
                    _cursorDisplayPosY = _noData;
                    break;
                }
            }
            if (_cursorDisplayPosX == _noData &&
                _cursorDisplayPosY == _noData)
            {
                if (cursorX == _clickedCandy.transform.position.x &&
                    cursorY == _clickedCandy.transform.position.y)
                {
                    cursor.gameObject.SetActive(true);
                    _cursorDisplayPosX = cursorX;
                    _cursorDisplayPosY = cursorY;
                    break;
                }
            }              
        }
    }
    private void CandyChange(int firstClickCandyPosX, int firstClickCandyPosY, int secondClickCandyPosX, int secondClickCandyPosY)
    {
        ///<summary>
        ///�N���b�N�ɂă��C���Ƃ΂��āA�Փ˂���GameObject���擾���A
        ///�N���b�N1��ځA2��ڂ�X�AY�̍��W��ێ����܂�
        ///�ǂ����GameObject���擾�o��������W���������A�擾�ł��Ȃ������ꍇ�́A
        ///�N���b�N�̉񐔂ƕێ����Ă�����W�����Z�b�g���܂�
        ///</summary>
        if (_cursorNotAcceptable == default)
        {
            GameObject firstClickCandyObj = default;
            int firstClickCandycolor = default;
            foreach (Transform firstClickCandy in _candys.transform)
            {
                if (firstClickCandyPosX == firstClickCandy.transform.position.x &&
                    firstClickCandyPosY == firstClickCandy.transform.position.y)
                {
                    firstClickCandyObj = firstClickCandy.gameObject;
                    firstClickCandycolor = _fieldNormalCandySearch[firstClickCandyPosX, firstClickCandyPosY];
                    break;
                }
            }
            foreach (Transform secondClickCandy in _candys.transform)
            {
                if (secondClickCandyPosX == secondClickCandy.transform.position.x &&
                    secondClickCandyPosY == secondClickCandy.transform.position.y)
                {
                    firstClickCandyObj.transform.position = new Vector2(secondClickCandyPosX, secondClickCandyPosY);
                    secondClickCandy.gameObject.transform.position = new Vector2(firstClickCandyPosX, firstClickCandyPosY);
                    int i = _fieldNormalCandySearch[secondClickCandyPosX, secondClickCandyPosY];
                    _fieldNormalCandySearch[secondClickCandyPosX, secondClickCandyPosY] = _fieldNormalCandySearch[firstClickCandyPosX, firstClickCandyPosY];
                    _fieldNormalCandySearch[firstClickCandyPosX, firstClickCandyPosY] = i;
                    break;
                }
            }
            StartCoroutine(ConnectCandyCheck());
        }
    }
    private IEnumerator ConnectCandyCheck()
    {
        //�N���b�N�ł̃L�����f�B�̈ړ����ł��Ȃ��悤�������܂�
        _cursorNotAcceptable = true;
        //�{������������Ȃ��ʏ����
        int vanishCandy = 2;
        //�{�[�_�[���{���̐�����
        int stripeBom = 3;

        _reFallCandyCheck = false;
        for (int Y = 0; Y <= _fieldMaxY; Y++)
        {
            for (int X = 0; X <= _fieldMaxY; X++)
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
                if (verticalConnectionCandyCount >= stripeBom)
                {
                    _fieldVerticalBoms[X + UnityEngine.Random.Range(0, verticalConnectionCandyCount), Y] = _fieldNormalCandySearch[X + verticalConnectionCandyCount, Y];
                }
                if (verticalConnectionCandyCount >= vanishCandy)
                {
                    for (int i = 0; i <= verticalConnectionCandyCount; i++)
                    {
                        _fieldConnectionSearch[X + i, Y] = _noData;
                    }
                }
                for (int i = Y; i < _fieldMaxY && _fieldNormalCandySearch[X, i] == _fieldNormalCandySearch[X, i + 1]; i++)
                {
                    horizontalConnectionCandyCount++;
                }
                if (horizontalConnectionCandyCount >= stripeBom)
                {
                    _fieldHorizontalBoms[X , Y + UnityEngine.Random.Range(0, horizontalConnectionCandyCount)] = _fieldNormalCandySearch[X , Y + horizontalConnectionCandyCount];
                }
                if (horizontalConnectionCandyCount >= vanishCandy)
                {
                    for (int i = 0; i <= horizontalConnectionCandyCount; i++)
                    {
                        _fieldConnectionSearch[X, Y + i] = _noData;
                    }
                }
            }
        }
        //3�ȏセ����Ă���L�����f�B��GameObject�������܂�
        foreach (Transform Candy in _candys.transform)
        {
            int CandyPosX = Mathf.FloorToInt(Candy.position.x);
            int CandyPosY = Mathf.FloorToInt(Candy.position.y);
            if (_fieldConnectionSearch[CandyPosX, CandyPosY] == _noData)
            {
                Destroy(Candy.gameObject);
                _fieldNormalCandySearch[CandyPosX, CandyPosY] = _noData;
                CreateBoms(CandyPosX, CandyPosY);
            }
        }
        for (int X = 0; X <= _fieldMaxX; X++)
        {
            ///<summary>
            ///�z��̋󂫂̕������������A
            ///�c�����ɘA�����ċ󂢂Ă镪�����ufallCount�v�Ƃ���
            ///�uFallMoveCandy�v���\�b�h�ɓn��
            ///</summary>
            int Y = 0;
            for (int count = Y; count <= _fieldMaxY; count++)
            {
                yield return null;
                StartCoroutine(CandyFall(X, count));
                yield return null;
            }
        }
        _fieldConnectionSearch = (int[,])_fieldNormalCandySearch.Clone();
        //�z��̓������s����悤�Ƀf�B���C�������܂�
        yield return new WaitForSeconds(0.5f);
        if (_reFallCandyCheck)
        {
            //�L�����f�B�������������s�����ꍇ�A����ɃL�����f�B�������Ȃ����T�����Ȃ����܂�
            StartCoroutine(ConnectCandyCheck());
        }
        else
        {
            //������L�����f�B���Ȃ��Ȃ�ƍēx�L�����f�B�𑀍�ł���悤�ɂȂ�܂�
            _cursorNotAcceptable = false;
        }
    }
    private IEnumerator CandyFall(int X, int count)
    {
        int fallCount = 0;
        if (_fieldNormalCandySearch[X, count] == _noData)
        {
            fallCount++;
            _reFallCandyCheck = true;
            for (int i = count + 1; i <= _fieldMaxY && _fieldNormalCandySearch[X, i] == _noData; i++)
            {
                fallCount++;
            }
            ///<summary>
            ///�����Ώۂ̃L�����f�B���ufallCount�v�̕���������������
            ///_fieldNormalCandySearch[X, count]��_fieldNormalCandySearch[X, count + fallCount]��
            ///GameObject�Ɣz��̒��g���ړ������܂��B
            ///</summary>
            yield return null;
            if (count + fallCount <= _fieldMaxY)
            {
                GameObject dropCandy = GetFieldObject(X, count + fallCount);
                if (dropCandy != null)
                {
                    _fieldNormalCandySearch[X, count] = _fieldNormalCandySearch[X, count + fallCount];
                    _fieldNormalCandySearch[X, count + fallCount] = _noData;
                    dropCandy.transform.position = new Vector2(X, count);
                }
            }
            else
            {
                ///<summary>
                ///����������Ώۂ̃L�����f�B�����݂��Ȃ��ꍇ
                ///�u_normalCandys�v�z�񂩂烉���_���ŃL�����f�B��GameObject�Ƃ��Ď��o���A
                ///�u_candys�v�̎q�I�u�W�F�N�g�Ƃ��Đ�������
                ///</summary>
                int randomCandy = UnityEngine.Random.Range(0, _normalCandys.Length);
                GameObject newCandy = Instantiate(_normalCandys[randomCandy]);
                newCandy.transform.position = new Vector2(X, count);
                newCandy.transform.SetParent(_candys.transform, true);
                _fieldNormalCandySearch[X, count] = randomCandy;
            }
        }
    }
    private void CreateBoms(int X, int Y)
    {
        ///<summary>
        ///�u_fieldNormalCandySearch[X, Y]���u_noData�v�̂Ƃ��A
        ///�e�{���z���[X, Y]��T�����u_noData�v�łȂ��Ƃ��A
        ///[X, Y]���W�ɑΉ������{����GameObject���u_candys�v�̎q�I�u�W�F�N�g�Ƃ��Đ������܂�
        ///</summary>
        if (_fieldVerticalBoms[X, Y] != _noData)
        {
            GameObject newVerticalBom = Instantiate(_verticalBoms[_fieldVerticalBoms[X, Y]]);
            newVerticalBom.transform.position = new Vector2(X, Y);
            newVerticalBom.transform.SetParent(_candys.transform, true);
            _fieldNormalCandySearch[X, Y] = _fieldVerticalBoms[X, Y];
            _fieldConnectionSearch[X, Y] = _fieldVerticalBoms[X, Y];
            _fieldVerticalBoms[X, Y] = _noData;
        }
        if (_fieldHorizontalBoms[X, Y] != _noData)
        {
            GameObject newHorizontalBom = Instantiate(_horizontalBoms[_fieldHorizontalBoms[X, Y]]);
            newHorizontalBom.transform.position = new Vector2(X, Y);
            newHorizontalBom.transform.SetParent(_candys.transform, true);
            _fieldNormalCandySearch[X, Y] = _fieldHorizontalBoms[X, Y];
            _fieldConnectionSearch[X, Y] = _fieldHorizontalBoms[X, Y];
            _fieldHorizontalBoms[X, Y] = _noData;
        }

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
    private void FolderSearch()
    {
        ///<summary>
        ///Resources�t�H���_���o�R���Ė������Ƃɕ������t�H���_�֐ڑ����A
        ///���̒���GameObject(Prefab)�����ꂼ���GameObject�^�̔z��֊i�[����
        ///</summary>
        _normalCandys = Resources.LoadAll<GameObject>("NormalCandys");
        _horizontalBoms = Resources.LoadAll<GameObject>("HorizontalBoms");
        _verticalBoms = Resources.LoadAll<GameObject>("VerticalBoms");
        _surroundingsBoms = Resources.LoadAll<GameObject>("SurroundingsBoms");
    }
    private void ArrySet()
    {
        ///<summary>
        ///�t�B�[���h�ɂ��镁�ʂ̃L�����f�BGameObject��
        ///foreach�Ŏ擾���A�擾�������̂�GameObject�^��
        ///�z��������Ƃɐ�����U��Aint�^�̔z��ɍ��W�Ɠ����ꏊ�Ɋi�[����
        ///</summary>
        foreach (Transform candy in _candys.transform)
        {
            int candyPosX = Mathf.FloorToInt(candy.position.x);
            int candyPosY = Mathf.FloorToInt(candy.position.y);
            GameObject CandyObj = candy.gameObject;
            for(int count = 0; count < _normalCandys.Length;)
            {
                if (CandyObj.GetComponent<SpriteRenderer>().sprite ==
                    _normalCandys[count].GetComponent<SpriteRenderer>().sprite)
                {
                    _fieldNormalCandySearch[candyPosX, candyPosY] = count;
                    break;
                }
                count++;
            }
            //�{�������̂��߂̔z��̒��g��S�āu_noData�v�ɂ��܂�
            _fieldVerticalBoms[candyPosX, candyPosY] = _noData;
            _fieldHorizontalBoms[candyPosX, candyPosY] = _noData;
        }
        //�T���p�̔z��ɃR�s�[���܂�
        _fieldConnectionSearch = (int[,])_fieldNormalCandySearch.Clone();
    }
}


