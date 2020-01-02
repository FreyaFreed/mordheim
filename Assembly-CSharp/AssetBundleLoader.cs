using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AssetBundleLoader : global::PandoraSingleton<global::AssetBundleLoader>
{
	public bool IsLoading
	{
		get
		{
			return this.jobs.Count > 0 || this.queues.Count > 0;
		}
	}

	private void Awake()
	{
		this.Init();
	}

	public void Init()
	{
		this.assetBundles = new global::System.Collections.Generic.Dictionary<string, global::UnityEngine.AssetBundleCreateRequest>();
		this.cache = new global::System.Collections.Generic.Dictionary<string, global::UnityEngine.Object>();
		this.jobs = new global::System.Collections.Generic.List<global::AssetBundleLoader.AsyncJob>(4096);
		this.queues = new global::System.Collections.Generic.List<global::AssetBundleLoader.QueuedLoad>();
		this.loadedScenes = new global::System.Collections.Generic.List<string>();
		this.asyncJobsPool = new global::System.Collections.Generic.Stack<global::AssetBundleLoader.AsyncJob>(4096);
		for (int i = 0; i < 4096; i++)
		{
			this.asyncJobsPool.Push(new global::AssetBundleLoader.AsyncJob());
		}
		this.queuedLoadsPool = new global::System.Collections.Generic.Stack<global::AssetBundleLoader.QueuedLoad>(4096);
		for (int j = 0; j < 4096; j++)
		{
			this.queuedLoadsPool.Push(new global::AssetBundleLoader.QueuedLoad());
		}
		this.initialized = true;
	}

	public T GetLatestObject<T>() where T : global::UnityEngine.Object
	{
		global::UnityEngine.Object @object = this.latestObject;
		this.latestObject = null;
		return (T)((object)@object);
	}

	public void Update()
	{
		if (!this.initialized)
		{
			return;
		}
		int num = 0;
		for (int i = this.jobs.Count - 1; i >= 0; i--)
		{
			if (num <= 100)
			{
				global::AssetBundleLoader.AsyncJob asyncJob = this.jobs[i];
				if (asyncJob.req == null)
				{
					global::PandoraDebug.LogError("Could not load asset " + asyncJob.name, "uncategorised", null);
					global::PandoraUtils.RemoveBySwap<global::AssetBundleLoader.AsyncJob>(this.jobs, i);
					if (asyncJob.cb != null)
					{
						asyncJob.cb(null);
					}
				}
				else if (asyncJob.req.isDone)
				{
					num++;
					global::PandoraUtils.RemoveBySwap<global::AssetBundleLoader.AsyncJob>(this.jobs, i);
					global::UnityEngine.Object @object = null;
					switch (asyncJob.jobType)
					{
					case global::AssetBundleLoader.JobType.ASSET_BUNDLE:
						@object = ((global::UnityEngine.AssetBundleRequest)asyncJob.req).asset;
						break;
					case global::AssetBundleLoader.JobType.RESOURCE:
						@object = ((global::UnityEngine.ResourceRequest)asyncJob.req).asset;
						if (asyncJob.cache)
						{
							this.cache[asyncJob.name] = @object;
						}
						break;
					case global::AssetBundleLoader.JobType.SCENE:
						this.loadedScenes.Add(asyncJob.name);
						break;
					}
					asyncJob.cb(@object);
					asyncJob.Reset();
					this.asyncJobsPool.Push(asyncJob);
				}
			}
		}
		for (int j = this.queues.Count - 1; j >= 0; j--)
		{
			global::AssetBundleLoader.QueuedLoad queuedLoad = this.queues[j];
			if (this.LoadAsync(queuedLoad.assetBundle))
			{
				global::AssetBundleLoader.AsyncJob asyncJob2 = this.asyncJobsPool.Pop();
				asyncJob2.cb = queuedLoad.cb;
				asyncJob2.name = queuedLoad.assetName;
				if (queuedLoad.isScene)
				{
					asyncJob2.req = global::UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(queuedLoad.assetName, global::UnityEngine.SceneManagement.LoadSceneMode.Additive);
					asyncJob2.jobType = global::AssetBundleLoader.JobType.SCENE;
				}
				else
				{
					asyncJob2.req = this.assetBundles[queuedLoad.assetBundle].assetBundle.LoadAssetAsync(queuedLoad.assetName);
					asyncJob2.jobType = global::AssetBundleLoader.JobType.ASSET_BUNDLE;
				}
				if (asyncJob2.req != null)
				{
					this.jobs.Add(asyncJob2);
					global::PandoraUtils.RemoveBySwap<global::AssetBundleLoader.QueuedLoad>(this.queues, j);
				}
				else
				{
					global::PandoraDebug.LogError("Could not load asset " + queuedLoad.assetName, "uncategorised", null);
					if (asyncJob2.cb != null)
					{
						asyncJob2.cb(null);
					}
				}
				queuedLoad.Reset();
				this.queuedLoadsPool.Push(queuedLoad);
			}
		}
	}

	public bool LoadAsync(string assetBundle)
	{
		assetBundle = assetBundle.ToLowerString();
		if (!this.assetBundles.ContainsKey(assetBundle))
		{
			string path = global::UnityEngine.Application.streamingAssetsPath + "/asset_bundle/" + assetBundle + ".assetbundle";
			global::UnityEngine.AssetBundleCreateRequest value = global::UnityEngine.AssetBundle.LoadFromFileAsync(path);
			this.assetBundles.Add(assetBundle, value);
			return false;
		}
		return this.assetBundles[assetBundle].isDone;
	}

	public T LoadAsset<T>(string path, global::AssetBundleId bundleId, string assetName) where T : global::UnityEngine.Object
	{
		string assetBundle = bundleId.ToLowerString();
		return this.LoadAsset<T>(path, assetBundle, assetName);
	}

	public T LoadAsset<T>(string path, string assetBundle, string assetName) where T : global::UnityEngine.Object
	{
		global::UnityEngine.Object @object = null;
		if (this.LoadAsync(assetBundle))
		{
			@object = this.assetBundles[assetBundle].assetBundle.LoadAsset(assetName, typeof(T));
		}
		return (T)((object)@object);
	}

	public global::System.Collections.IEnumerator LoadAssetAsync<T>(string path, global::AssetBundleId bundleId, string assetName) where T : global::UnityEngine.Object
	{
		string assetBundle = bundleId.ToLowerString();
		if (this.LoadAsync(assetBundle))
		{
			global::UnityEngine.AsyncOperation ao = this.assetBundles[assetBundle].assetBundle.LoadAssetAsync(assetName, typeof(T));
			yield return ao;
			if (ao != null)
			{
				this.latestObject = ((global::UnityEngine.AssetBundleRequest)ao).asset;
			}
		}
		yield break;
	}

	public void LoadAssetAsync<T>(string path, global::AssetBundleId bundleId, string assetName, global::System.Action<global::UnityEngine.Object> callback) where T : global::UnityEngine.Object
	{
		string assetBundle = bundleId.ToLowerString();
		this.LoadAssetAsync<T>(path, assetBundle, assetName, callback);
	}

	public void LoadAssetAsync<T>(string path, string assetBundle, string assetName, global::System.Action<global::UnityEngine.Object> callback) where T : global::UnityEngine.Object
	{
		assetBundle = assetBundle.ToLowerString();
		if (this.LoadAsync(assetBundle))
		{
			global::UnityEngine.AssetBundleRequest assetBundleRequest = this.assetBundles[assetBundle].assetBundle.LoadAssetAsync(assetName, typeof(T));
			if (assetBundleRequest != null)
			{
				global::AssetBundleLoader.AsyncJob asyncJob = this.asyncJobsPool.Pop();
				asyncJob.name = assetName;
				asyncJob.cb = callback;
				asyncJob.req = assetBundleRequest;
				asyncJob.jobType = global::AssetBundleLoader.JobType.ASSET_BUNDLE;
				this.jobs.Add(asyncJob);
			}
			else
			{
				global::PandoraDebug.LogError("Could not load asset " + assetName, "uncategorised", null);
				if (callback != null)
				{
					callback(null);
				}
			}
		}
		else
		{
			global::AssetBundleLoader.QueuedLoad queuedLoad = this.queuedLoadsPool.Pop();
			queuedLoad.assetBundle = assetBundle;
			queuedLoad.assetName = assetName;
			queuedLoad.cb = callback;
			queuedLoad.isScene = false;
			this.queues.Add(queuedLoad);
		}
	}

	public void LoadSceneAssetAsync(string sceneName, string assetBundleName, global::System.Action<global::UnityEngine.Object> callback)
	{
		assetBundleName = assetBundleName.ToLowerString();
		if (this.LoadAsync(assetBundleName))
		{
			if (!this.assetBundles[assetBundleName].assetBundle.Contains(sceneName))
			{
				return;
			}
			global::UnityEngine.AsyncOperation asyncOperation = global::UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, global::UnityEngine.SceneManagement.LoadSceneMode.Additive);
			if (asyncOperation != null)
			{
				global::AssetBundleLoader.AsyncJob asyncJob = this.asyncJobsPool.Pop();
				asyncJob.cb = callback;
				asyncJob.req = asyncOperation;
				asyncJob.jobType = global::AssetBundleLoader.JobType.SCENE;
				asyncJob.name = sceneName;
				this.jobs.Add(asyncJob);
			}
			else
			{
				global::PandoraDebug.LogError("Could not load asset " + sceneName, "uncategorised", null);
				if (callback != null)
				{
					callback(null);
				}
			}
		}
		else
		{
			global::AssetBundleLoader.QueuedLoad queuedLoad = this.queuedLoadsPool.Pop();
			queuedLoad.assetBundle = assetBundleName;
			queuedLoad.assetName = sceneName;
			queuedLoad.cb = callback;
			queuedLoad.isScene = true;
			this.queues.Add(queuedLoad);
		}
	}

	public T LoadResource<T>(string name, bool cached = false) where T : global::UnityEngine.Object
	{
		global::UnityEngine.Object @object;
		if (this.cache.TryGetValue(name, out @object))
		{
			return (T)((object)@object);
		}
		@object = global::UnityEngine.Resources.Load<T>(name);
		if (cached)
		{
			this.cache[name] = @object;
		}
		return (T)((object)@object);
	}

	public void LoadResourceAsync<T>(string name, global::System.Action<global::UnityEngine.Object> callback, bool cached = false) where T : global::UnityEngine.Object
	{
		global::UnityEngine.Object obj;
		if (this.cache.TryGetValue(name, out obj))
		{
			if (callback != null)
			{
				callback(obj);
			}
			return;
		}
		global::UnityEngine.ResourceRequest resourceRequest = global::UnityEngine.Resources.LoadAsync<T>(name);
		if (resourceRequest != null)
		{
			global::AssetBundleLoader.AsyncJob asyncJob = this.asyncJobsPool.Pop();
			asyncJob.name = name;
			asyncJob.cb = callback;
			asyncJob.req = resourceRequest;
			asyncJob.jobType = global::AssetBundleLoader.JobType.RESOURCE;
			if (cached)
			{
				asyncJob.cache = true;
			}
			this.jobs.Add(asyncJob);
		}
		else
		{
			global::PandoraDebug.LogError("Could not load asset " + name, "uncategorised", null);
			if (callback != null)
			{
				callback(null);
			}
		}
	}

	public void UnloadScenes()
	{
		for (int i = 0; i < this.loadedScenes.Count; i++)
		{
			global::UnityEngine.SceneManagement.SceneManager.UnloadScene(this.loadedScenes[i]);
		}
		this.loadedScenes.Clear();
	}

	public void Unload(string assetbundleName)
	{
		if (this.assetBundles.ContainsKey(assetbundleName))
		{
			this.assetBundles[assetbundleName].assetBundle.Unload(false);
			this.assetBundles.Remove(assetbundleName);
		}
	}

	public global::System.Collections.IEnumerator UnloadAll(bool allLoadedObjects = false)
	{
		global::System.Collections.Generic.List<global::UnityEngine.AssetBundleCreateRequest> reqs = new global::System.Collections.Generic.List<global::UnityEngine.AssetBundleCreateRequest>();
		foreach (global::System.Collections.Generic.KeyValuePair<string, global::UnityEngine.AssetBundleCreateRequest> pair in this.assetBundles)
		{
			if (pair.Value.assetBundle != null)
			{
				if (pair.Key == "sounds" || pair.Key == "loading")
				{
					reqs.Add(pair.Value);
				}
				else
				{
					pair.Value.assetBundle.Unload(allLoadedObjects);
				}
			}
		}
		this.assetBundles.Clear();
		for (int i = 0; i < reqs.Count; i++)
		{
			this.assetBundles.Add(reqs[i].assetBundle.name.Replace(".assetbundle", string.Empty), reqs[i]);
		}
		reqs.Clear();
		reqs = null;
		for (int j = 0; j < this.jobs.Count; j++)
		{
			this.jobs[j].Reset();
			this.asyncJobsPool.Push(this.jobs[j]);
		}
		this.jobs.Clear();
		for (int k = 0; k < this.queues.Count; k++)
		{
			this.queues[k].Reset();
			this.queuedLoadsPool.Push(this.queues[k]);
		}
		this.queues.Clear();
		global::PandoraDebug.LogDebug("UnloadUnusedAssets", "LOADING", this);
		yield return global::UnityEngine.Resources.UnloadUnusedAssets();
		global::PandoraDebug.LogDebug("GC Collect", "LOADING", this);
		global::System.GC.Collect();
		yield return null;
		yield break;
	}

	public const string ASSET_BUNDLE_FOLDER = "/asset_bundle/";

	public const string ASSSET_BUNDLE_EXTENSION = ".assetbundle";

	public const int MAX_ITEM_BY_FRAME = 100;

	public global::System.Collections.Generic.Dictionary<string, global::UnityEngine.AssetBundleCreateRequest> assetBundles;

	public global::System.Collections.Generic.Dictionary<string, global::UnityEngine.Object> cache;

	private global::System.Collections.Generic.Stack<global::AssetBundleLoader.AsyncJob> asyncJobsPool;

	private global::System.Collections.Generic.Stack<global::AssetBundleLoader.QueuedLoad> queuedLoadsPool;

	private global::System.Collections.Generic.List<global::AssetBundleLoader.AsyncJob> jobs;

	private global::System.Collections.Generic.List<global::AssetBundleLoader.QueuedLoad> queues;

	private global::System.Collections.Generic.List<string> loadedScenes;

	private bool initialized;

	private global::UnityEngine.Object latestObject;

	private enum JobType
	{
		ASSET_BUNDLE,
		RESOURCE,
		SCENE
	}

	private class AsyncJob
	{
		public void Reset()
		{
			this.req = null;
			this.cb = null;
			this.cache = false;
			this.name = null;
		}

		public global::UnityEngine.AsyncOperation req;

		public global::System.Action<global::UnityEngine.Object> cb;

		public global::AssetBundleLoader.JobType jobType;

		public bool cache;

		public string name;
	}

	private class QueuedLoad
	{
		public void Reset()
		{
			this.assetBundle = null;
			this.assetName = null;
			this.cb = null;
			this.isScene = false;
		}

		public string assetBundle;

		public string assetName;

		public global::System.Action<global::UnityEngine.Object> cb;

		public bool isScene;
	}
}
