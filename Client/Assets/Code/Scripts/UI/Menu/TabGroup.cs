using ScotlandYard.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ScotlandYard.Scripts.UI.Menu
{

    public class TabGroup : MonoBehaviour
    {
        [SerializeField] protected List<TabButton> tabButtons;
        [SerializeField] protected PanelGroup panelGroup;

        [SerializeField] protected Color tabIdleColor;
        [SerializeField] protected Color tabHoverColor;
        [SerializeField] protected Color tabActiveColor;

        protected TabButton selectedButton;

        public void Subscribe(TabButton button)
        {
            if(tabButtons == null)
            {
                tabButtons = new List<TabButton>();
            }

            tabButtons.Add(button);
        }

        public void OnTabEnter(TabButton button)
        {
            if (!button.Equals(selectedButton))
            {
                ResetTabs();
                button.Background.color = tabHoverColor;
            }
        }

        public void OnTabExit(TabButton button)
        {
            ResetTabs();
        }

        public void OnTabSelected(TabButton button)
        {
            selectedButton = button;
            ResetTabs();
            button.Background.color = tabActiveColor;
            panelGroup.PanelIndex = button.transform.GetSiblingIndex();
        }

        public void ResetTabs()
        {
            foreach(TabButton button in tabButtons)
            {
                if(!button.Equals(selectedButton))
                {
                    button.Background.color = tabIdleColor;
                }
            }
        }
    }
}
