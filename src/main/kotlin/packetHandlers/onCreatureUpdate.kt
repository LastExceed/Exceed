package packetHandlers

import Player
import com.github.lastexceed.cubeworldnetworking.packets.*
import modules.*

suspend fun onCreatureUpdate(packet: CreatureUpdate, source: Player) {
	AntiCheat.inspect(packet, source.character)?.let {
		source.kick(it)
	}
	//Neverland.onCreatureUpdate(source, packet)

//	if (!Balancing.ensureLowLevelWeapon(packet, source)) {
//		return
//	}

//	source.layer.broadcast(
//		packet.copy(
//			id = CreatureId(packet.id.value + 600),
//			appearance = source.character.appearance,
//			affiliation = Affiliation.Player
//		)
//	)
//	source.layer.broadcast(
//		WorldUpdate(
//			statusEffects = listOf(
//				StatusEffect(
//					source = CreatureId(packet.id.value + 600),
//					target = CreatureId(packet.id.value + 600),
//					type = StatusEffect.Type.Camouflage,
//					modifier = 999999f,
//					duration = 9999999,
//					creatureId3 = CreatureId(packet.id.value + 600)
//				)
//			)
//		)
//	)

	val filtered = TrafficReduction.onCreatureUpdate(packet, source)
	source.character.update(packet)
	Balancing.onCreatureUpdate(packet, source)
	if (filtered == null) return
	source.layer.broadcast(Pvp.makeAttackable(filtered), source)
	//ModelImport.onCreatureUpdate(source, packet)
}