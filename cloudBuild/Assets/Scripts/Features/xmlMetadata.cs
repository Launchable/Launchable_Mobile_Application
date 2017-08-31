using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;


public class xmlMetadata : MonoBehaviour {

    private string amazonS3Path = "https://s3.us-east-2.amazonaws.com/launchable-mobile-application/";
    private string datasetsDirectory = "Datasets/";
    private string metadataDirectory = "Metadata/";

    string streamPath;
    string savePath;

    // Use this for initialization
    void Start () {
        //local path for phone
        streamPath = Application.persistentDataPath + "/";
#if UNITY_IOS
		    streamPath += "Library/Application Support/";
#endif

        StartCoroutine(downloadXML(amazonS3Path + metadataDirectory + "target_default.xml"));

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.X))
        {
            TestParseXML(savePath);
        }
	}

    IEnumerator downloadXML(string url)
    {

        //get file name
        string[] splitURL = url.Split('/');
        string fileName = splitURL[splitURL.Length - 1];

        //download 
        WWW www = new WWW(url);
        yield return www;

        //save file
        savePath = streamPath + fileName;
        File.WriteAllBytes(savePath, www.bytes);


        //parse XML and download .txt files

    }

    void TestParseXML(string path)
    {

        Debug.Log("Parsing the XML");
        XmlDocument xmlDoc = new XmlDocument();
        if (File.Exists(path))
        {
            xmlDoc.LoadXml(File.ReadAllText(path));
        }

        foreach (XmlElement node in xmlDoc.SelectNodes("TargetData/video"))
        {
            string url_Video = node.GetAttribute("url");
            if(url_Video != "")
            {
                Debug.Log("XML Video URL: " + url_Video);
            }
        }

        foreach (XmlElement node in xmlDoc.SelectNodes("TargetData/content3D"))
        {
            string url_3d = node.GetAttribute("url");
            if (url_3d != "")
            {
                Debug.Log("XML 3D Content URL: " + url_3d);
            }
        }

        foreach (XmlElement node in xmlDoc.SelectNodes("TargetData/contact"))
        {
            string contact_title = node.SelectSingleNode("title").InnerText;
            if (contact_title != "")
            {
                Debug.Log("XML Contact Title: " + contact_title);
            }
            string contact_description = node.SelectSingleNode("description").InnerText;
            if (contact_description != "")
            {
                Debug.Log("XML Contact Description: " + contact_description);
            }
            string contact_phone = node.SelectSingleNode("phone").InnerText;
            if (contact_phone != "")
            {
                Debug.Log("XML Contact Phone: " + contact_phone);
            }
            string contact_email = node.SelectSingleNode("email").InnerText;
            if (contact_email != "")
            {
                Debug.Log("XML Contact Email: " + contact_email);
            }
            string contact_web = node.SelectSingleNode("web").InnerText;
            if (contact_web != "")
            {
                Debug.Log("XML Contact Web: " + contact_web);
            }
        }

    }

    void PrintXMLData ()
    {

    }
}
