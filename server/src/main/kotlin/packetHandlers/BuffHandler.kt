package exceed.packetHandlers

import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.delay
import kotlinx.coroutines.launch
import packets.*
import utils.Vector3
import exceed.Player
import kotlinx.coroutines.Dispatchers

object BuffHandler : PacketHandler<Buff> {
	override suspend fun handlePacket(packet: Buff, source: Player) {
		if (packet.type == BuffType.Poison) {
			applyPoison(source, packet)
		}

		val su = ServerUpdate()
		su.buffs.add(packet)
		source.layer.broadcast(su, source)
	}

	private fun applyPoison(source: Player, buff: Buff) {
		val targetPlayer = source.layer.players[buff.target]!!
		GlobalScope.launch(Dispatchers.IO) {
			fun createPoisonTick(): ServerUpdate {
				val poisonTick = Hit(
					source.character.id,
					buff.target,
					buff.modifier,
					false,
					0,
					0,
					source.layer.creatures[buff.target]!!.position,
					Vector3(0f, 0f, 0f),
					false,
					DamageType.Unknown,
					false,
					0
				)
				val su = ServerUpdate()
				su.hits.add(poisonTick)
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