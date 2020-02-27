package packets

import io.ktor.util.toByteArray
import io.ktor.utils.io.ByteChannel
import io.ktor.utils.io.close
import utils.*

data class ServerUpdate(
	val worldEdits: MutableList<WorldEdit> = mutableListOf(),
	val hits: MutableList<Hit> = mutableListOf(),
	val particles: MutableList<Particle> = mutableListOf(),
	val sounds: MutableList<Sound> = mutableListOf(),
	val shots: MutableList<Shot> = mutableListOf(),
	val worldObjects: MutableList<WorldObject> = mutableListOf(),
	val chunkLoots: MutableList<ChunkLoot> = mutableListOf(),
	val p48s: MutableList<P48> = mutableListOf(),
	val pickups: MutableList<Pickup> = mutableListOf(),
	val kills: MutableList<Kill> = mutableListOf(),
	val attacks: MutableList<Attack> = mutableListOf(),
	val buffs: MutableList<Buff> = mutableListOf(),
	val missions: MutableList<Mission> = mutableListOf()
) : Packet(Opcode.ServerUpdate) {
	override suspend fun writeTo(writer: Writer) {
		val inflatedChannel = ByteChannel(true)
		val inflatedWriter = Writer(inflatedChannel)

		listOf(
			worldEdits,
			hits,
			particles,
			sounds,
			shots,
			worldObjects,
			chunkLoots,
			p48s,
			pickups,
			kills,
			attacks,
			buffs,
			missions
		).forEach {
			inflatedWriter.writeInt(it.size)
			it.forEach { subPacket ->
				subPacket.writeTo(inflatedWriter)
			}
		}
		inflatedChannel.close()
		val deflated = Zlib.deflate(inflatedChannel.toByteArray())
		writer.writeInt(deflated.size)
		writer.writeByteArray(deflated)
	}

	companion object {
		suspend fun readFrom(reader: Reader): ServerUpdate {
			val serverUpdate = ServerUpdate()

			val length = reader.readInt()
			val compressed = reader.readByteArray(length)
			val uncompressed = Zlib.inflate(compressed)
			val uncompressedReader = Reader(uncompressed)

			repeat(uncompressedReader.readInt()) {
				serverUpdate.worldEdits.add(WorldEdit.readFrom(uncompressedReader))
			}
			repeat(uncompressedReader.readInt()) {
				serverUpdate.hits.add(Hit.readFrom(uncompressedReader))
			}
			repeat(uncompressedReader.readInt()) {
				serverUpdate.particles.add(Particle.readFrom(uncompressedReader))
			}
			repeat(uncompressedReader.readInt()) {
				serverUpdate.sounds.add(Sound.readFrom(uncompressedReader))
			}
			repeat(uncompressedReader.readInt()) {
				serverUpdate.shots.add(Shot.readFrom(uncompressedReader))
			}
			repeat(uncompressedReader.readInt()) {
				serverUpdate.worldObjects.add(WorldObject.readFrom(uncompressedReader))
			}
			repeat(uncompressedReader.readInt()) {
				serverUpdate.chunkLoots.add(ChunkLoot.readFrom(uncompressedReader))
			}
			repeat(uncompressedReader.readInt()) {
				serverUpdate.p48s.add(P48.readFrom(uncompressedReader))
			}
			repeat(uncompressedReader.readInt()) {
				serverUpdate.pickups.add(Pickup.readFrom(uncompressedReader))
			}
			repeat(uncompressedReader.readInt()) {
				serverUpdate.kills.add(Kill.readFrom(uncompressedReader))
			}
			repeat(uncompressedReader.readInt()) {
				serverUpdate.attacks.add(Attack.readFrom(uncompressedReader))
			}
			repeat(uncompressedReader.readInt()) {
				serverUpdate.buffs.add(Buff.readFrom(uncompressedReader))
			}
			repeat(uncompressedReader.readInt()) {
				serverUpdate.missions.add(Mission.readFrom(uncompressedReader))
			}
			return serverUpdate
		}
	}
}

data class WorldEdit(
	val position: Vector3<Int>,
	val color: Vector3<Byte>,
	val blockType: BlockType,
	val unknown: Int
) : SubPacket {
	override suspend fun writeTo(writer: Writer) {
		writer.writeVector3Int(position)
		writer.writeVector3Byte(color)
		writer.writeByte(blockType.value)
		writer.writeInt(unknown)
	}

	companion object {
		internal suspend fun readFrom(reader: Reader): WorldEdit {
			return WorldEdit(
				position = reader.readVector3Int(),
				color = reader.readVector3Byte(),
				blockType = BlockType(reader.readByte()),
				unknown = reader.readInt()
			)
		}
	}
}

