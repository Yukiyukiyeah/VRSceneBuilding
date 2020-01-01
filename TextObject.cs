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
    private GameObject wordObject;
    private GameObject letterTransform;

    void Awake()
    {
        //LetterPrefab();
        WordPrefab();
    }
    private void LetterPrefab()
    {
        string localPath = "Assets/Alphabets" + gameObject.name + ".prefab";

        // Loop through every GameObject in the array above
        foreach (GameObject gameObject in objectArray)
        {
            // Make sure the file name is unique, in case an existing Prefab has the same name.
            localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

            // Create the new Prefab.
            PrefabUtility.SaveAsPrefabAssetAndConnect(gameObject, localPath, InteractionMode.UserAction);
        }
        
    }

    private void WordPrefab()
    {
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
        foreach (Sentence sentence in sentences)
        {
            foreach (string word in sentence.words)
            {
                wordObject = new GameObject(string.Format("{0}", word));
                //Instantiate(wordObject);
                
                foreach (char letter in word)
                {
                    //Debug.Log(string.Format("AlphatbetsPrefabs/{0}.prefab", letter));
                    try
                    {
                        Object prefab = AssetDatabase.LoadAssetAtPath(string.Format("Assets/AlphabetsPrefabs/{0}.prefab", letter), typeof(GameObject));
                        letterTransform = Instantiate(prefab, new Vector3(2.0F, 0, 0), Quaternion.identity) as GameObject;
                        //Debug.Log(letterTransform);
                        Transform parent = wordObject.transform;
                        //Debug.Log("mama"+ parent);
                        letterTransform.transform.SetParent(parent);
                        Debug.Log("my momo:"+letterTransform.transform.parent);
                    }
                    catch(FileNotFoundException e)
                    {

                    }
                    
                }           
               
                
            }
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