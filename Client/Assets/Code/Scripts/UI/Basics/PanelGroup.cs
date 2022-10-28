namespace ScotlandYard.Scripts.UI.Basics
{
    using UnityEngine;

    public class PanelGroup : MonoBehaviour
    {
        [SerializeField] protected GameObject[] panels;
        [SerializeField] protected TabGroup tabGroup;
        protected int panelIndex = -1;

        public int PanelIndex
        {
            get => panelIndex;
            set
            {
                if(panelIndex != value)
                {
                    panelIndex = value;
                    for (int i = 0; i < panels.Length; i++)
                    {
                        panels[i].SetActive(panelIndex == i);
                    }
                }
            }
        }

        private void Awake()
        {
            PanelIndex = 0;
        }
    }
}
