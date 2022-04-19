package modules

import Player
import com.github.lastexceed.cubeworldnetworking.packets.*
import com.github.lastexceed.cubeworldnetworking.utils.*
import java.io.DataInputStream
import java.io.File
import kotlin.math.*

//this entire file is complete garbage code, it was quickly hacked together to get things to work asap

object ModelImport {
	var cache: WorldUpdate? = null
	val spawnPoint = Vector3<Long>(0x802080_0000, 0x802080_0000, 0x200_0000)

	suspend fun onJoin(player: Player) {
		player.send(
			WorldUpdate(
				worldEdits = listOf(
					WorldEdit(
						position = Vector3(0x802080, 0x802080, 0x200),
						color = Vector3(-1, -1, -1),
						blockType = WorldEdit.BlockType.Solid
					)
				),
				soundEffects = listOf(
					SoundEffect(
						position = Utils.creatureToSoundPosition(player.character.position),
						sound = SoundEffect.Sound.Craft
					)
				)
			)
		)

		ExperimentalStuff.teleport(player, spawnPoint)

		if (cache == null) {
			cache = WorldUpdate(
				worldEdits = parseVoxModel("FD_A_2B_minifed.vox").map {
					it.copy(
						position = Vector3(
							it.position.x + 0x802080 - 63,
							it.position.y + 0x802080 - 63,
							it.position.z + 0x200 - 63 - 15,
						)
					)
				},
				soundEffects = listOf(
					SoundEffect(
						position = Utils.creatureToSoundPosition(player.character.position),
						sound = SoundEffect.Sound.DropCoin
					)
				)
			)
		}

		player.send(cache!!)
	}

	suspend fun onCreatureUpdate(source: Player, packet: CreatureUpdate) {
		val distanceFromSpawn = packet.position?.let {
			listOf(
				it.x - spawnPoint.x,
				it.y - spawnPoint.y,
				it.z - spawnPoint.z,
			).map { it.toDouble().pow(2) }.sum().let { sqrt(it) }
		} ?: return

		if (distanceFromSpawn > 100 * Utils.SIZE_BLOCK) {
			ExperimentalStuff.teleport(source, spawnPoint)
		}
	}

