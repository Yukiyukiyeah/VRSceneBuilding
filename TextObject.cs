using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class TextObject : MonoBehaviour
{
    public string Filename;

    private string FilePath;
    private string jsonString;

    public int size { get; set; }
    public List<string> nouns { get; set; }
    public List<string> noun_phrases { get; set; }
    public List<Sentence> sentences { get; set; }
    public float polarity { get; set; }

    private Object[] objectArray;
    private GameObject sentenceObject;
    private GameObject wordObject;
    private GameObject letterTransform;

    void Awake()
    {
        WordPrefab();
    }
    

    private void WordPrefab()
    {
        //parse the json file
        FilePath = "../Text2VR/data/test_20191229.json";
        jsonString = File.ReadAllText(FilePath);
        JObject userText = JObject.Parse(jsonString);
        JToken jText = userText;
        //Debug.Log(jText["sentences"]);
        size = (int)jText["size"];
        nouns = jText["noun"].ToObject<List<string>>();
        noun_phrases = jText["noun_phrases"].ToObject<List<string>>();
        sentences = jText["sentences"].ToObject<List<Sentence>>();
        polarity = (int)jText["polarity"];

        //create text objects
        foreach (Sentence sentence in sentences)
        {
            sentenceObject = new GameObject(string.Format("sentence{0}", sentence.id));
            float wordLength = 0;
            foreach (string word in sentence.words)
            {
                wordObject = new GameObject(string.Format("{0}", word));
                //Instantiate(wordObject);
                int count = 0;
                foreach (char letter in word)
                {
                    //Debug.Log(string.Format("AlphatbetsPrefabs/{0}.prefab", letter));
                    try
                    {
                        Object prefab;
                        if ((letter >=65 &&letter<=90)||letter =='\''||letter == '-')
                        {
                            prefab = AssetDatabase.LoadAssetAtPath(string.Format("Assets/AlphabetsPrefabs/{0}.prefab", letter), typeof(GameObject));
                        }
                        else
                        {
                            prefab = AssetDatabase.LoadAssetAtPath(string.Format("Assets/AlphabetsPrefabs/l{0}.prefab", letter), typeof(GameObject));
                        }                        
                        letterTransform = Instantiate(prefab, new Vector3(count*0.15f, 0, 0), Quaternion.Euler(0,180,0)) as GameObject;
                        //Debug.Log(letterTransform);
                        Transform parent = wordObject.transform;
                        //Debug.Log("mama"+ parent);
                        letterTransform.transform.SetParent(parent);
                        //Debug.Log("my momo:"+letterTransform.transform.parent);
                        count++;
                    }
                    catch(FileNotFoundException e)
                    {

                    }                                        
                }
                
                wordObject.transform.position = new Vector3(wordLength, 0, 0);
                wordObject.transform.SetParent(sentenceObject.transform);
                wordLength += count * 0.15f + 0.2f;

            }
            sentenceObject.transform.position += new Vector3(0, -sentence.id * 0.2f, 0);
        }
    }

    public class Sentence
    {
        public int id;
        public string content;
        public float polarity;
        public List<string> words;
    }
         
}
