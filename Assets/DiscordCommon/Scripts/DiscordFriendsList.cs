using System.Collections.Generic;
using Discord;
using UnityEngine;

namespace DiscordCommon
{
    public class DiscordFriendsList : MonoBehaviour
    {
        #region Serialized

        public Transform contentContainer;
        public DiscordFriendItem itemPrefab;
        
        #endregion
        
        #region Setup
        
        public Discord.Discord discord;
        public RelationshipManager manager;
        
        public void Init(Discord.Discord discord)
        {
            this.discord = discord;
            this.manager = discord.GetRelationshipManager();

            RegisterCallbacks();

            void RegisterCallbacks()
            {
                manager.OnRefresh            -= OnRefresh;
                manager.OnRelationshipUpdate -= OnRelationshipUpdate;
                
                manager.OnRefresh            += OnRefresh;
                manager.OnRelationshipUpdate += OnRelationshipUpdate;
            }
        }
        
        #endregion

        #region Callbacks

        private void OnRefresh()
        {
            BuildFriendsList();
        }
        
        private void OnRelationshipUpdate(ref Relationship relationship)
        {
            UpdateFriend(ref relationship);
        }

        #endregion

        #region Friends List

        private Dictionary<long, DiscordFriendItem> _friendList = new Dictionary<long, DiscordFriendItem>();

        public void BuildFriendsList()
        {
            CleanupFriendsList();
            manager.Filter(((ref Relationship relationship) => relationship.Type == RelationshipType.Friend));

            for (uint i = 0; i < manager.Count(); i++)
            {
                var rlxn = manager.GetAt(i);
                UpdateFriend(ref rlxn);
            }
        }

        public void UpdateFriend(ref Relationship relationship)
        {
            if(!_friendList.TryGetValue(relationship.User.Id, out var item))
            {
                item = Instantiate(itemPrefab, contentContainer);
                _friendList.Add(relationship.User.Id, item);
            }

            item.Refresh(ref relationship, discord);
        }

        #endregion
        
        #region Cleanup
        
        public void CleanupFriendsList()
        {
            foreach (var pair in _friendList)
            {
                Destroy(pair.Value.gameObject);
            }
            
            _friendList.Clear();
        }

        private bool applicationClosing = false;

        public void OnDestroy()
        {
            if (applicationClosing) return;
            CleanupFriendsList();
        }

        public void OnApplicationQuit()
        {
            applicationClosing = true;
        }

        #endregion
    }
}