namespace ScotlandYard.Scripts
{
    using ScotlandYard.Enums;
    using System;
	using System.Collections.Generic;
	
	public class SceneHelper
	{		
		#region Methods
		public static List<string> GetMapList()
        {
			List<string> names = new List<string>();
			var values = Enum.GetValues(typeof(EMap));
			
			foreach(var v in values)
            {
				string name = Enum.GetName(typeof(EMap), v);
				names.Add(char.ToUpper(name[0]) + name.Substring(1).ToLower());
            }

			return names;
        }

		public static string GetSceneByMapValue(EMap mapId)
        {
			string name = Enum.GetName(typeof(EMap), mapId);
			return $"{char.ToUpper(name[0])}{name.Substring(1).ToLower()}Map";
		}
		#endregion
	}
}