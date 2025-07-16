using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using System;
using System.Text.RegularExpressions;

namespace HealthInfo
{
    public class HealthInfo : Plugin<Config>
    {
        public override string Author => "Vretu";
        public override string Name => "HealthInfo";
        public override string Prefix => "HealthInfo";
        public override Version Version => new Version(1, 1, 0);
        public override Version RequiredExiledVersion { get; } = new Version(9, 6, 0);
        public override PluginPriority Priority => PluginPriority.Lowest;

        private static readonly Regex HpRegex = new Regex(@"^\(\d{1,5}/\d{1,5}\)\s*\n?", RegexOptions.Compiled);

        public override void OnEnabled()
        {
            Exiled.Events.Handlers.Player.Hurt += OnHurt;
            Exiled.Events.Handlers.Player.Spawned += OnSpawned;
            Exiled.Events.Handlers.Player.Healed += OnHealed;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.Hurt -= OnHurt;
            Exiled.Events.Handlers.Player.Spawned -= OnSpawned;
            Exiled.Events.Handlers.Player.Healed -= OnHealed;
            base.OnDisabled();
        }

        private void OnHurt(HurtEventArgs ev) => UpdateHp(ev.Player);
        private void OnHealed(HealedEventArgs ev) => UpdateHp(ev.Player);
        private void OnSpawned(SpawnedEventArgs ev) => UpdateHp(ev.Player);

        private void UpdateHp(Player player)
        {
            if (player == null)
                return;

            string roleTeam = player.Role.Team.ToString();

            if (player.IsAlive && !Config.IgnoredTeams.Contains(roleTeam))
            {
                float totalHp = player.Health + player.ArtificialHealth;
                string hpInfo = $"({totalHp:0}/{player.MaxHealth:0})\n";

                string restInfo = player.CustomInfo ?? string.Empty;

                if (HpRegex.IsMatch(restInfo))
                    restInfo = HpRegex.Replace(restInfo, "");

                player.CustomInfo = hpInfo + restInfo;
            }
        }
    }
}