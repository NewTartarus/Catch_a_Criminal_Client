namespace ScotlandYard.Scripts.UI.Basics
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class TabGroup : MonoBehaviour
    {
        [SerializeField] protected List<TabButton> tabButtons;
        [SerializeField] protected PanelGroup panelGroup;

        [SerializeField] protected Color tabIdleColor;
        [SerializeField] protected Color tabHoverColor;
        [SerializeField] protected Color tabActiveColor;

        protected TabButton selectedButton;

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
            panelGroup.PanelIndex = tabButtons.Select((t, i) => new { tab = t, index = i })
                                              .First(a => a.tab.ButtonId == button.ButtonId).index;
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
