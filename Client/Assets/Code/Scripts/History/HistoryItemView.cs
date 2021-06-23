using ScotlandYard.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScotlandYard.Scripts.History
{
    public class HistoryItemView : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI roundText;
        [SerializeField] protected GameObject positionObject;
        [SerializeField] protected TextMeshProUGUI positionText;
        [SerializeField] protected Image ticketImage;
        [SerializeField] protected Sprite taxiSprite;
        [SerializeField] protected Sprite busSprite;
        [SerializeField] protected Sprite undergroundSprite;
        [SerializeField] protected Sprite blackTicketSprite;
        [SerializeField] protected Sprite doubleTicketSprite;

        protected HistoryItem item;

        public void Init(HistoryItem item, bool displayPosition)
        {
            roundText.SetText(item.Round.ToString("00"));
            
            positionObject.SetActive(displayPosition);
            if (displayPosition)
            {
                positionText.SetText(item.Data.CurrentPosition.name);
            }

            switch(item.Ticket)
            {
                case ETicket.TAXI:
                    ticketImage.sprite = taxiSprite;
                    break;
                case ETicket.BUS:
                    ticketImage.sprite = busSprite;
                    break;
                case ETicket.UNDERGROUND:
                    ticketImage.sprite = undergroundSprite;
                    break;
                case ETicket.BLACK_TICKET:
                    ticketImage.sprite = blackTicketSprite;
                    break;
                case ETicket.DOUBLE_TICKET:
                    ticketImage.sprite = doubleTicketSprite;
                    break;
            }
        }
    }
}
