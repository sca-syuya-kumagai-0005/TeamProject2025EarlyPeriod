using UnityEngine;
using UnityEngine.UI;
using System;

public class VirtualKeyboard : MonoBehaviour
{
    [Header("仮想キーボード設定")]
    public GameObject buttonPrefab;     //文字ボタンのプレハブ
    public Transform buttonContainer;   //ボタンの並べる親オブジェクト

    [Header("UIコンポーネント")]
    public Text nameText;               //入力中の名前のテキスト
    public Button deleteButton;          //1文字削除ボタン
    public Button submitButton;         //名前の確定ボタン

    public Action<string> OnNameSubmitted;  //名前が入力された合図

    private string currentName = "";        //入力中の名前
    private const int maxLength = 7;        //最大の入力数

    string[] katakanaChars = new string[]
    {
        "ア","イ","ウ","エ","オ",
        "カ","キ","ク","ケ","コ",
        "タ","チ","ツ","テ","ト",
        "ナ","ニ","ヌ","ネ","ノ",
        "ハ","ヒ","フ","ヘ","ホ",
        "マ","ミ","ム","メ","モ",
        "ヤ",　　　"ユ",　　"ヨ",
        "ラ","リ","ル","レ","ロ",
        "リ",　　　"ヲ",　　"ン",
        "ー","ッ","ャ","ュ","ョ",
  
    };


    void Start()
    {
        foreach(string kana in katakanaChars)
        {
            GameObject btn = Instantiate(buttonPrefab, buttonContainer);
            btn.GetComponentInChildren<Text>().text = kana;

            //ボタンにクリックイベントの登録
            btn.GetComponent<Button>().onClick.AddListener(() => OnClickCharacter(kana));
        }
        // 削除ボタンと送信ボタンの設定
        deleteButton.onClick.AddListener(DeleteCharacter);
        submitButton.onClick.AddListener(SubmitName);

        UpdateDisplay(); // 最初に表示を更新
    }
    //文字の追加処理
    void OnClickCharacter(string character)
    {
        if (currentName.Length < maxLength)
        {
            currentName+= character;
            UpdateDisplay();
        }
    }
    //1文字削除
    void DeleteCharacter()
    {
        if (currentName.Length > 0)
        {
            currentName = currentName.Substring(0, currentName.Length - 1);
            UpdateDisplay();
        }
    }
    //名前の確定
    void SubmitName()
    {
        if(!string.IsNullOrEmpty(currentName))
        {
            OnNameSubmitted?.Invoke(currentName);//コールバックでScoreRankingに渡す
        }
    }
    /// <summary>
    /// 入力中の名前の表示の更新
    /// </summary>
    void UpdateDisplay()
    {
        nameText.text = currentName;
    }
}
