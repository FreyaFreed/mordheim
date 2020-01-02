using System;
using System.Collections.Generic;
using UnityEngine;

public class PointsChecker
{
	public PointsChecker(global::UnityEngine.Transform transform, bool hasOffset)
	{
		this.zoneTransform = transform;
		this.applyOffset = hasOffset;
	}

	public void UpdateControlPoints(global::UnitController unit, global::System.Collections.Generic.List<global::UnitController> allUnits)
	{
		this.validPoints.Clear();
		float floatSqr = global::Constant.GetFloatSqr(global::ConstantId.MELEE_RANGE_LARGE);
		float floatSqr2 = global::Constant.GetFloatSqr(global::ConstantId.MELEE_RANGE_NORMAL);
		float num = unit.CapsuleRadius / 2f;
		float dist = ((unit.unit.Data.UnitSizeId != global::UnitSizeId.LARGE) ? global::Constant.GetFloat(global::ConstantId.MELEE_RANGE_NORMAL) : global::Constant.GetFloat(global::ConstantId.MELEE_RANGE_LARGE)) + 0.2f;
		global::UnityEngine.Vector3 vector = this.zoneTransform.position;
		if (this.applyOffset)
		{
			vector += this.zoneTransform.forward * -num;
		}
		global::UnityEngine.Vector2 vector2 = new global::UnityEngine.Vector2(vector.x, vector.z);
		this.UpdateUnitsOnActionZone(unit, allUnits, vector2, vector);
		for (float num2 = 0f; num2 < 360f; num2 += 60f)
		{
			global::UnityEngine.Vector3 vector3;
			if (this.GetPoint(vector, num2, dist, out vector3))
			{
				global::UnityEngine.Vector2 vector4 = new global::UnityEngine.Vector2(vector3.x, vector3.z);
				global::UnityEngine.Vector2 checkDestPoint = vector4 + (vector2 - vector4) * 100f;
				bool flag = true;
				for (int i = 0; i < allUnits.Count; i++)
				{
					if (!(allUnits[i] == unit) && !this.alliesOnZone.Contains(allUnits[i]) && !this.enemiesOnZone.Contains(allUnits[i]))
					{
						float num3 = (allUnits[i].unit.Data.UnitSizeId != global::UnitSizeId.LARGE) ? floatSqr2 : floatSqr;
						if (global::UnityEngine.Vector3.SqrMagnitude(allUnits[i].transform.position - vector) < floatSqr * 2f && global::UnityEngine.Vector3.SqrMagnitude(allUnits[i].transform.position - vector3) < num3 && global::PandoraUtils.IsPointInsideEdges(allUnits[i].combatCircle.Edges, vector4, checkDestPoint, -1f))
						{
							flag = false;
						}
					}
				}
				if (flag)
				{
					this.validPoints.Add(vector3);
				}
			}
		}
	}

	public virtual bool GetPoint(global::UnityEngine.Vector3 startPoint, float angle, float dist, out global::UnityEngine.Vector3 pos)
	{
		global::UnityEngine.Vector3 vector = global::UnityEngine.Vector3.forward;
		vector = global::UnityEngine.Quaternion.Euler(0f, angle, 0f) * vector;
		vector.Normalize();
		pos = global::UnityEngine.Vector3.zero;
		if (!global::PandoraUtils.SendCapsule(startPoint, vector, 0.6f, 1.5f, dist, 0.5f))
		{
			pos = startPoint + vector * dist;
			return true;
		}
		return false;
	}

	private void UpdateUnitsOnActionZone(global::UnitController unit, global::System.Collections.Generic.List<global::UnitController> allUnits, global::UnityEngine.Vector2 zoneflatPos, global::UnityEngine.Vector3 zonePos)
	{
		this.alliesOnZone.Clear();
		this.enemiesOnZone.Clear();
		float floatSqr = global::Constant.GetFloatSqr(global::ConstantId.MELEE_RANGE_LARGE);
		float floatSqr2 = global::Constant.GetFloatSqr(global::ConstantId.MELEE_RANGE_NORMAL);
		for (int i = allUnits.Count - 1; i >= 0; i--)
		{
			if (allUnits[i] != unit)
			{
				global::UnityEngine.Vector2 vector = new global::UnityEngine.Vector2(allUnits[i].transform.position.x, allUnits[i].transform.position.z);
				global::UnityEngine.Vector2 checkDestPoint = zoneflatPos + (vector - zoneflatPos) * 100f;
				float num = global::UnityEngine.Vector3.SqrMagnitude(allUnits[i].transform.position - zonePos);
				float num2 = (unit.unit.Data.UnitSizeId != global::UnitSizeId.LARGE && allUnits[i].unit.Data.UnitSizeId != global::UnitSizeId.LARGE) ? floatSqr2 : floatSqr;
				if (num < 0.1f || (num < num2 && global::PandoraUtils.IsPointInsideEdges(allUnits[i].combatCircle.Edges, vector, checkDestPoint, -1f)))
				{
					if (unit.IsEnemy(allUnits[i]))
					{
						this.enemiesOnZone.Add(allUnits[i]);
					}
					else
					{
						this.alliesOnZone.Add(allUnits[i]);
					}
				}
			}
		}
	}

	public bool IsAvailable()
	{
		if (this.alliesOnZone.Count == 0 && this.enemiesOnZone.Count == 0)
		{
			return true;
		}
		for (int i = 0; i < this.alliesOnZone.Count; i++)
		{
			if (this.alliesOnZone[i].Engaged || this.alliesOnZone[i].unit.Data.UnitSizeId == global::UnitSizeId.LARGE)
			{
				return false;
			}
		}
		for (int j = 0; j < this.enemiesOnZone.Count; j++)
		{
			if (this.enemiesOnZone[j].Engaged || this.enemiesOnZone[j].unit.Data.UnitSizeId == global::UnitSizeId.LARGE)
			{
				return false;
			}
		}
		return this.validPoints.Count >= this.alliesOnZone.Count + this.enemiesOnZone.Count;
	}

	public bool CanDoAthletic()
	{
		return this.alliesOnZone.Count == 0 && this.enemiesOnZone.Count == 0;
	}

	public const float CAPSULE_MIN_HEIGHT = 0.6f;

	private const float RAY_RANGE = 100f;

	private const float ANGLE_INCREMENT = 60f;

	protected const float SPHERE_RADIUS = 0.5f;

	public const float ALLY_DIST_OFFSET = 0.2f;

	public const float ENEMY_DIST_OFFSET = -0.22f;

	protected readonly global::UnityEngine.Transform zoneTransform;

	private readonly bool applyOffset;

	public global::System.Collections.Generic.List<global::UnityEngine.Vector3> validPoints = new global::System.Collections.Generic.List<global::UnityEngine.Vector3>();

	public global::System.Collections.Generic.List<global::UnitController> alliesOnZone = new global::System.Collections.Generic.List<global::UnitController>();

	public global::System.Collections.Generic.List<global::UnitController> enemiesOnZone = new global::System.Collections.Generic.List<global::UnitController>();
}
