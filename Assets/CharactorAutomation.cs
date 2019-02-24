using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEngine;
using UnityEngine.UI;
using ObjectFieldAlignment = Sirenix.OdinInspector.ObjectFieldAlignment;

public class CharactorAutomation : SerializedMonoBehaviour
{
    public Text charName;
    public Image charImage;

    public bool switcher = false;

    [System.Serializable]
    public struct CharData
    {
        [FoldoutGroup("Split/$Name", false)]
        public int Index;

        [FoldoutGroup("Split/$Name", false)]
        public string Name;

        [FoldoutGroup("Split/$Name", false)]
        public Sprite CharSprite;

        [FoldoutGroup("Split/$Name", false)]
        public string DisplayName;
    }

    public CharData[] chars;

    public Dictionary<string,int> charIndex;


    // Start is called before the first frame update
    void Start()
    {
        charName.text = chars[0].Name;
        charImage.sprite = chars[0].CharSprite;
    }

    [Button("加载人物", ButtonSizes.Gigantic)]
    public void Convert()
    {
        charIndex=new Dictionary<string, int>();
        for (int i = 0; i < chars.Length; i++)
        {
            charIndex.Add(chars[i].Name,i);
        }
    }

    public void NextChar(string name="")
    {
        var index = charIndex[name];
        charName.text = chars[index].DisplayName;
        charImage.sprite = chars[index].CharSprite;
    }
}
