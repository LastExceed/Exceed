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
		val miscellaneous = Miscellaneous(
			sounds = soundList,
			statusEffects = listOf(packet)
		)
		source.layer.broadcast(miscellaneous, source)
	}

	private fun applyPoison(source: Player, statusEffect: StatusEffect) {

		GlobalScope.launch(Dispatchers.IO) {//todo: pls no
			repeat(statusEffect.duration / 500 + 1) {
				if (it != 0) delay(500)

				//todo: support non-players
				val targetPlayer = source.layer.players[statusEffect.target]!! //todo: sanity check
				val damageTick = Hit(
					attacker = source.character.id,
					target = statusEffect.target,
					damage = statusEffect.modifier,
					critical = false,
					stuntime = 0,
					position = targetPlayer.character.position,
					direction = Vector3(0f, 0f, 0f),
					isYellow = false,
					type = Hit.Type.Unknown,
					flash = false
				)
				val sound = Sound(
					position = Utils.creatureToSoundPosition(targetPlayer.character.position),
					type = Sound.Type.Absorb
				)
				val miscellaneous = Miscellaneous(
					hits = listOf(damageTick),
					sounds = listOf(sound)
				)

				targetPlayer.send(miscellaneous)
			}
		}
	}
}