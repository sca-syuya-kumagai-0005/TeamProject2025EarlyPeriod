using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class VirtualKeyboard : MonoBehaviour
{
    [Header("仮想キーボード設定")]
    public GameObject buttonPrefab;     //文字ボタンのプレハブ
    public Transform buttonContainer;   //ボタンの並べる親オブジェクト
    public GameObject emptoPrefab;      //空白ボタン

    [Header("UIコンポーネント")]
    public Text nameText;               //入力中の名前のテキスト
    public Button dakutenButton;    //濁点ボタン
    public Button handakutenButton; //半濁点ボタン
    public Button deleteButton;          //1文字削除ボタン
    public Button alldeteButton;        //全文字削除ボタン
    public Button submitButton;         //名前の確定ボタン

    public Action<string> OnNameSubmitted;  //名前が入力された合図

    private string currentName = "";        //入力中の名前
    [Header("文字数制限")]
    [SerializeField]private int maxLength = 7;        //最大の入力数

    //レイアウト
    string[] katakanaChars = new string[]
    {
        "[←]","ー","ワ","ラ","ヤ","マ","ハ","ナ","タ","サ","カ","ア",
        "[˝]","ッ","","リ","","ミ","ヒ","ニ","チ","シ","キ","イ",
        "[〇]","ャ","ヲ","ル","ユ","ム","フ","ヌ","ツ","ス","ク","ウ",
        "[決定]","ュ","","レ","","メ","ヘ","ネ","テ","セ","ケ","エ",
        "[取消]","ョ","ン","ロ","ヨ","モ","ホ","ノ","ト","ソ","コ","オ",
    
    };

    Dictionary<string, string> dakutenMap = new Dictionary<string, string>() {
    {"カ", "ガ"}, {"キ", "ギ"}, {"ク", "グ"}, {"ケ", "ゲ"}, {"コ", "ゴ"},
    {"サ", "ザ"}, {"シ", "ジ"}, {"ス", "ズ"}, {"セ", "ゼ"}, {"ソ", "ゾ"},
    {"タ", "ダ"}, {"チ", "ヂ"}, {"ツ", "ヅ"}, {"テ", "デ"}, {"ト", "ド"},
    {"ハ", "バ"}, {"ヒ", "ビ"}, {"フ", "ブ"}, {"ヘ", "ベ"}, {"ホ", "ボ"},
    {"パ", "バ"}, {"ピ", "ビ"}, {"プ", "ブ"}, {"ペ", "ベ"}, {"ポ", "ボ"}
    };

    Dictionary<string, string> reverseDakutenMap = new()
{
    {"ガ", "カ"}, {"ギ", "キ"}, {"グ", "ク"}, {"ゲ", "ケ"}, {"ゴ", "コ"},
    {"ザ", "サ"}, {"ジ", "シ"}, {"ズ", "ス"}, {"ゼ", "セ"}, {"ゾ", "ソ"},
    {"ダ", "タ"}, {"ヂ", "チ"}, {"ヅ", "ツ"}, {"デ", "テ"}, {"ド", "ト"},
    {"バ", "ハ"}, {"ビ", "ヒ"}, {"ブ", "フ"}, {"ベ", "ヘ"}, {"ボ", "ホ"}
};

    Dictionary<string, string> handakutenMap = new Dictionary<string, string>() {
    {"ハ", "パ"}, {"ヒ", "ピ"}, {"フ", "プ"}, {"ヘ", "ペ"}, {"ホ", "ポ"},
    {"バ", "パ"}, {"ビ", "ピ"}, {"ブ", "プ"}, {"ベ", "ペ"}, {"ボ", "ポ"}
    };

    Dictionary<string, string> reverseHandakutenMap = new()
    {
    {"パ", "ハ"}, {"ピ", "ヒ"}, {"プ", "フ"}, {"ペ", "ヘ"}, {"ポ", "ホ"}
    };

    void Start()
    {
        foreach(string Key in katakanaChars)
        {
            GameObject btn;

            if (string.IsNullOrWhiteSpace(Key))
            {
                btn = Instantiate(emptoPrefab,buttonContainer);
            }
            else
            {
                btn = Instantiate(buttonPrefab, buttonContainer);
               var buttontext = btn.GetComponentInChildren<Text>();
               buttontext.text = Key;;

                //特殊キーの設定
                if (Key.StartsWith("[") && Key.EndsWith("]"))
                {
                    string command = Key.Trim('[',']');
                    switch (command)
                    {
                        case "˝":
                            Debug.Log("濁点キー");
                            btn.GetComponent<Button>().onClick.AddListener(AddDakuten);
                            break;
                        case "〇":
                            btn.GetComponent<Button>().onClick.AddListener(AddHandakuten);
                            break ;
                        case "←":
                            btn.GetComponent<Button>().onClick.AddListener(DeleteCharacter);
                            break;
                        case "決定":
                            btn.GetComponent<Button>().onClick.AddListener(SubmitName);
                            break ;
                        case "取消":
                            btn.GetComponent<Button>().onClick.AddListener(AllDelete);
                            break ;
                    }
                }
                else
                {
                    //ボタンにクリックイベントの登録
                    string captured = Key;
                    btn.GetComponent<Button>().onClick.AddListener(() => OnClickCharacter(captured));
                }


            }
        }
        // 削除ボタンと送信ボタンの設定
        deleteButton.onClick.AddListener(DeleteCharacter);
        submitButton.onClick.AddListener(SubmitName);
        dakutenButton.onClick.AddListener(AddDakuten);
        handakutenButton.onClick.AddListener(AddHandakuten);
        alldeteButton.onClick.AddListener(AllDelete);

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

    void AllDelete()
    {
        if (currentName.Length > 0)
        {
            currentName = "";
            UpdateDisplay();
        }
    }
    /// <summary>
    /// 文字の濁点化
    /// </summary>
    void AddDakuten()
    {
        if(currentName.Length == 0)  return;
        string last = currentName[^1].ToString();
        if(dakutenMap.TryGetValue(last, out string converted))
        {
            currentName = currentName.Substring(0,currentName.Length-1)+converted;
        }
        else if(reverseDakutenMap.TryGetValue(last, out string original))
        {
            currentName = currentName.Substring(0, currentName.Length - 1) + original;
        }
        UpdateDisplay();
    }
    /// <summary>
    /// 文字の半濁点化
    /// </summary>
    void AddHandakuten()
    {
        if (currentName.Length == 0) return;
        string last = currentName[^1].ToString();
        if (handakutenMap.TryGetValue(last, out string converted))
        {
            currentName = currentName.Substring(0, currentName.Length - 1) + converted;
        }
        else if (reverseHandakutenMap.TryGetValue(last, out string original))
        {
            currentName = currentName.Substring(0, currentName.Length - 1) + original;
        }
        UpdateDisplay();
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
