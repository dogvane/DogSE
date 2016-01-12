using UnityEngine;
using System.Collections.Generic;

// NOTE: With the addition of script execution order in Unity 3.4, it's now possible to eliminate this class.
// However as of the time of this writing (3.4.1 rc1), the script execution order doesn't carry across from
// one project to the next with SVN revisioning... and so this class will remain until that gets fixed.

[AddComponentMenu("Internal/Update Manager")]
public class UpdateManager : MonoBehaviour
{
	static UpdateManager mInstance = null;
	
	public delegate bool UpdateCallback();

	class Entry
	{
		public int level;
		public Component mb;
		public UpdateCallback callback;
	}

	/// <summary>
	/// Function used to sort entries.
	/// </summary>

	static int Comparer (Entry a, Entry b)
	{
		if (a.level > b.level) return -1;
		if (a.level < b.level) return 1;
		return 0;
	}

	List<Entry> mUpdate = new List<Entry>();
	List<Entry> mLate = new List<Entry>();
	
	/// <summary>
	/// Ensure that there is an instance of this class somewhere in the scene.
	/// </summary>
	
	static void CreateInstance()
	{
		if (mInstance == null)
		{
			GameObject go = new GameObject("_UpdateManager");
			mInstance = go.AddComponent<UpdateManager>();
			DontDestroyOnLoad(go);
		}
	}

	/// <summary>
	/// Register a new update function at the specified level (executed low to high).
	/// </summary>

	static public void AddUpdate (int level, Component mb, UpdateCallback callback)
	{
		if (Application.isPlaying)
		{
			CreateInstance();
			AddUpdate(mInstance.mUpdate, level, mb, callback);
		}
	}

	/// <summary>
	/// Register a new late update function at the specified level (executed low to high).
	/// </summary>

	static public void AddLateUpdate (int level, Component mb, UpdateCallback callback)
	{
		if (Application.isPlaying)
		{
			CreateInstance();
			AddUpdate(mInstance.mLate, level, mb, callback);
		}
	}

	/// <summary>
	/// Helper function that checks to see if an entry already exists before adding it.
	/// </summary>

	static void AddUpdate (List<Entry> list, int level, Component mb, UpdateCallback cb)
	{
		// Try to find an existing entry
		foreach (Entry e in list)
		{
			if (e.mb == mb && e.callback == cb)
			{
				e.level = level;
				return;
			}
		}

		// Add a new entry
		Entry ent = new Entry();
		ent.level = level;
		ent.mb = mb;
		ent.callback = cb;
		list.Add(ent);
		list.Sort(Comparer);
	}

	void Update()
	{
		for (int i = mUpdate.Count; i > 0; )
		{
			Entry ent = mUpdate[--i];
			if (ent.mb == null || !ent.callback()) mUpdate.RemoveAt(i);
		}
		DeleteIfEmpty();
	}

	void LateUpdate()
	{
		for (int i = mLate.Count; i > 0; )
		{
			Entry ent = mLate[--i];
			if (ent.mb == null || !ent.callback()) mLate.RemoveAt(i);
		}
		DeleteIfEmpty();
	}

	void DeleteIfEmpty()
	{
		if (mUpdate.Count == 0 && mLate.Count == 0)
		{
			Destroy(this);
		}
	}

	void OnDestroy ()
	{
		mInstance = null;
	}

	void OnApplicationQuit ()
	{
		Tools.Broadcast("End");
	}
}