using Resources;
using Resources.Datagram;
using Resources.Packet;
using Resources.Utilities;
using System;
using System.Drawing;
using System.Linq;

namespace Bridge.Extensions {
    public static class NameSubjectToChange {
        public static void Init() {
            BridgeCore.EntityUpdateReceived += Teleport;
            BridgeCore.AttackReceived += Knockback;
            BridgeCore.PassiveProcReceived += Poison;
            BridgeCore.EntityActionSent += HandleEntityAction;
        }

        private static void Teleport(EntityUpdate entityUpdate) {
            if (BridgeCore.status == BridgeStatus.Playing) {
                if (entityUpdate.guid == BridgeCore.guid) {
                    CwRam.Teleport(entityUpdate.position);
                    //todo: prevent packet from being sent to client
                }
            }
        }
        private static void Knockback(Attack attack) {
            CwRam.Knockback(attack.Direction);
        }
        private static void Poison(Proc proc) {
            if (proc.Type == ProcType.Poison && proc.Target == BridgeCore.guid) {
                var su = new ServerUpdate();
                su.hits.Add(new Hit() {
                    damage = proc.Modifier,
                    target = BridgeCore.guid,
                    position = BridgeCore.dynamicEntities[BridgeCore.guid].position,
                });
                bool tick() {
                    bool f = BridgeCore.status == BridgeStatus.Playing && BridgeCore.dynamicEntities[BridgeCore.guid].HP > 0;
                    if (f) {
                        BridgeCore.SendToClient(su);
                    }
                    return !f;
                }
                Tools.DoLater(tick, 500, 7);
            }
        }
        
        private static void HandleEntityAction(EntityAction entityAction) {
            switch (entityAction.type) {
                case ActionType.Talk:
                    #region Talk
                    break;
                #endregion
                case ActionType.StaticInteraction:
                    #region StaticInteraction
                    ChatMessage x = new ChatMessage() {
                        message = "static interation is disabled",
                        sender = 0
                    };
                    BridgeCore.SendToClient(x);
                    break;
                #endregion
                case ActionType.PickUp:
                    #region PickUp
                    break;
                #endregion
                case ActionType.Drop: //send item back to dropper because dropping is disabled to prevent chatspam
                    #region Drop
                    if (BridgeCore.form.radioButtonDestroy.Checked) {
                        BridgeCore.SendToClient(new ChatMessage() {
                            message = "item destroyed",
                            sender = 0,
                        });
                    }
                    else {
                        var serverUpdate = new ServerUpdate();
                        var pickup = new ServerUpdate.Pickup() {
                            guid = BridgeCore.guid,
                            item = entityAction.item
                        };
                        serverUpdate.pickups.Add(pickup);
                        if (BridgeCore.form.radioButtonDuplicate.Checked) {
                            serverUpdate.pickups.Add(pickup);
                        }
                        BridgeCore.SendToClient(serverUpdate);
                    }
                    break;
                #endregion
                case ActionType.CallPet:
                    #region CallPet
                    break;
                #endregion
                default:
                    //unknown type
                    break;
            }
        }
        private static void HandlePassiveProc(PassiveProc passiveProc) {
            switch (passiveProc.type) {
                case ProcType.Bulwalk:
                    BridgeCore.SendToClient(new ChatMessage() {
                        message = string.Format("bulwalk: {0}% dmg reduction", 1.0f - passiveProc.modifier),
                        sender = 0,
                    });
                    break;
                case ProcType.WarFrenzy:
                    CwRam.PlayerEntity.BossBuff = true;
                    bool DisableBossBuff() {
                        bool f = BridgeCore.status == BridgeStatus.Playing && BridgeCore.dynamicEntities[BridgeCore.guid].HP > 0;
                        if (f) {
                            CwRam.PlayerEntity.BossBuff = false;
                        }
                        return !f;
                    }
                    Tools.DoLater(DisableBossBuff, passiveProc.duration, 1);
                    break;
                case ProcType.Camouflage:
                    break;
                case ProcType.Poison:
                    break;
                case ProcType.UnknownA:
                    break;
                case ProcType.ManaShield:
                    BridgeCore.SendToClient(new ChatMessage() {
                        message = string.Format("manashield: {0}", passiveProc.modifier),
                        sender = 0,
                    });
                    break;
                case ProcType.UnknownB:
                    break;
                case ProcType.UnknownC:
                    break;
                case ProcType.FireSpark:
                    break;
                case ProcType.Intuition:
                    break;
                case ProcType.Elusiveness:
                    break;
                case ProcType.Swiftness:
                    break;
                default:
                    break;
            }
        }
        private static void ClientSideChatCommands(ChatMessage chatMessage) {
            if (chatMessage.message.ToLower() == @"/plane") {
                Console.Beep();
                var serverUpdate = new ServerUpdate() {
                    blockDeltas = new Vox("model.vox").Parse(),
                };
                foreach (var block in serverUpdate.blockDeltas) {
                    block.position.x += 0x802080;//(int)(dynamicEntities[guid].position.x / 0x10000);//8286946;
                    block.position.y += 0x802080;//(int)(dynamicEntities[guid].position.y / 0x10000);//8344456;
                    block.position.z += 150;// (int)(dynamicEntities[guid].position.z / 0x10000);//220;
                }
                BridgeCore.SendToClient(serverUpdate);
            }
            if (chatMessage.message.ToLower() == @"/spawn") {
                CwRam.Teleport(new LongVector() {
                    x = 0x8020800000,
                    y = 0x8020800000,
                    z = 0,
                });
            }
        }
    }
}
