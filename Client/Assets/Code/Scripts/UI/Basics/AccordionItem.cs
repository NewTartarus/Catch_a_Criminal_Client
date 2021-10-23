namespace ScotlandYard.Scripts.UI.Basics
{
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class AccordionItem : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {
        [SerializeField] protected GameObject icon;
        [SerializeField] protected GameObject content;
        [SerializeField] protected GameObject parentContentList;

        [SerializeField] protected bool collapsed = true;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (collapsed)
            {
                icon.transform.Rotate(180, 0, 0);
                content.SetActive(true);
                
            }
            else
            {
                icon.transform.Rotate(-180, 0, 0);
                content.SetActive(false);
            }

            if(parentContentList != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(parentContentList.GetComponent<RectTransform>());
            }

            collapsed = !collapsed;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            
        }
    }
}


