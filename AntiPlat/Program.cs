﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TShockAPI;
using Terraria;
using TerrariaApi.Server;
using Terraria.Localization;
using Terraria.ID;

namespace AntiPlatPlugin
{
    [ApiVersion(2, 1)]
    public class AntiPlat : TerrariaPlugin
    {
        public override string Author => "Ozz5581";

        public override string Description => "Prevents 999 Plat Stacks";

        public override string Name => "AntiPlat";

        public override Version Version => new Version(1, 0, 0, 0);

        private ulong UpdateCount = 0;

        public AntiPlat(Main game) : base(game)
        {

        }

        public override void Initialize()
        {
            ServerApi.Hooks.GameUpdate.Register(this, OnUpdate);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ServerApi.Hooks.GameUpdate.Deregister(this, OnUpdate);
            }
            base.Dispose(disposing);
        }

        private IEnumerable<TSPlayer> GetLoggedInPlayers()
        {
            return TShock.Players.Where(p => p != null && p.IsLoggedIn);
        }

        private void OnUpdate(EventArgs args)
        {
            UpdateCount++;

            // Check for 999 platinum coins every 15 frames
            if (UpdateCount % 4 == 0)
            {
                foreach (TSPlayer plr in GetLoggedInPlayers())
                {
                    for (int i = 0; i < 260; i++)
                    {
                        if (plr.TPlayer.inventory[i].type == ItemID.PlatinumCoin &&
                            plr.TPlayer.inventory[i].stack == 999)
                        {
                            NetMessage.SendData((int)PacketTypes.PlayerSlot, -1, -1, NetworkText.Empty, plr.Index, i);
                        }
                    }
                }
            }
        }
    }
}