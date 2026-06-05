using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RemoteWebControl : MonoBehaviour
{
    public string websiteURL = "https://sites.google.com/view/y498w8596sdsokdhiwueg239/clean-world-controls";

    public string pageStartString = "###*PageStart*###";

    public string pageEndString = "###*PageEnd*###";

    public float maxWaitTimeout = 10f;

    public string enableCheatsString = "Allow Cheats: Yes";


    public bool enableCheats = false;

    // Pulls data from webpage
    void Start()
    {
        StartCoroutine(LoadDataFromWebPage(websiteURL));
    }


    IEnumerator LoadDataFromWebPage (string url)
    {

        float startTime = Time.time;

        WWW webpage = new WWW(url);

        //Waits for web page to load
        while (!webpage.isDone){

            //If it times out, it gives up
            if((Time.time - startTime) > maxWaitTimeout){
                Debug.LogError("Web Controls Page Timed out");
                yield break;
            } else{
                yield return false;
            }

        }

        //Debug.Log("Found Web Page!");


        string webContent = webpage.text;

        if(webContent == ""){
            yield break;
        }
        
        //Finds start and end of web content
        int startOfPage = webContent.IndexOf(pageStartString);
        int endOfPage = webContent.IndexOf(pageEndString);

        //Makes sure that the start and end strings are in fact in the web content
        if(startOfPage == -1 || endOfPage == -1){
            Debug.LogError("Improperly formated web control page. No start string or end string detected");
            yield break;
        }

        //Narrows down string to needed data
        // int startIndex = (startOfPage + (pageStartString.Length) + 1);
        // int endIndex = endOfPage;
        // string webControlInfo = webContent.Substring( startIndex, endIndex - startIndex);

        //Debug.Log(webControlInfo);

        //Enables cheats if the web page contains "Allow Cheats: Yes"
        if(webContent.ToLower().Contains(enableCheatsString.ToLower())){
            enableCheats = true;
        }

        EnactWebCommands();


    }

    public void EnactWebCommands(){
        if(enableCheats){
            creativeModeScript.current.allowCheats = true;
        }
    }


}