inline class BlockType(val value: Byte) {
	companion object {
		val Air = BlockType(0)
		val Solid = BlockType(1)
		val Liquid = BlockType(2)
		val Wet = BlockType(3)
	}
}

data class Particle(
	val position: Vector3<Long>,
	val velocity: Vector3<Float>,
	val color: Vector3<Float>,
	val alpha: Float,
	val size: Float,
	val count: Int,
	val particleType: ParticleType,
	val spread: Float,
	val unknown: Int
) : SubPacket {
	override suspend fun writeTo(writer: Writer) {
		writer.writeVector3Long(position)
		writer.writeVector3Float(velocity)
		writer.writeVector3Float(color)
		writer.writeFloat(alpha)
		writer.writeFloat(size)
		writer.writeInt(count)
		writer.writeInt(particleType.value)
		writer.writeFloat(spread)
		writer.writeInt(unknown)
	}

	companion object {
		internal suspend fun readFrom(reader: Reader): Particle {
			return Particle(
				position = reader.readVector3Long(),
				velocity = reader.readVector3Float(),
				color = reader.readVector3Float(),
				alpha = reader.readFloat(),
				size = reader.readFloat(),
				count = reader.readInt(),
				particleType = ParticleType(reader.readInt()),
				spread = reader.readFloat(),
				unknown = reader.readInt()
			)
		}
	}
}

inline class ParticleType(val value: Int) {
	companion object {
		val Normal = ParticleType(0)
		val Spark = ParticleType(1)
		val Unknown = ParticleType(2)
		val NoSpreadNoRotation = ParticleType(3)
		val NoGravity = ParticleType(4)
	}
}

data class Sound(
	val position: Vector3<Float>,
	val soundType: SoundType,
	val pitch: Float = 1f,
	val volume: Float = 1f
) : SubPacket {
	override suspend fun writeTo(writer: Writer) {
		writer.writeVector3Float(position)
		writer.writeInt(soundType.value)
		writer.writeFloat(pitch)
		writer.writeFloat(volume)
	}

	companion object {
		internal suspend fun readFrom(reader: Reader): Sound {
			return Sound(
				position = reader.readVector3Float(),
				soundType = SoundType(reader.readInt()),
				pitch = reader.readFloat(),
				volume = reader.readFloat()
			)
		}
	}
}

