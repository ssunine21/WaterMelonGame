using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LocalizationManager : MonoBehaviour
{

    public SystemLanguage systemLanguage;
    
    public static LocalizationManager init;
    private TextAsset textData;
    private Dictionary<string, string> localizedText;
    private string missingTextString = "Localized text not found";

    private void Awake() {
        if (init == null) {
            init = this;
        } else if (init != this) {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        InitLanguage();
    }

    public void LoadLocalizedText(string fileName) {

        textData = Resources.Load<TextAsset>("localized/" + fileName);
        localizedText = new Dictionary<string, string>();

        LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(textData.ToString());

        for (int i = 0; i < loadedData.items.Length; ++i) {
            localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
        }
        Debug.Log("Data loaded. Dictionary containts :" + localizedText.Count + " entries");
    }

    public string GetLocalizedValue(Definition.LocalizeKey key) {
        string result = missingTextString;
        string keyString = key.ToString();
        if (localizedText.ContainsKey(keyString)) {
            result = localizedText[keyString];
        }

        return result;
    }

    private void InitLanguage() {
        string languageName;
        SystemLanguage language;
        #if UNITY_EDITOR
        language = systemLanguage;
        #else
        language = Application.systemLanguage;
        #endif
        
		switch (language) {
            case SystemLanguage.Korean:
                languageName = "ko";
                break;
            case SystemLanguage.Japanese:
                languageName = "ja";
                break;
            case SystemLanguage.Chinese:
            case SystemLanguage.ChineseSimplified:
                languageName = "zh_chs";
                break;
            case SystemLanguage.ChineseTraditional:
                languageName = "zh_cht";
                break;
            /*            case SystemLanguage.French:
                            languageName = "fr";
                            break;
                        case SystemLanguage.Spanish:
                            languageName = "es";
                            break;
                        case SystemLanguage.German:
                            languageName = "de";
                            break;
                        case SystemLanguage.Italian:
                            languageName = "it";
                            break;
                        case SystemLanguage.Russian:
                            languageName = "ru";
                            break;
                        case SystemLanguage.Danish:
                            languageName = "da";
                            break;
                        case SystemLanguage.Norwegian:
                            languageName = "no";
                            break;
                        case SystemLanguage.Portuguese:
                            languageName = "pt";
                            break;
                        case SystemLanguage.Swedish:
                            languageName = "sv";
                            break;
                        case SystemLanguage.Polish:
                            languageName = "pl";
                            break;
                        case SystemLanguage.Turkish:
                            languageName = "tr";
                            break;
                        case SystemLanguage.Vietnamese:
                            languageName = "vt";
                            break;
                        case SystemLanguage.Indonesian:
                            languageName = "in";
                            break;*/
            default:
                languageName = "en";
                break;

        }

        languageName = "localizedText_" + languageName;
        LoadLocalizedText(languageName);
    }
}