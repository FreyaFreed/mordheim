using System;
using System.Collections.Generic;
using Pathfinding.Serialization;
using UnityEngine;

namespace Pathfinding
{
	[global::System.Serializable]
	public class GraphCollision
	{
		public void Initialize(global::UnityEngine.Matrix4x4 matrix, float scale)
		{
			this.up = matrix.MultiplyVector(global::UnityEngine.Vector3.up);
			this.upheight = this.up * this.height;
			this.finalRadius = this.diameter * scale * 0.5f;
			this.finalRaycastRadius = this.thickRaycastDiameter * scale * 0.5f;
		}

		public bool Check(global::UnityEngine.Vector3 position)
		{
			if (!this.collisionCheck)
			{
				return true;
			}
			if (this.use2D)
			{
				global::Pathfinding.ColliderType colliderType = this.type;
				if (colliderType == global::Pathfinding.ColliderType.Sphere)
				{
					return global::UnityEngine.Physics2D.OverlapCircle(position, this.finalRadius, this.mask) == null;
				}
				if (colliderType != global::Pathfinding.ColliderType.Capsule)
				{
					return global::UnityEngine.Physics2D.OverlapPoint(position, this.mask) == null;
				}
				throw new global::System.Exception("Capsule mode cannot be used with 2D since capsules don't exist in 2D. Please change the Physics Testing -> Collider Type setting.");
			}
			else
			{
				position += this.up * this.collisionOffset;
				global::Pathfinding.ColliderType colliderType = this.type;
				if (colliderType == global::Pathfinding.ColliderType.Sphere)
				{
					return !global::UnityEngine.Physics.CheckSphere(position, this.finalRadius, this.mask);
				}
				if (colliderType != global::Pathfinding.ColliderType.Capsule)
				{
					switch (this.rayDirection)
					{
					case global::Pathfinding.RayDirection.Up:
						return !global::UnityEngine.Physics.Raycast(position, this.up, this.height, this.mask);
					case global::Pathfinding.RayDirection.Both:
						return !global::UnityEngine.Physics.Raycast(position, this.up, this.height, this.mask) && !global::UnityEngine.Physics.Raycast(position + this.upheight, -this.up, this.height, this.mask);
					}
					return !global::UnityEngine.Physics.Raycast(position + this.upheight, -this.up, this.height, this.mask);
				}
				return !global::UnityEngine.Physics.CheckCapsule(position, position + this.upheight, this.finalRadius, this.mask);
			}
		}

		public global::UnityEngine.Vector3 CheckHeight(global::UnityEngine.Vector3 position)
		{
			global::UnityEngine.RaycastHit raycastHit;
			bool flag;
			return this.CheckHeight(position, out raycastHit, out flag);
		}

		public global::UnityEngine.Vector3 CheckHeight(global::UnityEngine.Vector3 position, out global::UnityEngine.RaycastHit hit, out bool walkable)
		{
			walkable = true;
			if (!this.heightCheck || this.use2D)
			{
				hit = default(global::UnityEngine.RaycastHit);
				return position;
			}
			if (this.thickRaycast)
			{
				global::UnityEngine.Ray ray = new global::UnityEngine.Ray(position + this.up * this.fromHeight, -this.up);
				if (global::UnityEngine.Physics.SphereCast(ray, this.finalRaycastRadius, out hit, this.fromHeight + 0.005f, this.heightMask))
				{
					return global::Pathfinding.VectorMath.ClosestPointOnLine(ray.origin, ray.origin + ray.direction, hit.point);
				}
				walkable &= !this.unwalkableWhenNoGround;
			}
			else
			{
				if (global::UnityEngine.Physics.Raycast(position + this.up * this.fromHeight, -this.up, out hit, this.fromHeight + 0.005f, this.heightMask))
				{
					return hit.point;
				}
				walkable &= !this.unwalkableWhenNoGround;
			}
			return position;
		}

