using System;
using System.Collections.Generic;
using Prometheus;
using UnityEngine;

public class ItemController : global::UnityEngine.MonoBehaviour
{
	public int CurrentShots()
	{
		return this.Item.Save.shots;
	}

	public global::Item Item { get; private set; }

	public static void Instantiate(global::Item item, global::RaceId raceId, global::WarbandId warbandId, global::UnitId unitId, global::UnitSlotId slotId, global::System.Action<global::ItemController> callback)
	{
		if (item.Id == global::ItemId.NONE)
		{
			return;
		}
		global::ItemAssetData assetData = item.GetAssetData(raceId, warbandId, unitId);
		global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAssetAsync<global::UnityEngine.GameObject>("Assets/prefabs/equipments/", global::AssetBundleId.EQUIPMENTS, assetData.Asset + ".prefab", delegate(global::UnityEngine.Object ec)
		{
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>((global::UnityEngine.GameObject)ec);
			gameObject.SetLayerRecursively(global::UnityEngine.LayerMask.NameToLayer("characters"));
			global::ItemController component = gameObject.GetComponent<global::ItemController>();
			component.SetItem(item, slotId, assetData.NoTrail);
			callback(component);
		});
	}

	public void SetItem(global::Item item, global::UnitSlotId slotId, bool noTrail)
	{
		this.Item = item;
		this.trails = new global::System.Collections.Generic.List<global::WeaponTrail>(base.GetComponents<global::WeaponTrail>());
		this.anim = base.GetComponent<global::UnityEngine.Animation>();
		for (int i = this.trails.Count - 1; i >= 0; i--)
		{
			if (noTrail)
			{
				global::UnityEngine.Object.Destroy(this.trails[i]);
				this.trails.RemoveAt(i);
			}
			else
			{
				this.trails[i].Emit(false);
			}
		}
		if (this.Item.ProjectileId != global::ProjectileId.NONE)
		{
			global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadAssetAsync<global::UnityEngine.GameObject>("Assets/prefabs/projectiles/", global::AssetBundleId.EQUIPMENTS, item.ProjectileData.Name + ".prefab", delegate(global::UnityEngine.Object p)
			{
				this.projectilePrefab = global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>((global::UnityEngine.GameObject)p);
				this.projectilePrefab.SetActive(false);
				if (!global::PandoraSingleton<global::MissionStartData>.Exists() || !global::PandoraSingleton<global::MissionStartData>.Instance.isReload)
				{
					this.Reload();
				}
				else if (this.Item.Save.shots > 0)
				{
					this.AddProjectile();
					this.Play("reload");
				}
			});
		}
		global::UnityEngine.Renderer[] componentsInChildren = base.GetComponentsInChildren<global::UnityEngine.Renderer>();
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			if (componentsInChildren[j].material.HasProperty("_Color"))
			{
				componentsInChildren[j].material.color = componentsInChildren[j].sharedMaterial.color;
			}
		}
		global::System.Collections.Generic.List<global::ItemSlotEnchantmentData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::ItemSlotEnchantmentData>(new string[]
		{
			"fk_item_id",
			"fk_unit_slot_id"
		}, new string[]
		{
			((int)item.Id).ToConstantString(),
			((int)slotId).ToConstantString()
		});
		for (int k = 0; k < list.Count; k++)
		{
			if (!this.Item.HasEnchantment(list[k].EnchantmentId))
			{
				this.Item.Enchantments.Add(new global::Enchantment(list[k].EnchantmentId, null, null, false, true, global::AllegianceId.NONE, true));
			}
		}
	}

	public void Play(string label)
	{
		if (this.anim != null && label != string.Empty && this.anim[label])
		{
			this.anim.Play(label);
		}
	}

	public void Unsheathe(global::System.Collections.Generic.Dictionary<global::BoneId, global::UnityEngine.Transform> BonesTr, bool offhand = false)
	{
		global::BoneId boneId = this.Item.BoneId;
		if (offhand)
		{
			global::BoneData boneData = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::BoneData>((int)boneId);
			boneId = boneData.BoneIdMirror;
		}
		base.transform.SetParent(BonesTr[boneId]);
		base.transform.localPosition = global::UnityEngine.Vector3.zero;
		base.transform.localRotation = global::UnityEngine.Quaternion.identity;
		if (offhand && this.Item.StyleData.Id == global::AnimStyleId.CLAW)
		{
			base.transform.Rotate(global::UnityEngine.Vector3.forward, 180f);
			base.transform.Translate(0.08f, 0f, 0f);
		}
		if (this.projectile != null)
		{
			this.projectile.gameObject.SetActive(true);
		}
	}

	public void Sheathe(global::System.Collections.Generic.Dictionary<global::BoneId, global::UnityEngine.Transform> BonesTr, bool offhand = false, global::UnitId unitId = global::UnitId.NONE)
	{
		if (this.projectile != null)
		{
			this.projectile.gameObject.SetActive(false);
		}
		base.transform.SetParent(BonesTr[global::BoneId.RIG_SPINE3]);
		if (this.Item.TypeData.Id == global::ItemTypeId.SHIELD)
		{
			base.transform.localPosition = new global::UnityEngine.Vector3(0.118f, -0.191f, 0.043f);
			base.transform.localRotation = global::UnityEngine.Quaternion.Euler(11.4f, 16.7f, 0.8f);
		}
		else if (unitId == global::UnitId.OGRE_MERCENARY)
		{
			if (offhand)
			{
				base.transform.localPosition = new global::UnityEngine.Vector3(-0.511f, -0.361f, 0.15f);
				base.transform.localRotation = global::UnityEngine.Quaternion.Euler(356.4f, 16.15f, 176.41f);
			}
			else
			{
				base.transform.localPosition = new global::UnityEngine.Vector3(-0.4f, -0.37f, -0.17f);
				base.transform.localRotation = global::UnityEngine.Quaternion.Euler(356.8f, 334.7f, 163.1f);
			}
		}
		else if (unitId == global::UnitId.EXECUTIONER)
		{
			if (offhand)
			{
				base.transform.localPosition = new global::UnityEngine.Vector3(-0.439f, -0.258f, 0.157f);
				base.transform.localRotation = global::UnityEngine.Quaternion.Euler(4.2f, 16.2f, 180.2f);
			}
			else
			{
				base.transform.localPosition = new global::UnityEngine.Vector3(-0.341f, -0.205f, -0.171f);
				base.transform.localRotation = global::UnityEngine.Quaternion.Euler(356.8f, 334.7f, 163.1f);
			}
		}
		else if (unitId == global::UnitId.DREG)
		{
			if (this.Item.StyleData.Id == global::AnimStyleId.SPEAR_NO_SHIELD || this.Item.StyleData.Id == global::AnimStyleId.SPEAR_SHIELD || this.Item.StyleData.Id == global::AnimStyleId.WARHAMMER)
			{
				base.transform.localPosition = new global::UnityEngine.Vector3(0.235f, -0.179f, 0.074f);
				base.transform.localRotation = global::UnityEngine.Quaternion.Euler(9.2f, 338.5f, 192.12f);
			}
			else if (offhand && (this.Item.StyleData.Id == global::AnimStyleId.SPEAR_SHIELD || this.Item.StyleData.Id == global::AnimStyleId.ONE_HAND_SHIELD))
			{
				base.transform.localPosition = new global::UnityEngine.Vector3(-0.339f, -0.181f, 0.143f);
				base.transform.localRotation = global::UnityEngine.Quaternion.Euler(11.4f, 16.2f, 180.2f);
			}
			else if (offhand)
			{
				base.transform.localPosition = new global::UnityEngine.Vector3(-0.328f, -0.289f, 0.117f);
				base.transform.localRotation = global::UnityEngine.Quaternion.Euler(8.875f, 32.28f, 191.5f);
			}
			else
			{
				base.transform.localPosition = new global::UnityEngine.Vector3(-0.281f, -0.298f, -0.15f);
				base.transform.localRotation = global::UnityEngine.Quaternion.Euler(9.2f, 338.5f, 192.12f);
			}
		}
		else if (unitId == global::UnitId.GHOUL)
		{
			if (this.Item.StyleData.Id == global::AnimStyleId.SPEAR_NO_SHIELD || this.Item.StyleData.Id == global::AnimStyleId.WARHAMMER)
			{
				base.transform.localPosition = new global::UnityEngine.Vector3(0.211f, -0.148f, 0.07f);
				base.transform.localRotation = global::UnityEngine.Quaternion.Euler(11.25f, 334.75f, 208.8f);
			}
			else if (offhand)
			{
				base.transform.localPosition = new global::UnityEngine.Vector3(-0.15f, -0.297f, 0.101f);
				base.transform.localRotation = global::UnityEngine.Quaternion.Euler(9.93f, 44.92f, 195.475f);
			}
			else
			{
				base.transform.localPosition = new global::UnityEngine.Vector3(-0.241f, -0.287f, -0.146f);
				base.transform.localRotation = global::UnityEngine.Quaternion.Euler(9.2f, 338.5f, 186.43f);
			}
		}
		else if (unitId == global::UnitId.CRYPT_HORROR)
		{
			if (this.Item.StyleData.Id == global::AnimStyleId.SPEAR_NO_SHIELD || this.Item.StyleData.Id == global::AnimStyleId.WARHAMMER)
			{
				base.transform.localPosition = new global::UnityEngine.Vector3(0.335f, -0.357f, 0.082f);
				base.transform.localRotation = global::UnityEngine.Quaternion.Euler(11.25f, 334.75f, 208.8f);
			}
			else if (offhand)
			{
				base.transform.localPosition = new global::UnityEngine.Vector3(-0.173f, -0.684f, 0.223f);
				base.transform.localRotation = global::UnityEngine.Quaternion.Euler(4.75f, 58.387f, 202.28f);
			}
			else
			{
				base.transform.localPosition = new global::UnityEngine.Vector3(-0.222f, -0.707f, -0.257f);
				base.transform.localRotation = global::UnityEngine.Quaternion.Euler(11.25f, 334.75f, 208.8f);
			}
		}
		else
		{
			switch (this.Item.StyleData.Id)
			{
			case global::AnimStyleId.ONE_HAND_NO_SHIELD:
			case global::AnimStyleId.ONE_HAND_SHIELD:
			case global::AnimStyleId.DUAL_WIELD:
				if (offhand)
				{
					base.transform.localPosition = new global::UnityEngine.Vector3(-0.339f, -0.181f, 0.143f);
					base.transform.localRotation = global::UnityEngine.Quaternion.Euler(11.4f, 16.2f, 180.2f);
				}
				else
				{
					base.transform.localPosition = new global::UnityEngine.Vector3(-0.337f, -0.123f, -0.155f);
					base.transform.localRotation = global::UnityEngine.Quaternion.Euler(9.2f, 338.5f, 173.2f);
				}
				break;
			case global::AnimStyleId.SPEAR_NO_SHIELD:
			case global::AnimStyleId.SPEAR_SHIELD:
				if (this.Item.owner.RaceId == global::RaceId.SKAVEN)
				{
					base.transform.localPosition = new global::UnityEngine.Vector3(-0.653f, -0.142f, -0.156f);
					base.transform.localRotation = global::UnityEngine.Quaternion.Euler(9.2f, 338.5f, 173.2f);
				}
				else
				{
					base.transform.localPosition = new global::UnityEngine.Vector3(0.25f, -0.213f, 0.074f);
					base.transform.localRotation = global::UnityEngine.Quaternion.Euler(9.2f, 338.5f, 173.2f);
				}
				break;
			case global::AnimStyleId.CLAW:
				if (offhand)
				{
					base.transform.localPosition = new global::UnityEngine.Vector3(-0.145f, -0.192f, 0.146f);
					base.transform.localRotation = global::UnityEngine.Quaternion.Euler(357.2f, 119.1f, 181.4f);
				}
				else
				{
					base.transform.localPosition = new global::UnityEngine.Vector3(-0.226f, -0.161f, -0.081f);
					base.transform.localRotation = global::UnityEngine.Quaternion.Euler(8f, 56.7f, 195f);
				}
				break;
			case global::AnimStyleId.TWO_HANDED:
				base.transform.SetParent(BonesTr[global::BoneId.RIG_SPINE3]);
				base.transform.localPosition = new global::UnityEngine.Vector3(-0.34426f, -0.19861f, -0.19507f);
				base.transform.localRotation = global::UnityEngine.Quaternion.Euler(-3.2f, -25.3f, -174.3f);
				break;
			case global::AnimStyleId.HALBERD:
				base.transform.localPosition = new global::UnityEngine.Vector3(-0.337f, -0.155f, -0.16f);
				base.transform.localRotation = global::UnityEngine.Quaternion.Euler(9.2f, 338.5f, 173.2f);
				break;
			case global::AnimStyleId.WARHAMMER:
				base.transform.localPosition = new global::UnityEngine.Vector3(0.25f, -0.213f, 0.074f);
				base.transform.localRotation = global::UnityEngine.Quaternion.Euler(9.2f, 338.5f, 173.2f);
				break;
			case global::AnimStyleId.DUAL_PISTOL:
				if (offhand)
				{
					base.transform.localPosition = new global::UnityEngine.Vector3(-0.264f, -0.162f, 0.243f);
					base.transform.localRotation = global::UnityEngine.Quaternion.Euler(2.4f, 83.4f, 176.9f);
				}
				else
				{
					base.transform.localPosition = new global::UnityEngine.Vector3(-0.312f, -0.132f, -0.167f);
					base.transform.localRotation = global::UnityEngine.Quaternion.Euler(6.1f, 111.5f, 7.3f);
				}
				break;
			case global::AnimStyleId.BOW:
				base.transform.localPosition = new global::UnityEngine.Vector3(0.1061865f, -0.1313297f, -0.01045499f);
				base.transform.localRotation = global::UnityEngine.Quaternion.Euler(-7.007904f, 169.1543f, 167.5401f);
				break;
			case global::AnimStyleId.CROSSBOW:
				base.transform.localPosition = new global::UnityEngine.Vector3(-0.094f, -0.136f, -0.127f);
				base.transform.localRotation = global::UnityEngine.Quaternion.Euler(9.2f, 338.5f, 173.2f);
				break;
			case global::AnimStyleId.RIFLE:
				base.transform.localPosition = new global::UnityEngine.Vector3(-0.036184f, -0.15823f, -0.0025011f);
				base.transform.localRotation = global::UnityEngine.Quaternion.Euler(9.2f, 24.91f, 173.2f);
				break;
			}
		}
	}

	public void Reload()
	{
		this.AddProjectile();
		this.Play("reload");
		this.Item.Save.shots = this.Item.Shots;
	}

	public global::UnityEngine.GameObject GetProjectile()
	{
		if (this.projectile == null)
		{
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate<global::UnityEngine.GameObject>(this.projectilePrefab);
			gameObject.SetActive(true);
			this.projectile = gameObject.GetComponent<global::Projectile>();
		}
		return this.projectile.gameObject;
	}

	public void AttachProjectile()
	{
		global::UnityEngine.GameObject gameObject = this.projectile.gameObject;
		global::UnityEngine.MeshFilter componentInChildren = gameObject.GetComponentInChildren<global::UnityEngine.MeshFilter>();
		gameObject.transform.SetParent(this.projectileStartPoint);
		gameObject.transform.localPosition = new global::UnityEngine.Vector3(0f, 0f, global::UnityEngine.Mathf.Max(new float[]
		{
			componentInChildren.mesh.bounds.extents.x,
			componentInChildren.mesh.bounds.extents.y,
			componentInChildren.mesh.bounds.extents.z
		}) * 2f);
		gameObject.transform.localRotation = global::UnityEngine.Quaternion.identity;
	}

	private void AddProjectile()
	{
		this.GetProjectile();
		this.AttachProjectile();
	}

	public void Aim()
	{
		this.Play("aim");
	}

	public void Shoot(global::UnitController shooter, global::System.Collections.Generic.List<global::UnityEngine.Vector3> targets, global::System.Collections.Generic.List<global::UnityEngine.MonoBehaviour> defenders, global::System.Collections.Generic.List<bool> noCollisions, global::System.Collections.Generic.List<global::UnityEngine.Transform> parts, bool isSecondary = false)
	{
		this.Item.Save.shots--;
		this.Play("shooting");
		for (int i = 0; i < defenders.Count; i++)
		{
			this.AddProjectile();
			this.projectile.Launch(targets[i], shooter, defenders[i], noCollisions[i], parts[i], isSecondary);
			this.projectile = null;
		}
		if (this.shootFxPrefab != null)
		{
			if (this.shootFx != null)
			{
				global::UnityEngine.Object.Destroy(this.shootFx);
			}
			global::PandoraSingleton<global::Prometheus.Prometheus>.Instance.SpawnFx(this.shootFxPrefab.name, shooter, null, delegate(global::UnityEngine.GameObject fx)
			{
				this.shootFx = fx;
				if (this.shootFx != null)
				{
					this.shootFx.transform.SetParent(this.projectileStartPoint);
					this.shootFx.transform.localPosition = global::UnityEngine.Vector3.zero;
					this.shootFx.transform.localRotation = global::UnityEngine.Quaternion.identity;
				}
			}, default(global::UnityEngine.Vector3), default(global::UnityEngine.Vector3), null);
		}
	}

	public global::UnityEngine.Transform projectileStartPoint;

	public global::UnityEngine.GameObject shootFxPrefab;

	public global::UnityEngine.Transform parryFxAnchor;

	private global::UnityEngine.GameObject shootFx;

	public global::System.Collections.Generic.List<global::WeaponTrail> trails;

	private global::UnityEngine.GameObject projectilePrefab;

	private global::UnityEngine.Animation anim;

	public global::Projectile projectile;
}
