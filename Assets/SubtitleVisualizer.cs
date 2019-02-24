using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using System.IO;
using System.Linq;

[System.Serializable]
public struct SingleLine{
    [FoldoutGroup("Split/$Name", false)]
    public int StartTime { set; get; }

    [FoldoutGroup("Split/$Name", false)]
    public string Content { set; get; }

    [FoldoutGroup("Split/$Name", false)]
    public string PlayerName { set; get; }
}

public class SubtitleVisualizer : MonoBehaviour
{
    private int _ptr = 0;

    private bool _play = false;

    public AudioSource Sound;

    public CharactorAutomation charactorAutomation;

    [Title("字幕效果")]

    [FilePath]
    public string FilePath = "";

    public Text TargetText;
    public Text TimerText;

    [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "Content")]
    public List<SingleLine> Lines;

    [Button("转换", ButtonSizes.Large)]
    public void Convert() {
        var reader = new StreamReader(FilePath);
        Lines = new List<SingleLine>();
        //reader.ReadLine();
        while (!reader.EndOfStream)
        {
            string str = reader.ReadLine();
            string timehead= str.Substring(1,10);
            str = str.Substring(11);
            string plName= str.Split(']')[0].Substring(1);
            str =str.Split(']')[1];
            var line = new SingleLine {Content = str, StartTime = GetTimeStampFromString(timehead), PlayerName=plName };
            Lines.Add(line);
        }
        reader.Close();
    }

    [Button("播放", ButtonSizes.Large)]
    public void Play()
    {
        _play = true;
        if (Sound != null)
        {
            Sound.time = 0.0f;
            Sound.Play();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        Convert();
        _ptr = 0;
        TargetText.text = Lines[0].Content;
    }

    int GetTimeStampFromString(string str)
    {
        return int.Parse(str.Substring(0,2))*60000+ int.Parse(str.Substring(3, 2)) * 1000 + int.Parse(str.Substring(6, 3));
    }

    string AddReturn(string str)
    {
        string res = "";
        while (str.Length>20)
        {
            res += str.Substring(0, 20) + "\n";
            str = str.Substring(20);
        }
        res += str;
        return res;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!_play)return;
        if (Sound.time*1000.0 >= Lines[_ptr+1].StartTime)
        {
            _ptr++;
            TargetText.text = AddReturn(Lines[_ptr].Content);
            charactorAutomation.NextChar(Lines[_ptr].PlayerName);
        }
    }
}
