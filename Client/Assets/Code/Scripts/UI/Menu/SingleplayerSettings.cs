namespace ScotlandYard.Scripts.UI.Menu
{
    using ScotlandYard.Enums;
    using ScotlandYard.Scripts.Database.DAOs;
    using ScotlandYard.Scripts.GameSettings;
    using ScotlandYard.Scripts.UI.Color;
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

        protected List<PlayerSettingsView> playerSettingsList = new List<PlayerSettingsView>();

        protected AiTemplateDAO aiTemplateDAO = AiTemplateDAO.getInstance();
        protected List<object[]> aiTemplates;
        #endregion

        #region Properties
        #endregion

        #region Methods
        protected void OnEnable()
        {
            aiTemplates = new List<object[]>();
            int maxTemplates = aiTemplateDAO.Count();
            List<int> usedIds = new List<int>();
            for (int i = 0; i < settings.MaxPlayer; i++)
            {
                int id = GetRandomAiTemplateId(maxTemplates, usedIds);
                aiTemplates.Add(aiTemplateDAO.Read(id)[0]);
                usedIds.Add(id);
            }

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
            int playerSettingsCount = playerSettingsList.Count;

            if (count > playerSettingsCount)
            {
                int dif = count - playerSettingsCount;
                
                for (int i = 0; i < dif; i++)
                {
                    PlayerSettingsView temp = Instantiate(playerSettingsPrefab);
                    temp.AiTemplate = aiTemplates[playerSettingsCount + i];
                    temp.SetType(((int)EPlayerType.AI));
                    temp.SetColor(settings.GetAvailableColors()[playerSettingsCount+i]);
                    temp.SetColorPicker(colorPicker);
                    temp.transform.SetParent(playerSettingsParent, false);
                    playerSettingsList.Add(temp);
                }
            }
            else
            {
                for (int i = playerSettingsCount - 1; i > count-1; i--)
                {
                    settings.RemoveColor(playerSettingsList[i].GetColor());
                    GameObject.Destroy(playerSettingsList[i].gameObject);
                    playerSettingsList.RemoveAt(i);
                }
            }
        }

        protected int GetRandomAiTemplateId(int maxCount, List<int> usedIds)
        {
            if(usedIds.Count == maxCount) { return 1; }

            System.Random random = new System.Random();
            int id;

            do
            {
                id = random.Next(1, maxCount+1);
            }
            while (usedIds.Contains(id)) ;

            return id;
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
            SceneManager.LoadScene("GameMenu", LoadSceneMode.Additive);
        }
		#endregion
	}
}