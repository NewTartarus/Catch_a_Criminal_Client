using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScotlandYard.Interface;
using ScotlandYard.Enums;
using System;
using ScotlandYard.Events;

namespace ScotlandYard.Scripts.PlayerScripts
{
    public class Player : MonoBehaviour
    {
        protected bool isMoving = false;
        protected Dictionary<ETicket, int> Tickets = new Dictionary<ETicket, int>();

        [SerializeField] protected new string name;
        protected string id;
        [SerializeField] protected float speed;
        protected IStreet _streetPath;
        protected bool hasLost;

        public GameObject position;
        [SerializeField] protected EPlayerType type;

        public string Name { get => name; set => name = value; }
        public string ID { get => id; set => id = value; }
        public EPlayerType PlayerType { get; set; }
        public bool HasLost 
        { 
            get => hasLost; 
            set
            {
                if(value)
                {
                    switch(PlayerType)
                    {
                        case EPlayerType.DETECTIVE:
                            hasLost = value;
                            GameEvents.Current.PlayerLost(null, new PlayerEventArgs(this));
                            break;
                        case EPlayerType.MISTERX:
                            hasLost = value;
                            GameEvents.Current.DetectivesWon(null, null);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        public IStreet StreetPath
        {
            get => _streetPath;
            set
            {
                if (isMoving == false)
                {
                    _streetPath = value;
                    if (_streetPath != null)
                    {
                        StopCoroutine(nameof(Move));
                        StartCoroutine(nameof(Move), _streetPath);
                    }
                }
            }
        }

        public void GeneratePlayerId()
        {
            int id = int.Parse(Math.Abs(name.GetHashCode() * DateTime.Now.Millisecond).ToString().Substring(0, 5));
            this.ID = $"{this.Name}#{id}";
        }

        #region Movement
        public IEnumerator Move(IStreet path)
        {
            if (isMoving == false)
            {
                isMoving = true;
                StopCoroutine(nameof(MoveForwards));
                StopCoroutine(nameof(MoveBackwards));

                if (this.position.Equals(path.StartPoint))
                {
                    yield return StartCoroutine(nameof(MoveForwards), path);
                    this.position = path.EndPoint;
                }
                else if (this.position.Equals(path.EndPoint))
                {
                    yield return StartCoroutine(nameof(MoveBackwards), path);
                    this.position = path.StartPoint;
                }
                transform.rotation = Quaternion.AngleAxis(0f, Vector3.down);

                yield return new WaitForSeconds(0.2f);

                Debug.Log($"Player reached Destination: {this.position.ToString()}");
                this.StreetPath = null;

                isMoving = false;
                GameEvents.Current.PlayerMoveFinished(this, new PlayerEventArgs(this));
            }

        }

        protected IEnumerator MoveForwards(IStreet path)
        {
            for (int i = -1; i <= path.GetNumberOfWaypoints(); i++)
            {
                while (Vector3.Distance(transform.position, path.GetWaypoint(i).position) > 0.05f)
                {
                    MoveOneStep(path, i);

                    yield return null;
                }
            }
        }

        protected IEnumerator MoveBackwards(IStreet path)
        {
            for (int i = path.GetNumberOfWaypoints(); i >= -1; i--)
            {
                while (Vector3.Distance(transform.position, path.GetWaypoint(i).position) > 0.05f)
                {
                    MoveOneStep(path, i);

                    yield return null;
                }
            }
        }

        protected void MoveOneStep(IStreet path, int index)
        {
            transform.position = Vector3.MoveTowards(transform.position, path.GetWaypoint(index).position, speed * Time.deltaTime);

            Vector3 dir = path.GetWaypoint(index).position - transform.position;
            float angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.down);
        }
        #endregion

        #region Tickets
        public virtual Dictionary<ETicket, int> GetTickets()
        {
            return this.Tickets;
        }

        public int GetTicketCount(ETicket ticket)
        {
            if(HasTicket(ticket))
            {
                return this.Tickets[ticket];
            }

            return 0;
        }
        
        public virtual void AddTickets(ETicket ticket, int amount)
        {
            if(Tickets.ContainsKey(ticket))
            {
                Tickets[ticket] += amount;
            }
            else
            {
                Tickets.Add(ticket, amount);
            }
        }

        public virtual bool HasTicket(ETicket ticket)
        {
            if(Tickets.ContainsKey(ticket) && Tickets[ticket] > 0)
            {
                return true;
            }

            return false;
        }

        public virtual void RemoveTicket(ETicket ticket)
        {
            if(HasTicket(ticket))
            {
                Tickets[ticket]--;
            }
        }
        #endregion
    }
}

