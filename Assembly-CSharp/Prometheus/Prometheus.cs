using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

namespace Prometheus
{
	public class Prometheus : global::PandoraSingleton<global::Prometheus.Prometheus>
	{
		public void SpawnFx(global::Prometheus.OlympusFireStarter starter, global::UnityEngine.Events.UnityAction<global::UnityEngine.GameObject> createdCb)
		{
			this.SpawnFx(starter.fxName, starter.transform, true, createdCb);
		}

		public void SpawnFx(global::Prometheus.OlympusFireStarter starter, global::UnityEngine.Transform root, global::UnityEngine.Events.UnityAction<global::UnityEngine.GameObject> createdCb)
		{
			this.SpawnFx(starter.fxName, root, true, createdCb);
		}

		public void SpawnFx(string name, global::UnityEngine.Transform anchor, bool attached, global::UnityEngine.Events.UnityAction<global::UnityEngine.GameObject> createdCb)
		{
			this.CreateFx(name, delegate(global::Prometheus.OlympusFire fire)
			{
				if (fire == null)
				{
					if (createdCb != null)
					{
						createdCb(null);
					}
					return;
				}
				this.StartFx(fire, anchor, attached);
				if (createdCb != null)
				{
					createdCb(fire.gameObject);
				}
			});
		}

		public void SpawnFx(string name, global::UnityEngine.Vector3 pos)
		{
			this.CreateFx(name, delegate(global::Prometheus.OlympusFire fire)
			{
				if (fire == null)
				{
					return;
				}
				this.StartFx(fire, pos);
			});
		}

		public void SpawnFx(string name, global::Unit unit, global::UnityEngine.Events.UnityAction<global::UnityEngine.GameObject> createdCb)
		{
			if (global::PandoraSingleton<global::MissionManager>.Exists())
			{
				global::UnitController unitController = global::PandoraSingleton<global::MissionManager>.Instance.GetUnitController(unit, false);
				if (unitController != null)
				{
					this.SpawnFx(name, unitController, null, createdCb, default(global::UnityEngine.Vector3), default(global::UnityEngine.Vector3), null);
					return;
				}
			}
			if (global::PandoraSingleton<global::HideoutManager>.Exists())
			{
				global::UnitMenuController unitMenuController = global::PandoraSingleton<global::HideoutManager>.Instance.GetUnitMenuController(unit);
				if (unitMenuController != null)
				{
					this.SpawnFx(name, unitMenuController, null, createdCb, default(global::UnityEngine.Vector3), default(global::UnityEngine.Vector3), null);
					return;
				}
			}
			if (createdCb != null)
			{
				createdCb(null);
			}
		}

		public void SpawnFx(string name, global::UnitMenuController unitCtrlr, global::UnitMenuController endUnitCtrlr, global::UnityEngine.Events.UnityAction<global::UnityEngine.GameObject> createdCb, [global::System.Runtime.InteropServices.Optional] global::UnityEngine.Vector3 startPos, [global::System.Runtime.InteropServices.Optional] global::UnityEngine.Vector3 endPos, global::UnityEngine.Events.UnityAction endAction = null)
		{
			this.CreateFx(name, delegate(global::Prometheus.OlympusFire fire)
			{
				if (fire == null)
				{
					if (createdCb != null)
					{
						createdCb(null);
					}
					return;
				}
				fire.destroyMe = false;
				if (global::PandoraSingleton<global::MissionManager>.Exists() && (!string.IsNullOrEmpty(fire.allyFxName) || !string.IsNullOrEmpty(fire.enemyFxName)))
				{
					bool flag = global::PandoraSingleton<global::MissionManager>.Instance.GetAliveAllies(global::PandoraSingleton<global::MissionManager>.Instance.GetMyWarbandCtrlr().idx).IndexOf((global::UnitController)unitCtrlr) != -1;
					fire.destroyMe = true;
					if (flag && !string.IsNullOrEmpty(fire.allyFxName))
					{
						this.SpawnFx(fire.allyFxName, unitCtrlr, endUnitCtrlr, createdCb, startPos, endPos, endAction);
						return;
					}
					if (!flag && !string.IsNullOrEmpty(fire.enemyFxName))
					{
						this.SpawnFx(fire.enemyFxName, unitCtrlr, endUnitCtrlr, createdCb, startPos, endPos, endAction);
						return;
					}
					if (createdCb != null)
					{
						createdCb(null);
					}
					return;
				}
				else if (!string.IsNullOrEmpty(fire.mediumFxName) && !string.IsNullOrEmpty(fire.largeFxName))
				{
					fire.destroyMe = true;
					if (unitCtrlr.unit.Data.UnitSizeId == global::UnitSizeId.LARGE)
					{
						this.SpawnFx(fire.largeFxName, unitCtrlr, endUnitCtrlr, createdCb, startPos, endPos, endAction);
						return;
					}
					this.SpawnFx(fire.mediumFxName, unitCtrlr, endUnitCtrlr, createdCb, startPos, endPos, endAction);
					return;
				}
				else
				{
					if (fire.weaponBased)
					{
						fire.destroyMe = true;
						string text = name + "_";
						global::Mutation mutation = unitCtrlr.unit.GetMutation(unitCtrlr.unit.ActiveWeaponSlot);
						if (mutation == null)
						{
							text += unitCtrlr.unit.Items[(int)unitCtrlr.unit.ActiveWeaponSlot].GetAssetData(unitCtrlr.unit.RaceId, unitCtrlr.unit.WarbandId, unitCtrlr.unit.Id).Asset;
						}
						else
						{
							string text2 = text;
							text = string.Concat(new string[]
							{
								text2,
								unitCtrlr.unit.WarData.Asset,
								"_",
								unitCtrlr.unit.Data.Asset,
								"_",
								unitCtrlr.unit.bodyParts[mutation.RelatedBodyParts[0].BodyPartId].Name,
								"_",
								mutation.Data.Id.ToLowerString(),
								"_01"
							});
						}
						this.SpawnFx(text, unitCtrlr, endUnitCtrlr, createdCb, startPos, endPos, endAction);
						return;
					}
					this.StartFx(fire, unitCtrlr, endUnitCtrlr, startPos, endPos, endAction);
					if (createdCb != null)
					{
						createdCb(fire.gameObject);
					}
					return;
				}
			});
		}

