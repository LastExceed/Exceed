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
		val soundList =
			if (packet.type == StatusEffect.Type.Poison) {
				applyPoison(source, packet)

				listOf(
					Sound(
						Utils.creatureToSoundPosition(source.character.position),
						Sound.Type.FireTrap
					)
				)
			} else emptyList()
		val serverUpdate = ServerUpdate(
			sounds = soundList,
			statusEffects = listOf(packet)
		)
		source.layer.broadcast(serverUpdate, source)
	}

	private fun applyPoison(source: Player, statusEffect: StatusEffect) {

		GlobalScope.launch(Dispatchers.IO) {//todo: pls no
			repeat(statusEffect.duration / 500 + 1) {
				if (it != 0) delay(500)

				//todo: support non-players
				val targetPlayer = source.layer.players[statusEffect.target]!! //todo: sanity check
				val damageTick = Hit(
					source.character.id,
					statusEffect.target,
					statusEffect.modifier,
					false,
					0,
					0,
					targetPlayer.character.position,
					Vector3(0f, 0f, 0f),
					false,
					Hit.Type.Unknown,
					false,
					0
				)
				val sound = Sound(
					Utils.creatureToSoundPosition(targetPlayer.character.position),
					Sound.Type.Absorb
				)
				val serverUpdate = ServerUpdate(
					hits = listOf(damageTick),
					sounds = listOf(sound)
				)

				targetPlayer.send(serverUpdate)
			}
		}
	}
}