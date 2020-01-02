using System;
using System.Diagnostics;

namespace DataPlatform
{
	public class Events
	{
		[global::System.Diagnostics.Conditional("UNITY_XBOXONE")]
		public static void SendGameProgress(string UserId, ref global::System.Guid PlayerSessionId, float CompletionPercent)
		{
		}

		[global::System.Diagnostics.Conditional("UNITY_XBOXONE")]
		public static void SendIncrementStat(string UserId, ref global::System.Guid PlayerSessionId, string StatId, int Increment)
		{
		}

		[global::System.Diagnostics.Conditional("UNITY_XBOXONE")]
		public static void SendItemAcquired(string UserId, int SectionId, ref global::System.Guid PlayerSessionId, string MultiplayerCorrelationId, int GameplayModeId, int DifficultyLevelId, int ItemId, int AcquisitionMethodId, float LocationX, float LocationY, float LocationZ)
		{
		}

		[global::System.Diagnostics.Conditional("UNITY_XBOXONE")]
		public static void SendItemUsed(string UserId, int SectionId, ref global::System.Guid PlayerSessionId, string MultiplayerCorrelationId, int GameplayModeId, int DifficultyLevelId, int ItemId, float LocationX, float LocationY, float LocationZ)
		{
		}

		[global::System.Diagnostics.Conditional("UNITY_XBOXONE")]
		public static void SendMediaUsage(string AppSessionId, string AppSessionStartDateTime, uint UserIdType, string UserId, string SubscriptionTierType, string SubscriptionTier, string MediaType, string ProviderId, string ProviderMediaId, string ProviderMediaInstanceId, ref global::System.Guid BingId, ulong MediaLengthMs, uint MediaControlAction, float PlaybackSpeed, ulong MediaPositionMs, ulong PlaybackDurationMs, string AcquisitionType, string AcquisitionContext, string AcquisitionContextType, string AcquisitionContextId, int PlaybackIsStream, int PlaybackIsTethered, string MarketplaceLocation, string ContentLocale, float TimeZoneOffset, uint ScreenState)
		{
		}

		[global::System.Diagnostics.Conditional("UNITY_XBOXONE")]
		public static void SendMultiplayerRoundEnd(string UserId, ref global::System.Guid RoundId, int SectionId, ref global::System.Guid PlayerSessionId, string MultiplayerCorrelationId, int GameplayModeId, int MatchTypeId, int DifficultyLevelId, float TimeInSeconds, int ExitStatusId)
		{
		}

		[global::System.Diagnostics.Conditional("UNITY_XBOXONE")]
		public static void SendMultiplayerRoundStart(string UserId, ref global::System.Guid RoundId, int SectionId, ref global::System.Guid PlayerSessionId, string MultiplayerCorrelationId, int GameplayModeId, int MatchTypeId, int DifficultyLevelId)
		{
		}

		[global::System.Diagnostics.Conditional("UNITY_XBOXONE")]
		public static void SendObjectiveEnd(string UserId, int SectionId, ref global::System.Guid PlayerSessionId, string MultiplayerCorrelationId, int GameplayModeId, int DifficultyLevelId, int ObjectiveId, int ExitStatusId)
		{
		}

		[global::System.Diagnostics.Conditional("UNITY_XBOXONE")]
		public static void SendObjectiveStart(string UserId, int SectionId, ref global::System.Guid PlayerSessionId, string MultiplayerCorrelationId, int GameplayModeId, int DifficultyLevelId, int ObjectiveId)
		{
		}

		[global::System.Diagnostics.Conditional("UNITY_XBOXONE")]
		public static void SendPageAction(string UserId, ref global::System.Guid PlayerSessionId, int ActionTypeId, int ActionInputMethodId, string Page, string TemplateId, string DestinationPage, string Content)
		{
		}

		[global::System.Diagnostics.Conditional("UNITY_XBOXONE")]
		public static void SendPageView(string UserId, ref global::System.Guid PlayerSessionId, string Page, string RefererPage, int PageTypeId, string PageTags, string TemplateId, string Content)
		{
		}

		[global::System.Diagnostics.Conditional("UNITY_XBOXONE")]
		public static void SendPlayerSessionEnd(string UserId, ref global::System.Guid PlayerSessionId, string MultiplayerCorrelationId, int GameplayModeId, int DifficultyLevelId, int ExitStatusId)
		{
		}

		[global::System.Diagnostics.Conditional("UNITY_XBOXONE")]
		public static void SendPlayerSessionPause(string UserId, ref global::System.Guid PlayerSessionId, string MultiplayerCorrelationId)
		{
		}

		[global::System.Diagnostics.Conditional("UNITY_XBOXONE")]
		public static void SendPlayerSessionResume(string UserId, ref global::System.Guid PlayerSessionId, string MultiplayerCorrelationId, int GameplayModeId, int DifficultyLevelId)
		{
		}

		[global::System.Diagnostics.Conditional("UNITY_XBOXONE")]
		public static void SendPlayerSessionStart(string UserId, ref global::System.Guid PlayerSessionId, string MultiplayerCorrelationId, int GameplayModeId, int DifficultyLevelId)
		{
		}

		[global::System.Diagnostics.Conditional("UNITY_XBOXONE")]
		public static void SendSectionEnd(string UserId, int SectionId, ref global::System.Guid PlayerSessionId, string MultiplayerCorrelationId, int GameplayModeId, int DifficultyLevelId, int ExitStatusId)
		{
		}

		[global::System.Diagnostics.Conditional("UNITY_XBOXONE")]
		public static void SendSectionStart(string UserId, int SectionId, ref global::System.Guid PlayerSessionId, string MultiplayerCorrelationId, int GameplayModeId, int DifficultyLevelId)
		{
		}

		[global::System.Diagnostics.Conditional("UNITY_XBOXONE")]
		public static void SendUnlockAchievement(string UserId, ref global::System.Guid PlayerSessionId, string TrophyId)
		{
		}

		[global::System.Diagnostics.Conditional("UNITY_XBOXONE")]
		public static void SendViewOffer(string UserId, ref global::System.Guid PlayerSessionId, ref global::System.Guid OfferGuid, ref global::System.Guid ProductGuid)
		{
		}
	}
}
