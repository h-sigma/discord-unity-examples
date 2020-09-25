using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace DiscordCommon
{
    public abstract class AvatarBase : MonoBehaviour
    {
        public RawImage image;
        public Texture  avatar;
    
        [Serializable]
        public class AvatarAvailableEvent : UnityEvent<Texture> {}

        public AvatarAvailableEvent onAvatarAvailable;

        public abstract void LoadAvatar();
    
        protected void FetchImage(string url)
        {
            StartCoroutine(_FetchImage(url));
        }
    
        private IEnumerator _FetchImage(string url)
        {
            var webr = UnityWebRequestTexture.GetTexture(url);

            yield return webr.SendWebRequest();

            if (webr.isHttpError || webr.isNetworkError)
            {
                Debug.Log($"Network error fetching texture from URL: {url}");
            }
            else
            {
                avatar = ((DownloadHandlerTexture) webr.downloadHandler).texture;

                if(image != null)
                {
                    image.texture = avatar;
                }
            
                onAvatarAvailable.Invoke(avatar);
            }
        }
    }
}