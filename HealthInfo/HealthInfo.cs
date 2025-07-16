using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using System;

namespace HealthInfo
{
    public class HealthInfo : Plugin<Config>
    {
        public override string Author => "Vretu";
        public override string Name => "HealthInfo";
        public override string Prefix => "HealthInfo";
        public override Version Version => new Version(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(9, 6, 0);

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

        private void OnHurt(HurtEventArgs ev)
        {
            UpdateHp(ev.Player);
        }

        private void OnHealed(HealedEventArgs ev)
        {
            UpdateHp(ev.Player);
        }

        private void OnSpawned(SpawnedEventArgs ev)
        {
            UpdateHp(ev.Player);
        }

        private void UpdateHp(Player player)
        {
            if (player == null)
                return;

            string roleTeam = player.Role.Team.ToString();

            if (player.IsAlive && !Config.IgnoredTeams.Contains(roleTeam))
            {
                float totalHp = player.Health + player.ArtificialHealth;
                player.CustomInfo = $"({totalHp:0}/{player.MaxHealth:0})\n";
            }
        }
    }
}