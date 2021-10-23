namespace ScotlandYard.Scripts.UI.Menu
{
    using ScotlandYard.Scripts.Events;
    using ScotlandYard.Scripts.Localisation;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using TMPro;

    public class Menu : MonoBehaviour
    {
        public TMP_Dropdown resoDropdown;
        protected Resolution[] resolutions;

        public TMP_Dropdown languagesDropdown;
        protected List<string> languages;

        public void Awake()
        {
            initResolutionDropdown();
            initLanguagesDropDown();
        }

        public void initResolutionDropdown()
        {
            resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();

            resoDropdown.ClearOptions();
            resoDropdown.AddOptions(this.resolutions.Select(res => $"{res.width} x {res.height}").ToList());

            Resolution currentReso = Screen.currentResolution;
            resoDropdown.value = this.resolutions
                                        .Select((r, i) => new { reso = r, index = i })
                                        .First(a => a.reso.height == currentReso.height && a.reso.width == currentReso.width).index;
            resoDropdown.RefreshShownValue();
        }

        public void SetFullscreen(bool isFoolscreen)
        {
            Screen.fullScreen = isFoolscreen;
        }

        public void SetResolution(int resolutionIndex)
        {
            Resolution res = resolutions[resolutionIndex];
            Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        }

        public void initLanguagesDropDown()
        {
            languages = LocalisationSystem.GetLanguages();

            languagesDropdown.ClearOptions();
            languagesDropdown.AddOptions(languages);

            Language currentLanguage = LocalisationSystem.Lang;
            languagesDropdown.value = this.languages
                                        .Select((l, i) => new { lang = l, index = i })
                                        .First(a => a.lang == currentLanguage.Name).index;
            languagesDropdown.RefreshShownValue();
        }

        public void SetLanguage(int languageIndex)
        {
            LocalisationSystem.Lang = new Language(languageIndex, languages[languageIndex]);
            GameEvents.Current.LanguageChanged(this, null);
        }
    }
}