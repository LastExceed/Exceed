package packetHandlers

import kotlinx.coroutines.delay
import kotlinx.coroutines.launch
import com.github.lastexceed.cubeworldnetworking.packets.*
import com.github.lastexceed.cubeworldnetworking.utils.*
import Player
import modules.Balancing

object StatusEffectHandler : PacketHandler<StatusEffect> {
	override suspend fun handlePacket(packet: StatusEffect, source: Player) {
		when (val type = packet.type) {
			StatusEffect.Type.Poison -> {
				applyPoisonDamageTicks(source, packet)
				WorldUpdate(
					statusEffects = listOf(packet),
					soundEffects = listOf(
						SoundEffect(
							Utils.creatureToSoundPosition(source.character.position),
							SoundEffect.Sound.FireTrap
						)
					)
				)
			}
			StatusEffect.Type.WarFrenzy,
			StatusEffect.Type.Bulwalk,
			StatusEffect.Type.Camouflage,
			StatusEffect.Type.ManaShield,
			StatusEffect.Type.FireSpark,
			StatusEffect.Type.Intuition,
			StatusEffect.Type.Elusiveness,
			StatusEffect.Type.Swiftness -> {}

			else -> error("unknown status effect type $type")
		}
		source.layer.broadcast(WorldUpdate(statusEffects = listOf(packet)), source)
	}

	private fun applyPoisonDamageTicks(source: Player, statusEffect: StatusEffect) {
		//todo: support non-players
		val targetPlayer = source.layer.players[statusEffect.target] ?: return
		targetPlayer.scope.launch {
			repeat(statusEffect.duration / 500 + 1) {
				if (it != 0) delay(500)

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
				val sound = SoundEffect(
					position = Utils.creatureToSoundPosition(targetPlayer.character.position),
					sound = SoundEffect.Sound.Absorb
				)
				val miscellaneous = WorldUpdate(
					hits = listOf(damageTick),
					soundEffects = listOf(sound)
				)

				targetPlayer.send(miscellaneous)
			}
		}
	}
}