package exceed

import exceed.packetHandlers.*
import io.ktor.network.selector.ActorSelectorManager
import io.ktor.network.sockets.*
import kotlinx.coroutines.*
import kotlinx.coroutines.CancellationException
import packets.*
import java.io.IOException
import java.net.InetSocketAddress

object Server {
	private val listener = aSocket(ActorSelectorManager(Dispatchers.IO)).tcp().bind(InetSocketAddress(12345))
	private val mainLayer = Layer()
	private val idPool = CreatureIdPool()

	suspend fun start() {
		idPool.claim() //claim 0 because its reserved for server messages
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
		println("new connection")
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

		val join = Join(0, idPool.claim(), ByteArray(0x1168))
		writer.writeInt(join.opcode.value)
		join.writeTo(writer)

		if (Opcode(reader.readInt()) != Opcode.CreatureUpdate) {
			socket.dispose()
			idPool.free(join.assignedID)
			return
		}

		val creatureUpdate = CreatureUpdate.readFrom(reader)//TODO: inspect with anticheat

		val character = try {
			Creature(creatureUpdate)
		} catch (ex: KotlinNullPointerException) { //TODO: use null-checks instead of try/catch
			socket.dispose()
			idPool.free(join.assignedID)
			return
		}

		val player = Player.create(writer, character, mainLayer)
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
		} catch (cancellationException: CancellationException) {
			if (cancellationException.cause !is IOException) {
				throw cancellationException
			}
			println("player disconnected")
			player.layer.removePlayer(player)
			idPool.free(player.character.id)
			socket.dispose()
			return
		}
	}
}