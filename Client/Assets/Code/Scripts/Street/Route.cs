namespace ScotlandYard.Scripts.Street
{
    using ScotlandYard.Enums;
    using ScotlandYard.Interfaces;
    using System;
    using UnityEngine;

    class Route : Street, IStreet
    {
        public override int GetNumberOfWaypoints()
        {
            throw new NotImplementedException();
        }

        public override Vector3 GetWaypoint(int i)
        {
            throw new NotImplementedException();
        }
    }
}
