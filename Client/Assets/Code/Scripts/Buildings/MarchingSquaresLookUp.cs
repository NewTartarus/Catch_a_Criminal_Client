namespace ScotlandYard.Scripts
{
	using System;
	
	public static class MarchingSquaresLookUp
	{		
		#region Methods
		public static Tuple<byte, float[]> GetModelAndRotation(byte modelId)
        {
			switch(modelId)
            {
				case  0: return new Tuple<byte, float[]>( 0, new float[] { 0f,   0f, 0f });
				case  1: return new Tuple<byte, float[]>( 1, new float[] { 0f,  90f, 0f });
				case  2: return new Tuple<byte, float[]>( 1, new float[] { 0f,   0f, 0f });
				case  3: return new Tuple<byte, float[]>( 3, new float[] { 0f,   0f, 0f });
				case  4: return new Tuple<byte, float[]>( 1, new float[] { 0f, 180f, 0f });
				case  5: return new Tuple<byte, float[]>( 3, new float[] { 0f,  90f, 0f });
				case  6: return new Tuple<byte, float[]>( 6, new float[] { 0f,   0f, 0f });
				case  7: return new Tuple<byte, float[]>( 7, new float[] { 0f,   0f, 0f });
				case  8: return new Tuple<byte, float[]>( 1, new float[] { 0f, 270f, 0f });
				case  9: return new Tuple<byte, float[]>( 6, new float[] { 0f,  90f, 0f });
				case 10: return new Tuple<byte, float[]>( 3, new float[] { 0f, 270f, 0f });
				case 11: return new Tuple<byte, float[]>( 7, new float[] { 0f, 270f, 0f });
				case 12: return new Tuple<byte, float[]>( 3, new float[] { 0f, 180f, 0f });
				case 13: return new Tuple<byte, float[]>( 7, new float[] { 0f,  90f, 0f });
				case 14: return new Tuple<byte, float[]>( 7, new float[] { 0f, 180f, 0f });
				case 15: return new Tuple<byte, float[]>(15, new float[] { 0f,   0f, 0f });
				default: return new Tuple<byte, float[]>( 0, new float[] { 0f,   0f, 0f });
			}
        }
		#endregion
	}
}