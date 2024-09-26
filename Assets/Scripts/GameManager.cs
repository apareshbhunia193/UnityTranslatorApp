using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Fluent.LibreTranslate;
using System.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine.Networking;
using Flurl;

public class GameManager : MonoBehaviour
{
    [SerializeField] Text hindiTextFromInput;
    [SerializeField] InputField inputField;
    [SerializeField] TMP_Text englishTranslationText;

    string inputHindiText, outputEnglishText;

    private string apiUrl = "https://api.mymemory.translated.net/get";

    // Start is called before the first frame update
     void Start()
    {
       //StartCoroutine(TranslateText("पाठ दर्ज करें"));
    }


    IEnumerator TranslateText(string text)
    {
        string url = $"{apiUrl}?q={UnityWebRequest.EscapeURL(text)}&langpair=hi|en";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            TranslationResult result = JsonUtility.FromJson<TranslationResult>(request.downloadHandler.text);
            outputEnglishText = result.responseData.translatedText;
            if(outputEnglishText != null)
                englishTranslationText.text = outputEnglishText;
            else
                englishTranslationText.text = "An Error Occurred!";
            Debug.Log(result.responseData.translatedText);
        }
    }

    public void OnClickTranslateButton(){
        inputHindiText = inputField.text;
        if(inputHindiText.Length > 0)
            StartCoroutine(TranslateText(inputHindiText));
    }
}

[System.Serializable]
public class TranslationResult
{
    public ResponseData responseData;
}

[System.Serializable]
public class ResponseData
{
    public string translatedText;
}
