using System;
using System.Collections.Generic;
using System.IO;

public class MissionSave : global::IThoth
{
	public MissionSave(float defaultRoutThreshold)
	{
		this.teams = new global::System.Collections.Generic.List<int>();
		this.deployScenarioSlotIds = new global::System.Collections.Generic.List<int>();
		this.objectiveTypeIds = new global::System.Collections.Generic.List<int>();
		this.objectiveTargets = new global::System.Collections.Generic.List<int>();
		this.objectiveSeeds = new global::System.Collections.Generic.List<int>();
		this.randomObjectives = new global::System.Collections.Generic.List<bool>();
		this.autoDeploy = false;
		this.routThreshold = defaultRoutThreshold;
		this.ratingId = 1;
	}

	int global::IThoth.GetVersion()
	{
		return 18;
	}

	void global::IThoth.Write(global::System.IO.BinaryWriter writer)
	{
		int version = ((global::IThoth)this).GetVersion();
		global::Thoth.Write(writer, version);
		int crc = this.GetCRC(false);
		global::Thoth.Write(writer, crc);
		global::Thoth.Write(writer, this.campaignId);
		global::Thoth.Write(writer, this.isCampaign);
		global::Thoth.Write(writer, this.isSkirmish);
		global::Thoth.Write(writer, this.isTuto);
		global::Thoth.Write(writer, this.mapPosition);
		global::Thoth.Write(writer, this.rating);
		global::Thoth.Write(writer, this.deployScenarioMapLayoutId);
		global::Thoth.Write(writer, this.mapLayoutId);
		global::Thoth.Write(writer, this.mapGameplayId);
		global::Thoth.Write(writer, this.VictoryTypeId);
		global::Thoth.Write(writer, this.turnTimer);
		global::Thoth.Write(writer, this.deployTimer);
		global::Thoth.Write(writer, this.beaconLimit);
		global::Thoth.Write(writer, this.deployCount);
		for (int i = 0; i < this.deployCount; i++)
		{
			global::Thoth.Write(writer, this.teams[i]);
			global::Thoth.Write(writer, this.deployScenarioSlotIds[i]);
			global::Thoth.Write(writer, this.objectiveTypeIds[i]);
			global::Thoth.Write(writer, this.objectiveTargets[i]);
			global::Thoth.Write(writer, this.objectiveSeeds[i]);
			global::Thoth.Write(writer, this.randomObjectives[i]);
		}
		global::Thoth.Write(writer, this.wyrdPlacementId);
		global::Thoth.Write(writer, this.wyrdDensityId);
		global::Thoth.Write(writer, this.searchDensityId);
		global::Thoth.Write(writer, this.randomMap);
		global::Thoth.Write(writer, this.randomLayout);
		global::Thoth.Write(writer, this.randomGameplay);
		global::Thoth.Write(writer, this.randomDeployment);
		global::Thoth.Write(writer, this.randomSlots);
		global::Thoth.Write(writer, this.autoDeploy);
		global::Thoth.Write(writer, this.ratingId);
		global::Thoth.Write(writer, this.randomRoaming);
		global::Thoth.Write(writer, this.roamingUnitId);
		global::Thoth.Write(writer, this.routThreshold);
	}

	void global::IThoth.Read(global::System.IO.BinaryReader reader)
	{
		int num = 0;
		this.teams = new global::System.Collections.Generic.List<int>();
		this.deployScenarioSlotIds = new global::System.Collections.Generic.List<int>();
		this.objectiveTypeIds = new global::System.Collections.Generic.List<int>();
		this.objectiveTargets = new global::System.Collections.Generic.List<int>();
		this.objectiveSeeds = new global::System.Collections.Generic.List<int>();
		this.randomObjectives = new global::System.Collections.Generic.List<bool>();
		global::Thoth.Read(reader, out this.lastVersion);
		int num2 = this.lastVersion;
		if (num2 >= 17)
		{
			global::Thoth.Read(reader, out num);
		}
		global::Thoth.Read(reader, out this.campaignId);
		global::Thoth.Read(reader, out this.isCampaign);
		if (num2 >= 10)
		{
			global::Thoth.Read(reader, out this.isSkirmish);
		}
		if (num2 >= 7)
		{
			global::Thoth.Read(reader, out this.isTuto);
		}
		global::Thoth.Read(reader, out this.mapPosition);
		global::Thoth.Read(reader, out this.rating);
		global::Thoth.Read(reader, out this.deployScenarioMapLayoutId);
		global::Thoth.Read(reader, out this.mapLayoutId);
		if (num2 >= 11)
		{
			global::Thoth.Read(reader, out this.mapGameplayId);
		}
		if (num2 >= 4)
		{
			global::Thoth.Read(reader, out this.VictoryTypeId);
		}
		if (num2 >= 6)
		{
			global::Thoth.Read(reader, out this.turnTimer);
		}
		if (num2 >= 16)
		{
			global::Thoth.Read(reader, out this.deployTimer);
		}
		if (num2 >= 9)
		{
			global::Thoth.Read(reader, out this.beaconLimit);
		}
		global::Thoth.Read(reader, out this.deployCount);
		for (int i = 0; i < this.deployCount; i++)
		{
			int item;
			global::Thoth.Read(reader, out item);
			this.teams.Add(item);
			global::Thoth.Read(reader, out item);
			this.deployScenarioSlotIds.Add(item);
			global::Thoth.Read(reader, out item);
			this.objectiveTypeIds.Add(item);
			global::Thoth.Read(reader, out item);
			this.objectiveTargets.Add(item);
			if (num2 >= 5)
			{
				global::Thoth.Read(reader, out item);
				this.objectiveSeeds.Add(item);
			}
			if (num2 >= 3)
			{
				bool item2;
				global::Thoth.Read(reader, out item2);
				this.randomObjectives.Add(item2);
			}
		}
		global::Thoth.Read(reader, out this.wyrdPlacementId);
		global::Thoth.Read(reader, out this.wyrdDensityId);
		global::Thoth.Read(reader, out this.searchDensityId);
		global::Thoth.Read(reader, out this.randomMap);
		global::Thoth.Read(reader, out this.randomLayout);
		if (num2 >= 11)
		{
			global::Thoth.Read(reader, out this.randomGameplay);
		}
		global::Thoth.Read(reader, out this.randomDeployment);
		global::Thoth.Read(reader, out this.randomSlots);
		if (num2 >= 12)
		{
			global::Thoth.Read(reader, out this.autoDeploy);
		}
		if (num2 >= 13)
		{
			global::Thoth.Read(reader, out this.ratingId);
		}
		else if (this.rating < 0)
		{
			this.ratingId = -this.rating;
		}
		if (num2 >= 15)
		{
			global::Thoth.Read(reader, out this.randomRoaming);
			global::Thoth.Read(reader, out this.roamingUnitId);
		}
		if (num2 >= 18)
		{
			global::Thoth.Read(reader, out this.routThreshold);
		}
	}

