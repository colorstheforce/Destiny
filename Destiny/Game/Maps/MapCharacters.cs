﻿using Destiny.Game.Characters;
using Destiny.Network.Packet;

namespace Destiny.Game.Maps
{
    public sealed class MapCharacters : MapObjects<Character>
    {
        public MapCharacters(Map map) : base(map) { }

        protected override void InsertItem(int index, Character item)
        {
            // TODO: Broadcast characters enter field packet to character.

            base.InsertItem(index, item);

            lock (this.Map.Mobs)
            {
                foreach (Mob mob in this.Map.Mobs)
                {
                    item.Client.Send(MobPacket.MobEnterField(mob));
                }
            }

            lock (this.Map.Npcs)
            {
                foreach (Npc npc in this.Map.Npcs)
                {
                    item.Client.Send(NpcPacket.NpcEnterField(npc));
                }
            }

            lock (this.Map.Mobs)
            {
                foreach (Mob mob in this.Map.Mobs)
                {
                    mob.AssignController();
                }
            }

            lock (this.Map.Npcs)
            {
                foreach (Npc npc in this.Map.Npcs)
                {
                    npc.AssignController();
                }
            }

            // TODO: Broadcast character enter field packet to map.
        }

        protected override void RemoveItem(int index)
        {
            Character item = base.Items[index];

            item.ControlledMobs.Clear();
            item.ControlledNpcs.Clear();

            base.RemoveItem(index);

            lock (this.Map.Mobs)
            {
                foreach (Mob mob in this.Map.Mobs)
                {
                    mob.AssignController();
                }
            }

            lock (this.Map.Npcs)
            {
                foreach (Npc npc in this.Map.Npcs)
                {
                    npc.AssignController();
                }
            }

            // TODO: Broadcast character leave field packet to map.
        }
    }
}
