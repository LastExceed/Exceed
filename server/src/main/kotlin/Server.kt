package exceed

import exceed.packetHandlers.*
import io.ktor.network.selector.ActorSelectorManager
import io.ktor.network.sockets.*
import kotlinx.coroutines.*
import kotlinx.coroutines.CancellationException
import packets.*
import java.io.IOException
import java.net.InetSocketAddress
import java.net.SocketException

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
		val player = handShake(reader, writer)
		try {
			while (true) {
				val opcode = Opcode(reader.readInt())
				when (opcode) {
					Opcode.CreatureUpdate -> CreatureUpdateHandler.handlePacket(CreatureUpdate.readFrom(reader), player)
					Opcode.CreatureAction -> CreatureActionHandler.handlePacket(CreatureAction.readFrom(reader), player)
					Opcode.Hit -> HitHandler.handlePacket(Hit.readFrom(reader), player)
					Opcode.Buff -> BuffHandler.handlePacket(Buff.readFrom(reader), player)
					Opcode.Shot -> ShotHandler.handlePacket(Shot.readFrom(reader), player)
					Opcode.ChatMessage -> ChatMessageHandler.handlePacket(ChatMessage.readFrom(reader, false), player)
					Opcode.ChunkDiscovery -> ChunkDiscoveryHandler.handlePacket(ChunkDiscovery.readFrom(reader), player)
					Opcode.SectorDiscovery -> SectorDiscoveryHandler.handlePacket(SectorDiscovery.readFrom(reader), player)
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

	private suspend fun handShake(reader: Reader, writer: Writer): Player {
		var nextOpcode = Opcode(reader.readInt())
		if (nextOpcode != Opcode.ProtocolVersion) {
			TODO("expected ${Opcode.ProtocolVersion}")
		}
		val protocolVersion = ProtocolVersion.readFrom(reader)
		if (protocolVersion.version != 3) {
			TODO("wrong protocol version")
		}

		val newID = idPool.claim()

		val join = Join(0, newID, ByteArray(0x1168))
		writer.writeInt(join.opcode.value)
		join.writeTo(writer)

		nextOpcode = Opcode(reader.readInt())
		if (nextOpcode != Opcode.CreatureUpdate) {
			TODO("expected ${Opcode.CreatureUpdate}")
		}
		val creatureUpdate = CreatureUpdate.readFrom(reader)
		//TODO: make sure bitfield is full
		//TODO: inspect with anticheat
		return Player.create(writer, Creature(creatureUpdate), mainLayer)
	}
}