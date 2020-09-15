using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

namespace ScotlandYard.Scripts.Menu
{
    public class Menu : MonoBehaviour
    {
        public TMP_Dropdown resoDropdown;
        protected Resolution[] resolutions;

        public void Awake()
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
    }
}