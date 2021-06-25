namespace ScotlandYard.Scripts.UI.Menu
{
    using ScotlandYard.Enums;
    using ScotlandYard.Scripts.Events;
    using System.Linq;

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

        protected void OnDestroy()
        {
            GameEvents.Current.OnMainMenuButtonPressed -= Current_OnMainMenuButtonPressed;
        }
    }
}
