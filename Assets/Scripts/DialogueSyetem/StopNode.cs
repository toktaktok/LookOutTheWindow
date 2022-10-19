using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[System.Serializable]
[NodeTint("#ff5338")]
public class StopNode : BaseNode {

	[Input] public int entry;
	

	public override string GetString()
	{
		return "Stop";
	}
	public override void GetValue()
	{
		return;
	}
}