inline class SoundType(val value: Int) {
	companion object {
		val Hit = SoundType(0)
		val Blade1 = SoundType(1)
		val Blade2 = SoundType(2)
		val LongBlade1 = SoundType(3)
		val LongBlade2 = SoundType(4)
		val Hit1 = SoundType(5)
		val Hit2 = SoundType(6)
		val Punch1 = SoundType(7)
		val Punch2 = SoundType(8)
		val HitArrow = SoundType(9)
		val HitArrowCritical = SoundType(10)
		val Smash1 = SoundType(11)
		val SlamGround = SoundType(12)
		val SmashHit2 = SoundType(13)
		val SmashJump = SoundType(14)
		val Swing = SoundType(15)
		val ShieldSwing = SoundType(16)
		val SwingSlow = SoundType(17)
		val SwingSlow2 = SoundType(18)
		val ArrowDestroy = SoundType(19)
		val Blade3 = SoundType(20)
		val Punch3 = SoundType(21)
		val Salvo2 = SoundType(22)
		val SwordHit03 = SoundType(23)
		val Block = SoundType(24)
		val ShieldSlam = SoundType(25)
		val Roll = SoundType(26)
		val Destroy2 = SoundType(27)
		val Cry = SoundType(28)
		val Levelup2 = SoundType(29)
		val Missioncomplete = SoundType(30)
		val Watersplash01 = SoundType(31)
		val Step2 = SoundType(32)
		val StepWater = SoundType(33)
		val StepWater2 = SoundType(34)
		val StepWater3 = SoundType(35)
		val Channel2 = SoundType(36)
		val ChannelHit = SoundType(37)
		val Fireball = SoundType(38)
		val FireHit = SoundType(39)
		val Magic01 = SoundType(40)
		val Watersplash = SoundType(41)
		val WatersplashHit = SoundType(42)
		val LichScream = SoundType(43)
		val Drink2 = SoundType(44)
		val Pickup = SoundType(45)
		val Disenchant2 = SoundType(46)
		val Upgrade2 = SoundType(47)
		val Swirl = SoundType(48)
		val HumanVoice01 = SoundType(49)
		val HumanVoice02 = SoundType(50)
		val Gate = SoundType(51)
		val SpikeTrap = SoundType(52)
		val FireTrap = SoundType(53)
		val Lever = SoundType(54)
		val Charge2 = SoundType(55)
		val Magic02 = SoundType(56)
		val Drop = SoundType(57)
		val DropCoin = SoundType(58)
		val DropItem = SoundType(59)
		val MaleGroan = SoundType(60)
		val FemaleGroan = SoundType(61)
		val MaleGroan2 = SoundType(62)
		val FemaleGroan2 = SoundType(63)
		val GoblinMaleGroan = SoundType(64)
		val GoblinFemaleGroan = SoundType(65)
		val LizardMaleGroan = SoundType(66)
		val LizardFemaleGroan = SoundType(67)
		val DwarfMaleGroan = SoundType(68)
		val DwarfFemaleGroan = SoundType(69)
		val OrcMaleGroan = SoundType(70)
		val OrcFemaleGroan = SoundType(71)
		val UndeadMaleGroan = SoundType(72)
		val UndeadFemaleGroan = SoundType(73)
		val FrogmanMaleGroan = SoundType(74)
		val FrogmanFemaleGroan = SoundType(75)
		val MonsterGroan = SoundType(76)
		val TrollGroan = SoundType(77)
		val MoleGroan = SoundType(78)
		val SlimeGroan = SoundType(79)
		val ZombieGroan = SoundType(80)
		val Explosion = SoundType(81)
		val Punch4 = SoundType(82)
		val MenuOpen2 = SoundType(83)
		val MenuClose2 = SoundType(84)
		val MenuSelect = SoundType(85)
		val MenuTab = SoundType(86)
		val MenuGrabItem = SoundType(87)
		val MenuDropItem = SoundType(88)
		val Craft = SoundType(89)
		val CraftProc = SoundType(90)
		val Absorb = SoundType(91)
		val Manashield = SoundType(92)
		val Bulwark = SoundType(93)
		val Bird1 = SoundType(94)
		val Bird2 = SoundType(95)
		val Bird3 = SoundType(96)
		val Cricket1 = SoundType(97)
		val Cricket2 = SoundType(98)
		val Owl1 = SoundType(99)
		val Owl2 = SoundType(100)
	}
}

data class WorldObject(
	val chunk: Vector2<Int>,
	val objectID: Int,
	val paddingA: Int,
	val objectType: ObjectType,
	val paddingB: Int,
	val position: Vector3<Long>,
	val orientation: Orientation,
	val size: Vector3<Float>,
	val isClosed: Boolean,
	val transformTime: Int,
	val unknown: Int,
	val paddingC: Int,
	val interactor: Long
) : SubPacket {
	override suspend fun writeTo(writer: Writer) {
		writer.writeVector2Int(chunk)
		writer.writeInt(objectID)
		writer.writeInt(paddingA)
		writer.writeInt(objectType.value)
		writer.writeInt(paddingB)
		writer.writeVector3Long(position)
		writer.writeInt(orientation.value)
		writer.writeVector3Float(size)
		writer.writeBoolean(isClosed); writer.pad(3)
		writer.writeInt(transformTime)
		writer.writeInt(unknown)
		writer.writeInt(paddingC)
		writer.writeLong(interactor)
	}

	companion object {
		internal suspend fun readFrom(reader: Reader): WorldObject {
			return WorldObject(
				chunk = reader.readVector2Int(),
				objectID = reader.readInt(),
				paddingA = reader.readInt(),
				objectType = ObjectType(reader.readInt()),
				paddingB = reader.readInt(),
				position = reader.readVector3Long(),
				orientation = Orientation(reader.readInt()),
				size = reader.readVector3Float(),
				isClosed = reader.readInt() > 0,
				transformTime = reader.readInt(),
				unknown = reader.readInt(),
				paddingC = reader.readInt(),
				interactor = reader.readLong()
			)
		}
	}
}

