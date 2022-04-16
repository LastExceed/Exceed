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

		//seed must be sent BEFORE the player is added to the layer
		//else the seed change will unload all cratures on their end
		MapSeed(225).run {
			packetId.writeTo(writer)
			writeTo(writer)
		}
		val player = Player.create(socket, writer, character, mainLayer)
		try {
			player.layer.announce("[+] ${player.character.name}")
			while (true) {
				when (val packet = getNextPacket(reader)) {
					is CreatureUpdate -> CreatureUpdateHandler.handlePacket(packet, player)
					is CreatureAction -> CreatureActionHandler.handlePacket(packet, player)
					is Hit -> HitHandler.handlePacket(packet, player)
					is StatusEffect -> StatusEffectHandler.handlePacket(packet, player)
					is Projectile -> ProjectileHandler.handlePacket(packet, player)
					is ChatMessage.FromClient -> ChatMessageHandler.handlePacket(packet, player)
					is ResidenceChunk -> ResidenceChunkHandler.handlePacket(packet, player)
					is ResidenceBiome -> ResidenceBiomeHandler.handlePacket(packet, player)
					else -> error("unexpected packet type ${packet::class}")
				}
			}
		} catch (exception: Throwable) {
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
			player.scope.cancel()
			player.layer.announce("[-] ${player.character.name}")
		}
	}
}

suspend fun getNextPacket(reader: Reader) =
	when (PacketId.readFrom(reader)) {
		PacketId.CreatureUpdate -> CreatureUpdate
		PacketId.CreatureAction -> CreatureAction
		PacketId.Hit -> Hit
		PacketId.StatusEffect -> StatusEffect
		PacketId.Projectile -> Projectile
		PacketId.ChatMessage -> ChatMessage.FromClient
		PacketId.ResidenceBiome -> ResidenceChunk
		PacketId.ResidenceChunk -> ResidenceBiome
		else -> throw IllegalStateException("unexpected packet id")
	}.readFrom(reader)