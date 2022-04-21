import com.andreapivetta.kolor.*
import com.github.lastexceed.cubeworldnetworking.packets.*
import com.github.lastexceed.cubeworldnetworking.utils.*
import io.ktor.network.selector.ActorSelectorManager
import io.ktor.network.sockets.*
import io.ktor.util.network.hostname
import kotlinx.coroutines.*
import kotlinx.coroutines.channels.ClosedReceiveChannelException
import modules.*
import packetHandlers.*
import java.io.File
import java.net.SocketException
import java.time.*

object Server {
	private val listener = aSocket(ActorSelectorManager(Dispatchers.IO)).tcp().bind(InetSocketAddress("0.0.0.0", 12345))
	private val mainLayer = Layer()

	suspend fun start() {
		CreatureIdPool.claim() //claim 0 because its reserved for server messages
		supervisorScope {
			launch {//todo: layer scope
				while (true) {
					mainLayer.broadcast(GameDateTime(0, Duration.ofHours(12).toMillis().toInt()))
					delay(5000)
				}
			}
			println(Kolor.foreground("start listening", Color.DARK_GRAY))
			while (true) {
				val socket = listener.accept()
				launch {
					try {
						handleNewConnection(socket)
					} catch (exception: Exception) {
						if (exception !is ClosedReceiveChannelException) {//when cubeworld-servers.com checks the online status
							val logFile = File("crashlog_${LocalDateTime.now().toString().replace(":", "-")}.log").apply {
								createNewFile()
								appendText(exception.stackTraceToString())
							}
							println(Kolor.foreground(logFile.name, Color.RED))
						}
					}
					socket.dispose()
				}
			}
		}
	}

	private suspend fun handleNewConnection(socket: Socket) {
		println(Kolor.foreground("new connection from " + socket.remoteAddress.toJavaAddress().hostname, Color.DARK_GRAY))
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
		handleNewPlayer(socket, reader, writer, assignedId)
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
//		MapSeed(225).run {
//			packetId.writeTo(writer)
//			writeTo(writer)
//		}
		val player = Player.create(socket, writer, character, mainLayer)

		player.layer.announce("[+] ${player.character.name}")
		ModelImport.onJoin(player)
		while (true) {
			try {
				val packet = try {
					getNextPacket(reader)
				} catch (_: SocketException) { break } //regular disconnect
				handlePacket(packet, player)
			} catch (exception: Throwable) {
				val logFile = File("connection_error_${LocalDateTime.now().toString().replace(":", "-")}.log").apply {
					createNewFile()
					appendText(exception.stackTraceToString())
					appendText(player.character.toString())
				}
				try {
					player.kick(logFile.name)
				} catch (_: Throwable) {}
				println(Kolor.foreground(logFile.name, Color.RED))
				break
			}
		}
		player.run {
			layer.removePlayer(player)
			layer.announce("[-] ${player.character.name}")
			scope.cancel()
		}
	}
}

private suspend fun getNextPacket(reader: Reader) =
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

private suspend fun handlePacket(packet: Packet, source: Player) =
	when (packet) {
		is CreatureUpdate -> onCreatureUpdate(packet, source)
		is CreatureAction -> onCreatureAction(packet, source)
		is Hit -> onHit(packet, source)
		is StatusEffect -> onStatusEffect(packet, source)
		is Projectile -> onProjectile(packet, source)
		is ChatMessage.FromClient -> onChatMessage(packet, source)
		is ResidenceChunk -> onResidenceChunk(packet, source)
		is ResidenceBiome -> onResidenceBiome(packet, source)
		else -> error("unexpected packet type ${packet::class}")
	}