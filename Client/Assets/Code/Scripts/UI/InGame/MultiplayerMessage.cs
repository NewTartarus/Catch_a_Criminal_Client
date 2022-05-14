namespace ScotlandYard.Scripts.UI.InGame
{
    using ScotlandYard.Scripts.Transfer;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class MultiplayerMessage : MonoBehaviour
	{
		#region Members
		[SerializeField] protected Image profile;
		[SerializeField] protected TextMeshProUGUI userNameText;
		[SerializeField] protected TextMeshProUGUI timeText;
		[SerializeField] protected TextMeshProUGUI messageText;

		protected Color32 userColor = new Color32(69, 233, 187, 255);
		protected Color32 adminColor = new Color32(255, 100, 85, 255);
		protected Color32 systemColor = new Color32(230, 230, 230, 255);
		#endregion
		
		#region Methods
		public void Init(Message msg, Sprite img)
		{
			profile.sprite = img;
			userNameText.text = msg.userName;
			timeText.text = msg.time;
			messageText.text = msg.text;

			switch(msg.userName)
			{
				case "Admin":
					userNameText.color = adminColor;
					break;
				case "Server":
					userNameText.color = systemColor;
					break;
				default:
					userNameText.color = userColor;
					break;
			}
		}
		#endregion
	}
}