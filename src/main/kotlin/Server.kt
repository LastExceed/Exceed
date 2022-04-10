import io.ktor.network.selector.ActorSelectorManager
import io.ktor.network.sockets.*
import kotlinx.coroutines.*
import com.github.lastexceed.cubeworldnetworking.packets.*
import com.github.lastexceed.cubeworldnetworking.utils.*
import kotlinx.coroutines.channels.ClosedReceiveChannelException
import modules.AntiCheat
import packetHandlers.*
import java.io.File
import java.net.SocketException
import java.time.LocalTime

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
					} catch (ex: Exception) {
						//usually when cubeworld-servers.com checks the online status
						//todo: logging
					}
					socket.dispose()
				}
			}
		}
	}

	private suspend fun handleNewConnection(socket: Socket) {
		println("new connection " + socket.remoteAddress)
		val reader = Reader(socket.openReadChannel())
		val writer = ByteWriteChannelAdapter(socket.openWriteChannel(true))

		if (PacketId.readFrom(reader) != PacketId.ProtocolVersion) {
			return
		}

		val protocolVersion = ProtocolVersion.readFrom(reader)
		if (protocolVersion.version != 3) {
			ProtocolVersion(3).writeTo(writer)
			return
		}

		val assignedId = CreatureIdPool.claim()
		try {
			handleNewPlayer(socket, reader, writer, assignedId)
		} catch (ex: Exception) {
			//todo: logging
		}
		CreatureIdPool.free(assignedId)
	}

	private suspend fun handleNewPlayer(socket: Socket, reader: Reader, writer: Writer, assignedId: CreatureId) {
		PlayerInitialization(assignedId = assignedId).run {
			packetId.writeTo(writer)
			writeTo(writer)
		}

		if (PacketId.readFrom(reader) != PacketId.CreatureUpdate) {
			return
		}

		val creatureUpdate = CreatureUpdate.readFrom(reader)

		val character = try {
			Creature(creatureUpdate)
		} catch (ex: KotlinNullPointerException) { //TODO: use null-checks instead of try/catch
			return
		}

		AntiCheat.inspect(creatureUpdate, character)?.let {
			PacketId.ChatMessage.writeTo(writer)
			ChatMessage.FromServer(CreatureId(0), it).writeTo(writer)
			delay(100)
			return
		}

		val player = Player.create(socket, writer, character, mainLayer)
		try {
			player.layer.announce("[+] ${player.character.name}")
			player.send(MapSeed(225))
			while (true) {
				when (PacketId.readFrom(reader)) {
					PacketId.CreatureUpdate -> CreatureUpdateHandler.handlePacket(CreatureUpdate.readFrom(reader), player)
					PacketId.CreatureAction -> CreatureActionHandler.handlePacket(CreatureAction.readFrom(reader), player)
					PacketId.Hit -> HitHandler.handlePacket(Hit.readFrom(reader), player)
					PacketId.StatusEffect -> StatusEffectHandler.handlePacket(StatusEffect.readFrom(reader), player)
					PacketId.Projectile -> ProjectileHandler.handlePacket(Projectile.readFrom(reader), player)
					PacketId.ChatMessage -> ChatMessageHandler.handlePacket(ChatMessage.FromClient.readFrom(reader), player)
					PacketId.ChunkDiscovery -> ResidenceChunkHandler.handlePacket(ResidenceChunk.readFrom(reader), player)
					PacketId.SectorDiscovery -> ResidenceBiomeHandler.handlePacket(ResidenceBiome.readFrom(reader), player)
					else -> throw IllegalStateException("unexpected packet id")
				}
			}
		} catch (exception: Exception) {
			//todo: do this properly
			when (exception) {
				is SocketException,
				is CancellationException -> {}
				is IllegalStateException -> player.kick("invalid data received")
				else -> {
					val timestamp = LocalTime.now()
					println("!!! CRASH !!! $timestamp")
					File("crash-${timestamp.toString().replace(":", "_")}.log").run {
						createNewFile()
						appendText(exception.stackTraceToString())
						appendText(player.character.toString())
					}
				}
			}
			player.layer.removePlayer(player)
			player.layer.announce("[-] ${player.character.name}")
		}
	}
}