	suspend fun parseVoxModel(filepath: String): List<WorldEdit> {
		val models = mutableListOf<List<MutableList<Int>>>()
		var palette = listOf(
			0x00000000u, 0xffffffffu, 0xffccffffu, 0xff99ffffu, 0xff66ffffu, 0xff33ffffu, 0xff00ffffu, 0xffffccffu, 0xffccccffu, 0xff99ccffu, 0xff66ccffu, 0xff33ccffu, 0xff00ccffu, 0xffff99ffu, 0xffcc99ffu, 0xff9999ffu,
			0xff6699ffu, 0xff3399ffu, 0xff0099ffu, 0xffff66ffu, 0xffcc66ffu, 0xff9966ffu, 0xff6666ffu, 0xff3366ffu, 0xff0066ffu, 0xffff33ffu, 0xffcc33ffu, 0xff9933ffu, 0xff6633ffu, 0xff3333ffu, 0xff0033ffu, 0xffff00ffu,
			0xffcc00ffu, 0xff9900ffu, 0xff6600ffu, 0xff3300ffu, 0xff0000ffu, 0xffffffccu, 0xffccffccu, 0xff99ffccu, 0xff66ffccu, 0xff33ffccu, 0xff00ffccu, 0xffffccccu, 0xffccccccu, 0xff99ccccu, 0xff66ccccu, 0xff33ccccu,
			0xff00ccccu, 0xffff99ccu, 0xffcc99ccu, 0xff9999ccu, 0xff6699ccu, 0xff3399ccu, 0xff0099ccu, 0xffff66ccu, 0xffcc66ccu, 0xff9966ccu, 0xff6666ccu, 0xff3366ccu, 0xff0066ccu, 0xffff33ccu, 0xffcc33ccu, 0xff9933ccu,
			0xff6633ccu, 0xff3333ccu, 0xff0033ccu, 0xffff00ccu, 0xffcc00ccu, 0xff9900ccu, 0xff6600ccu, 0xff3300ccu, 0xff0000ccu, 0xffffff99u, 0xffccff99u, 0xff99ff99u, 0xff66ff99u, 0xff33ff99u, 0xff00ff99u, 0xffffcc99u,
			0xffcccc99u, 0xff99cc99u, 0xff66cc99u, 0xff33cc99u, 0xff00cc99u, 0xffff9999u, 0xffcc9999u, 0xff999999u, 0xff669999u, 0xff339999u, 0xff009999u, 0xffff6699u, 0xffcc6699u, 0xff996699u, 0xff666699u, 0xff336699u,
			0xff006699u, 0xffff3399u, 0xffcc3399u, 0xff993399u, 0xff663399u, 0xff333399u, 0xff003399u, 0xffff0099u, 0xffcc0099u, 0xff990099u, 0xff660099u, 0xff330099u, 0xff000099u, 0xffffff66u, 0xffccff66u, 0xff99ff66u,
			0xff66ff66u, 0xff33ff66u, 0xff00ff66u, 0xffffcc66u, 0xffcccc66u, 0xff99cc66u, 0xff66cc66u, 0xff33cc66u, 0xff00cc66u, 0xffff9966u, 0xffcc9966u, 0xff999966u, 0xff669966u, 0xff339966u, 0xff009966u, 0xffff6666u,
			0xffcc6666u, 0xff996666u, 0xff666666u, 0xff336666u, 0xff006666u, 0xffff3366u, 0xffcc3366u, 0xff993366u, 0xff663366u, 0xff333366u, 0xff003366u, 0xffff0066u, 0xffcc0066u, 0xff990066u, 0xff660066u, 0xff330066u,
			0xff000066u, 0xffffff33u, 0xffccff33u, 0xff99ff33u, 0xff66ff33u, 0xff33ff33u, 0xff00ff33u, 0xffffcc33u, 0xffcccc33u, 0xff99cc33u, 0xff66cc33u, 0xff33cc33u, 0xff00cc33u, 0xffff9933u, 0xffcc9933u, 0xff999933u,
			0xff669933u, 0xff339933u, 0xff009933u, 0xffff6633u, 0xffcc6633u, 0xff996633u, 0xff666633u, 0xff336633u, 0xff006633u, 0xffff3333u, 0xffcc3333u, 0xff993333u, 0xff663333u, 0xff333333u, 0xff003333u, 0xffff0033u,
			0xffcc0033u, 0xff990033u, 0xff660033u, 0xff330033u, 0xff000033u, 0xffffff00u, 0xffccff00u, 0xff99ff00u, 0xff66ff00u, 0xff33ff00u, 0xff00ff00u, 0xffffcc00u, 0xffcccc00u, 0xff99cc00u, 0xff66cc00u, 0xff33cc00u,
			0xff00cc00u, 0xffff9900u, 0xffcc9900u, 0xff999900u, 0xff669900u, 0xff339900u, 0xff009900u, 0xffff6600u, 0xffcc6600u, 0xff996600u, 0xff666600u, 0xff336600u, 0xff006600u, 0xffff3300u, 0xffcc3300u, 0xff993300u,
			0xff663300u, 0xff333300u, 0xff003300u, 0xffff0000u, 0xffcc0000u, 0xff990000u, 0xff660000u, 0xff330000u, 0xff0000eeu, 0xff0000ddu, 0xff0000bbu, 0xff0000aau, 0xff000088u, 0xff000077u, 0xff000055u, 0xff000044u,
			0xff000022u, 0xff000011u, 0xff00ee00u, 0xff00dd00u, 0xff00bb00u, 0xff00aa00u, 0xff008800u, 0xff007700u, 0xff005500u, 0xff004400u, 0xff002200u, 0xff001100u, 0xffee0000u, 0xffdd0000u, 0xffbb0000u, 0xffaa0000u,
			0xff880000u, 0xff770000u, 0xff550000u, 0xff440000u, 0xff220000u, 0xff110000u, 0xffeeeeeeu, 0xffddddddu, 0xffbbbbbbu, 0xffaaaaaau, 0xff888888u, 0xff777777u, 0xff555555u, 0xff444444u, 0xff222222u, 0xff111111u
		)

		val data = DataInputStream(File(filepath).inputStream()).readAllBytes()
		val reader = Reader(data)
		assert(reader.readChars(4) == "VOX ")
		assert(reader.readInt() == 150) //file format version

		var nTRNindex = 0

		while (true) {
			val chunkId = try {
				reader.readChars(4)
			} catch (ex: Exception) {
				break
			}
			val ownBytes = reader.readInt()
			val childrenBytes = reader.readInt()

			when (chunkId) {
				"MAIN" -> {
					assert(ownBytes == 0)
				}
				"PACK" -> {
					assert(ownBytes == 4)
					assert(childrenBytes == 0)
					val numModels = reader.readInt()
				}
				"SIZE" -> {
					assert(ownBytes == 12)
					assert(childrenBytes == 0)
					val x = reader.readInt()
					val y = reader.readInt()
					val z = reader.readInt()
				}
				"XYZI" -> {
					assert(ownBytes >= 4)
					assert(childrenBytes == 0)
					val voxelCount = reader.readInt()
					assert(ownBytes == 4 + voxelCount * 4)


					models.add(List(voxelCount) { reader.readByteArray(4).map { it.toUByte().toInt() }.toMutableList() })
				}
				"nTRN" -> {
					assert(childrenBytes == 0)
					if (ownBytes >= 43) {
						reader.skip(38)
						val offsets = reader.readChars(ownBytes - 38).split(' ').map { it.toInt() }
						assert(offsets.size == 3)
						models[nTRNindex].forEach { voxel ->
							offsets.forEachIndexed { dimension, offset ->
								voxel[dimension] += offset
							}
						}
						nTRNindex++
					} else {
						reader.skip(ownBytes)
					}
				}
				"RGBA" -> {
					assert(ownBytes == 256 * 4)
					palette = List(256) { reader.readInt().toUInt() }
				}
				else -> {
					reader.skip(ownBytes)
				}
			}
		}
		return models.flatten().map {

			val colorInt = palette[it[3] - 1]

			val color = Vector3(colorInt.toByte(), colorInt.shr(8).toByte(), colorInt.shr(16).toByte())
			WorldEdit(
				position = Vector3(it[0], it[1], it[2]),
				color = color,
				blockType = if (color == Vector3<Byte>(0, 0, -1)) WorldEdit.BlockType.Liquid else WorldEdit.BlockType.Solid
			)
		}
	}
}

private suspend fun Reader.readChars(count: Int) =
	readByteArray(count).joinToString("") { it.toInt().toChar().toString() }