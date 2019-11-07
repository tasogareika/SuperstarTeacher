using System.IO;
using System.Xml;
using UnityEngine;

public class LoadXMLFile : MonoBehaviour {

    public TextAsset questionsXML;
    private TextAsset xmlRawFile;
    public static LoadXMLFile singleton;
    [HideInInspector] public string data, label, question, questionPic, choices;
    [HideInInspector] public int answer;

    void Awake()
    {
        singleton = this;
    }

    void Start()
    {
        //in future: figure out external xml load?
        /*AssetDatabase.ImportAsset(Application.dataPath + "/Resources/_Quetions.xml");
        questionsXML = (TextAsset)Resources.Load("_Questions");*/

        questionsXML = (TextAsset)Resources.Load("_Questions");
        xmlRawFile = questionsXML;
        data = xmlRawFile.text;
    }

    public void updateQuestion()
    {
        parseXMLFile(data);
    }

    public int getNodeNumber()
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(new StringReader(data));
        XmlNode root = xmlDoc.FirstChild;
        return root.ChildNodes.Count;
    }

    void parseXMLFile (string xmlData)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(new StringReader(xmlData));

        string xmlPathPattern = "//document/" + label;
        XmlNodeList myNodeList = xmlDoc.SelectNodes(xmlPathPattern);
        XmlNode firstNode = myNodeList.Item(0);
        XmlNodeList theseNodes = firstNode.ChildNodes;

        for (int i = 0; i < theseNodes.Count; i++)
        {
            XmlNode finalNode = theseNodes[i];
            switch (finalNode.Name)
            {
                case "qns":
                    question = finalNode.InnerXml;
                    break;

                case "picture":
                    questionPic = finalNode.InnerXml;
                    break;

                case "choices":
                    choices = finalNode.InnerXml;
                    break;

                case "answer":
                    answer = int.Parse(finalNode.InnerXml);
                    break;
            }
        }
    }
}
