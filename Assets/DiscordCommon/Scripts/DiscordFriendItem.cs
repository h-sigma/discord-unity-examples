using System;
using Discord;
using TMPro;
using UnityEngine;

namespace DiscordCommon
{
    public class DiscordFriendItem : MonoBehaviour
    {
        private Discord.Discord _discord;
        private Relationship _current;

        private static ImageManager _imageManager;

        #region UI

        public TextMeshProUGUI username;
        public TextMeshProUGUI discriminator;
        public DiscordAvatar    avatar;
        public StatusIndicator status;

        public void Refresh(ref Relationship relationship, Discord.Discord discord)
        {
            _discord = discord;
            _current = relationship;
            if (_imageManager == null)
            {
                _imageManager = discord.GetImageManager();
            }
            Refresh();
        }

        public void Refresh()
        {
            username.text      = _current.User.Username;
            discriminator.text = "#" + _current.User.Discriminator;

            if (avatar.userId != _current.User.Id) //save downloads
            {
                avatar.LoadAvatar(_current.User.Id, _imageManager);
            }
            
            status.SetStatus(_current.Presence.Status);
        }

        #endregion
    }
}