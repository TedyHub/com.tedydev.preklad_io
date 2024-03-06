<p align="center">
<a href="https://prekald.io/">Official page →</a><br>
</p>

<p align="center" style="color: #AAA">
  Unity custom library to  <a href="https://www.preklad.io">Preklad.IO</a> <br>API translation service.
</p>


# API Client Library for Unity

## What?

The library provides easy integration with  **Preklad.IO** service. It allows  to use all the features of the service in your code.
Such as:
- key-value pairs  text ranslations
- plain text translations


# Install
Add  the library from  the following link: <a href="https://github.com/TedyHub/com.tedydev.preklad_io.git">com.tedydev.preklad_io</a>



# Quick start


### Translate key-value dictionary  
You can translate already predefined  collection of texts:
```
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
}


// get translated  value  later
APIClient.Instance.GetTranslationDictionaryByKey("buttonLabel")

```

### Translate  plan text 
You can translate a single plain text messsage:
```
// translate plain text to other language
var translatedText = await APIClient.Instance.TranslateText("The result of asynchoruns function", "en", "cs");
```

### Languages
List of supported languages can be found in the  [Documenation](https://preklad.io/docs).

In case the source language is not defined - 'en' is used by default.


# Terms and usage limitations
The terms amd privacy plicy  can be found on the website [Terms and Conditions](https://preklad.io/terms) or contact us.
