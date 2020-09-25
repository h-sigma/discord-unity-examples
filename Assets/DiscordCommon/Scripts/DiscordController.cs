using System;
using UnityEngine;
using Discord;
using UnityEngine.Events;

namespace DiscordCommon
{
	public class DiscordController : MonoBehaviour
	{
		#region Serializable
		
		public int instanceId = 0;
		public Discord.Discord discord;

		#endregion
		
		#region Events
		
		[Serializable]
		public class DiscordCreatedEvent : UnityEvent<Discord.Discord> {}

		public DiscordCreatedEvent onCreated;
		
		#endregion
		
		#region Unity Callbacks

		// Use this for initialization
		void Start () {
			Environment.SetEnvironmentVariable("DISCORD_INSTANCE_ID", instanceId.ToString());
			discord = new Discord.Discord(759008389677580308, (UInt64)CreateFlags.Default);
			
			discord.SetLogHook(LogLevel.Info, LogDiscord);
			
			onCreated?.Invoke(discord);
		}

		public void OnDestroy()
		{
			discord.Dispose();
		}


		// Update is called once per frame
		void Update () {
			discord.RunCallbacks();
		}

		#endregion
		
		#region Callbacks
		
		private void LogDiscord(LogLevel level, string message)
		{
			switch (level)
			{
				case LogLevel.Error:
					Debug.LogError($"Discord:{level} - {message}");
					break;
				case LogLevel.Warn: 
					Debug.LogWarning($"Discord:{level} - {message}");
					break;
				case LogLevel.Info: 
					Debug.Log($"Discord:{level} - {message}");
					break;
				case LogLevel.Debug: 
					Debug.Log($"Discord:{level} - {message}");
					break;
				default: throw new ArgumentOutOfRangeException(nameof(level), level, null);
			}
		}
		
		#endregion
	}
}