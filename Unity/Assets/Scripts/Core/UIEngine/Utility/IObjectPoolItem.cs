using System.Collections.Generic;
using UnityEngine;
using System;

public interface IObjectPoolItem
{
	void OnReuse();
	void OnRecycle();
}