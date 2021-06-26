namespace ScotlandYard.Scripts.PlayerScripts
{
    using ScotlandYard.Interfaces;
    using ScotlandYard.Scripts.Helper;

    public class Player : Agent
    {
        public override IStreet StreetPath
        {
            get => streetPath;
            set
            {
                if (isMoving == false)
                {
                    streetPath = value;
                    if (streetPath != null)
                    {
                        StopCoroutine(nameof(Move));
                        StartCoroutine(nameof(Move), streetPath);
                    }
                }
            }
        }

        public override void BeginRound()
        {
            HighlightBehavior.HighlightAccesPoints(this);
        }
    }
}

