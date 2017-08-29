using UnityEngine;
using System.Collections;
using System.IO;
using Vuforia;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Xml;
using System;

public class DynamicDataSetLoader : MonoBehaviour
{
    // specify these in Unity Inspector
    public GameObject augmentationObject = null;  // you can use teapot or other object
    public string dataSetName = "Launchable_Mobile_Application";  //  Assets/StreamingAssets/QCAR/DataSetName
    public string currentTrackable = "none";
    public Text debugText;
    public string phoneContact;
    public string emailContact;
    public string webContact;
    WWW wwwHelper;
    public float progressNum;
    [SerializeField]
    Slider datSlider;

    //private string amazonS3Path = "https://s3-eu-west-1.amazonaws.com/launchables/metadata/main/";
    private string amazonS3Path = "https://s3.us-east-2.amazonaws.com/launchable-mobile-application/";
    private string datasetsDirectory = "Datasets/";
    private string metadataDirectory = "Metadata/";

    string streamPath;
    string tempStreamPath;

    // every time the app starts it'll update the database. later on we'll do this every once in a while
    void Start()
    {
        datSlider.gameObject.SetActive(false);

        streamPath = Application.persistentDataPath + "/";
#if UNITY_IOS
		streamPath += "Library/Application Support/";
#endif
        updateDatabase();
    }

    void updateDatabase()
    {
        downloadFiles();
    }

    //go through each URL and download. These are hardcoded now but these should be grabbed from the dashboard
    void downloadFiles()
    {

        // Create root directory
        if (!Directory.Exists(streamPath))
        {
            Directory.CreateDirectory(streamPath);
        }

        // Create ../temp directory
        tempStreamPath = streamPath + "temp/";
        if (!Directory.Exists(tempStreamPath))
        {
            Directory.CreateDirectory(tempStreamPath);
        }

        //download helper

        StartCoroutine(downloadHelper());
    }

    IEnumerator wwwHelperLoad()
    {
        wwwHelper = new WWW(amazonS3Path + datasetsDirectory + "helper.txt");
        yield return wwwHelper;
    }

