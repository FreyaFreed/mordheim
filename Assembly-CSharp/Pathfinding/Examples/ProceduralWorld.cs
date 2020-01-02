using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Examples
{
	[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_examples_1_1_procedural_world.php")]
	public class ProceduralWorld : global::UnityEngine.MonoBehaviour
	{
		private void Start()
		{
			this.Update();
			global::AstarPath.active.Scan();
			base.StartCoroutine(this.GenerateTiles());
		}

		private void Update()
		{
			global::Pathfinding.Int2 @int = new global::Pathfinding.Int2(global::UnityEngine.Mathf.RoundToInt((this.target.position.x - this.tileSize * 0.5f) / this.tileSize), global::UnityEngine.Mathf.RoundToInt((this.target.position.z - this.tileSize * 0.5f) / this.tileSize));
			this.range = ((this.range >= 1) ? this.range : 1);
			bool flag = true;
			while (flag)
			{
				flag = false;
				foreach (global::System.Collections.Generic.KeyValuePair<global::Pathfinding.Int2, global::Pathfinding.Examples.ProceduralWorld.ProceduralTile> keyValuePair in this.tiles)
				{
					if (global::UnityEngine.Mathf.Abs(keyValuePair.Key.x - @int.x) > this.range || global::UnityEngine.Mathf.Abs(keyValuePair.Key.y - @int.y) > this.range)
					{
						keyValuePair.Value.Destroy();
						this.tiles.Remove(keyValuePair.Key);
						flag = true;
						break;
					}
				}
			}
			for (int i = @int.x - this.range; i <= @int.x + this.range; i++)
			{
				for (int j = @int.y - this.range; j <= @int.y + this.range; j++)
				{
					if (!this.tiles.ContainsKey(new global::Pathfinding.Int2(i, j)))
					{
						global::Pathfinding.Examples.ProceduralWorld.ProceduralTile proceduralTile = new global::Pathfinding.Examples.ProceduralWorld.ProceduralTile(this, i, j);
						global::System.Collections.IEnumerator enumerator2 = proceduralTile.Generate();
						enumerator2.MoveNext();
						this.tileGenerationQueue.Enqueue(enumerator2);
						this.tiles.Add(new global::Pathfinding.Int2(i, j), proceduralTile);
					}
				}
			}
			for (int k = @int.x - 1; k <= @int.x + 1; k++)
			{
				for (int l = @int.y - 1; l <= @int.y + 1; l++)
				{
					this.tiles[new global::Pathfinding.Int2(k, l)].ForceFinish();
				}
			}
		}

		private global::System.Collections.IEnumerator GenerateTiles()
		{
			for (;;)
			{
				if (this.tileGenerationQueue.Count > 0)
				{
					global::System.Collections.IEnumerator generator = this.tileGenerationQueue.Dequeue();
					yield return base.StartCoroutine(generator);
				}
				yield return null;
			}
			yield break;
		}

		public global::UnityEngine.Transform target;

		public global::Pathfinding.Examples.ProceduralWorld.ProceduralPrefab[] prefabs;

		public int range = 1;

		public float tileSize = 100f;

		public int subTiles = 20;

		public bool staticBatching;

		private global::System.Collections.Generic.Queue<global::System.Collections.IEnumerator> tileGenerationQueue = new global::System.Collections.Generic.Queue<global::System.Collections.IEnumerator>();

		private global::System.Collections.Generic.Dictionary<global::Pathfinding.Int2, global::Pathfinding.Examples.ProceduralWorld.ProceduralTile> tiles = new global::System.Collections.Generic.Dictionary<global::Pathfinding.Int2, global::Pathfinding.Examples.ProceduralWorld.ProceduralTile>();

		[global::System.Serializable]
		public class ProceduralPrefab
		{
			public global::UnityEngine.GameObject prefab;

			public float density;

			public float perlin;

			public float perlinPower = 1f;

			public global::UnityEngine.Vector2 perlinOffset = global::UnityEngine.Vector2.zero;

			public float perlinScale = 1f;

			public float random = 1f;

			public bool singleFixed;
		}

		private class ProceduralTile
		{
			public ProceduralTile(global::Pathfinding.Examples.ProceduralWorld world, int x, int z)
			{
				this.x = x;
				this.z = z;
				this.world = world;
				this.rnd = new global::System.Random(x * 10007 ^ z * 36007);
			}

			public bool destroyed { get; private set; }

			public global::System.Collections.IEnumerator Generate()
			{
				this.ie = this.InternalGenerate();
				global::UnityEngine.GameObject rt = new global::UnityEngine.GameObject(string.Concat(new object[]
				{
					"Tile ",
					this.x,
					" ",
					this.z
				}));
				this.root = rt.transform;
				while (this.ie != null && this.root != null && this.ie.MoveNext())
				{
					yield return this.ie.Current;
				}
				this.ie = null;
				yield break;
			}

			public void ForceFinish()
			{
				while (this.ie != null && this.root != null && this.ie.MoveNext())
				{
				}
				this.ie = null;
			}

			private global::UnityEngine.Vector3 RandomInside()
			{
				return new global::UnityEngine.Vector3
				{
					x = ((float)this.x + (float)this.rnd.NextDouble()) * this.world.tileSize,
					z = ((float)this.z + (float)this.rnd.NextDouble()) * this.world.tileSize
				};
			}

			private global::UnityEngine.Vector3 RandomInside(float px, float pz)
			{
				return new global::UnityEngine.Vector3
				{
					x = (px + (float)this.rnd.NextDouble() / (float)this.world.subTiles) * this.world.tileSize,
					z = (pz + (float)this.rnd.NextDouble() / (float)this.world.subTiles) * this.world.tileSize
				};
			}

			private global::UnityEngine.Quaternion RandomYRot()
			{
				return global::UnityEngine.Quaternion.Euler(360f * (float)this.rnd.NextDouble(), 0f, 360f * (float)this.rnd.NextDouble());
			}

			private global::System.Collections.IEnumerator InternalGenerate()
			{
				global::UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"Generating tile ",
					this.x,
					", ",
					this.z
				}));
				int counter = 0;
				float[,] ditherMap = new float[this.world.subTiles + 2, this.world.subTiles + 2];
				for (int i = 0; i < this.world.prefabs.Length; i++)
				{
					global::Pathfinding.Examples.ProceduralWorld.ProceduralPrefab pref = this.world.prefabs[i];
					if (pref.singleFixed)
					{
						global::UnityEngine.Vector3 p = new global::UnityEngine.Vector3(((float)this.x + 0.5f) * this.world.tileSize, 0f, ((float)this.z + 0.5f) * this.world.tileSize);
						global::UnityEngine.GameObject ob = global::UnityEngine.Object.Instantiate(pref.prefab, p, global::UnityEngine.Quaternion.identity) as global::UnityEngine.GameObject;
						ob.transform.parent = this.root;
					}
					else
					{
						float subSize = this.world.tileSize / (float)this.world.subTiles;
						for (int sx = 0; sx < this.world.subTiles; sx++)
						{
							for (int sz = 0; sz < this.world.subTiles; sz++)
							{
								ditherMap[sx + 1, sz + 1] = 0f;
							}
						}
						for (int sx2 = 0; sx2 < this.world.subTiles; sx2++)
						{
							for (int sz2 = 0; sz2 < this.world.subTiles; sz2++)
							{
								float px = (float)this.x + (float)sx2 / (float)this.world.subTiles;
								float pz = (float)this.z + (float)sz2 / (float)this.world.subTiles;
								float perl = global::UnityEngine.Mathf.Pow(global::UnityEngine.Mathf.PerlinNoise((px + pref.perlinOffset.x) * pref.perlinScale, (pz + pref.perlinOffset.y) * pref.perlinScale), pref.perlinPower);
								float density = pref.density * global::UnityEngine.Mathf.Lerp(1f, perl, pref.perlin) * global::UnityEngine.Mathf.Lerp(1f, (float)this.rnd.NextDouble(), pref.random);
								float fcount = subSize * subSize * density + ditherMap[sx2 + 1, sz2 + 1];
								int count = global::UnityEngine.Mathf.RoundToInt(fcount);
								ditherMap[sx2 + 1 + 1, sz2 + 1] += 0.4375f * (fcount - (float)count);
								ditherMap[sx2 + 1 - 1, sz2 + 1 + 1] += 0.1875f * (fcount - (float)count);
								ditherMap[sx2 + 1, sz2 + 1 + 1] += 0.3125f * (fcount - (float)count);
								ditherMap[sx2 + 1 + 1, sz2 + 1 + 1] += 0.0625f * (fcount - (float)count);
								for (int j = 0; j < count; j++)
								{
									global::UnityEngine.Vector3 p2 = this.RandomInside(px, pz);
									global::UnityEngine.GameObject ob2 = global::UnityEngine.Object.Instantiate(pref.prefab, p2, this.RandomYRot()) as global::UnityEngine.GameObject;
									ob2.transform.parent = this.root;
									counter++;
									if (counter % 2 == 0)
									{
										yield return null;
									}
								}
							}
						}
					}
				}
				ditherMap = null;
				yield return null;
				yield return null;
				if (global::UnityEngine.Application.HasProLicense() && this.world.staticBatching)
				{
					global::UnityEngine.StaticBatchingUtility.Combine(this.root.gameObject);
				}
				yield break;
			}

			public void Destroy()
			{
				if (this.root != null)
				{
					global::UnityEngine.Debug.Log(string.Concat(new object[]
					{
						"Destroying tile ",
						this.x,
						", ",
						this.z
					}));
					global::UnityEngine.Object.Destroy(this.root.gameObject);
					this.root = null;
				}
				this.ie = null;
			}

			private int x;

			private int z;

			private global::System.Random rnd;

			private bool staticBatching;

			private global::Pathfinding.Examples.ProceduralWorld world;

			private global::UnityEngine.Transform root;

			private global::System.Collections.IEnumerator ie;
		}
	}
}
