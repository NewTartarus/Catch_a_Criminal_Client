using ScotlandYard.Enums;
using ScotlandYard.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScotlandYard.Scripts.UI
{
    public class TicketButton : MonoBehaviour
    {
        [SerializeField] protected Image imageSrc;
        [SerializeField] protected Sprite selectedImage;
        [SerializeField] protected Sprite unselectedImage;

        [SerializeField] protected TextMeshProUGUI text;
        [SerializeField] protected ETicket ticket;

        protected bool isSelected;

        public bool Select
        {
            get => isSelected;
            set
            { 
                if(value == true)
                {
                    imageSrc.sprite = selectedImage;
                }
                else
                {
                    imageSrc.sprite = unselectedImage;
                }

                isSelected = value;
            }
        }

        public void SetTicketCount(int ticketCount)
        {
            if(ticketCount > 0)
            {
                this.gameObject.SetActive(true);
                text.text = $"{ticketCount:00}";
            }
            else
            {
                this.gameObject.SetActive(false);
            }
            
        }

        public ETicket GetTicket()
        {
            return this.ticket;
        }

        public void SelectButton()
        {
            GameEvents.current.TicketSelected(null, this);
        }

        public override bool Equals(object other)
        {
            if(other is TicketButton button)
            {
                if(button.ticket == this.ticket)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
