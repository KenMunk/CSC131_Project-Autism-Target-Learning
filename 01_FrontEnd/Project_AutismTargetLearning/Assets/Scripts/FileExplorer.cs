using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

//Apply to button and image obj

public class FileExplorer : MonoBehaviour
{
    string path;
    public RawImage rawImage;

    public void OpenFileExplorer()
    {
        //path = EditorUtility.OpenFilePanel("images .png", "", "png");
        //StartCoroutine(GetTexture());
    }


    IEnumerator GetTexture()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("file:///" + path);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            rawImage.texture = myTexture;
        }
    }    
}
