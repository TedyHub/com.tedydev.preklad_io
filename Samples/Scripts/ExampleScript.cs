using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Tedydev.PrekladIO;

public class ExampleScript : MonoBehaviour
{
    [SerializeField]
    private TMP_Text buttonLabel;

    [SerializeField]
    private TMP_Text outputPanel;

    [SerializeField]
    private TMP_Text resultText;


    async void  Awake()
    {
        resultText.text = "";

        APIClient.Instance.Initialize("YOUR_API_KEY");

        Dictionary<string, string> translateDictionary = new Dictionary<string, string> {
            { "buttonLabel", "Click Me!"},
            { "outputLabel", "Result"},
        };


        // translate dictionary
        var result  = await APIClient.Instance.LoadDictionary(translateDictionary, "en", "es");
        if (result.Count > 0)
        {
            Debug.Log($"result:  {result}");
            buttonLabel.text = result["buttonLabel"];
            outputPanel.text = result["outputLabel"];

        }

    }


    public async  void ClickButton()
    {
        // translate  result of the operation
        var translatedText = await APIClient.Instance.TranslateText("The result of asynchoruns function");
        resultText.text = translatedText;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
