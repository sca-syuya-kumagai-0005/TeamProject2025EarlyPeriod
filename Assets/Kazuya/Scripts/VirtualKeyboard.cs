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

    public enum InputMode
    {
        Katakana,
        Hiragana,
    }

    private InputMode currentMode = InputMode.Katakana;

    //レイアウト
    string[] katakanaLayout = new string[]
    {
        "[←]","ー","ワ","ラ","ヤ","マ","ハ","ナ","タ","サ","カ","ア",
        "[˝]","ッ","","リ","","ミ","ヒ","ニ","チ","シ","キ","イ",
        "[〇]","ャ","ヲ","ル","ユ","ム","フ","ヌ","ツ","ス","ク","ウ",
        "[カナ]","ュ","","レ","","メ","ヘ","ネ","テ","セ","ケ","エ",
        "[ヒラ]","ョ","ン","ロ","ヨ","モ","ホ","ノ","ト","ソ","コ","オ",
    
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


    string[] hiraganaLayout = new string[]
    {
        "[←]", "ー","わ","ら","や","ま","は","な","た","さ","か","あ",
        "[˝]", "っ", "",  "り", "","み","ひ","に","ち","し","き","い",
        "[〇]", "ゃ","を","る","ゆ","む","ふ","ぬ","つ","す","く","う",
        "[カナ]","ゅ", "","れ", "","め","へ","ね","て","せ","け","え",
        "[ヒラ]","ょ","ん","ろ","よ","も","ほ","の","と","そ","こ","お",
    };

    Dictionary<string, string> dakutenhiraMap = new Dictionary<string, string>()
    {
    {"か", "が"}, {"き", "ぎ"}, {"く", "ぐ"}, {"け", "げ"}, {"こ", "ご"},
    {"さ", "ざ"}, {"し", "じ"}, {"す", "ず"}, {"せ", "ぜ"}, {"そ", "ぞ"},
    {"た", "だ"}, {"ち", "ぢ"}, {"つ", "づ"}, {"て", "で"}, {"と", "ど"},
    {"は", "ば"}, {"ひ", "び"}, {"ふ", "ぶ"}, {"へ", "べ"}, {"ほ", "ぼ"},
    {"ぱ", "ば"}, {"ぴ", "び"}, {"ぷ", "ぶ"}, {"ぺ", "べ"}, {"ぽ", "ぼ"}
    };

    Dictionary<string, string> reverseDakutenhiraMap = new()
    {
    {"が", "か"}, {"ぎ", "き"}, {"ぐ", "く"}, {"げ", "け"}, {"ご", "こ"},
    {"ざ", "さ"}, {"じ", "し"}, {"ず", "す"}, {"ぜ", "せ"}, {"ぞ", "そ"},
    {"だ", "た"}, {"ぢ", "ち"}, {"づ", "つ"}, {"で", "て"}, {"ど", "と"},
    {"ば", "は"}, {"び", "ひ"}, {"ぶ", "ふ"}, {"べ", "へ"}, {"ぼ", "ほ"}
    };
    
    Dictionary<string, string> handakutenhiraMap = new Dictionary<string, string>() {
    {"は", "ぱ"}, {"ひ", "ぴ"}, {"ふ", "ぷ"}, {"へ", "ぺ"}, {"ほ", "ぽ"},
    {"ば", "ぱ"}, {"び", "ぴ"}, {"ぶ", "ぷ"}, {"べ", "ぺ"}, {"ぼ", "ぽ"}
    };

    Dictionary<string, string> reverseHandakutenhiraMap = new()
    {
    {"ぱ", "は"}, {"ぴ", "ひ"}, {"ぷ", "ふ"}, {"ぺ", "へ"}, {"ぽ", "ほ"}
    };

    void Start()
    {
        foreach(string Key in katakanaLayout)
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
                        case "カナ":
                            btn.GetComponent<Button>().onClick.AddListener(() =>SwitchMode(InputMode.Katakana));
                            break ;
                        case "ヒラ":
                            btn.GetComponent<Button>().onClick.AddListener(() => SwitchMode(InputMode.Hiragana));
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
            alldeteButton.onClick.AddListener(AllDelete);
            submitButton.onClick.AddListener(SubmitName);
        }

        UpdateDisplay(); // 最初に表示を更新
    }

    public void SwitchMode(InputMode mode)
    {
        if(mode == currentMode) return;
        currentMode = mode;
        GenerateKeyboard();
    }

    void GenerateKeyboard()
    {
        //既存のボタン削除
        foreach(Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }

        //レイアウト選択
        string[] layout = currentMode 
        switch
        {
            InputMode.Katakana => katakanaLayout,
            InputMode.Hiragana => hiraganaLayout,
        };

        foreach (string Key  in layout)
        {
            GameObject btn;
            if (string.IsNullOrWhiteSpace(Key))
            {
                btn = Instantiate(emptoPrefab, buttonContainer);
            }
            else
            {
                btn = Instantiate(buttonPrefab, buttonContainer);
                btn.GetComponentInChildren<Text>().text = Key;

                string captured = Key;

                //特殊キーの設定
                if (captured.StartsWith("[") && captured.EndsWith("]"))
                {
                    string command = Key.Trim('[', ']');
                    switch (command)
                    {
                        case "˝":
                            Debug.Log("濁点キー");
                            btn.GetComponent<Button>().onClick.AddListener(AddDakuten);
                            break;
                        case "〇":
                            btn.GetComponent<Button>().onClick.AddListener(AddHandakuten);
                            break;
                        case "←":
                            btn.GetComponent<Button>().onClick.AddListener(DeleteCharacter);
                            break;
                        case "決定":
                            btn.GetComponent<Button>().onClick.AddListener(SubmitName);
                            break;
                        case "取消":
                            btn.GetComponent<Button>().onClick.AddListener(AllDelete);
                            break;
                        case "カナ":
                            btn.GetComponent<Button>().onClick.AddListener(() => SwitchMode(InputMode.Katakana));
                            break;
                        case "ヒラ":
                            btn.GetComponent<Button>().onClick.AddListener(() => SwitchMode(InputMode.Hiragana));
                            break;
                    }
                }
                else
                {
                    //ボタンにクリックイベントの登録
                    btn.GetComponent<Button>().onClick.AddListener(() => OnClickCharacter(captured));
                }
            }

            var rt = btn.GetComponent<RectTransform>();
            rt.localScale = Vector3.one;
            rt.anchoredPosition3D = Vector3.zero;
        }
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
        if(currentMode == InputMode.Katakana)
        {
            if (dakutenMap.TryGetValue(last, out string converted))
            {
                currentName = currentName.Substring(0, currentName.Length - 1) + converted;
            }
            else if (reverseDakutenMap.TryGetValue(last, out string original))
            {
                currentName = currentName.Substring(0, currentName.Length - 1) + original;
            }
        }
        else if(currentMode == InputMode.Hiragana)
        {
            if(dakutenhiraMap.TryGetValue(last, out string converted))
            {
                currentName = currentName.Substring(0,currentName.Length - 1) + converted;
            }
            else if(reverseDakutenhiraMap.TryGetValue(last,out string original))
            {
                currentName = currentName.Substring(0,currentName.Length-1) + original;
            }
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
        if(currentMode == InputMode.Katakana)
        {
            if (handakutenMap.TryGetValue(last, out string converted))
            {
                currentName = currentName.Substring(0, currentName.Length - 1) + converted;
            }
            else if (reverseHandakutenMap.TryGetValue(last, out string original))
            {
                currentName = currentName.Substring(0, currentName.Length - 1) + original;
            }
        }
        else if(currentMode == InputMode.Hiragana)
        {
            if (handakutenhiraMap.TryGetValue(last, out string converted))
            {
                currentName = currentName.Substring(0, currentName.Length - 1) + converted;
            }
            else if (reverseHandakutenhiraMap.TryGetValue(last, out string original))
            {
                currentName = currentName.Substring(0, currentName.Length - 1) + original;
            }
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