		public global::UnityEngine.Vector3 Raycast(global::UnityEngine.Vector3 origin, out global::UnityEngine.RaycastHit hit, out bool walkable)
		{
			walkable = true;
			if (!this.heightCheck || this.use2D)
			{
				hit = default(global::UnityEngine.RaycastHit);
				return origin - this.up * this.fromHeight;
			}
			if (this.thickRaycast)
			{
				global::UnityEngine.Ray ray = new global::UnityEngine.Ray(origin, -this.up);
				if (global::UnityEngine.Physics.SphereCast(ray, this.finalRaycastRadius, out hit, this.fromHeight + 0.005f, this.heightMask))
				{
					return global::Pathfinding.VectorMath.ClosestPointOnLine(ray.origin, ray.origin + ray.direction, hit.point);
				}
				walkable &= !this.unwalkableWhenNoGround;
			}
			else
			{
				if (global::UnityEngine.Physics.Raycast(origin, -this.up, out hit, this.fromHeight + 0.005f, this.heightMask))
				{
					return hit.point;
				}
				walkable &= !this.unwalkableWhenNoGround;
			}
			return origin - this.up * this.fromHeight;
		}

		public global::UnityEngine.RaycastHit[] CheckHeightAll(global::UnityEngine.Vector3 position)
		{
			if (!this.heightCheck || this.use2D)
			{
				return new global::UnityEngine.RaycastHit[]
				{
					new global::UnityEngine.RaycastHit
					{
						point = position,
						distance = 0f
					}
				};
			}
			if (this.thickRaycast)
			{
				return new global::UnityEngine.RaycastHit[0];
			}
			global::System.Collections.Generic.List<global::UnityEngine.RaycastHit> list = new global::System.Collections.Generic.List<global::UnityEngine.RaycastHit>();
			global::UnityEngine.Vector3 vector = position + this.up * this.fromHeight;
			global::UnityEngine.Vector3 vector2 = global::UnityEngine.Vector3.zero;
			int num = 0;
			for (;;)
			{
				global::UnityEngine.RaycastHit item;
				bool flag;
				this.Raycast(vector, out item, out flag);
				if (item.transform == null)
				{
					break;
				}
				if (item.point != vector2 || list.Count == 0)
				{
					vector = item.point - this.up * 0.005f;
					vector2 = item.point;
					num = 0;
					list.Add(item);
				}
				else
				{
					vector -= this.up * 0.001f;
					num++;
					if (num > 10)
					{
						goto Block_5;
					}
				}
			}
			goto IL_15A;
			Block_5:
			global::UnityEngine.Debug.LogError(string.Concat(new object[]
			{
				"Infinite Loop when raycasting. Please report this error (arongranberg.com)\n",
				vector,
				" : ",
				vector2
			}));
			IL_15A:
			return list.ToArray();
		}

		public void DeserializeSettingsCompatibility(global::Pathfinding.Serialization.GraphSerializationContext ctx)
		{
			this.type = (global::Pathfinding.ColliderType)ctx.reader.ReadInt32();
			this.diameter = ctx.reader.ReadSingle();
			this.height = ctx.reader.ReadSingle();
			this.collisionOffset = ctx.reader.ReadSingle();
			this.rayDirection = (global::Pathfinding.RayDirection)ctx.reader.ReadInt32();
			this.mask = ctx.reader.ReadInt32();
			this.heightMask = ctx.reader.ReadInt32();
			this.fromHeight = ctx.reader.ReadSingle();
			this.thickRaycast = ctx.reader.ReadBoolean();
			this.thickRaycastDiameter = ctx.reader.ReadSingle();
			this.unwalkableWhenNoGround = ctx.reader.ReadBoolean();
			this.use2D = ctx.reader.ReadBoolean();
			this.collisionCheck = ctx.reader.ReadBoolean();
			this.heightCheck = ctx.reader.ReadBoolean();
		}

		public const float RaycastErrorMargin = 0.005f;

		public global::Pathfinding.ColliderType type = global::Pathfinding.ColliderType.Capsule;

		public float diameter = 1f;

		public float height = 2f;

		public float collisionOffset;

		public global::Pathfinding.RayDirection rayDirection = global::Pathfinding.RayDirection.Both;

		public global::UnityEngine.LayerMask mask;

		public global::UnityEngine.LayerMask heightMask = -1;

		public float fromHeight = 100f;

		public bool thickRaycast;

		public float thickRaycastDiameter = 1f;

		public bool unwalkableWhenNoGround = true;

		public bool use2D;

		public bool collisionCheck = true;

		public bool heightCheck = true;

		public global::UnityEngine.Vector3 up;

		private global::UnityEngine.Vector3 upheight;

		private float finalRadius;

		private float finalRaycastRadius;
	}
}
