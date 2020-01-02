using System;
using System.Collections.Generic;
using UnityEngine;

namespace mset
{
	[global::UnityEngine.RequireComponent(typeof(global::mset.Sky))]
	public class SkyApplicator : global::UnityEngine.MonoBehaviour
	{
		public global::UnityEngine.Bounds TriggerDimensions
		{
			get
			{
				return this.triggerDimensions;
			}
			set
			{
				this.HasChanged = true;
				this.triggerDimensions = value;
			}
		}

		private void Awake()
		{
			this.TargetSky = base.GetComponent<global::mset.Sky>();
		}

		private void Start()
		{
		}

		private void OnEnable()
		{
			base.gameObject.isStatic = true;
			base.transform.root.gameObject.isStatic = true;
			this.LastPosition = base.transform.position;
			if (this.ParentApplicator == null && base.transform.parent != null && base.transform.parent.GetComponent<global::mset.SkyApplicator>() != null)
			{
				this.ParentApplicator = base.transform.parent.GetComponent<global::mset.SkyApplicator>();
			}
			if (this.ParentApplicator != null)
			{
				this.ParentApplicator.Children.Add(this);
			}
			else
			{
				global::mset.SkyManager skyManager = global::mset.SkyManager.Get();
				if (skyManager != null)
				{
					skyManager.RegisterApplicator(this);
				}
			}
		}

		private void OnDisable()
		{
			if (this.ParentApplicator != null)
			{
				this.ParentApplicator.Children.Remove(this);
			}
			global::mset.SkyManager skyManager = global::mset.SkyManager.Get();
			if (skyManager)
			{
				skyManager.UnregisterApplicator(this, this.AffectedRenderers);
				this.AffectedRenderers.Clear();
			}
		}

		public void RemoveRenderer(global::UnityEngine.Renderer rend)
		{
			if (this.AffectedRenderers.Contains(rend))
			{
				this.AffectedRenderers.Remove(rend);
				global::mset.SkyAnchor component = rend.GetComponent<global::mset.SkyAnchor>();
				if (component && component.CurrentApplicator == this)
				{
					component.CurrentApplicator = null;
				}
			}
		}

		public void AddRenderer(global::UnityEngine.Renderer rend)
		{
			global::mset.SkyAnchor component = rend.GetComponent<global::mset.SkyAnchor>();
			if (component != null)
			{
				if (component.CurrentApplicator != null)
				{
					component.CurrentApplicator.RemoveRenderer(rend);
				}
				component.CurrentApplicator = this;
			}
			this.AffectedRenderers.Add(rend);
		}

		public bool ApplyInside(global::UnityEngine.Renderer rend)
		{
			if (this.TargetSky == null || !this.TriggerIsActive)
			{
				return false;
			}
			global::mset.SkyAnchor component = rend.gameObject.GetComponent<global::mset.SkyAnchor>();
			if (component && component.BindType == global::mset.SkyAnchor.AnchorBindType.TargetSky && component.AnchorSky == this.TargetSky)
			{
				this.TargetSky.Apply(rend, 0);
				component.Apply();
				return true;
			}
			foreach (global::mset.SkyApplicator skyApplicator in this.Children)
			{
				if (skyApplicator.ApplyInside(rend))
				{
					return true;
				}
			}
			global::UnityEngine.Vector3 vector = rend.bounds.center;
			if (component)
			{
				component.GetCenter(ref vector);
			}
			vector = base.transform.worldToLocalMatrix.MultiplyPoint(vector);
			if (this.TriggerDimensions.Contains(vector))
			{
				this.TargetSky.Apply(rend, 0);
				return true;
			}
			return false;
		}

		public bool RendererInside(global::UnityEngine.Renderer rend)
		{
			global::mset.SkyAnchor skyAnchor = rend.gameObject.GetComponent<global::mset.SkyAnchor>();
			if (skyAnchor && skyAnchor.BindType == global::mset.SkyAnchor.AnchorBindType.TargetSky && skyAnchor.AnchorSky == this.TargetSky)
			{
				this.AddRenderer(rend);
				skyAnchor.Apply();
				return true;
			}
			if (!this.TriggerIsActive)
			{
				return false;
			}
			foreach (global::mset.SkyApplicator skyApplicator in this.Children)
			{
				if (skyApplicator.RendererInside(rend))
				{
					return true;
				}
			}
			if (skyAnchor == null)
			{
				skyAnchor = (rend.gameObject.AddComponent(typeof(global::mset.SkyAnchor)) as global::mset.SkyAnchor);
			}
			skyAnchor.GetCenter(ref this._center);
			this._center = base.transform.worldToLocalMatrix.MultiplyPoint(this._center);
			if (this.TriggerDimensions.Contains(this._center))
			{
				if (!this.AffectedRenderers.Contains(rend))
				{
					this.AddRenderer(rend);
					if (!skyAnchor.HasLocalSky)
					{
						skyAnchor.SnapToSky(global::mset.SkyManager.Get().GlobalSky);
					}
					skyAnchor.BlendToSky(this.TargetSky);
				}
				return true;
			}
			this.RemoveRenderer(rend);
			return false;
		}

		private void LateUpdate()
		{
			if (this.TargetSky.Dirty)
			{
				foreach (global::UnityEngine.Renderer renderer in this.AffectedRenderers)
				{
					if (!(renderer == null))
					{
						this.TargetSky.Apply(renderer, 0);
					}
				}
				this.TargetSky.Dirty = false;
			}
			if (base.transform.position != this.LastPosition)
			{
				this.HasChanged = true;
			}
		}

		public global::mset.Sky TargetSky;

		public bool TriggerIsActive = true;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Bounds triggerDimensions = new global::UnityEngine.Bounds(global::UnityEngine.Vector3.zero, global::UnityEngine.Vector3.one);

		public bool HasChanged = true;

		public global::mset.SkyApplicator ParentApplicator;

		public global::System.Collections.Generic.List<global::mset.SkyApplicator> Children = new global::System.Collections.Generic.List<global::mset.SkyApplicator>();

		private global::System.Collections.Generic.HashSet<global::UnityEngine.Renderer> AffectedRenderers = new global::System.Collections.Generic.HashSet<global::UnityEngine.Renderer>();

		private global::UnityEngine.Vector3 LastPosition = global::UnityEngine.Vector3.zero;

		private global::UnityEngine.Vector3 _center;
	}
}
