using System;
using UnityEngine;

namespace mset
{
	public class GLUtil
	{
		public static void StripFirstVertex(global::UnityEngine.Vector3 v)
		{
			global::mset.GLUtil.prevStripVertex = v;
		}

		public static void StripFirstVertex3(float x, float y, float z)
		{
			global::mset.GLUtil.prevStripVertex.Set(x, y, z);
		}

		public static void StripVertex3(float x, float y, float z)
		{
			global::UnityEngine.GL.Vertex(global::mset.GLUtil.prevStripVertex);
			global::UnityEngine.GL.Vertex3(x, y, z);
			global::mset.GLUtil.prevStripVertex.Set(x, y, z);
		}

		public static void StripVertex(global::UnityEngine.Vector3 v)
		{
			global::UnityEngine.GL.Vertex(global::mset.GLUtil.prevStripVertex);
			global::UnityEngine.GL.Vertex(v);
			global::mset.GLUtil.prevStripVertex = v;
		}

		public static void DrawCube(global::UnityEngine.Vector3 pos, global::UnityEngine.Vector3 radius)
		{
			global::UnityEngine.Vector3 vector = pos - radius;
			global::UnityEngine.Vector3 vector2 = pos + radius;
			global::UnityEngine.GL.Begin(7);
			global::UnityEngine.GL.Vertex3(vector.x, vector.y, vector.z);
			global::UnityEngine.GL.Vertex3(vector2.x, vector.y, vector.z);
			global::UnityEngine.GL.Vertex3(vector2.x, vector.y, vector2.z);
			global::UnityEngine.GL.Vertex3(vector.x, vector.y, vector2.z);
			global::UnityEngine.GL.Vertex3(vector2.x, vector2.y, vector.z);
			global::UnityEngine.GL.Vertex3(vector.x, vector2.y, vector.z);
			global::UnityEngine.GL.Vertex3(vector.x, vector2.y, vector2.z);
			global::UnityEngine.GL.Vertex3(vector2.x, vector2.y, vector2.z);
			global::UnityEngine.GL.Vertex3(vector2.x, vector.y, vector.z);
			global::UnityEngine.GL.Vertex3(vector2.x, vector2.y, vector.z);
			global::UnityEngine.GL.Vertex3(vector2.x, vector2.y, vector2.z);
			global::UnityEngine.GL.Vertex3(vector2.x, vector.y, vector2.z);
			global::UnityEngine.GL.Vertex3(vector.x, vector2.y, vector.z);
			global::UnityEngine.GL.Vertex3(vector.x, vector.y, vector.z);
			global::UnityEngine.GL.Vertex3(vector.x, vector.y, vector2.z);
			global::UnityEngine.GL.Vertex3(vector.x, vector2.y, vector2.z);
			global::UnityEngine.GL.Vertex3(vector2.x, vector2.y, vector2.z);
			global::UnityEngine.GL.Vertex3(vector.x, vector2.y, vector2.z);
			global::UnityEngine.GL.Vertex3(vector.x, vector.y, vector2.z);
			global::UnityEngine.GL.Vertex3(vector2.x, vector.y, vector2.z);
			global::UnityEngine.GL.Vertex3(vector.x, vector2.y, vector.z);
			global::UnityEngine.GL.Vertex3(vector2.x, vector2.y, vector.z);
			global::UnityEngine.GL.Vertex3(vector2.x, vector.y, vector.z);
			global::UnityEngine.GL.Vertex3(vector.x, vector.y, vector.z);
			global::UnityEngine.GL.End();
		}

		public static void DrawWireCube(global::UnityEngine.Vector3 pos, global::UnityEngine.Vector3 radius)
		{
			global::UnityEngine.Vector3 vector = pos - radius;
			global::UnityEngine.Vector3 vector2 = pos + radius;
			global::UnityEngine.GL.Begin(1);
			global::mset.GLUtil.StripFirstVertex3(vector.x, vector.y, vector.z);
			global::mset.GLUtil.StripVertex3(vector2.x, vector.y, vector.z);
			global::mset.GLUtil.StripVertex3(vector2.x, vector.y, vector2.z);
			global::mset.GLUtil.StripVertex3(vector.x, vector.y, vector2.z);
			global::mset.GLUtil.StripFirstVertex3(vector2.x, vector2.y, vector.z);
			global::mset.GLUtil.StripVertex3(vector.x, vector2.y, vector.z);
			global::mset.GLUtil.StripVertex3(vector.x, vector2.y, vector2.z);
			global::mset.GLUtil.StripVertex3(vector2.x, vector2.y, vector2.z);
			global::mset.GLUtil.StripFirstVertex3(vector2.x, vector.y, vector.z);
			global::mset.GLUtil.StripVertex3(vector2.x, vector2.y, vector.z);
			global::mset.GLUtil.StripVertex3(vector2.x, vector2.y, vector2.z);
			global::mset.GLUtil.StripVertex3(vector2.x, vector.y, vector2.z);
			global::mset.GLUtil.StripFirstVertex3(vector.x, vector2.y, vector.z);
			global::mset.GLUtil.StripVertex3(vector.x, vector.y, vector.z);
			global::mset.GLUtil.StripVertex3(vector.x, vector.y, vector2.z);
			global::mset.GLUtil.StripVertex3(vector.x, vector2.y, vector2.z);
			global::mset.GLUtil.StripFirstVertex3(vector2.x, vector2.y, vector2.z);
			global::mset.GLUtil.StripVertex3(vector.x, vector2.y, vector2.z);
			global::mset.GLUtil.StripVertex3(vector.x, vector.y, vector2.z);
			global::mset.GLUtil.StripVertex3(vector2.x, vector.y, vector2.z);
			global::mset.GLUtil.StripFirstVertex3(vector.x, vector2.y, vector.z);
			global::mset.GLUtil.StripVertex3(vector2.x, vector2.y, vector.z);
			global::mset.GLUtil.StripVertex3(vector2.x, vector.y, vector.z);
			global::mset.GLUtil.StripVertex3(vector.x, vector.y, vector.z);
			global::UnityEngine.GL.End();
		}

		private static global::UnityEngine.Vector3 prevStripVertex = global::UnityEngine.Vector3.zero;
	}
}