		private void CreateFx(string fxName, global::UnityEngine.Events.UnityAction<global::Prometheus.OlympusFire> cb)
		{
			if (string.IsNullOrEmpty(fxName))
			{
				cb(null);
				return;
			}
			base.StartCoroutine(this.LoadFx(fxName, cb));
		}

		private global::System.Collections.IEnumerator LoadFx(string fxName, global::UnityEngine.Events.UnityAction<global::Prometheus.OlympusFire> cb)
		{
			if (this.cachedFx.ContainsKey(fxName))
			{
				if (this.cachedFx[fxName] == null)
				{
					while (this.cachedFx.ContainsKey(fxName) && this.cachedFx[fxName] == null)
					{
						yield return null;
					}
				}
				this.InstantiateFx(fxName, cb);
			}
			else
			{
				global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAssetAsync<global::UnityEngine.GameObject>("Assets/prefabs/fx/", global::AssetBundleId.FX, fxName + ".prefab", delegate(global::UnityEngine.Object go)
				{
					if (go != null)
					{
						this.cachedFx[fxName] = (global::UnityEngine.GameObject)go;
						this.InstantiateFx(fxName, cb);
					}
					else
					{
						this.cachedFx.Remove(fxName);
					}
				});
			}
			yield break;
		}

		private void InstantiateFx(string fxName, global::UnityEngine.Events.UnityAction<global::Prometheus.OlympusFire> cb)
		{
			global::UnityEngine.GameObject gameObject = null;
			this.cachedFx.TryGetValue(fxName, out gameObject);
			if (gameObject == null)
			{
				global::PandoraDebug.LogWarning("Loading Fx that does not exist : " + fxName, "PROMETHEUS", null);
				cb(null);
				return;
			}
			global::UnityEngine.GameObject gameObject2 = global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>(gameObject);
			global::Prometheus.OlympusFire component = gameObject2.GetComponent<global::Prometheus.OlympusFire>();
			cb(component);
		}

		private void StartFx(global::Prometheus.OlympusFire fire, global::UnitMenuController unitCtrlr, global::UnitMenuController endUnitCtrlr = null, [global::System.Runtime.InteropServices.Optional] global::UnityEngine.Vector3 startPos, [global::System.Runtime.InteropServices.Optional] global::UnityEngine.Vector3 endPos, global::UnityEngine.Events.UnityAction endAction = null)
		{
			fire.Spawn(unitCtrlr, endUnitCtrlr, startPos, endPos, endAction);
			this.CheckDelay(fire);
		}

		private void StartFx(global::Prometheus.OlympusFire fire, global::UnityEngine.Transform anchor, bool attached)
		{
			fire.Spawn(anchor, attached);
			this.CheckDelay(fire);
		}

		private void StartFx(global::Prometheus.OlympusFire fire, global::UnityEngine.Vector3 pos)
		{
			fire.Spawn(pos);
			this.CheckDelay(fire);
		}

		private void CheckDelay(global::Prometheus.OlympusFire fire)
		{
			if (fire.delay != 0f)
			{
				fire.gameObject.SetActive(false);
				this.delayedFires.Add(fire);
			}
		}

		private void Update()
		{
			for (int i = this.delayedFires.Count - 1; i >= 0; i--)
			{
				if (this.delayedFires[i] == null)
				{
					this.delayedFires.RemoveAt(i);
				}
				else
				{
					this.delayedFires[i].delay -= global::UnityEngine.Time.deltaTime;
					if (this.delayedFires[i].delay <= 0f)
					{
						this.delayedFires[i].gameObject.SetActive(true);
						this.delayedFires[i].Reactivate();
						this.delayedFires.RemoveAt(i);
					}
				}
			}
		}

		private global::System.Collections.Generic.Dictionary<string, global::UnityEngine.GameObject> cachedFx = new global::System.Collections.Generic.Dictionary<string, global::UnityEngine.GameObject>();

		private global::System.Collections.Generic.List<global::Prometheus.OlympusFire> delayedFires = new global::System.Collections.Generic.List<global::Prometheus.OlympusFire>();
	}
}
