namespace ScotlandYard.Scripts.UI.Basics
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

        void Start()
        {
            background = GetComponent<Image>();
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
