using Resources;
using Resources.Datagram;
using Resources.Packet;
using Resources.Utilities;
using System.Windows.Forms;

namespace Bridge.Extensions {
    public static class SpecialMoves {
        private static ushort lastTarget;

        public static void Init() {
            BridgeCore.HitSent += RememberLastTarget;
            BridgeCore.SpecialMoveReceived += HandleSpecialMoves;
            BridgeCore.ClientConnected += EnableHotkeys;
            BridgeCore.ClientDisconnected += DisableHotkeys;
        }

        private static void RememberLastTarget(Hit hit) {
            lastTarget = (ushort)hit.target;
        }
        private static void HandleSpecialMoves(SpecialMove specialMove) {
            switch (specialMove.Id) {
                case SpecialMoveID.Taunt:
                    if (BridgeCore.dynamicEntities.ContainsKey(specialMove.Guid)) {
                        if (BridgeCore.status == BridgeStatus.Playing) {
                            CwRam.Teleport(BridgeCore.dynamicEntities[specialMove.Guid].position);
                            CwRam.Freeze(5000);
                        }
                    }
                    break;
                case SpecialMoveID.CursedArrow:
                    break;
                case SpecialMoveID.ArrowRain:
                    break;
                case SpecialMoveID.Shrapnel:
                    var su = new ServerUpdate();
                    var blood_hit = new Hit() {
                        damage = 5f,
                        target = specialMove.Guid,
                    };
                    su.hits.Add(blood_hit);
                    var blood_particles = new ServerUpdate.Particle() {
                        count = 10,
                        spread = 2f,
                        type = ParticleType.Normal,
                        size = 0.1f,
                        velocity = new FloatVector() {
                            z = 1f,
                        },
                        color = new FloatVector() {
                            x = 1f,
                            y = 0f,
                            z = 0f
                        },
                        alpha = 1f,
                    };
                    su.particles.Add(blood_particles);
                    bool tick() {
                        bool f = BridgeCore.status == BridgeStatus.Playing && BridgeCore.dynamicEntities[specialMove.Guid].HP > 0;
                        if (f) {
                            blood_hit.position = blood_particles.position = BridgeCore.dynamicEntities[specialMove.Guid].position;
                            BridgeCore.SendToClient(su);
                        }
                        return !f;
                    }
                    Tools.DoLater(tick, 50, 100);
                    break;
                case SpecialMoveID.SmokeBomb:
                    var serverUpdate = new ServerUpdate();
                    serverUpdate.particles.Add(new ServerUpdate.Particle() {
                        count = 1000,
                        spread = 5f,
                        type = ParticleType.NoGravity,
                        size = 5f,
                        velocity = new FloatVector(),
                        color = new FloatVector() {
                            x = 1f,
                            y = 1f,
                            z = 1f
                        },
                        alpha = 1f,
                        position = BridgeCore.dynamicEntities[specialMove.Guid].position
                    });
                    if (BridgeCore.status == BridgeStatus.Playing) {
                        BridgeCore.SendToClient(serverUpdate);
                    }
                    break;
                case SpecialMoveID.IceWave:
                    if (specialMove.Guid != BridgeCore.guid) {//distance small enough
                        CwRam.Freeze(3000);
                    }
                    serverUpdate = new ServerUpdate();
                    serverUpdate.particles.Add(new ServerUpdate.Particle() {
                        count = 100,
                        spread = 4f,
                        type = ParticleType.NoGravity,
                        size = 0.3f,
                        velocity = new FloatVector(),
                        color = new FloatVector() {
                            x = 0f,
                            y = 1f,
                            z = 1f
                        },
                        alpha = 1f,
                        position = BridgeCore.dynamicEntities[specialMove.Guid].position
                    });
                    serverUpdate.particles.Add(new ServerUpdate.Particle() {
                        count = 100,
                        spread = 10f,
                        type = ParticleType.NoGravity,
                        size = 0.1f,
                        velocity = new FloatVector(),
                        color = new FloatVector() {
                            x = 1f,
                            y = 1f,
                            z = 1f
                        },
                        alpha = 1f,
                        position = BridgeCore.dynamicEntities[specialMove.Guid].position
                    });
                    serverUpdate.particles.Add(new ServerUpdate.Particle() {
                        count = 100,
                        spread = 2f,
                        type = ParticleType.NoGravity,
                        size = 0.7f,
                        velocity = new FloatVector(),
                        color = new FloatVector() {
                            x = 0f,
                            y = 0f,
                            z = 1f
                        },
                        alpha = 1f,
                        position = BridgeCore.dynamicEntities[specialMove.Guid].position
                    });
                    if (BridgeCore.status == BridgeStatus.Playing) {
                        BridgeCore.SendToClient(serverUpdate);
                    }
                    break;
                case SpecialMoveID.Confusion:
                    break;
                case SpecialMoveID.ShadowStep:
                    break;
                default:
                    break;
            }
        }

