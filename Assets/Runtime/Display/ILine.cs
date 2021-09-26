using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Display
{
    public interface ILine
	{
		#region public-property
		IPoint U
		{
			get;
		}

		IPoint V
		{
			get;
		}
		#endregion public-property
	}
}