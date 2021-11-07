namespace ScotlandYard.Scripts.UI.Menu
{
    using ScotlandYard.Enums;
    using ScotlandYard.Scripts.GameSettings;
    using ScotlandYard.Scripts.UI.Color;
    using System;
	using System.Collections;
	using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    public class SingleplayerSettings : MonoBehaviour
	{
		#region Members
		[SerializeField] protected SettingsSO settings;
		[SerializeField] protected GameObject startMenu;
		[SerializeField] protected Transform playerSettingsParent;
		[SerializeField] protected PlayerSettingsView playerSettingsPrefab;

        [SerializeField] protected Image mapImage;
        [SerializeField] protected TMP_Dropdown mapDropDown;
        [SerializeField] protected TMP_Dropdown playerCountDropDown;

        [SerializeField] protected ColorPicker colorPicker;

        List<PlayerSettingsView> playerSettingsList = new List<PlayerSettingsView>();
        #endregion

        #region Properties
        #endregion

        #region Methods
        protected void OnEnable()
        {
            settings.Reset();
            SetPlayerCount(2);
            playerSettingsList[0].SetType(((int)EPlayerType.PLAYER));

            mapDropDown.ClearOptions();
            mapDropDown.AddOptions(SceneHelper.GetMapList());
            mapDropDown.value = 0;
            
            playerCountDropDown.value = 0;
        }

        protected void SetPlayerCount(int count)
        {
            if(count > playerSettingsList.Count)
            {
                int dif = count - playerSettingsList.Count;
                int playerSettingsCount = playerSettingsList.Count;
                for (int i = 0; i < dif; i++)
                {
                    PlayerSettingsView temp = Instantiate(playerSettingsPrefab);
                    temp.SetType(((int)EPlayerType.AI));
                    temp.SetColor(settings.GetAvailableColors()[playerSettingsCount+i]);
                    temp.SetColorPicker(colorPicker);
                    temp.transform.SetParent(playerSettingsParent, false);
                    playerSettingsList.Add(temp);
                }
            }
            else
            {
                for (int i = playerSettingsList.Count-1; i > count-1; i--)
                {
                    settings.RemoveColor(playerSettingsList[i].GetColor());
                    GameObject.Destroy(playerSettingsList[i].gameObject);
                    playerSettingsList.RemoveAt(i);
                }
            }
        }

        public void PlayerCountChanged(int value)
        {
            SetPlayerCount(value + 2);
        }

        public void Back()
        {
            SetPlayerCount(0);

            startMenu.SetActive(true);
			this.gameObject.SetActive(false);
        }
		
		public void StartGame()
        {
            foreach(PlayerSettingsView psv in playerSettingsList)
            {
                settings.PlayerSettings.Add(psv.SaveSetting());
            }

			SceneManager.LoadScene(SceneHelper.GetSceneByMapValue((EMap)mapDropDown.value), LoadSceneMode.Single);
		}
		#endregion
	}
}