    IEnumerator downloadHelper()
    {
        Debug.Log("Pre helper download @ time: " + Time.time);
        //download helper file
        //StartCoroutine(wwwHelperLoad());

        wwwHelper = new WWW(amazonS3Path + datasetsDirectory + "helper.txt");
        yield return wwwHelper;

        Debug.Log("Post www helper @ time: " + Time.time);

        //yield return null;

        //save helper file to temporary folder
        string tempHelperSavePath = tempStreamPath + "helper.txt";
        Debug.Log("Before error");
        File.WriteAllBytes(tempHelperSavePath, wwwHelper.bytes);
        Debug.Log("After error");

        /* Check if helper.txt exists locally
         * TRUE - Compare the local helper.txt with the cloud helper.txt
         * FALSE - Run update procedure
         */
        if (File.Exists(streamPath + "helper.txt"))
        {

            // *** DEVELOPER UPDATE NEEDED: Currently this is reading the entire file and comparing. Change to be specific to "timestamp"

            string helperSavePath = streamPath + "helper.txt";
            StreamReader tempHelperFile = new StreamReader(tempHelperSavePath);
            string tempHelperLine = tempHelperFile.ReadLine();
            StreamReader helperFile = new StreamReader(helperSavePath);
            string helperLine = helperFile.ReadLine();

            tempHelperFile.Close();
            helperFile.Close();

            if (tempHelperLine == helperLine)
            {
                //Files are timestamped the same, no update needed, proceed to loading the database
                Debug.Log("Files are timestamped the same, no update needed, proceed to loading the database @ time: " + Time.time);
                debugText.text += "Welcome Back. No update needed." + "\n";
                VuforiaARController.Instance.RegisterVuforiaStartedCallback(LoadDataSet);
            }
            else
            {

                //Update needed, save helper file to root, delete temp, and proceed to downloading the XML
                Debug.Log("Update needed, save helper file to root, delete temp, and proceed to downloading the XML");
                debugText.text += "Welcome Back. Update needed, updating database" + "\n";
                File.WriteAllBytes(helperSavePath, wwwHelper.bytes);
                File.Delete(tempHelperSavePath);
                StartCoroutine(downloadXML(amazonS3Path + datasetsDirectory + "Launchable_Mobile_Application.xml"));
            }
        }
        else
        {
            //No helper file found. Save helper.txt and proceed to downloading the XML
            Debug.Log("No helper file found. Save helper.txt and proceed to downloading the XML");
            debugText.text += "No helper.txt, updating database" + "\n";
            string helperSavePath = streamPath + "helper.txt";
            File.WriteAllBytes(helperSavePath, wwwHelper.bytes);
            StartCoroutine(downloadXML(amazonS3Path + datasetsDirectory + "Launchable_Mobile_Application.xml"));
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
        string savePath = streamPath + fileName;
        File.WriteAllBytes(savePath, www.bytes);


        //parse XML and download .txt files
        ParseXML(savePath);
        datSlider.gameObject.SetActive(true);
        StartCoroutine(downloadDat(amazonS3Path + datasetsDirectory + "Launchable_Mobile_Application.dat"));
    }

    IEnumerator downloadDat(string url)
    {
        //WWW download = WWW.LoadFromCacheOrDownload(url, 1);
        WWW download = new WWW(url);
        while (!download.isDone)
        {
            progressNum = download.progress * 100;
            datSlider.value = progressNum;
            yield return null;
        }
        if (!string.IsNullOrEmpty(download.error))
        {
            // error!
        }
        else
        {
            // success!
            StartCoroutine(removeProgressBar());
        }


        //get file name
        string[] splitURL = url.Split('/');
        string fileName = splitURL[splitURL.Length - 1];

        //save file
        string savePath = streamPath + fileName;
        File.WriteAllBytes(savePath, download.bytes);

        VuforiaARController.Instance.RegisterVuforiaStartedCallback(LoadDataSet);

    }

    IEnumerator removeProgressBar()
    {
        yield return new WaitForSeconds(0.5f);
        datSlider.gameObject.SetActive(false);
    }

    void ParseXML(string path)
    {

        StartCoroutine(downloadFile(amazonS3Path + metadataDirectory + "target_default.txt"));

        Debug.Log("Parsing the XML");
        debugText.text += "Parsing the XML" + "\n";
        XmlDocument xmlDoc = new XmlDocument();
        if (File.Exists(path))
        {
            xmlDoc.LoadXml(File.ReadAllText(path));
        }

        foreach (XmlElement node in xmlDoc.SelectNodes("QCARConfig/Tracking/ImageTarget"))
        {
            string tempFileName = node.GetAttribute("name");
            tempFileName += ".txt";
            string tempUrl = amazonS3Path + metadataDirectory + tempFileName;
            StartCoroutine(downloadFile(tempUrl));
        }

    }

    IEnumerator downloadFile(string url)
    {
        //get file name
        string[] splitURL = url.Split('/');
        string fileName = splitURL[splitURL.Length - 1];

        //download
        if (fileName.Contains(".dat"))
        {
            Debug.Log("About to download dataset @ " + Time.time + "\n");
        }
        WWW www = new WWW(url);
        if (fileName.Contains(".dat"))
        {
            Debug.Log("Progress: " + www.progress);
        }
        yield return www;
        if (fileName.Contains(".dat"))
            Debug.Log("Just downloaded dataset @ " + Time.time + "\n");



        //save file
        string savePath = streamPath + fileName;
        File.WriteAllBytes(savePath, www.bytes);
        //debugText.text += "saving " + fileName + "\n";

        CleanUpFile(savePath, fileName);

        //wait until database is updated to load into app
        if (fileName == "Launchable_Mobile_Application.dat")
        {

            Debug.Log("Found .dat");
            //debugText.text += "loading dataset \n";
            VuforiaARController.Instance.RegisterVuforiaStartedCallback(LoadDataSet);
        }

    }

    void CleanUpFile(string filePath, string fileName)
    {
        if (filePath.Contains(".txt"))
        {
            StreamReader tempFile = new StreamReader(filePath);
            string tempLine = tempFile.ReadLine();
            if (tempLine.Contains("<?xml"))
            {
                tempFile.Close();
                File.Delete(filePath);
                File.Copy(streamPath + "target_default.txt", streamPath + fileName);
            }
            tempFile.Close();
        }
    }

    //uses vuforia library to load a dataset with all the image targets then creates image target objects that are AR trackable
    void LoadDataSet()
    {
        //object tracker is what finds targets on the camera feed. 
        ObjectTracker objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
        DataSet dataSet = objectTracker.CreateDataSet();

        string dataSetPath = streamPath + dataSetName + ".xml";
        //debugText.text += dataSetPath + "\n";

        //load dataset
        if (dataSet.Load(dataSetPath, VuforiaUnity.StorageType.STORAGE_ABSOLUTE))
        {

            objectTracker.Stop();  // stop tracker so that we can add new dataset

            if (!objectTracker.ActivateDataSet(dataSet))
            {
                // Note: ImageTracker cannot have more than 1000 total targets activated
                Debug.Log("<color=yellow>Failed to Activate DataSet: " + dataSetName + "</color>");
            }

            if (!objectTracker.Start())
            {
                Debug.Log("<color=yellow>Tracker Failed to Start.</color>");
            }
            float lastTime = Time.time;
            debugText.text += "Loading dataset @ time: " + lastTime + "\n";

            //create image targets and add vuforia components
            int counter = 0;
            IEnumerable<TrackableBehaviour> tbs = TrackerManager.Instance.GetStateManager().GetTrackableBehaviours();

            foreach (TrackableBehaviour tb in tbs)
            {
                if (tb.name == "New Game Object")
                {

                    // change generic name to include trackable name
                    tb.gameObject.name = ++counter + ":DynamicImageTarget-" + tb.TrackableName;
                    lastTime = Time.time;
                    // add additional script components for trackable
                    tb.gameObject.AddComponent<DefaultTrackableEventHandler>();
                    tb.gameObject.AddComponent<TurnOffBehaviour>();

                    // attaches all the gameobjects for functionality like video and contact card
                    if (augmentationObject != null)
                    {
                        // instantiate augmentation object and parent to trackable
                        GameObject augmentation = (GameObject)GameObject.Instantiate(augmentationObject);
                        augmentation.transform.SetParent(tb.gameObject.transform, true);
                        augmentation.gameObject.SetActive(true);
                    }
                    else
                    {
                        Debug.Log("<color=yellow>Warning: No augmentation object specified for: " + tb.TrackableName + "</color>");
                    }
                    lastTime = Time.time;
                }
            }
        }
        else
        {
            //debugText.text += "<color=red>Failed to load dataset: '" + dataSetName + "'</color>";
            Debug.LogError("<color=red>Failed to load dataset: '" + dataSetName + "'</color>");
        }
    }
}