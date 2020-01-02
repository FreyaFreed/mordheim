using System;
using UnityEngine;

public class KGFConsoleCommands
{
	public static void AddCommands()
	{
		global::KGFConsoleCommands.itsInstance = new global::KGFConsoleCommands();
		global::KGFConsole.AddCommand("u.t.hpe", "HeightmapPixelError", "unity.terrain", global::KGFConsoleCommands.itsInstance, "TerrainHeightmapPixelError");
		global::KGFConsole.AddCommand("u.t.bd", "BasemapDistance", "unity.terrain", global::KGFConsoleCommands.itsInstance, "TerrainBasemapDistance");
		global::KGFConsole.AddCommand("u.t.cs", "CastShadows", "unity.terrain", global::KGFConsoleCommands.itsInstance, "TerrainCastShadows");
		global::KGFConsole.AddCommand("u.t.dodi", "DetailObjectDistance", "unity.terrain", global::KGFConsoleCommands.itsInstance, "TerrainDetailObjectDistance");
		global::KGFConsole.AddCommand("u.t.dode", "DetailObjectDensity", "unity.terrain", global::KGFConsoleCommands.itsInstance, "TerrainDetailObjectDensity");
		global::KGFConsole.AddCommand("u.t.td", "TreeDistance", "unity.terrain", global::KGFConsoleCommands.itsInstance, "TerrainTreeDistance");
		global::KGFConsole.AddCommand("u.t.tbd", "TreeBillboardDistance", "unity.terrain", global::KGFConsoleCommands.itsInstance, "TerrainTreeBillboardDistance");
		global::KGFConsole.AddCommand("u.t.tcfl", "TreeCrossFadeLength", "unity.terrain", global::KGFConsoleCommands.itsInstance, "TerrainTreeCrossFadeLength");
		global::KGFConsole.AddCommand("u.t.tmfl", "TreeMaximumFullLODCount", "unity.terrain", global::KGFConsoleCommands.itsInstance, "TerrainTreeMaximumFullLODCount");
		global::KGFConsole.AddCommand("u.t.hml", "HeightmapMaximumLOD", "unity.terrain", global::KGFConsoleCommands.itsInstance, "TerrainHeightmapMaximumLOD");
		global::KGFConsole.AddCommand("u.a.q", "Quit", "unity.application", global::KGFConsoleCommands.itsInstance, "ApplicationQuit");
	}

	public void TerrainHeightmapPixelError(float theHeightmapPixelError)
	{
		if (global::UnityEngine.Terrain.activeTerrain != null)
		{
			global::UnityEngine.Terrain.activeTerrain.heightmapPixelError = theHeightmapPixelError;
		}
	}

	public void TerrainBasemapDistance(float theBasemapDistance)
	{
		if (global::UnityEngine.Terrain.activeTerrain != null)
		{
			global::UnityEngine.Terrain.activeTerrain.basemapDistance = theBasemapDistance;
		}
	}

	public void TerrainCastShadows(bool theCastShadows)
	{
		if (global::UnityEngine.Terrain.activeTerrain != null)
		{
			global::UnityEngine.Terrain.activeTerrain.castShadows = theCastShadows;
		}
	}

	public void TerrainDetailObjectDistance(float theDetailObjectDistance)
	{
		if (global::UnityEngine.Terrain.activeTerrain != null)
		{
			global::UnityEngine.Terrain.activeTerrain.detailObjectDistance = theDetailObjectDistance;
		}
	}

	public void TerrainDetailObjectDensity(float theDetailObjectDensity)
	{
		if (global::UnityEngine.Terrain.activeTerrain != null)
		{
			global::UnityEngine.Terrain.activeTerrain.detailObjectDensity = theDetailObjectDensity;
		}
	}

	public void TerrainTreeDistance(float theTreeDistance)
	{
		if (global::UnityEngine.Terrain.activeTerrain != null)
		{
			global::UnityEngine.Terrain.activeTerrain.treeDistance = theTreeDistance;
		}
	}

	public void TerrainTreeBillboardDistance(float theTreeBillboardDistance)
	{
		if (global::UnityEngine.Terrain.activeTerrain != null)
		{
			global::UnityEngine.Terrain.activeTerrain.treeBillboardDistance = theTreeBillboardDistance;
		}
	}

	public void TerrainTreeCrossFadeLength(float theTreeCrossFadeLength)
	{
		if (global::UnityEngine.Terrain.activeTerrain != null)
		{
			global::UnityEngine.Terrain.activeTerrain.treeCrossFadeLength = theTreeCrossFadeLength;
		}
	}

	public void TerrainTreeMaximumFullLODCount(int theTreeMaximumFullLODCount)
	{
		if (global::UnityEngine.Terrain.activeTerrain != null)
		{
			global::UnityEngine.Terrain.activeTerrain.treeMaximumFullLODCount = theTreeMaximumFullLODCount;
		}
	}

	public void TerrainHeightmapMaximumLOD(int theHeightmapMaximumLOD)
	{
		if (global::UnityEngine.Terrain.activeTerrain != null)
		{
			global::UnityEngine.Terrain.activeTerrain.heightmapMaximumLOD = theHeightmapMaximumLOD;
		}
	}

	public void ApplicationQuit()
	{
		global::UnityEngine.Application.Quit();
	}

	private static global::KGFConsoleCommands itsInstance;
}
