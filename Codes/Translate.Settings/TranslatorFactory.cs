using System;
using System.Text.RegularExpressions;
using Translate.Core.Translator;
using Translate.Core.Translator.Deepl;
using Translate.Core.Translator.Google;

namespace Translate.Settings
{
    public class TranslatorFactory
    {
        private static ITranslator _googleTranslator;
        private static ITranslator _deeplTranslator;

        /// <summary>
        /// 不含标点
        /// </summary>
        private static readonly Regex ChineseRegex = new Regex(@"[\u4e00-\u9fa5]");

        public static string GetSourceLanguage(TranslateType type, string selectedText)
        {
            if (OptionsSettings.Settings.IsEnabledFirstJudgeChinese && ChineseRegex.IsMatch(selectedText))
            {
                return GetChineseLanguage(type);
            }
            string sourceLanguage = string.Empty;

            switch (type)
            {
                case TranslateType.Google:
                    sourceLanguage = GoogleTranslator.GetSourceLanguages()[OptionsSettings.Settings.GoogleSettings.SourceLanguageIndex].Code;
                    break;
                case TranslateType.Deepl:
                    sourceLanguage = DeeplTranslator.GetSourceLanguages()[OptionsSettings.Settings.DeeplSettings.SourceLanguageIndex].Code;
                    break;                   
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            return sourceLanguage;
        }

        public static string GetTargetLanguage(TranslateType type, string selectedText)
        {
            string targetLanguage = string.Empty;
            if (ChineseRegex.IsMatch(selectedText))
            {
                switch (type)
                {
                    case TranslateType.Google:
                        targetLanguage = GoogleTranslator.GetTargetLanguages()[OptionsSettings.Settings.GoogleSettings.LastLanguageIndex].Code;
                        break;
                    case TranslateType.Deepl:
                        targetLanguage = DeeplTranslator.GetTargetLanguages()[OptionsSettings.Settings.DeeplSettings.LastLanguageIndex].Code;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
            }
            else
            {
                switch (type)
                {
                    case TranslateType.Google:
                        targetLanguage = GoogleTranslator.GetTargetLanguages()[OptionsSettings.Settings.GoogleSettings.TargetLanguageIndex].Code;
                        break;
                    case TranslateType.Deepl:
                        targetLanguage = DeeplTranslator.GetTargetLanguages()[OptionsSettings.Settings.DeeplSettings.TargetLanguageIndex].Code;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
            }
            return targetLanguage;
        }


        private static string GetChineseLanguage(TranslateType type)
        {
            string chineseLanguage = string.Empty;
            switch (type)
            {
                case TranslateType.Google:
                    chineseLanguage = GoogleTranslator.GetChineseLanguage();
                    break;
                case TranslateType.Deepl:
                    chineseLanguage = DeeplTranslator.GetChineseLanguage();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            return chineseLanguage;
        }


        public static ITranslator GetTranslator(TranslateType type)
        {
            ITranslator translator = _googleTranslator;
            switch (type)
            {
                case TranslateType.Deepl:
                    if (_deeplTranslator == null)
                    {
                        _deeplTranslator = new DeeplTranslator();
                    }
                    translator = _deeplTranslator;
                    break;
                case TranslateType.Google:
                    if (_googleTranslator == null)
                    {
                        _googleTranslator = new GoogleTranslator();
                    }
                    translator = _googleTranslator;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            return translator;
        }

    }
}