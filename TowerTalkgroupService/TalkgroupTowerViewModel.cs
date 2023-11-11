﻿using ServiceCommon;
using System;

namespace TowerTalkgroupService
{
    public sealed class TalkgroupTowerViewModel : IViewModel
    {
        public int TowerNumber { get; private set; }
        public string TowerName { get; private set; }
        public DateTime FirstSeen { get; private set; }
        public DateTime LastSeen { get; private set; }
        public int AffiliationCount { get; private set; }
        public int DeniedCount { get; private set; }
        public int VoiceCount { get; private set; }
        public int EncryptedCount { get; private set; }

        public TalkgroupTowerViewModel()
        {
        }

        public TalkgroupTowerViewModel(int towerNumber, string towerName, DateTime firstSeen, DateTime lastSeen, int affiliationCount, int deniedCount,
            int voiceCount, int encryptedCount) =>

            (TowerNumber, TowerName, FirstSeen, LastSeen, AffiliationCount, DeniedCount, VoiceCount, EncryptedCount) =
            (towerNumber, towerName, firstSeen, lastSeen, affiliationCount, deniedCount, voiceCount, encryptedCount);
    }
}
