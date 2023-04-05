using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameContolloer : MonoBehaviour
 {
    //フィールドのキャンディを管理するための配列
    private int[,] _fieldNormalCandySearch = null;
    private int[,] _fieldConnectionSearch = null;
    //生成するボムの情報を管理する配列
    private int[,] _fieldVerticalBoms = null;
    private int[,] _fieldHorizontalBoms = null;
    //フィールドのサイズ
    private int _fieldMaxX = 4;
    private int _fieldMaxY = 4;
    private int _cursorDisplayPosX = 99;
    private int _cursorDisplayPosY = 99;
    //配列の空きを確認するための変数(「0」はキャンディ指定で使用しているため「99」)
    private int _noData = 99;
    //落下処理をした後再度キャンディが消せるか確認するための変数
    private bool _reFallCandyCheck = false;

    private bool _cursorNotAcceptable = false;
    //役割の違うキャンディのPrefabを取得するためのGameObject型の配列
    private GameObject[] _normalCandys;
    private GameObject[] _horizontalBoms;
    private GameObject[] _verticalBoms;
    private GameObject[] _surroundingsBoms;

    [SerializeField]  public  TextMeshProUGUI _text;
    //フィールドにあるキャンディの親オブジェクト
    private GameObject _candys;
    //クリックした時表示させるカーソルの親オブジェクト
    private GameObject _cursors;
    //クリック時に飛ばしたレイと接触したオブジェクトを取得
    private GameObject _clickedCandy;
    //表示しているカーソルオブジェクト
    private void Start()
     {
        //配列の初期化
        _fieldNormalCandySearch = new int[_fieldMaxX + 1, _fieldMaxY + 1];
        _fieldVerticalBoms = new int[_fieldMaxX + 1, _fieldMaxY + 1];
        _fieldHorizontalBoms = new int[_fieldMaxX + 1, _fieldMaxY + 1];
        //フィールドにあるキャンディの親オブジェクトを取得
        _candys = GameObject.Find("CandyObjects");
        //クリックした時表示させるカーソルの親オブジェクトを取得
        _cursors = GameObject.Find("Cursors");
        FolderSearch();
        ArrySet();
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
                    outPutString += _fieldConnectionSearch[x, y];
                }
                print(outPutString);
            }
            print("Field------------------------------------------");
        }
        if (Input.GetMouseButtonDown(0))
        {
            //キャンディが消えているときには操作できないようにします
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

            //フィールドを描画
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
        ///クリックにてレイをとばして、衝突したGameObjectを取得し、
        ///クリック1回目、2回目のX、Yの座標を保持します
        ///どちらもGameObjectを取得出来たら座標を交換し、取得できなかった場合は、
        ///クリックの回数と保持している座標をリセットします
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
        //クリックでのキャンディの移動ができないよう制限します
        _cursorNotAcceptable = true;
        //ボムが生成されない通常消し
        int vanishCandy = 2;
        //ボーダー柄ボムの生成個数
        int stripeBom = 3;

        _reFallCandyCheck = false;
        for (int Y = 0; Y <= _fieldMaxY; Y++)
        {
            for (int X = 0; X <= _fieldMaxY; X++)
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
        //3つ以上そろっているキャンディのGameObjectを消します
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
            ///配列の空きの部分を検索し、
            ///縦方向に連続して空いてる分だけ「fallCount」として
            ///「FallMoveCandy」メソッドに渡す
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
        //配列の同期が行われるようにディレイをかけます
        yield return new WaitForSeconds(0.5f);
        if (_reFallCandyCheck)
        {
            //キャンディを消す処理を行った場合、さらにキャンディが消せないか探索しなおします
            StartCoroutine(ConnectCandyCheck());
        }
        else
        {
            //消せるキャンディがなくなると再度キャンディを操作できるようになります
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
            ///落下対象のキャンディを「fallCount」の分だけ落下させる
            ///_fieldNormalCandySearch[X, count]に_fieldNormalCandySearch[X, count + fallCount]の
            ///GameObjectと配列の中身を移動させます。
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
                ///落下させる対象のキャンディが存在しない場合
                ///「_normalCandys」配列からランダムでキャンディをGameObjectとして取り出し、
                ///「_candys」の子オブジェクトとして生成する
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
        ///「_fieldNormalCandySearch[X, Y]が「_noData」のとき、
        ///各ボム配列の[X, Y]を探索し「_noData」でないとき、
        ///[X, Y]座標に対応したボムのGameObjectを「_candys」の子オブジェクトとして生成します
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
    private void FolderSearch()
    {
        ///<summary>
        ///Resourcesフォルダを経由して役割ごとに分けたフォルダへ接続し、
        ///その中のGameObject(Prefab)をそれぞれのGameObject型の配列へ格納する
        ///</summary>
        _normalCandys = Resources.LoadAll<GameObject>("NormalCandys");
        _horizontalBoms = Resources.LoadAll<GameObject>("HorizontalBoms");
        _verticalBoms = Resources.LoadAll<GameObject>("VerticalBoms");
        _surroundingsBoms = Resources.LoadAll<GameObject>("SurroundingsBoms");
    }
    private void ArrySet()
    {
        ///<summary>
        ///フィールドにある普通のキャンディGameObjectを
        ///foreachで取得し、取得したものをGameObject型の
        ///配列情報をもとに数字を振り、int型の配列に座標と同じ場所に格納する
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
            //ボム生成のための配列の中身を全て「_noData」にします
            _fieldVerticalBoms[candyPosX, candyPosY] = _noData;
            _fieldHorizontalBoms[candyPosX, candyPosY] = _noData;
        }
        //探索用の配列にコピーします
        _fieldConnectionSearch = (int[,])_fieldNormalCandySearch.Clone();
    }
}


