using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class NLoadBytes:MonoBehaviour
{
    public Text text;
    private TextAsset data;
    private byte[] buffer;
    private string sData;    
    private List<List<string>> sFormat = new List<List<string>>();
    private Dictionary<string, string> _textLanguageDictionary = new Dictionary<string, string>();
    private string path = "res";

    private void Start()
    {
        LoadBytes(path);
        StringFormat1(sData);
    }

    private void LoadBytes(string path)
    {
        data = Resources.Load(path) as TextAsset;
        buffer = data.bytes;
        sData = System.Text.Encoding.Default.GetString(buffer);
        Debug.Log(sData);
        text.text = sData;
    }
    //处理data类型bytes文件
    private void StringFormat1(string data)
    {
        string[] strList1 = sData.Split('\n');
        string[] strList2 = strList1[0].Split('@');
        for (int i = 0; i < strList2.Length; i++)
        {
            sFormat.Add(new List<string>());
        }

        for (int i = 0; i < strList1.Length; i++)
        {
            strList2 = strList1[i].Split('@');
            for (int j = 0; j < strList2.Length; j++)
            {
                sFormat[j].Add(strList2[j]);
            }
        }
        //处理最后第一列多一个空元素的问题
        sFormat[0].RemoveAt(sFormat[0].Count - 1);
    }

    //处理chinese类型文件
    private void StringFormat2(string data)
    {
        string[] strList1 = sData.Split('\n');
        string[] strList2 = strList1[0].Split(',');
        for (int i = 0; i < strList2.Length; i++)
        {
            sFormat.Add(new List<string>());
        }

        for (int i = 0; i < strList1.Length; i++)
        {
            strList2 = strList1[i].Split(',');
            for (int j = 0; j < strList2.Length; j++)
            {
                sFormat[j].Add(strList2[j]);
            }
        }
        //处理最后第一列多一个空元素的问题
        sFormat[0].RemoveAt(sFormat[0].Count - 1);
    }

    
}
