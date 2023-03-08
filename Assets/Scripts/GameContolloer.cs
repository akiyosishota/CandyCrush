using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class GameContolloer : MonoBehaviour
 {
    //�t�B�[���h�̃L�����f�B���Ǘ����邽�߂�int�^�̔z��
    private int[,] _fieldCandy = null;
    //�t�B�[���h�̃T�C�Y
    private int _fieldMaxX = 5;
    private int _fieldMaxY = 5;
    //�����̈Ⴄ�L�����f�B��Prefab���擾���邽�߂�GameObject�^�̔z��
    private GameObject[] _normalCandys;
    private GameObject[] _sideBoms;
    private GameObject[] _VerticalBoms;
    private GameObject[]_surroundingsBoms;
    //�t�B�[���h�ɂ���L�����f�B�̐e�I�u�W�F�N�g���擾
    [SerializeField] GameObject _candys;
     private void Start()
     {
        //�z��̏�����
        _fieldCandy = new int[_fieldMaxX, _fieldMaxY];        
        FolderSearch();
        CandyArrySet();
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
            for (int y = _fieldMaxY - 1; y >= 0; y--)
            {
                string outPutString = "";
                for (int x = 0; x < _fieldMaxX; x++)
                {
                    Debug.Log("bbb");
                    outPutString += _fieldCandy[x, y];
                }
                print(outPutString);
            }
            print("Field------------------------------------------");
        }
    }
    private void FolderSearch()
    {
        ///<summary>
        ///Resources�t�H���_���o�R���Ė������Ƃɕ������t�H���_�֐ڑ����A
        ///���̒���GameObject(Prefab)�����ꂼ���GameObject�^�̔z��֊i�[����
        ///</summary>
        _normalCandys = Resources.LoadAll<GameObject>("NormalCandys");
        _sideBoms = Resources.LoadAll<GameObject>("SideBoms");
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
                    _fieldCandy[CandyPosX, CandyPosY] = count;
                    count = 0;
                    break;
                }
                count++;
            }           
        }
    }
}


