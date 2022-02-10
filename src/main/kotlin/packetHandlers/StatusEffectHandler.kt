package packetHandlers

import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.delay
import kotlinx.coroutines.launch
import com.github.lastexceed.cubeworldnetworking.packets.*
import com.github.lastexceed.cubeworldnetworking.utils.*
import Player
import kotlinx.coroutines.Dispatchers

object StatusEffectHandler : PacketHandler<StatusEffect> {
	override suspend fun handlePacket(packet: StatusEffect, source: Player) {
		val sound = if (packet.type == StatusEffect.Type.Poison) {
			applyPoison(source, packet)
			Sound(
				Utils.creatureToSoundPosition(source.character.position),
				Sound.Type.FireTrap
			)
		} else null
		val su = ServerUpdate(
			sounds = if (sound != null) listOf(sound) else emptyList(), //todo: ugly
			statusEffects = listOf(packet)
		)
		source.layer.broadcast(su, source)
	}

	private fun applyPoison(source: Player, statusEffect: StatusEffect) {
		val targetPlayer = source.layer.players[statusEffect.target]!!
		GlobalScope.launch(Dispatchers.IO) {
			fun createPoisonTick(): ServerUpdate {
				val target = source.layer.creatures[statusEffect.target]!!

				val poisonTick = Hit(
					source.character.id,
					statusEffect.target,
					statusEffect.modifier,
					false,
					0,
					0,
					target.position,
					Vector3(0f, 0f, 0f),
					false,
					Hit.Type.Unknown,
					false,
					0
				)
				val sound = Sound(
					Utils.creatureToSoundPosition(target.position),
					Sound.Type.Absorb
				)
				val su = ServerUpdate(
					hits = listOf(poisonTick),
					sounds = listOf(sound)
				)
				return su
			}

			targetPlayer.send(createPoisonTick())
			repeat(statusEffect.duration / 500) {
				delay(500)
				targetPlayer.send(createPoisonTick())
			}
		}
	}
}