inline class ObjectType(val value: Int) {
	companion object {
		val Statue = ObjectType(0)
		val Door = ObjectType(1)
		val BigDoor = ObjectType(2)
		val Window = ObjectType(3)
		val CastleWindow = ObjectType(4)
		val Gate = ObjectType(5)
		val FireTrap = ObjectType(6)
		val SpikeTrap = ObjectType(7)
		val StompTrap = ObjectType(8)
		val Lever = ObjectType(9)
		val Chest = ObjectType(10)
		val ChestTop02 = ObjectType(11)
		val Table1 = ObjectType(12)
		val Table2 = ObjectType(13)
		val Table3 = ObjectType(14)
		val Stool1 = ObjectType(15)
		val Stool2 = ObjectType(16)
		val Stool3 = ObjectType(17)
		val Bench = ObjectType(18)
		val Bed = ObjectType(19)
		val BedTable = ObjectType(20)
		val MarketStand1 = ObjectType(21)
		val MarketStand2 = ObjectType(22)
		val MarketStand3 = ObjectType(23)
		val Barrel = ObjectType(24)
		val Crate = ObjectType(25)
		val OpenCrate = ObjectType(26)
		val Sack = ObjectType(27)
		val Shelter = ObjectType(28)
		val Cupboard = ObjectType(29)
		val Desktop = ObjectType(30)
		val Counter = ObjectType(31)
		val Shelf1 = ObjectType(32)
		val Shelf2 = ObjectType(33)
		val Shelf3 = ObjectType(34)
		val CastleShelf1 = ObjectType(35)
		val CastleShelf2 = ObjectType(36)
		val CastleShelf3 = ObjectType(37)
		val StoneShelf1 = ObjectType(38)
		val StoneShelf2 = ObjectType(39)
		val StoneShelf3 = ObjectType(40)
		val SandstoneShelf1 = ObjectType(41)
		val SandstoneShelf2 = ObjectType(42)
		val SandstoneShelf3 = ObjectType(43)
		val Corpse = ObjectType(44)
		val RuneStone = ObjectType(45)
		val Artifact = ObjectType(46)
		val FlowerBox1 = ObjectType(47)
		val FlowerBox2 = ObjectType(48)
		val FlowerBox3 = ObjectType(49)
		val StreetLight = ObjectType(50)
		val FireStreetLight = ObjectType(51)
		val Fence1 = ObjectType(52)
		val Fence2 = ObjectType(53)
		val Fence3 = ObjectType(54)
		val Fence4 = ObjectType(55)
		val Vase1 = ObjectType(56)
		val Vase2 = ObjectType(57)
		val Vase3 = ObjectType(58)
		val Vase4 = ObjectType(59)
		val Vase5 = ObjectType(60)
		val Vase6 = ObjectType(61)
		val Vase7 = ObjectType(62)
		val Vase8 = ObjectType(63)
		val Vase9 = ObjectType(64)
		val Campfire = ObjectType(65)
		val Tent = ObjectType(66)
		val BeachUmbrella = ObjectType(67)
		val BeachTowel = ObjectType(68)
		val SleepingMat = ObjectType(69)
		val Furnace = ObjectType(70)
		val Anvil = ObjectType(71)
		val SpinningWheel = ObjectType(72)
		val Loom = ObjectType(73)
		val SawBench = ObjectType(74)
		val Workbench = ObjectType(75)
		val CustomizationBench = ObjectType(76)
	}
}

inline class Orientation(val value: Int) {
	companion object {
		val South = Orientation(0)
		val East = Orientation(1)
		val North = Orientation(2)
		val West = Orientation(3)
	}
}

data class ChunkLoot(
	val chunk: Vector2<Int>,
	val drops: MutableList<Drop> = mutableListOf()
) : SubPacket {
	override suspend fun writeTo(writer: Writer) {
		writer.writeVector2Int(chunk)
		drops.forEach {
			it.writeTo(writer)
		}
	}

	companion object {
		internal suspend fun readFrom(reader: Reader): ChunkLoot {
			val chunk = reader.readVector2Int()
			val drops = mutableListOf<Drop>()
			repeat(reader.readInt()) {
				drops.add(Drop.readFrom(reader))
			}
			return ChunkLoot(chunk, drops)
		}
	}
}

data class Drop(
	val item: Item,
	val position: Vector3<Long>,
	val rotation: Float,
	val scale: Float,
	val unknownA: Int,
	val droptime: Int,
	val unknownB: Int,
	val unknownC: Int
) : SubPacket {
	override suspend fun writeTo(writer: Writer) {
		item.writeTo(writer)
		writer.writeVector3Long(position)
		writer.writeFloat(rotation)
		writer.writeFloat(scale)
		writer.writeInt(unknownA)
		writer.writeInt(droptime)
		writer.writeInt(unknownB)
		writer.writeInt(unknownC)
	}

	companion object {
		internal suspend fun readFrom(reader: Reader): Drop {
			return Drop(
				item = Item.readFrom(reader),
				position = reader.readVector3Long(),
				rotation = reader.readFloat(),
				scale = reader.readFloat(),
				unknownA = reader.readInt(),
				droptime = reader.readInt(),
				unknownB = reader.readInt(),
				unknownC = reader.readInt()
			)
		}
	}
}

