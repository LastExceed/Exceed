package packetHandlers

import Player
import com.github.lastexceed.cubeworldnetworking.packets.*

object ProjectileHandler : PacketHandler<Projectile> {
	override suspend fun handlePacket(packet: Projectile, source: Player) {
		source.layer.broadcast(
			Miscellaneous(projectiles = listOf(packet)),
			source
		)
	}
}
//unknownA = 0
//paddingA = -1
//unknownV = 0 0 0
//skill = 0x00ffffee
//paddingB = -256
//unknownC = 0.0
//unknownD = 0.0