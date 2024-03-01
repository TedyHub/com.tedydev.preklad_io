using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Networking;
using UnityEngine;
using Newtonsoft.Json;


namespace Tedydev.PrekladIO
{
    public class HttpHelper
    {
        const string baseUrl = "https://api.preklad.io/api/translate";
        private string _apiKey;

        public HttpHelper(string apiKey)
        {
            _apiKey = apiKey;
        }


        public async Task<Dictionary<string, string>> TranslateDictionaty(Dictionary<string, string> dictionary, string source, string target, List<string> ignoreWords)
        {
            try
            {
                var url = baseUrl + "/json";
                var jsonString = JsonConvert.SerializeObject(dictionary);
                Debug.Log($"Data, {jsonString}");

                using var request = new UnityWebRequest(url, "POST");
                byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonString);
                request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
                request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("Accept", "application/json");
                request.SetRequestHeader("X-key", _apiKey);
                request.SetRequestHeader("X-from", source);
                request.SetRequestHeader("X-to", target);


                if (ignoreWords != null && ignoreWords.Count > 0)
                {
                    request.SetRequestHeader("X-Ignore", String.Join(",", ignoreWords));
                }


                var operation = request.SendWebRequest();

                while (!operation.isDone)
                    await Task.Yield();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    
                    var errorResult = JsonConvert.DeserializeObject<Dictionary<string, string>>(request.downloadHandler.text);
                    throw new Exception($"Request failed: error code: {errorResult["errorCode"]}, message: {errorResult["message"]}");
                }

                SuccessResultDictionary result = JsonConvert.DeserializeObject<SuccessResultDictionary>(request.downloadHandler.text);
                return result.data;
            }
            catch (Exception ex)
            {
                Debug.LogError($"TranslateDictionaty  failed: {ex.Message}");
                return default;
            }
        }

    

        public async Task<string> TranslateText(string textToTranslate, string source, string target, List<string> ignoreWords)
        {
            try
            {
                var url = baseUrl + "/text";

                using var request = new UnityWebRequest(url, "POST");
                byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(textToTranslate);
                request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
                request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                request.SetRequestHeader("X-key", _apiKey);
                request.SetRequestHeader("X-from", source);
                request.SetRequestHeader("X-to", target);


                if (ignoreWords != null && ignoreWords.Count > 0)
                {
                    request.SetRequestHeader("X-Ignore", String.Join(",", ignoreWords));
                }


                var operation = request.SendWebRequest();

                while (!operation.isDone)
                    await Task.Yield();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    var errorResult = JsonConvert.DeserializeObject<Dictionary<string, string>>(request.downloadHandler.text);
                    throw new Exception($"Request failed: error code: {errorResult["errorCode"]}, message: {errorResult["message"]}");
                }

                return request.downloadHandler.text;
            }
            catch (Exception ex)
            {
                Debug.LogError($"TranslateText  failed: {ex.Message}");
                return default;
            }
        }


    }




    [System.Serializable]
    public class SuccessResultDictionary
    {
        public bool result;
        public Dictionary<string, string> data;
    }

    [System.Serializable]
    public class ErrorResult
    {
        public int errorCode;
        public string message;
    }


}

