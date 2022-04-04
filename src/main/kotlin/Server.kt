import io.ktor.network.selector.ActorSelectorManager
import io.ktor.network.sockets.*
import kotlinx.coroutines.*
import com.github.lastexceed.cubeworldnetworking.packets.*
import com.github.lastexceed.cubeworldnetworking.utils.*
import modules.AntiCheat
import packetHandlers.*
import java.net.SocketException

object Server {
	private val listener = aSocket(ActorSelectorManager(Dispatchers.IO)).tcp().bind(InetSocketAddress("0.0.0.0", 12345))
	private val mainLayer = Layer()

	suspend fun start() {
		CreatureIdPool.claim() //claim 0 because its reserved for server messages
		println("start listening")
		supervisorScope {
			while (true) {
				val socket = listener.accept()
				launch {
					try {
						handleNewConnection(socket)
					} catch (exception: Exception) {
						exception.printStackTrace()
						println(exception)
					}

				}
			}
		}
	}

	private suspend fun handleNewConnection(socket: Socket) {
		println("new connection " + socket.remoteAddress)
		val reader = Reader(socket.openReadChannel())
		val writer = Writer(socket.openWriteChannel(true))

		if (PacketId.readFrom(reader) != PacketId.ProtocolVersion) {
			socket.dispose()
			return
		}

		val protocolVersion = ProtocolVersion.readFrom(reader)
		if (protocolVersion.version != 3) {
			ProtocolVersion(3).writeTo(writer)
			socket.dispose()
			return
		}

		val assignedId = CreatureIdPool.claim()

		PlayerInitialization(assignedId = assignedId).run {
			packetId.writeTo(writer)
			writeTo(writer)
		}

		if (PacketId.readFrom(reader) != PacketId.CreatureUpdate) {
			socket.dispose()
			CreatureIdPool.free(assignedId)
			return
		}

		val creatureUpdate = CreatureUpdate.readFrom(reader)

		val character = try {
			Creature(creatureUpdate)
		} catch (ex: KotlinNullPointerException) { //TODO: use null-checks instead of try/catch
			socket.dispose()
			CreatureIdPool.free(assignedId)
			return
		}

		MapSeed(225).run {
			packetId.writeTo(writer)
			writeTo(writer)
		}

		//TODO: deduplicate (CreatureUpdateHandler)
		AntiCheat.inspect(creatureUpdate, character)?.let {
			PacketId.ChatMessage.writeTo(writer)
			ChatMessage.FromServer(CreatureId(0), it).writeTo(writer)
			//delay(100)
			//socket.dispose()
			//return
		}

		val player = Player.create(socket, writer, character, mainLayer)
		player.send(MapSeed(225))
		try {
			while (true) {
				when (val packetId = PacketId.readFrom(reader)) {
					PacketId.CreatureUpdate -> CreatureUpdateHandler.handlePacket(CreatureUpdate.readFrom(reader), player)
					PacketId.CreatureAction -> CreatureActionHandler.handlePacket(CreatureAction.readFrom(reader), player)
					PacketId.Hit -> HitHandler.handlePacket(Hit.readFrom(reader), player)
					PacketId.StatusEffect -> StatusEffectHandler.handlePacket(StatusEffect.readFrom(reader), player)
					PacketId.Projectile -> ProjectileHandler.handlePacket(Projectile.readFrom(reader), player)
					PacketId.ChatMessage -> ChatMessageHandler.handlePacket(ChatMessage.FromClient.readFrom(reader), player)
					PacketId.ChunkDiscovery -> ResidenceChunkHandler.handlePacket(ResidenceChunk.readFrom(reader), player)
					PacketId.SectorDiscovery -> ResidenceBiomeHandler.handlePacket(ResidenceBiome.readFrom(reader), player)
					else -> println("unknown opcode: $packetId")
				}
			}
		} catch (exception: Exception) {
			when (exception) {
				is SocketException,
				is CancellationException -> {
					println("player disconnected")
					player.layer.removePlayer(player)
					CreatureIdPool.free(player.character.id)
					socket.dispose()
					return
				}
				else -> throw exception
			}

		}
	}

	suspend fun notify(session: Player, message: String) {
		val chatMessage = ChatMessage.FromServer(CreatureId(0), message)
		session.send(chatMessage)
	}
}