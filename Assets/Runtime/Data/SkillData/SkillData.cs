using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkillData", menuName = "Skill/SkillData", order = 1)]
public class SkillData : ScriptableObject
{
	#region private-field
	[SerializeField]
	private string _description;
	
	[HideInInspector]
	[SerializeField]
	private SkillAbility[] _abilities;
	#endregion private-field
}
