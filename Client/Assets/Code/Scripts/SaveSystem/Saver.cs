namespace ScotlandYard.Scripts.SaveSystem
{
    using ScotlandYard.Enums;
    using System;
	using UnityEngine;
	
	public abstract class Saver : MonoBehaviour
	{
		#region Methods
		public virtual void Save(EUserId userId)
        {
			throw new NotImplementedException();
        }

		protected virtual string SerializeValue(object value)
        {
			if (value is String valString)
            {
				return valString;
            }
			else if (value is Boolean valBool)
            {
				return valBool ? "1" : "0";
			}
			else if (value is Int32 valInt)
			{
				return $"{valInt}";
			}
			else if (value is Resolution valRes)
            {
				return $"{valRes.width}_{valRes.height}";
			}
			else
            {
				return String.Empty;
            }
		}
		#endregion
	}
}