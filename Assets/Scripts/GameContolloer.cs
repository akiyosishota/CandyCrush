using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class GameContolloer : MonoBehaviour
 {
    //フィールドのキャンディを管理するためのint型の配列
    private int[,] _fieldCandy = null;
    //フィールドのサイズ
    private int _fieldMaxX = 5;
    private int _fieldMaxY = 5;
    //役割の違うキャンディのPrefabを取得するためのGameObject型の配列
    private GameObject[] _normalCandys;
    private GameObject[] _sideBoms;
    private GameObject[] _VerticalBoms;
    private GameObject[]_surroundingsBoms;
    //フィールドにあるキャンディの親オブジェクトを取得
    [SerializeField] GameObject _candys;
     private void Start()
     {
        //配列の初期化
        _fieldCandy = new int[_fieldMaxX, _fieldMaxY];        
        FolderSearch();
        CandyArrySet();
     }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            ///<summary>
            ///配列の中身を出力するテスト
            ///フィールドと同じ形で配列表示させるため、
            ///Yの最大値からconsoleに出力
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
        ///Resourcesフォルダを経由して役割ごとに分けたフォルダへ接続し、
        ///その中のGameObject(Prefab)をそれぞれのGameObject型の配列へ格納する
        ///</summary>
        _normalCandys = Resources.LoadAll<GameObject>("NormalCandys");
        _sideBoms = Resources.LoadAll<GameObject>("SideBoms");
        _VerticalBoms = Resources.LoadAll<GameObject>("VerticalBoms");
        _surroundingsBoms = Resources.LoadAll<GameObject>("SurroundingsBoms");
    }
    private void CandyArrySet()
    {
        ///<summary>
        ///フィールドにある普通のキャンディGameObjectを
        ///foreachで取得し、取得したものをGameObject型の
        ///配列情報をもとに数字を振り、int型の配列に座標と同じ場所に格納する
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


