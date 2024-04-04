using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogManager : MonoBehaviour
{   
    public TextAsset textAsset;

    //人物图像
    public SpriteRenderer leftImage;
    public SpriteRenderer rightImage;

    //人物名
    public GameObject leftName;
    public GameObject rightName;

    //人物对话
    public TMP_Text dialogText;

    //图像组
    public List<Sprite> sprites = new List<Sprite>();

    //对话行
    public string[] dialogrows;

    //人物图像字典
    Dictionary<string,Sprite> imageDic = new Dictionary<string, Sprite>();
    
    //索引
    private int ID = 1;

    //继续按钮
    public Button continueButton;
    //选择按钮
    public GameObject choiceButton;

    //按钮列
    public Transform ButtonGrounp;

    IEnumerator Load()
    {   
        ResourceRequest loadAsync0 = Resources.LoadAsync("我",typeof(Sprite));
        ResourceRequest loadAsync1 = Resources.LoadAsync("帕克",typeof(Sprite));
        ResourceRequest loadAsync2 = Resources.LoadAsync("自走炮",typeof(Sprite));

        while(!loadAsync0.isDone || !loadAsync1.isDone || !loadAsync2.isDone)
        {
            yield return null;
        }
        
        sprites.Add(loadAsync0.asset as Sprite);
        sprites.Add(loadAsync1.asset as Sprite);
        sprites.Add(loadAsync2.asset as Sprite);
        imageDic["埃斯"] = sprites[0];
        imageDic["帕克"] = sprites[1];
        imageDic["自走炮"] = sprites[2];
        imageDic["???"] = sprites[0];
        imageDic["未知男人"] = sprites[1];

        ReadText(textAsset);
        ShowDialog();
    }


    // Start is called before the first frame update
    void Start()
    {   
        StartCoroutine(Load());
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {

    }

    public void ReadText(TextAsset _textAsset)
    {
        dialogrows = _textAsset.text.Split('\n');
        // foreach (var row in dialogrows)
        // {
        //     string[] cell = row.Split(separator: ',');
        // }
    }

    public void UpdateText(string _name,string _text,string _position)
    {   
        if(_position == "左")
        {   
            leftName.gameObject.SetActive(true);
            rightName.gameObject.SetActive(false);
            leftName.GetComponentInChildren<TMP_Text>().text = _name;
        }
        if(_position == "右")
        {
            rightName.gameObject.SetActive(true);
            leftName.gameObject.SetActive(false);
            rightName.GetComponentInChildren<TMP_Text>().text = _name;
        }
        dialogText.text = _text;
    }

    public void UpdateImage(string _name,string _position)
    {
        if(_position == "左")
        {
            leftImage.sprite = imageDic[_name];
        }
        if(_position == "右")
        {
            rightImage.sprite = imageDic[_name];
        }

    }

    public void ShowDialog()
    {
        foreach (var row in dialogrows)
        {
            string[] cell = row.Split(',');
            if(cell[0] == "#" && int.Parse(cell[1]) == ID)
            {
                UpdateText(cell[2],cell[4],cell[3]);
                UpdateImage(cell[2],cell[3]);
                ID = int.Parse(cell[5]);
                break;
            }
            if(cell[0] == "&" && int.Parse(cell[1]) == ID)
            {   
                continueButton.gameObject.SetActive(false);
                CreatButton(int.Parse(cell[1]));
            }
            if(cell[0] == "END" && int.Parse(cell[1]) == ID)
            {
                SceneManager.LoadScene(3);
            }
        }
    }

    public void Onclick()
    {
        ShowDialog();
    }

    public void CreatButton(int _id)
    {
        string[] cell = dialogrows[_id].Split(',');
        if(cell[0] == "&")
        {
            GameObject button = Instantiate(choiceButton,ButtonGrounp);
            button.GetComponentInChildren<TMP_Text>().text = cell[4];
            button.GetComponent<Button>().onClick.AddListener
            (
                delegate
                            {
                                ChoiceButton(int.Parse(cell[5]));
                            }
            );
            CreatButton(_id + 1);
        }
    }

    public void ChoiceButton(int _id)
    {
        ID = _id;
        ShowDialog();
        for (int i = 0; i < ButtonGrounp.childCount; i++)
        {
            Destroy(ButtonGrounp.GetChild(i).gameObject);
        }
        continueButton.gameObject.SetActive(true);
    }
}
