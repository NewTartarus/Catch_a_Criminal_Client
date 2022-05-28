namespace ScotlandYard.Scripts.PlayerScripts
{
    using ScotlandYard.Enums;
    using ScotlandYard.Interfaces;
    using ScotlandYard.Scripts.Events;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class Agent : MonoBehaviour
    {
        private const float MAX_STREETLENGTH = 15;

        //Fields
        [SerializeField] protected PlayerData data;
        [SerializeField] protected float speed;
        protected IStreet streetPath;
        protected bool isMoving = false;
        [SerializeField] protected AgentIndicator indicator;
        protected Transform ownTransform;

        protected Animator animator;

        //Properties
        public virtual PlayerData Data
        {
            get
            {
                if(data == null)
                {
                    data = new PlayerData();
                }

                return data;
            }
        }

        public virtual IStreet StreetPath
        {
            get => streetPath;
            set => streetPath = value;
        }

        public void SetDefaultValues(PlayerData data, float speed, AgentIndicator indicator)
        {
            this.data = data;
            this.speed = speed;
            this.indicator = indicator;
        }

        public virtual void Init()
        {
            GeneratePlayerId();
            indicator.SetColor(Data.PlayerColor);
            animator = GetComponentInChildren<Animator>(true);
        }

        protected virtual void Awake()
        {
            GameEvents.Current.OnPlayerActivated += Current_OnPlayerActivated;
        }

        protected virtual void OnDestroy()
        {
            GameEvents.Current.OnPlayerActivated -= Current_OnPlayerActivated;
        }

        protected void Current_OnPlayerActivated(object sender, PlayerEventArgs e)
        {
            if(this.Data.ID == e.PlayerId && this.Data.PlayerRole == e.PlayerRole)
            {
                indicator?.SetEmissive(e.IsActive);
            }
        }

        public virtual void GeneratePlayerId()
        {
            int id = int.Parse(Math.Abs(name.GetHashCode() * DateTime.Now.Millisecond).ToString().Substring(0, 5));
            this.Data.ID = $"{this.Data.AgentName}#{id}";
        }

        public virtual void SetActive(bool isActive)
        {
            Data.IsActive = isActive;
            GameEvents.Current.PlayerActivated(this, new PlayerEventArgs(this.Data));
        }

        public virtual void BeginRound()
        {
            throw new NotImplementedException("This Agent has nothing to do in this Round. Maybe implement the Method.");
        }

        #region Movement
        public virtual IEnumerator Move(IStreet path)
        {
            if (isMoving == false)
            {
                isMoving = true;
                StopCoroutine(nameof(MoveForwards));
                StopCoroutine(nameof(MoveBackwards));


                float multiplier = 1 + (path.Distance < MAX_STREETLENGTH ? 0 : Convert.ToInt32(Math.Ceiling(path.Distance / MAX_STREETLENGTH))) * 0.2f;
                float movementSpeed = speed * multiplier;

                animator?.SetFloat("speed", multiplier - 0.94f);

                Dictionary<string, object> parms = new Dictionary<string, object>()
                {
                    { "STREET", path },
                    { "SPEED", movementSpeed }
                };

                if (this.Data.CurrentPosition.Equals(path.StartPoint))
                {
                    yield return StartCoroutine(nameof(MoveForwards), parms);
                    this.Data.CurrentPosition = path.EndPoint;
                }
                else if (this.Data.CurrentPosition.Equals(path.EndPoint))
                {
                    yield return StartCoroutine(nameof(MoveBackwards), parms);
                    this.Data.CurrentPosition = path.StartPoint;
                }
                transform.rotation = Quaternion.AngleAxis(90, Vector3.up);

                yield return new WaitForSeconds(0.2f);

                Debug.Log($"{Data.AgentName} reached the Destination {this.Data.CurrentPosition.StreetPointName}\n"
                    + $"Tickets left: Taxi = {Data.Tickets[ETicket.TAXI]}, Bus = {Data.Tickets[ETicket.BUS]}, Underground = {Data.Tickets[ETicket.UNDERGROUND]}");
                this.StreetPath = null;

                isMoving = false;
                animator?.SetFloat("speed", 0f);
                GameEvents.Current.PlayerMoveFinished(this, new PlayerEventArgs(this.Data));
            }

        }

        protected IEnumerator MoveForwards(Dictionary<string, object> parms)
        {
            IStreet path = parms["STREET"] as IStreet;
            float mvmntSpeed = (float) parms["SPEED"];

            for (int i = -1; i <= path.GetNumberOfWaypoints(); i++)
            {
                while (Vector3.Distance(transform.position, path.GetWaypoint(i)) > 0.05f)
                {
                    MoveOneStep(path, i, mvmntSpeed);

                    yield return null;
                }
            }
        }

        protected IEnumerator MoveBackwards(Dictionary<string, object> parms)
        {
            IStreet path = parms["STREET"] as IStreet;
            float mvmntSpeed = (float)parms["SPEED"];

            for (int i = path.GetNumberOfWaypoints(); i >= -1; i--)
            {
                while (Vector3.Distance(transform.position, path.GetWaypoint(i)) > 0.05f)
                {
                    MoveOneStep(path, i, mvmntSpeed);

                    yield return null;
                }
            }
        }

        protected void MoveOneStep(IStreet path, int index, float mvmntSpeed)
        {
            transform.position = Vector3.MoveTowards(transform.position, path.GetWaypoint(index), mvmntSpeed * Time.deltaTime);

            Vector3 dir = path.GetWaypoint(index) - transform.position;
            float angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.down);
        }
        #endregion

        // Tickets
        public virtual Dictionary<ETicket, int> GetTickets()
        {
            return this.Data.Tickets;
        }

        public int GetTicketCount(ETicket ticket)
        {
            if (HasTicket(ticket))
            {
                return this.Data.Tickets[ticket];
            }

            return 0;
        }

        public virtual void AddTickets(ETicket ticket, int amount)
        {
            if (Data.Tickets.ContainsKey(ticket))
            {
                Data.Tickets[ticket] += amount;
            }
            else
            {
                Data.Tickets.Add(ticket, amount);
            }
        }

        public virtual bool HasTicket(ETicket ticket)
        {
            if (Data.Tickets.ContainsKey(ticket) && Data.Tickets[ticket] > 0)
            {
                return true;
            }

            return false;
        }

        public virtual void RemoveTicket(ETicket ticket)
        {
            if (HasTicket(ticket))
            {
                Data.Tickets[ticket]--;

                if (Data.PlayerRole == EPlayerRole.DETECTIVE)
                {
                    GameEvents.Current.DetectiveTicketRemoved(this, ticket);
                }
                else if (Data.PlayerRole == EPlayerRole.MISTERX)
                {
                    GameEvents.Current.TicketUpdated(this, new TicketUpdateEventArgs(this.Data));
                }
            }
        }

        public Transform GetTransform()
        {
            if(ownTransform == null)
            {
                ownTransform = this.transform;
            }

            return ownTransform;
        }
    }
}
