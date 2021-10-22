namespace ScotlandYard.Interfaces
{
    using UnityEngine;

    public interface IStreetPoint
	{
        string StreetPointName { get; set; }
        bool IsOccupied { get; set; }
        bool IsHighlighted { get; set; }
        IStreet GetPath(IStreetPoint target);

        void AddStreet(IStreet path);

        IStreet[] GetStreetArray();

        GameObject GetGameObject();

        Transform GetTransform();
    }
}