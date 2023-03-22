using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class GameContolloer : MonoBehaviour
 {
    //フィールドのキャンディを管理するためのint型の配列
    private int[,] _fieldNormalCandySearch = null;
    //フィールドのサイズ
    private int _fieldMaxX = 4;
    private int _fieldMaxY = 4;
    //配列の空きを確認するための変数(「0」はキャンディ指定で使用しているため「99」)
    private int _noData = 99;
    //役割の違うキャンディのPrefabを取得するためのGameObject型の配列
    private GameObject[] _normalCandys;
    private GameObject[] _horizontalBoms;
    private GameObject[] _VerticalBoms;
    private GameObject[]_surroundingsBoms;
    //フィールドにあるキャンディの親オブジェクトを取得
    [SerializeField, Label("フィールドの親オブジェクト")] GameObject _candys;
     private void Start()
     {
        //配列の初期化
        _fieldNormalCandySearch = new int[_fieldMaxX + 1, _fieldMaxY + 1];
        FolderSearch();
        CandyArrySet();
     }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            //キャンディを消します
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
            ///配列の中身を出力するテスト
            ///フィールドと同じ形で配列表示させるため、
            ///Yの最大値からconsoleに出力
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
        //3つ以上そろっているキャンディのGameObjectを消します
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
        ///配列の空きの部分を検索し、
        ///縦方向に連続して空いてる分だけ「fallCount」として
        ///「FallMoveCandy」メソッドに渡す
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
        ///落下対象のキャンディを「fallCount」の分だけ落下させる
        ///_fieldNormalCandySearch[X, Y]に_fieldNormalCandySearch[X, Y + fallCount]の
        ///GameObjectと配列の中身を移動させます。
        ///もし_fieldNormalCandySearch[X, Y + fallCount]が配列外の場合ランダムで
        ///キャンディを生成します
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
        ///落下させる対象のキャンディが存在しない場合
        ///「_normalCandys」配列からランダムでキャンディをGameObjectとして取り出し、
        ///「_candys」の子オブジェクトとして生成する
        ///</summary>
        int randomCandy = Random.Range(0, _normalCandys.Length);
        GameObject newCandy = Instantiate(_normalCandys[randomCandy]);
        newCandy.transform.position = new Vector2(X, Y);
        newCandy.transform.SetParent(_candys.transform, true);
        _fieldNormalCandySearch[X, Y] = randomCandy;
    }
    private GameObject GetFieldObject(int X, int Y)
    {
        //落下させるキャンディのGameObjectを取得
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
        ///同色のキャンディが縦方向＆横方向に3つ以上並んでいると
        ///消す対象として配列に格納
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
        ///Resourcesフォルダを経由して役割ごとに分けたフォルダへ接続し、
        ///その中のGameObject(Prefab)をそれぞれのGameObject型の配列へ格納する
        ///</summary>
        _normalCandys = Resources.LoadAll<GameObject>("NormalCandys");
        _horizontalBoms = Resources.LoadAll<GameObject>("HorizontalBoms");
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
                    _fieldNormalCandySearch[CandyPosX, CandyPosY] = count;
                    break;
                }
                count++;
            }           
        }
    }
}


