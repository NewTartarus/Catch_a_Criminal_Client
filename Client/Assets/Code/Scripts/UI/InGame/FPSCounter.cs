namespace ScotlandYard.Scripts
{
	using UnityEngine;
	using TMPro;
	
	public class FPSCounter : MonoBehaviour
	{
		#region Members
		[SerializeField] TextMeshProUGUI fpsCounterText;

		private float pollingTime = 1f;
		private float time;
		private int   frameCount;
		#endregion
		
		#region Methods		
		protected void Update()
		{
			time += Time.deltaTime;

			frameCount++;

			if (time >= pollingTime)
            {
				int frameRate = Mathf.RoundToInt(frameCount / time);
				fpsCounterText.text = $"{frameRate} FPS";

				time -= pollingTime;
				frameCount = 0;
            }
		}
		#endregion
	}
}