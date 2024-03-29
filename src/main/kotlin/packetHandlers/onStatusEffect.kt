package packetHandlers

import kotlinx.coroutines.delay
import kotlinx.coroutines.launch
import com.github.lastexceed.cubeworldnetworking.packets.*
import com.github.lastexceed.cubeworldnetworking.utils.*
import Player
import modules.Balancing

fun onStatusEffect(packet: StatusEffect, source: Player) {
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
		StatusEffect.Type.WarFrenzy -> Balancing.warFrenzyBuff(packet, source)
		StatusEffect.Type.Bulwalk,
		StatusEffect.Type.Camouflage,
		StatusEffect.Type.ManaShield,
		StatusEffect.Type.FireSpark,
		StatusEffect.Type.Intuition,
		StatusEffect.Type.Elusiveness,
		StatusEffect.Type.Swiftness -> {}

		else -> return//error("unknown status effect type $type")
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
				damage = statusEffect.modifier * (1f - Balancing.GLOBAL_DAMAGE_REDUCTION_PERCENT),
				critical = false,
				stuntime = 0,
				position = targetPlayer.character.position,
				direction = Vector3(0f, 0f, 0f),
				isYellow = true,
				type = Hit.Type.Default,
				flash = true
			)
			val sound = SoundEffect(
				position = Utils.creatureToSoundPosition(targetPlayer.character.position),
				sound = SoundEffect.Sound.SlimeGroan
			)
			val miscellaneous = WorldUpdate(
				hits = listOf(damageTick),
				soundEffects = listOf(sound)
			)

			targetPlayer.send(miscellaneous)
		}
	}
}