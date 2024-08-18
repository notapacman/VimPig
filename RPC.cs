using DiscordRPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VimPig
{
    internal class RPC
    {
        public static DiscordRpcClient client;
        public static Timestamps rpctimestamp { get; set; }
        private static RichPresence presence;
        public static void InitializeRPC()
        {
            client = new DiscordRpcClient("1274791778519613490");
            client.Initialize();
            DiscordRPC.Button[] buttons = { new DiscordRPC.Button() { Label = "Download Now", Url = "https://github.com/harampig/VimPig/releases/tag/SuperRelease" } };

            presence = new RichPresence()
            {
                Details = "The text editor BUT this is Vim!!! (no:))",
                State = "Editing..",
                Timestamps = rpctimestamp,
                Buttons = buttons,

                Assets = new Assets()
                {
                    LargeImageKey = "icon",
                    LargeImageText = "VimPig"
                }
            };
            SetState("Editing..");
        }
        public static void SetState(string state, bool watching = false)
        {
            if (watching)
                state = "" + state;

            presence.State = state;
            client.SetPresence(presence);
        }
    }
}