        private static void EnableHotkeys() {
            BridgeCore.form.keyboardHook.KeyboardChanged += OnKeyboardChanged;
        }
        private static void DisableHotkeys() {
            BridgeCore.form.keyboardHook.KeyboardChanged -= OnKeyboardChanged;
        }
        private static void OnKeyboardChanged(Keys key, bool isDown) {
            switch (key) {
                case Keys.D4 when isDown:
                    OnHotkey(HotkeyID.CtrlSpace);
                    break;
                case Keys.D5 when isDown:
                    OnHotkey(HotkeyID.SpecialMove2);
                    break;
                case Keys.D0 when isDown:
                    OnHotkey(HotkeyID.TeleportToTown);
                    break;
                default:
                    break;
            }
        }
        private static void OnHotkey(HotkeyID hotkey) {
            if (CwRam.AnyInterfaceOpen) return;

            if (hotkey == HotkeyID.TeleportToTown) {
                CwRam.SetMode(Mode.Teleport_To_City, 0);
                return;
            }
            var notification = new ChatMessage() {
                sender = 0,
            };
            bool spec = BridgeCore.dynamicEntities[BridgeCore.guid].specialization == 1;
            switch (BridgeCore.dynamicEntities[BridgeCore.guid].entityClass) {
                case EntityClass.Rogue when spec:
                    #region ninja
                    if (hotkey == HotkeyID.CtrlSpace) {
                        #region dash
                        notification.message = "using [dash]";
                        CwRam.SetMode(Mode.Spin_Run, 0);
                        #endregion
                    }
                    else {
                        #region blink
                        notification.message = "using [blink]";
                        if (BridgeCore.dynamicEntities.ContainsKey(lastTarget)) {
                            CwRam.Teleport(BridgeCore.dynamicEntities[lastTarget].position);
                        }
                        #endregion
                    }
                    break;
                #endregion
                case EntityClass.Rogue:
                    #region assassin
                    if (hotkey == HotkeyID.CtrlSpace) {
                        #region confusion
                        notification.message = "TODO: [confusion]";
                        var specialMove = new SpecialMove() {
                            Guid = BridgeCore.guid,
                            Id = SpecialMoveID.Confusion,
                        };
                        BridgeCore.SendUDP(specialMove.data);
                        #endregion
                    }
                    else {
                        #region shadow step
                        notification.message = "TOD: [shadow step]";
                        var specialMove = new SpecialMove() {
                            Guid = BridgeCore.guid,
                            Id = SpecialMoveID.ShadowStep,
                        };
                        BridgeCore.SendUDP(specialMove.data);
                        #endregion
                    }
                    break;
                #endregion
                case EntityClass.Warrior when spec:
                    #region guardian
                    if (hotkey == HotkeyID.CtrlSpace) {
                        #region taunt
                        notification.message = "using [taunt]";
                        var specialMove = new SpecialMove() {
                            Guid = lastTarget,
                            Id = SpecialMoveID.Taunt,
                        };
                        BridgeCore.SendUDP(specialMove.data);
                        #endregion
                    }
                    else {
                        #region steel wall
                        notification.message = "using [steel wall]";
                        CwRam.SetMode(Mode.Boss_Skill_Block, 0);
                        #endregion
                    }
                    break;
                #endregion
                case EntityClass.Warrior:
                    #region berserk
                    if (hotkey == HotkeyID.CtrlSpace) {
                        #region boulder toss
                        notification.message = "using [boulder toss]";
                        CwRam.SetMode(Mode.Boulder_Toss, 0);
                        #endregion
                    }
                    else {
                        #region earth shatter
                        notification.message = "using [earth shatter]";
                        CwRam.SetMode(Mode.Earth_Shatter, 0);
                        #endregion
                    }
                    break;
                #endregion
                case EntityClass.Mage when spec:
                    #region watermage
                    if (hotkey == HotkeyID.CtrlSpace) {
                        #region splash
                        notification.message = "using [splash]";
                        CwRam.SetMode(Mode.Splash, 0);
                        #endregion
                    }
                    else {
                        #region ice wave
                        notification.message = "using [ice wave]";
                        var specialMove = new SpecialMove() {
                            Guid = BridgeCore.guid,
                            Id = SpecialMoveID.IceWave,
                        };
                        BridgeCore.SendUDP(specialMove.data);
                        #endregion
                    }
                    break;
                #endregion
                case EntityClass.Mage:
                    #region firemage
                    if (hotkey == HotkeyID.CtrlSpace) {
                        #region lava
                        notification.message = "using [lava]";
                        CwRam.SetMode(Mode.Lava, 0);
                        #endregion
                    }
                    else {
                        #region beam
                        notification.message = "using [fire ray]";
                        CwRam.SetMode(Mode.FireRay, 0);
                        #endregion
                    }
                    break;
                #endregion
                case EntityClass.Ranger when spec:
                    #region scout
                    if (hotkey == HotkeyID.CtrlSpace) {
                        #region shrapnel
                        notification.message = "using [shrapnel] bleeding test";
                        var specialMove = new SpecialMove() {
                            Guid = BridgeCore.guid,
                            Id = SpecialMoveID.Shrapnel,
                        };
                        BridgeCore.SendUDP(specialMove.data);
                        #endregion
                    }
                    else {
                        #region smoke bomb
                        notification.message = "using [smoke bomb]";
                        var specialMove = new SpecialMove() {
                            Guid = BridgeCore.guid,
                            Id = SpecialMoveID.SmokeBomb,
                        };
                        BridgeCore.SendUDP(specialMove.data);

                        var fakeSmoke = new ServerUpdate();
                        fakeSmoke.particles.Add(new ServerUpdate.Particle() {
                            count = 1000,
                            spread = 5f,
                            type = ParticleType.NoGravity,
                            size = 0.3f,
                            velocity = new Resources.Utilities.FloatVector(),
                            color = new Resources.Utilities.FloatVector() {
                                x = 1f,
                                y = 1f,
                                z = 1f
                            },
                            alpha = 1f,
                            position = BridgeCore.dynamicEntities[specialMove.Guid].position
                        });
                        BridgeCore.SendToClient(fakeSmoke);
                        #endregion
                    }
                    break;
                #endregion
                case EntityClass.Ranger:
                    #region sniper
                    if (hotkey == HotkeyID.CtrlSpace) {
                        #region cursed arrow
                        //TODO
                        notification.message = "TODO: [cursed arrow]";
                        #endregion
                    }
                    else {
                        #region arrow rain
                        //TODO
                        notification.message = "TODO: [arrow rain]";
                        //const int rainSize = 7;
                        //for (int x = 0; x < rainSize; x++) {
                        //    for (int y = 0; y < rainSize; y++) {
                        //        var projectile = new Projectile() {
                        //            Scale = 1f,
                        //            Type = ProjectileType.Arrow,
                        //            Source = guid,
                        //            Velocity = new FloatVector() { x = 0, y = 0, z = -1f },
                        //            Position = new LongVector() {
                        //                x = 0x8020800000,//dynamicEntities[guid].position.x + (long)((dynamicEntities[guid].rayHit.x + x - rainSize / 2) * 0x10000),
                        //                y = 0x8020800000,//dynamicEntities[guid].position.y + (long)((dynamicEntities[guid].rayHit.y + y - rainSize / 2) * 0x10000),
                        //                z = 0x01000000,//dynamicEntities[guid].position.z + (long)((dynamicEntities[guid].rayHit.z + 10) * 0x10000),
                        //            }
                        //        };
                        //        SendUDP(projectile.data);
                        //        ProcessDatagram(projectile.data);
                        //    }
                        //}
                        #endregion
                    }
                    break;
                    #endregion
            }
            CwRam.memory.WriteInt(CwRam.EntityStart + 0x1164, 3);//mana cubes
            BridgeCore.SendToClient(notification);
        }
    }
}