	public int GetCRC(bool read)
	{
		return this.CalculateCRC(read);
	}

	private int CalculateCRC(bool read)
	{
		int num = (!read) ? ((global::IThoth)this).GetVersion() : this.lastVersion;
		int num2 = 0;
		num2 += this.campaignId;
		num2 += ((!this.isCampaign) ? 0 : 1);
		num2 += ((!this.isSkirmish) ? 0 : 1);
		num2 += ((!this.isTuto) ? 0 : 1);
		num2 += this.mapPosition;
		num2 += this.rating;
		num2 += this.deployScenarioMapLayoutId;
		num2 += this.mapLayoutId;
		num2 += this.mapGameplayId;
		num2 += this.VictoryTypeId;
		num2 += this.turnTimer;
		num2 += this.deployTimer;
		num2 += this.beaconLimit;
		num2 += this.deployCount;
		for (int i = 0; i < this.teams.Count; i++)
		{
			num2 += this.teams[i];
		}
		for (int j = 0; j < this.deployScenarioSlotIds.Count; j++)
		{
			num2 += this.deployScenarioSlotIds[j];
		}
		for (int k = 0; k < this.objectiveTypeIds.Count; k++)
		{
			num2 += this.objectiveTypeIds[k];
		}
		for (int l = 0; l < this.objectiveTargets.Count; l++)
		{
			num2 += this.objectiveTargets[l];
		}
		for (int m = 0; m < this.objectiveSeeds.Count; m++)
		{
			num2 += this.objectiveSeeds[m];
		}
		num2 += this.wyrdPlacementId;
		num2 += this.wyrdDensityId;
		num2 += this.searchDensityId;
		num2 += ((!this.randomMap) ? 0 : 1);
		num2 += ((!this.randomLayout) ? 0 : 1);
		num2 += ((!this.randomGameplay) ? 0 : 1);
		num2 += ((!this.randomDeployment) ? 0 : 1);
		for (int n = 0; n < this.randomObjectives.Count; n++)
		{
			num2 += ((!this.randomObjectives[n]) ? 0 : 1);
		}
		num2 += ((!this.autoDeploy) ? 0 : 1);
		num2 += this.ratingId;
		if (num >= 15)
		{
			num2 += ((!this.randomRoaming) ? 0 : 1);
			num2 += this.roamingUnitId;
		}
		if (num >= 18)
		{
			num2 += (int)(this.routThreshold * 100f);
		}
		return num2;
	}

	private int lastVersion;

	public int campaignId;

	public bool isCampaign;

	public bool isSkirmish;

	public bool isTuto;

	public int mapPosition;

	public int rating;

	public int ratingId;

	public int deployScenarioMapLayoutId;

	public int mapLayoutId;

	public int mapGameplayId;

	public int VictoryTypeId;

	public int turnTimer;

	public int deployTimer;

	public int beaconLimit;

	public int deployCount;

	public global::System.Collections.Generic.List<int> teams;

	public global::System.Collections.Generic.List<int> deployScenarioSlotIds;

	public global::System.Collections.Generic.List<int> objectiveTypeIds;

	public global::System.Collections.Generic.List<int> objectiveTargets;

	public global::System.Collections.Generic.List<int> objectiveSeeds;

	public int wyrdPlacementId;

	public int wyrdDensityId;

	public int searchDensityId;

	public bool randomMap;

	public bool randomLayout;

	public bool randomGameplay;

	public bool randomDeployment;

	public bool randomSlots;

	public global::System.Collections.Generic.List<bool> randomObjectives;

	public bool autoDeploy;

	public bool randomRoaming;

	public int roamingUnitId;

	public float routThreshold;
}
