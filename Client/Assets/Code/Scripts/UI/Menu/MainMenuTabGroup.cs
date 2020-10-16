using ScotlandYard.Enums;
using ScotlandYard.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScotlandYard.Scripts.UI.Menu
{
    public class MainMenuTabGroup : TabGroup
    {
        protected void Awake()
        {
            GameEvents.Current.OnMainMenuButtonPressed += Current_OnMainMenuButtonPressed;
        }

        protected void Current_OnMainMenuButtonPressed(object sender, EButtons e)
        {
            OnTabSelected(this.tabButtons.First(tb => tb.ButtonId == e) ?? selectedButton);
        }
    }
}