data class P48(
	val chunk: Vector2<Int>,
	val subPackets: MutableList<ByteArray> = mutableListOf()
) : SubPacket {
	override suspend fun writeTo(writer: Writer) {
		writer.writeVector2Int(chunk)
		writer.writeInt(subPackets.size)
		subPackets.forEach {
			writer.writeByteArray(it)
		}
	}

	companion object {
		internal suspend fun readFrom(reader: Reader): P48 {
			val chunk = reader.readVector2Int()
			val subPackets = mutableListOf<ByteArray>()
			repeat(reader.readInt()) {
				subPackets.add(reader.readByteArray(16))
			}
			return P48(
				chunk = chunk,
				subPackets = subPackets
			)
		}
	}
}

data class Pickup(
	val interactor: Long,
	val item: Item
) : SubPacket {
	override suspend fun writeTo(writer: Writer) {
		writer.writeLong(interactor)
		item.writeTo(writer)
	}

	companion object {
		internal suspend fun readFrom(reader: Reader): Pickup {
			return Pickup(
				interactor = reader.readLong(),
				item = Item.readFrom(reader)
			)
		}
	}
}

data class Kill(
	val killer: Long,
	val victim: Long,
	val unknown: Int,
	val xp: Int
) : SubPacket {
	override suspend fun writeTo(writer: Writer) {
		writer.writeLong(killer)
		writer.writeLong(victim)
		writer.writeInt(unknown)
		writer.writeInt(xp)
	}

	companion object {
		internal suspend fun readFrom(reader: Reader): Kill {
			return Kill(
				killer = reader.readLong(),
				victim = reader.readLong(),
				unknown = reader.readInt(),
				xp = reader.readInt()
			)
		}
	}
}

data class Attack(
	val target: Long,
	val attacker: Long,
	val damage: Float,
	val unknown: Int
) : SubPacket {
	override suspend fun writeTo(writer: Writer) {
		writer.writeLong(target)
		writer.writeLong(attacker)
		writer.writeFloat(damage)
		writer.writeInt(unknown)
	}

	companion object {
		internal suspend fun readFrom(reader: Reader): Attack {
			return Attack(
				target = reader.readLong(),
				attacker = reader.readLong(),
				damage = reader.readFloat(),
				unknown = reader.readInt()
			)
		}
	}
}

data class Mission(
	val sector: Vector2<Int>,
	val unknownA: Int,
	val unknownB: Int,
	val unknownC: Int,
	val id: Int,
	val type: Int,
	val boss: CreatureType,
	val level: Int,
	val unknownE: Byte,
	val state: MissionState,
	val padding: Short,
	val currentHP: Int,
	val maxHP: Int,
	val chunk: Vector2<Int>
) : SubPacket {
	override suspend fun writeTo(writer: Writer) {
		writer.writeVector2Int(sector)
		writer.writeInt(unknownA)
		writer.writeInt(unknownB)
		writer.writeInt(unknownC)
		writer.writeInt(id)
		writer.writeInt(type)
		writer.writeInt(boss.value)
		writer.writeInt(level)
		writer.writeByte(unknownE)
		writer.writeByte(state.value)
		writer.writeShort(padding)
		writer.writeInt(currentHP)
		writer.writeInt(maxHP)
		writer.writeVector2Int(chunk)
	}

	companion object {
		internal suspend fun readFrom(reader: Reader): Mission {
			return Mission(
				sector = reader.readVector2Int(),
				unknownA = reader.readInt(),
				unknownB = reader.readInt(),
				unknownC = reader.readInt(),
				id = reader.readInt(),
				type = reader.readInt(),
				boss = CreatureType(reader.readInt()),
				level = reader.readInt(),
				unknownE = reader.readByte(),
				state = MissionState(reader.readByte()),
				padding = reader.readShort(),
				currentHP = reader.readInt(),
				maxHP = reader.readInt(),
				chunk = reader.readVector2Int()
			)
		}
	}
}

inline class MissionState(val value: Byte) {
	companion object {
		val Ready = MissionState(0)
		val InProgress = MissionState(1)
		val Finished = MissionState(2)
	}
}