namespace ScotlandYard.Scripts.UI.Menu
{
    using ScotlandYard.Enums;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    [RequireComponent(typeof(Image))]
    public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        [SerializeField] protected TabGroup tabGroup;
        [SerializeField] protected EButtons buttonId;
        protected Image background;

        public Image Background
        {
            get => background;
            set => background = value;
        }

        public EButtons ButtonId
        {
            get => buttonId;
            set => buttonId = value;
        }

        // Start is called before the first frame update
        void Start()
        {
            background = GetComponent<Image>();
            tabGroup.Subscribe(this);
        }

        public void Select()
        {
            
        }

        public void Deselect()
        {

        }

        public void OnPointerClick(PointerEventData eventData)
        {
            tabGroup.OnTabSelected(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            tabGroup.OnTabEnter(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tabGroup.OnTabExit(this);
        }
    }
}
