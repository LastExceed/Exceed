using Resources;
using Resources.Datagram;
using Resources.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Extensions {
    public static class SpecialMoves {
        public static void Init() {
            Server.SpecialMoveUsed += OnSpecialMove;
        }

        private static void OnSpecialMove(SpecialMove specialMove, Player source) {
            switch (specialMove.Id) {
                case SpecialMoveID.Taunt:
                    var target = Server.players.FirstOrDefault(p => p.entity.guid == specialMove.Guid);
                    if (target != null) {
                        specialMove.Guid = (ushort)source.entity.guid;
                        Server.SendUDP(specialMove.data, target);
                    }
                    break;
                case SpecialMoveID.SmokeBomb:
                    Server.BroadcastUDP(specialMove.data, source);
                    break;
                case SpecialMoveID.CursedArrow:
                case SpecialMoveID.ArrowRain:
                case SpecialMoveID.Shrapnel:
                case SpecialMoveID.IceWave:
                case SpecialMoveID.Confusion:
                case SpecialMoveID.ShadowStep:
                    Server.BroadcastUDP(specialMove.data);
                    break;
                default:
                    break;
            }
        }
    }
}
