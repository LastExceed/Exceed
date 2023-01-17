package packetHandlers

import modules.Balancing
import Player
import com.github.lastexceed.cubeworldnetworking.packets.*
import com.github.lastexceed.cubeworldnetworking.utils.Utils

fun onHit(packet: Hit, source: Player) {
//	if (ClientBugFixes.ignoreSelfHeal(source, packet)) {
//		return
//	}

	val target = source.layer.players[packet.target] ?: return //in case target disconnected in this moment

	val adjustedHit = Balancing.adjustDamage(packet, source.character, target.character)
		.copy(flash = true)

	val soundEffects =
		if (packet.damage < 0) {
			target.layer.broadcast(
				WorldUpdate(
					hits = listOf(adjustedHit),
					soundEffects = listOf(
						SoundEffect(
							position = Utils.creatureToSoundPosition(packet.position),
							sound = SoundEffect.Sound.Watersplash
						)
					)
				),
				source
			)
			emptyList()
		} else
			when (packet.type) {
				Hit.Type.Absorb -> listOf(SoundEffect.Sound.Absorb)
				Hit.Type.Block -> listOf(SoundEffect.Sound.Block)
				Hit.Type.Default -> listOf(
					SoundEffect.Sound.Punch1,
					when (target.character.race) {
						Race.ElfMale -> SoundEffect.Sound.MaleGroan
						Race.ElfFemale -> SoundEffect.Sound.FemaleGroan
						Race.HumanMale -> SoundEffect.Sound.MaleGroan2
						Race.HumanFemale -> SoundEffect.Sound.FemaleGroan2
						Race.GoblinMale -> SoundEffect.Sound.GoblinMaleGroan
						Race.GoblinFemale -> SoundEffect.Sound.GoblinFemaleGroan
						Race.LizardmanMale -> SoundEffect.Sound.LizardMaleGroan
						Race.LizardmanFemale -> SoundEffect.Sound.LizardFemaleGroan
						Race.DwarfMale -> SoundEffect.Sound.DwarfMaleGroan
						Race.DwarfFemale -> SoundEffect.Sound.DwarfFemaleGroan
						Race.OrcMale -> SoundEffect.Sound.OrcMaleGroan
						Race.OrcFemale -> SoundEffect.Sound.OrcFemaleGroan
						Race.FrogmanMale -> SoundEffect.Sound.FrogmanMaleGroan
						Race.FrogmanFemale -> SoundEffect.Sound.FrogmanFemaleGroan
						Race.UndeadMale -> SoundEffect.Sound.UndeadMaleGroan
						Race.UndeadFemale -> SoundEffect.Sound.UndeadFemaleGroan
						else -> error("unreachable")
					}
				)

				else -> emptyList()
			}

	val worldUpdate = WorldUpdate(
		hits = listOf(adjustedHit),
		soundEffects = soundEffects.map {
			SoundEffect(
				position = Utils.creatureToSoundPosition(packet.position),
				sound = it
			)
		}
	)

	target.send(worldUpdate)
}