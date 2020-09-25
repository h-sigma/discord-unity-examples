using System;
using System.Collections.Generic;
using Discord;
using UnityEngine;
using UnityEngine.UI;

namespace DiscordCommon
{
    public class StatusIndicator : MonoBehaviour
    {
        [SerializeField]
        private Image indicator;

        [SerializeField]
        private List<Pair> statuses;

        [Serializable]
        public class Pair
        {
            public Status status;
            public Color  color;
        }

        private Status _currentStatus;

        public void SetStatus(Status status)
        {
            var result = statuses.Find(s => s.status == status);
            if (result == null) return;
            _currentStatus = status;
            indicator.color = result.color;
        }
    }
}