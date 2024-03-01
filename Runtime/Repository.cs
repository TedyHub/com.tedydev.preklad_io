using System;
using System.Collections.Generic;


namespace Tedydev.PrekladIO
{
    public class Repository
    {
        public string SourceLanguageCode { get; set; }
        public string TargetLanguageCode { get; set; }

        private Dictionary<string, string> _storage = new Dictionary<string, string>();


        public void SaveDictionary(Dictionary<string, string> dictionary, string source, string target)
        {
            if (dictionary == null || dictionary.Count == 0)
            {
                return;
            }


            foreach (KeyValuePair<string, string> entry in dictionary)
            {
                var storageKey = ComputeDictionaryKey(entry.Key, source, target);
                _storage[storageKey] = entry.Value;
            }


            SourceLanguageCode = source;
            TargetLanguageCode = target;
        }



        public string GetTranslatedTextFromDictionary(string key, string source, string target)
        {
            var storageKey = ComputeDictionaryKey(key, source, target);
            if (_storage.ContainsKey(storageKey))
            {
                return _storage[storageKey];
            }

            return key;
        }

        public string GetTranslatedText(string key, string source, string target)
        {
            if (String.IsNullOrEmpty(key))
            {
                return "";
            }

            var textKey = ComputeDictionaryKey(ComputeHash(key), source, target);

            if (_storage.ContainsKey(textKey))
            {
                return _storage[textKey];
            }

            return "";

        }

        public void SaveTranslatedText(string originalText, string translatedText, string source, string target)
        {
            if (String.IsNullOrEmpty(originalText))
            {
                return;
            }

            if (String.IsNullOrEmpty(translatedText))
            {
                return;
            }

            var textKey = ComputeDictionaryKey(ComputeHash(originalText), source, target);
            _storage[textKey] = translatedText;
            SourceLanguageCode = source;
            TargetLanguageCode = target;


        }


        private string ComputeKeyForText(string text, string source, string target)
        {
            return ComputeDictionaryKey(ComputeHash(text), source, target);
        }




        private string ComputeDictionaryKey(string key, string source, string target)
        {
            return $"{key}-${source}-${target}";
        }


        private string ComputeHash(string text, string salt = "")
        {
            if (String.IsNullOrEmpty(text))
            {
                return String.Empty;
            }

            // Uses SHA256 to create the hash
            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                // Convert the string to a byte array first, to be processed
                byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(text + salt);
                byte[] hashBytes = sha.ComputeHash(textBytes);

                // Convert back to a string, removing the '-' that BitConverter adds
                string hash = BitConverter
                    .ToString(hashBytes)
                    .Replace("-", String.Empty);

                return hash;
            }
        }
    }
    

}
