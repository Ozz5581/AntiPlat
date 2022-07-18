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

        public AntiPlat(Main game) : base(game)
        {

        }

        public override void Initialize()
        {
            ServerApi.Hooks.NetGetData.Register(this, OnGetData);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Deregister hooks here
            }
            base.Dispose(disposing);
        }

        private void OnGetData(GetDataEventArgs args)
        {
            for (int ii = 0; ii < 58; ii++)
            {
                foreach (TSPlayer plr in TShock.Players.Where(p => p != null && p.IsLoggedIn &&
                p.TPlayer.inventory[ii].netID == ItemID.PlatinumCoin &&
                p.TPlayer.inventory[ii].type == ItemID.PlatinumCoin &&
                p.TPlayer.inventory[ii].stack == 999))
                {
                    plr.TPlayer.inventory[ii].netDefaults(0);
                    NetMessage.SendData((int)PacketTypes.PlayerSlot, -1, -1, NetworkText.Empty, plr.Index, (float)ii);
                }
            }
        }

    }
}