namespace ScotlandYard.Scripts.Helper
{
    using System;
    using System.Text;
    using System.Security.Cryptography;
	
	public class HashHelper
	{		
		#region Methods
		public static string HashString(string text)
        {
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] message = UE.GetBytes(text);
        
            using (SHA256 mySHA256 = SHA256.Create())
            {
                byte[] hashValue = mySHA256.ComputeHash(message);
                string hex = "";
                foreach (byte x in hashValue)
                {
                    hex += String.Format("{0:x2}", x);
                }

                return hex;
            }
        }
		#endregion
	}
}