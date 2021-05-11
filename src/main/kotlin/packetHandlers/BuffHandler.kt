package packetHandlers

import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.delay
import kotlinx.coroutines.launch
import me.lastexceed.cubeworldnetworking.packets.*
import me.lastexceed.cubeworldnetworking.utils.*
import Player
import kotlinx.coroutines.Dispatchers

object BuffHandler : PacketHandler<Buff> {
	override suspend fun handlePacket(packet: Buff, source: Player) {
		val su = ServerUpdate()

		if (packet.type == BuffType.Poison) {
			applyPoison(source, packet)
			val sound = Sound(
				Utils.creatureToSoundPosition(source.character.position),
				SoundType.FireTrap
			)
			su.sounds.add(sound)
		}
		su.buffs.add(packet)
		source.layer.broadcast(su, source)
	}

	private fun applyPoison(source: Player, buff: Buff) {
		val targetPlayer = source.layer.players[buff.target]!!
		GlobalScope.launch(Dispatchers.IO) {
			fun createPoisonTick(): ServerUpdate {
				val target = source.layer.creatures[buff.target]!!

				val poisonTick = Hit(
					source.character.id,
					buff.target,
					buff.modifier,
					false,
					0,
					0,
					target.position,
					Vector3(0f, 0f, 0f),
					false,
					DamageType.Unknown,
					false,
					0
				)
				val sound = Sound(
					Utils.creatureToSoundPosition(target.position),
					SoundType.Absorb
				)
				val su = ServerUpdate()
				su.hits.add(poisonTick)
				su.sounds.add(sound)
				return su
			}

			targetPlayer.send(createPoisonTick())
			repeat(buff.duration / 500) {
				delay(500)
				targetPlayer.send(createPoisonTick())
			}
		}
	}
}