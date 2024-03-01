using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Tedydev.PrekladIO
{
    public class APIClient : Singleton<APIClient>
    {
        private HttpHelper _httpHelper;
        private Repository _repository;

        public void Initialize(string apiKey, string sourceLanguageCode = "", string targetLanguageCode = "")
        {
            _httpHelper = new HttpHelper(apiKey);
            _repository = new Repository
            {
                SourceLanguageCode = sourceLanguageCode,
                TargetLanguageCode = targetLanguageCode
            };
        }


        public async Task<Dictionary<string, string>> LoadDictionary(Dictionary<string, string> dictionaryForTranslation, string sourceLanguageCode = "en", string targetLanguageCode = "", List<string> ignoreWords = null)
        {
            var source = sourceLanguageCode ?? _repository.SourceLanguageCode;
            if (!SupportedLanguages.isExist(source))
            {
                throw new Exception("Not supported source language code: " + source);
            }


            var target = targetLanguageCode ?? _repository.TargetLanguageCode;
            if (target == null)
            {
                throw new Exception("Missing target language code: ");
            }

            if (!SupportedLanguages.isExist(target))
            {
                throw new Exception("Not supported target language code: " + target);
            }

            var result = await _httpHelper.TranslateDictionaty(dictionaryForTranslation, source, target, ignoreWords);
            _repository.SaveDictionary(result, source, target);

            return result;
        }

        public string GetTranslationDictionaryByKey(string key, string sourceLanguageCode = "en", string targetLanguageCode = "")
        {
            var source = sourceLanguageCode ?? _repository.SourceLanguageCode;
            if (!SupportedLanguages.isExist(source))
            {
                throw new Exception("Not supported source language code: " + source);
            }


            var target = targetLanguageCode != "" ? targetLanguageCode : _repository.TargetLanguageCode;
            if (String.IsNullOrEmpty(target))
            {
                throw new Exception("Missing target language code.");
            }

            if (!SupportedLanguages.isExist(target))
            {
                throw new Exception("Not supported target language code: " + target);
            }

            return _repository.GetTranslatedTextFromDictionary(key, source, target);

        }

        public async Task<string> TranslateText(string text, string sourceLanguageCode = "en", string targetLanguageCode = "", List<string> ignoreWords = null)
        {


            var source = sourceLanguageCode ?? _repository.SourceLanguageCode;
            if (!SupportedLanguages.isExist(source))
            {
                throw new Exception("Not supported source language code: " + source);
            }


            var target = targetLanguageCode != "" ? targetLanguageCode : _repository.TargetLanguageCode;
            if (String.IsNullOrEmpty(target))
            {
                throw new Exception("Missing target language code.");
            }

            if (!SupportedLanguages.isExist(target))
            {
                throw new Exception("Not supported target language code: " + target);
            }


            // check if text has been already translated

            var textFromRepository = _repository.GetTranslatedText(text, source, target);
            if (!String.IsNullOrEmpty(textFromRepository))
            {
                return textFromRepository;
            }



            var result = await _httpHelper.TranslateText(text, source, target, ignoreWords);
            if (String.IsNullOrEmpty(result))
            {
                throw new Exception("Translation has been failed");
            }


            // save  translated text for futher usage
            _repository.SaveTranslatedText(text, result, source, target);

            return result;

        }

    }
}

