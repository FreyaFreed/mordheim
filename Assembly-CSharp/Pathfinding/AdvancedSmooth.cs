using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	[global::UnityEngine.AddComponentMenu("Pathfinding/Modifiers/Advanced Smooth")]
	[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_advanced_smooth.php")]
	[global::System.Serializable]
	public class AdvancedSmooth : global::Pathfinding.MonoModifier
	{
		public override int Order
		{
			get
			{
				return 40;
			}
		}

		public override void Apply(global::Pathfinding.Path p)
		{
			global::UnityEngine.Vector3[] array = p.vectorPath.ToArray();
			if (array == null || array.Length <= 2)
			{
				return;
			}
			global::System.Collections.Generic.List<global::UnityEngine.Vector3> list = new global::System.Collections.Generic.List<global::UnityEngine.Vector3>();
			list.Add(array[0]);
			global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius = this.turningRadius;
			for (int i = 1; i < array.Length - 1; i++)
			{
				global::System.Collections.Generic.List<global::Pathfinding.AdvancedSmooth.Turn> turnList = new global::System.Collections.Generic.List<global::Pathfinding.AdvancedSmooth.Turn>();
				global::Pathfinding.AdvancedSmooth.TurnConstructor.Setup(i, array);
				this.turnConstruct1.Prepare(i, array);
				this.turnConstruct2.Prepare(i, array);
				global::Pathfinding.AdvancedSmooth.TurnConstructor.PostPrepare();
				if (i == 1)
				{
					this.turnConstruct1.PointToTangent(turnList);
					this.turnConstruct2.PointToTangent(turnList);
				}
				else
				{
					this.turnConstruct1.TangentToTangent(turnList);
					this.turnConstruct2.TangentToTangent(turnList);
				}
				this.EvaluatePaths(turnList, list);
				if (i == array.Length - 2)
				{
					this.turnConstruct1.TangentToPoint(turnList);
					this.turnConstruct2.TangentToPoint(turnList);
				}
				this.EvaluatePaths(turnList, list);
			}
			list.Add(array[array.Length - 1]);
			p.vectorPath = list;
		}

		private void EvaluatePaths(global::System.Collections.Generic.List<global::Pathfinding.AdvancedSmooth.Turn> turnList, global::System.Collections.Generic.List<global::UnityEngine.Vector3> output)
		{
			turnList.Sort();
			for (int i = 0; i < turnList.Count; i++)
			{
				if (i == 0)
				{
					turnList[i].GetPath(output);
				}
			}
			turnList.Clear();
			if (global::Pathfinding.AdvancedSmooth.TurnConstructor.changedPreviousTangent)
			{
				this.turnConstruct1.OnTangentUpdate();
				this.turnConstruct2.OnTangentUpdate();
			}
		}

		public float turningRadius = 1f;

		public global::Pathfinding.AdvancedSmooth.MaxTurn turnConstruct1 = new global::Pathfinding.AdvancedSmooth.MaxTurn();

		public global::Pathfinding.AdvancedSmooth.ConstantTurn turnConstruct2 = new global::Pathfinding.AdvancedSmooth.ConstantTurn();

		[global::System.Serializable]
		public class MaxTurn : global::Pathfinding.AdvancedSmooth.TurnConstructor
		{
			public override void OnTangentUpdate()
			{
				this.rightCircleCenter = global::Pathfinding.AdvancedSmooth.TurnConstructor.current + global::Pathfinding.AdvancedSmooth.TurnConstructor.normal * global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius;
				this.leftCircleCenter = global::Pathfinding.AdvancedSmooth.TurnConstructor.current - global::Pathfinding.AdvancedSmooth.TurnConstructor.normal * global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius;
				this.vaRight = base.Atan2(global::Pathfinding.AdvancedSmooth.TurnConstructor.current - this.rightCircleCenter);
				this.vaLeft = this.vaRight + 3.1415926535897931;
			}

			public override void Prepare(int i, global::UnityEngine.Vector3[] vectorPath)
			{
				this.preRightCircleCenter = this.rightCircleCenter;
				this.preLeftCircleCenter = this.leftCircleCenter;
				this.rightCircleCenter = global::Pathfinding.AdvancedSmooth.TurnConstructor.current + global::Pathfinding.AdvancedSmooth.TurnConstructor.normal * global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius;
				this.leftCircleCenter = global::Pathfinding.AdvancedSmooth.TurnConstructor.current - global::Pathfinding.AdvancedSmooth.TurnConstructor.normal * global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius;
				this.preVaRight = this.vaRight;
				this.preVaLeft = this.vaLeft;
				this.vaRight = base.Atan2(global::Pathfinding.AdvancedSmooth.TurnConstructor.current - this.rightCircleCenter);
				this.vaLeft = this.vaRight + 3.1415926535897931;
			}

			public override void TangentToTangent(global::System.Collections.Generic.List<global::Pathfinding.AdvancedSmooth.Turn> turnList)
			{
				this.alfaRightRight = base.Atan2(this.rightCircleCenter - this.preRightCircleCenter);
				this.alfaLeftLeft = base.Atan2(this.leftCircleCenter - this.preLeftCircleCenter);
				this.alfaRightLeft = base.Atan2(this.leftCircleCenter - this.preRightCircleCenter);
				this.alfaLeftRight = base.Atan2(this.rightCircleCenter - this.preLeftCircleCenter);
				double num = (double)(this.leftCircleCenter - this.preRightCircleCenter).magnitude;
				double num2 = (double)(this.rightCircleCenter - this.preLeftCircleCenter).magnitude;
				bool flag = false;
				bool flag2 = false;
				if (num < (double)(global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius * 2f))
				{
					num = (double)(global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius * 2f);
					flag = true;
				}
				if (num2 < (double)(global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius * 2f))
				{
					num2 = (double)(global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius * 2f);
					flag2 = true;
				}
				this.deltaRightLeft = ((!flag) ? (1.5707963267948966 - global::System.Math.Asin((double)(global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius * 2f) / num)) : 0.0);
				this.deltaLeftRight = ((!flag2) ? (1.5707963267948966 - global::System.Math.Asin((double)(global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius * 2f) / num2)) : 0.0);
				this.betaRightRight = base.ClockwiseAngle(this.preVaRight, this.alfaRightRight - 1.5707963267948966);
				this.betaRightLeft = base.ClockwiseAngle(this.preVaRight, this.alfaRightLeft - this.deltaRightLeft);
				this.betaLeftRight = base.CounterClockwiseAngle(this.preVaLeft, this.alfaLeftRight + this.deltaLeftRight);
				this.betaLeftLeft = base.CounterClockwiseAngle(this.preVaLeft, this.alfaLeftLeft + 1.5707963267948966);
				this.betaRightRight += base.ClockwiseAngle(this.alfaRightRight - 1.5707963267948966, this.vaRight);
				this.betaRightLeft += base.CounterClockwiseAngle(this.alfaRightLeft + this.deltaRightLeft, this.vaLeft);
				this.betaLeftRight += base.ClockwiseAngle(this.alfaLeftRight - this.deltaLeftRight, this.vaRight);
				this.betaLeftLeft += base.CounterClockwiseAngle(this.alfaLeftLeft + 1.5707963267948966, this.vaLeft);
				this.betaRightRight = base.GetLengthFromAngle(this.betaRightRight, (double)global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius);
				this.betaRightLeft = base.GetLengthFromAngle(this.betaRightLeft, (double)global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius);
				this.betaLeftRight = base.GetLengthFromAngle(this.betaLeftRight, (double)global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius);
				this.betaLeftLeft = base.GetLengthFromAngle(this.betaLeftLeft, (double)global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius);
				global::UnityEngine.Vector3 a = base.AngleToVector(this.alfaRightRight - 1.5707963267948966) * global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius + this.preRightCircleCenter;
				global::UnityEngine.Vector3 a2 = base.AngleToVector(this.alfaRightLeft - this.deltaRightLeft) * global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius + this.preRightCircleCenter;
				global::UnityEngine.Vector3 a3 = base.AngleToVector(this.alfaLeftRight + this.deltaLeftRight) * global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius + this.preLeftCircleCenter;
				global::UnityEngine.Vector3 a4 = base.AngleToVector(this.alfaLeftLeft + 1.5707963267948966) * global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius + this.preLeftCircleCenter;
				global::UnityEngine.Vector3 b = base.AngleToVector(this.alfaRightRight - 1.5707963267948966) * global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius + this.rightCircleCenter;
				global::UnityEngine.Vector3 b2 = base.AngleToVector(this.alfaRightLeft - this.deltaRightLeft + 3.1415926535897931) * global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius + this.leftCircleCenter;
				global::UnityEngine.Vector3 b3 = base.AngleToVector(this.alfaLeftRight + this.deltaLeftRight + 3.1415926535897931) * global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius + this.rightCircleCenter;
				global::UnityEngine.Vector3 b4 = base.AngleToVector(this.alfaLeftLeft + 1.5707963267948966) * global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius + this.leftCircleCenter;
				this.betaRightRight += (double)(a - b).magnitude;
				this.betaRightLeft += (double)(a2 - b2).magnitude;
				this.betaLeftRight += (double)(a3 - b3).magnitude;
				this.betaLeftLeft += (double)(a4 - b4).magnitude;
				if (flag)
				{
					this.betaRightLeft += 10000000.0;
				}
				if (flag2)
				{
					this.betaLeftRight += 10000000.0;
				}
				turnList.Add(new global::Pathfinding.AdvancedSmooth.Turn((float)this.betaRightRight, this, 2));
				turnList.Add(new global::Pathfinding.AdvancedSmooth.Turn((float)this.betaRightLeft, this, 3));
				turnList.Add(new global::Pathfinding.AdvancedSmooth.Turn((float)this.betaLeftRight, this, 4));
				turnList.Add(new global::Pathfinding.AdvancedSmooth.Turn((float)this.betaLeftLeft, this, 5));
			}

			public override void PointToTangent(global::System.Collections.Generic.List<global::Pathfinding.AdvancedSmooth.Turn> turnList)
			{
				bool flag = false;
				bool flag2 = false;
				float magnitude = (global::Pathfinding.AdvancedSmooth.TurnConstructor.prev - this.rightCircleCenter).magnitude;
				float magnitude2 = (global::Pathfinding.AdvancedSmooth.TurnConstructor.prev - this.leftCircleCenter).magnitude;
				if (magnitude < global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius)
				{
					flag = true;
				}
				if (magnitude2 < global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius)
				{
					flag2 = true;
				}
				double num = (!flag) ? base.Atan2(global::Pathfinding.AdvancedSmooth.TurnConstructor.prev - this.rightCircleCenter) : 0.0;
				double num2 = (!flag) ? (1.5707963267948966 - global::System.Math.Asin((double)(global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius / (global::Pathfinding.AdvancedSmooth.TurnConstructor.prev - this.rightCircleCenter).magnitude))) : 0.0;
				this.gammaRight = num + num2;
				double num3 = (!flag) ? base.ClockwiseAngle(this.gammaRight, this.vaRight) : 0.0;
				double num4 = (!flag2) ? base.Atan2(global::Pathfinding.AdvancedSmooth.TurnConstructor.prev - this.leftCircleCenter) : 0.0;
				double num5 = (!flag2) ? (1.5707963267948966 - global::System.Math.Asin((double)(global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius / (global::Pathfinding.AdvancedSmooth.TurnConstructor.prev - this.leftCircleCenter).magnitude))) : 0.0;
				this.gammaLeft = num4 - num5;
				double num6 = (!flag2) ? base.CounterClockwiseAngle(this.gammaLeft, this.vaLeft) : 0.0;
				if (!flag)
				{
					turnList.Add(new global::Pathfinding.AdvancedSmooth.Turn((float)num3, this, 0));
				}
				if (!flag2)
				{
					turnList.Add(new global::Pathfinding.AdvancedSmooth.Turn((float)num6, this, 1));
				}
			}

			public override void TangentToPoint(global::System.Collections.Generic.List<global::Pathfinding.AdvancedSmooth.Turn> turnList)
			{
				bool flag = false;
				bool flag2 = false;
				float magnitude = (global::Pathfinding.AdvancedSmooth.TurnConstructor.next - this.rightCircleCenter).magnitude;
				float magnitude2 = (global::Pathfinding.AdvancedSmooth.TurnConstructor.next - this.leftCircleCenter).magnitude;
				if (magnitude < global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius)
				{
					flag = true;
				}
				if (magnitude2 < global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius)
				{
					flag2 = true;
				}
				if (!flag)
				{
					double num = base.Atan2(global::Pathfinding.AdvancedSmooth.TurnConstructor.next - this.rightCircleCenter);
					double num2 = 1.5707963267948966 - global::System.Math.Asin((double)(global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius / magnitude));
					this.gammaRight = num - num2;
					double num3 = base.ClockwiseAngle(this.vaRight, this.gammaRight);
					turnList.Add(new global::Pathfinding.AdvancedSmooth.Turn((float)num3, this, 6));
				}
				if (!flag2)
				{
					double num4 = base.Atan2(global::Pathfinding.AdvancedSmooth.TurnConstructor.next - this.leftCircleCenter);
					double num5 = 1.5707963267948966 - global::System.Math.Asin((double)(global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius / magnitude2));
					this.gammaLeft = num4 + num5;
					double num6 = base.CounterClockwiseAngle(this.vaLeft, this.gammaLeft);
					turnList.Add(new global::Pathfinding.AdvancedSmooth.Turn((float)num6, this, 7));
				}
			}

			public override void GetPath(global::Pathfinding.AdvancedSmooth.Turn turn, global::System.Collections.Generic.List<global::UnityEngine.Vector3> output)
			{
				switch (turn.id)
				{
				case 0:
					base.AddCircleSegment(this.gammaRight, this.vaRight, true, this.rightCircleCenter, output, global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius);
					break;
				case 1:
					base.AddCircleSegment(this.gammaLeft, this.vaLeft, false, this.leftCircleCenter, output, global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius);
					break;
				case 2:
					base.AddCircleSegment(this.preVaRight, this.alfaRightRight - 1.5707963267948966, true, this.preRightCircleCenter, output, global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius);
					base.AddCircleSegment(this.alfaRightRight - 1.5707963267948966, this.vaRight, true, this.rightCircleCenter, output, global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius);
					break;
				case 3:
					base.AddCircleSegment(this.preVaRight, this.alfaRightLeft - this.deltaRightLeft, true, this.preRightCircleCenter, output, global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius);
					base.AddCircleSegment(this.alfaRightLeft - this.deltaRightLeft + 3.1415926535897931, this.vaLeft, false, this.leftCircleCenter, output, global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius);
					break;
				case 4:
					base.AddCircleSegment(this.preVaLeft, this.alfaLeftRight + this.deltaLeftRight, false, this.preLeftCircleCenter, output, global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius);
					base.AddCircleSegment(this.alfaLeftRight + this.deltaLeftRight + 3.1415926535897931, this.vaRight, true, this.rightCircleCenter, output, global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius);
					break;
				case 5:
					base.AddCircleSegment(this.preVaLeft, this.alfaLeftLeft + 1.5707963267948966, false, this.preLeftCircleCenter, output, global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius);
					base.AddCircleSegment(this.alfaLeftLeft + 1.5707963267948966, this.vaLeft, false, this.leftCircleCenter, output, global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius);
					break;
				case 6:
					base.AddCircleSegment(this.vaRight, this.gammaRight, true, this.rightCircleCenter, output, global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius);
					break;
				case 7:
					base.AddCircleSegment(this.vaLeft, this.gammaLeft, false, this.leftCircleCenter, output, global::Pathfinding.AdvancedSmooth.TurnConstructor.turningRadius);
					break;
				}
			}

			private global::UnityEngine.Vector3 preRightCircleCenter = global::UnityEngine.Vector3.zero;

			private global::UnityEngine.Vector3 preLeftCircleCenter = global::UnityEngine.Vector3.zero;

			private global::UnityEngine.Vector3 rightCircleCenter;

			private global::UnityEngine.Vector3 leftCircleCenter;

			private double vaRight;

			private double vaLeft;

			private double preVaLeft;

			private double preVaRight;

			private double gammaLeft;

			private double gammaRight;

			private double betaRightRight;

			private double betaRightLeft;

			private double betaLeftRight;

			private double betaLeftLeft;

			private double deltaRightLeft;

			private double deltaLeftRight;

			private double alfaRightRight;

			private double alfaLeftLeft;

			private double alfaRightLeft;

			private double alfaLeftRight;
		}

		[global::System.Serializable]
		public class ConstantTurn : global::Pathfinding.AdvancedSmooth.TurnConstructor
		{
			public override void Prepare(int i, global::UnityEngine.Vector3[] vectorPath)
			{
			}

			public override void TangentToTangent(global::System.Collections.Generic.List<global::Pathfinding.AdvancedSmooth.Turn> turnList)
			{
				global::UnityEngine.Vector3 dir = global::UnityEngine.Vector3.Cross(global::Pathfinding.AdvancedSmooth.TurnConstructor.t1, global::UnityEngine.Vector3.up);
				global::UnityEngine.Vector3 vector = global::Pathfinding.AdvancedSmooth.TurnConstructor.current - global::Pathfinding.AdvancedSmooth.TurnConstructor.prev;
				global::UnityEngine.Vector3 start = vector * 0.5f + global::Pathfinding.AdvancedSmooth.TurnConstructor.prev;
				vector = global::UnityEngine.Vector3.Cross(vector, global::UnityEngine.Vector3.up);
				bool flag;
				this.circleCenter = global::Pathfinding.VectorMath.LineDirIntersectionPointXZ(global::Pathfinding.AdvancedSmooth.TurnConstructor.prev, dir, start, vector, out flag);
				if (!flag)
				{
					return;
				}
				this.gamma1 = base.Atan2(global::Pathfinding.AdvancedSmooth.TurnConstructor.prev - this.circleCenter);
				this.gamma2 = base.Atan2(global::Pathfinding.AdvancedSmooth.TurnConstructor.current - this.circleCenter);
				this.clockwise = !global::Pathfinding.VectorMath.RightOrColinearXZ(this.circleCenter, global::Pathfinding.AdvancedSmooth.TurnConstructor.prev, global::Pathfinding.AdvancedSmooth.TurnConstructor.prev + global::Pathfinding.AdvancedSmooth.TurnConstructor.t1);
				double num = (!this.clockwise) ? base.CounterClockwiseAngle(this.gamma1, this.gamma2) : base.ClockwiseAngle(this.gamma1, this.gamma2);
				num = base.GetLengthFromAngle(num, (double)(this.circleCenter - global::Pathfinding.AdvancedSmooth.TurnConstructor.current).magnitude);
				turnList.Add(new global::Pathfinding.AdvancedSmooth.Turn((float)num, this, 0));
			}

			public override void GetPath(global::Pathfinding.AdvancedSmooth.Turn turn, global::System.Collections.Generic.List<global::UnityEngine.Vector3> output)
			{
				base.AddCircleSegment(this.gamma1, this.gamma2, this.clockwise, this.circleCenter, output, (this.circleCenter - global::Pathfinding.AdvancedSmooth.TurnConstructor.current).magnitude);
				global::Pathfinding.AdvancedSmooth.TurnConstructor.normal = (global::Pathfinding.AdvancedSmooth.TurnConstructor.current - this.circleCenter).normalized;
				global::Pathfinding.AdvancedSmooth.TurnConstructor.t2 = global::UnityEngine.Vector3.Cross(global::Pathfinding.AdvancedSmooth.TurnConstructor.normal, global::UnityEngine.Vector3.up).normalized;
				global::Pathfinding.AdvancedSmooth.TurnConstructor.normal = -global::Pathfinding.AdvancedSmooth.TurnConstructor.normal;
				if (!this.clockwise)
				{
					global::Pathfinding.AdvancedSmooth.TurnConstructor.t2 = -global::Pathfinding.AdvancedSmooth.TurnConstructor.t2;
					global::Pathfinding.AdvancedSmooth.TurnConstructor.normal = -global::Pathfinding.AdvancedSmooth.TurnConstructor.normal;
				}
				global::Pathfinding.AdvancedSmooth.TurnConstructor.changedPreviousTangent = true;
			}

			private global::UnityEngine.Vector3 circleCenter;

			private double gamma1;

			private double gamma2;

			private bool clockwise;
		}

		public abstract class TurnConstructor
		{
			public abstract void Prepare(int i, global::UnityEngine.Vector3[] vectorPath);

			public virtual void OnTangentUpdate()
			{
			}

			public virtual void PointToTangent(global::System.Collections.Generic.List<global::Pathfinding.AdvancedSmooth.Turn> turnList)
			{
			}

			public virtual void TangentToPoint(global::System.Collections.Generic.List<global::Pathfinding.AdvancedSmooth.Turn> turnList)
			{
			}

			public virtual void TangentToTangent(global::System.Collections.Generic.List<global::Pathfinding.AdvancedSmooth.Turn> turnList)
			{
			}

			public abstract void GetPath(global::Pathfinding.AdvancedSmooth.Turn turn, global::System.Collections.Generic.List<global::UnityEngine.Vector3> output);

			public static void Setup(int i, global::UnityEngine.Vector3[] vectorPath)
			{
				global::Pathfinding.AdvancedSmooth.TurnConstructor.current = vectorPath[i];
				global::Pathfinding.AdvancedSmooth.TurnConstructor.prev = vectorPath[i - 1];
				global::Pathfinding.AdvancedSmooth.TurnConstructor.next = vectorPath[i + 1];
				global::Pathfinding.AdvancedSmooth.TurnConstructor.prev.y = global::Pathfinding.AdvancedSmooth.TurnConstructor.current.y;
				global::Pathfinding.AdvancedSmooth.TurnConstructor.next.y = global::Pathfinding.AdvancedSmooth.TurnConstructor.current.y;
				global::Pathfinding.AdvancedSmooth.TurnConstructor.t1 = global::Pathfinding.AdvancedSmooth.TurnConstructor.t2;
				global::Pathfinding.AdvancedSmooth.TurnConstructor.t2 = (global::Pathfinding.AdvancedSmooth.TurnConstructor.next - global::Pathfinding.AdvancedSmooth.TurnConstructor.current).normalized - (global::Pathfinding.AdvancedSmooth.TurnConstructor.prev - global::Pathfinding.AdvancedSmooth.TurnConstructor.current).normalized;
				global::Pathfinding.AdvancedSmooth.TurnConstructor.t2 = global::Pathfinding.AdvancedSmooth.TurnConstructor.t2.normalized;
				global::Pathfinding.AdvancedSmooth.TurnConstructor.prevNormal = global::Pathfinding.AdvancedSmooth.TurnConstructor.normal;
				global::Pathfinding.AdvancedSmooth.TurnConstructor.normal = global::UnityEngine.Vector3.Cross(global::Pathfinding.AdvancedSmooth.TurnConstructor.t2, global::UnityEngine.Vector3.up);
				global::Pathfinding.AdvancedSmooth.TurnConstructor.normal = global::Pathfinding.AdvancedSmooth.TurnConstructor.normal.normalized;
			}

			public static void PostPrepare()
			{
				global::Pathfinding.AdvancedSmooth.TurnConstructor.changedPreviousTangent = false;
			}

			public void AddCircleSegment(double startAngle, double endAngle, bool clockwise, global::UnityEngine.Vector3 center, global::System.Collections.Generic.List<global::UnityEngine.Vector3> output, float radius)
			{
				double num = 0.062831853071795868;
				if (clockwise)
				{
					while (endAngle > startAngle + 6.2831853071795862)
					{
						endAngle -= 6.2831853071795862;
					}
					while (endAngle < startAngle)
					{
						endAngle += 6.2831853071795862;
					}
				}
				else
				{
					while (endAngle < startAngle - 6.2831853071795862)
					{
						endAngle += 6.2831853071795862;
					}
					while (endAngle > startAngle)
					{
						endAngle -= 6.2831853071795862;
					}
				}
				if (clockwise)
				{
					for (double num2 = startAngle; num2 < endAngle; num2 += num)
					{
						output.Add(this.AngleToVector(num2) * radius + center);
					}
				}
				else
				{
					for (double num3 = startAngle; num3 > endAngle; num3 -= num)
					{
						output.Add(this.AngleToVector(num3) * radius + center);
					}
				}
				output.Add(this.AngleToVector(endAngle) * radius + center);
			}

			public void DebugCircleSegment(global::UnityEngine.Vector3 center, double startAngle, double endAngle, double radius, global::UnityEngine.Color color)
			{
				double num = 0.062831853071795868;
				while (endAngle < startAngle)
				{
					endAngle += 6.2831853071795862;
				}
				global::UnityEngine.Vector3 start = this.AngleToVector(startAngle) * (float)radius + center;
				for (double num2 = startAngle + num; num2 < endAngle; num2 += num)
				{
					global::UnityEngine.Debug.DrawLine(start, this.AngleToVector(num2) * (float)radius + center);
				}
				global::UnityEngine.Debug.DrawLine(start, this.AngleToVector(endAngle) * (float)radius + center);
			}

			public void DebugCircle(global::UnityEngine.Vector3 center, double radius, global::UnityEngine.Color color)
			{
				double num = 0.062831853071795868;
				global::UnityEngine.Vector3 start = this.AngleToVector(-num) * (float)radius + center;
				for (double num2 = 0.0; num2 < 6.2831853071795862; num2 += num)
				{
					global::UnityEngine.Vector3 vector = this.AngleToVector(num2) * (float)radius + center;
					global::UnityEngine.Debug.DrawLine(start, vector, color);
					start = vector;
				}
			}

			public double GetLengthFromAngle(double angle, double radius)
			{
				return radius * angle;
			}

			public double ClockwiseAngle(double from, double to)
			{
				return this.ClampAngle(to - from);
			}

			public double CounterClockwiseAngle(double from, double to)
			{
				return this.ClampAngle(from - to);
			}

			public global::UnityEngine.Vector3 AngleToVector(double a)
			{
				return new global::UnityEngine.Vector3((float)global::System.Math.Cos(a), 0f, (float)global::System.Math.Sin(a));
			}

			public double ToDegrees(double rad)
			{
				return rad * 57.295780181884766;
			}

			public double ClampAngle(double a)
			{
				while (a < 0.0)
				{
					a += 6.2831853071795862;
				}
				while (a > 6.2831853071795862)
				{
					a -= 6.2831853071795862;
				}
				return a;
			}

			public double Atan2(global::UnityEngine.Vector3 v)
			{
				return global::System.Math.Atan2((double)v.z, (double)v.x);
			}

			public const double ThreeSixtyRadians = 6.2831853071795862;

			public float constantBias;

			public float factorBias = 1f;

			public static float turningRadius = 1f;

			public static global::UnityEngine.Vector3 prev;

			public static global::UnityEngine.Vector3 current;

			public static global::UnityEngine.Vector3 next;

			public static global::UnityEngine.Vector3 t1;

			public static global::UnityEngine.Vector3 t2;

			public static global::UnityEngine.Vector3 normal;

			public static global::UnityEngine.Vector3 prevNormal;

			public static bool changedPreviousTangent;
		}

		public struct Turn : global::System.IComparable<global::Pathfinding.AdvancedSmooth.Turn>
		{
			public Turn(float length, global::Pathfinding.AdvancedSmooth.TurnConstructor constructor, int id = 0)
			{
				this.length = length;
				this.id = id;
				this.constructor = constructor;
			}

			public float score
			{
				get
				{
					return this.length * this.constructor.factorBias + this.constructor.constantBias;
				}
			}

			public void GetPath(global::System.Collections.Generic.List<global::UnityEngine.Vector3> output)
			{
				this.constructor.GetPath(this, output);
			}

			public int CompareTo(global::Pathfinding.AdvancedSmooth.Turn t)
			{
				return (t.score <= this.score) ? ((t.score >= this.score) ? 0 : 1) : -1;
			}

			public static bool operator <(global::Pathfinding.AdvancedSmooth.Turn lhs, global::Pathfinding.AdvancedSmooth.Turn rhs)
			{
				return lhs.score < rhs.score;
			}

			public static bool operator >(global::Pathfinding.AdvancedSmooth.Turn lhs, global::Pathfinding.AdvancedSmooth.Turn rhs)
			{
				return lhs.score > rhs.score;
			}

			public float length;

			public int id;

			public global::Pathfinding.AdvancedSmooth.TurnConstructor constructor;
		}
	}
}
