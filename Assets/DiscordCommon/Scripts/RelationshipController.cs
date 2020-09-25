using Discord;
using UnityEngine;

namespace DiscordCommon
{
    public class RelationshipController : MonoBehaviour
    {
        public Discord.Discord discord;
        public RelationshipManager manager;
        
        public void Init(Discord.Discord discord)
        {
            this.discord = discord;
            manager = discord.GetRelationshipManager();
            RegisterCallbacks();
        }

        #region Callbacks

        private void RegisterCallbacks()
        {
            manager.OnRefresh -= OnRefresh;
            manager.OnRefresh += OnRefresh;

            manager.OnRelationshipUpdate -= OnRelationshipUpdate;
            manager.OnRelationshipUpdate += OnRelationshipUpdate;
        }

        private void OnRefresh()
        {
            manager.Filter((ref Relationship relationship) => relationship.Type == RelationshipType.Friend);
            
            // Loop over all friends a user has.
            Debug.LogFormat("relationships updated: {0}", manager.Count());

            for (var i = 0; i < manager.Count(); i++)
            {
                // Get an individual relationship from the list
                var r = manager.GetAt((uint)i);
                Debug.LogFormat("relationships: {0} {1}", r.Type, r.User.Username);
                // Save r off to a list of user's relationships
            }
        }
        
        private void OnRelationshipUpdate(ref Relationship relationship)
        {
            Debug.Log($"Updated: {JsonUtility.ToJson(relationship)}");
        }
        
        #endregion
    }
}