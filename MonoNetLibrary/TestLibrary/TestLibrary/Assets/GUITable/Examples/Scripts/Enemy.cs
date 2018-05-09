using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType 
{
	Normal,
	Flock,
	Boss
}

[System.Serializable]
public class Enemy : MonoBehaviour
{
	public EnemyType type;
	public int health;
	public float speed;
	public Color color;
	public bool canSwim;
	public int spawnersMask;

	public void Instantiate ()
	{
		UnityEditor.PrefabUtility.InstantiatePrefab(this);
	}
}