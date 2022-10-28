namespace ScotlandYard.Scripts.SaveSystem
{
    using ScotlandYard.Enums;
    using System;
    using System.Linq;
    using UnityEngine;

    public abstract class Loader : MonoBehaviour
    {
        public virtual void Load(EUserId userId)
        {
            throw new NotImplementedException();
        }

        protected virtual object DeserializeValue(string value, string type)
        {
            switch (type)
            {
                case nameof(String):
                    return value;
                case nameof(Boolean):
                    return "1".Equals(value);
                case nameof(Int32):
                    return String.IsNullOrEmpty(value) ? 0 : Int32.Parse(value);
                case nameof(Resolution):
                    return DeserializeResolution(value);
                default:
                    return value;
            }
        }

        private Resolution DeserializeResolution(string value)
        {
            string[] resolutionStrings = value.Split('_');
            int[] data = resolutionStrings.Select(x => Int32.Parse(x)).ToArray();

            return new Resolution() { width = data[0], height = data[1] };
        }
    }
}
