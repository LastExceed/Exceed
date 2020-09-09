package exceed

import exceed.packetHandlers.*
import io.ktor.network.selector.ActorSelectorManager
import io.ktor.network.sockets.*
import io.ktor.network.sockets.Socket
import kotlinx.coroutines.*
import kotlinx.coroutines.CancellationException
import packets.*
import java.io.IOException
import java.net.*

object Server {
	private val listener = aSocket(ActorSelectorManager(Dispatchers.IO)).tcp().bind(InetSocketAddress(12345))
	private val mainLayer = Layer()

	suspend fun start() {
		CreatureIdPool.claim() //claim 0 because its reserved for server messages
		println("start listening")
		supervisorScope {
			while (true) {
				val socket = listener.accept()
				launch {
					handleNewConnection(socket)
				}
			}
		}
	}

	private suspend fun handleNewConnection(socket: Socket) {
		println("new connection " + socket.remoteAddress)
		val reader = Reader(socket.openReadChannel())
		val writer = Writer(socket.openWriteChannel(true))

		if (Opcode(reader.readInt()) != Opcode.ProtocolVersion) {
			socket.dispose()
			return
		}

		val protocolVersion = ProtocolVersion.readFrom(reader)
		if (protocolVersion.version != 3) {
			ProtocolVersion(3).writeTo(writer)
			socket.dispose()
			return
		}

		val join = Join(0, CreatureIdPool.claim(), ByteArray(0x1168))
		writer.writeInt(join.opcode.value)
		join.writeTo(writer)

		if (Opcode(reader.readInt()) != Opcode.CreatureUpdate) {
			socket.dispose()
			CreatureIdPool.free(join.assignedID)
			return
		}

		val creatureUpdate = CreatureUpdate.readFrom(reader)//TODO: inspect with anticheat

		val character = try {
			Creature(creatureUpdate)
		} catch (ex: KotlinNullPointerException) { //TODO: use null-checks instead of try/catch
			socket.dispose()
			CreatureIdPool.free(join.assignedID)
			return
		}

		val player = Player.create(writer, character, mainLayer)
		player.send(MapSeed(3302))
		try {
			while (true) {
				val opcode = Opcode(reader.readInt())
				when (opcode) {
					Opcode.CreatureUpdate -> CreatureUpdateHandler.handlePacket(CreatureUpdate.readFrom(reader), player)
					Opcode.CreatureAction -> PlayerActionHandler.handlePacket(CreatureAction.readFrom(reader), player)
					Opcode.Hit -> HitHandler.handlePacket(Hit.readFrom(reader), player)
					Opcode.Buff -> BuffHandler.handlePacket(Buff.readFrom(reader), player)
					Opcode.Shot -> ShotHandler.handlePacket(Shot.readFrom(reader), player)
					Opcode.ChatMessage -> ChatMessageHandler.handlePacket(ChatMessage.readFrom(reader, false), player)
					Opcode.ChunkDiscovery -> ResidenceChunkHandler.handlePacket(ResidenceChunk.readFrom(reader), player)
					Opcode.SectorDiscovery -> ResidenceBiomeHandler.handlePacket(ResidenceBiome.readFrom(reader), player)
					else -> println("unknown opcode: $opcode")
				}
			}
		} catch (cancellationException: SocketException) {
			println("player disconnected")
			player.layer.removePlayer(player)
			CreatureIdPool.free(player.character.id)
			socket.dispose()
			return
		}
	}
}