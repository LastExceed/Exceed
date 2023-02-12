package modules

import Player
import com.github.lastexceed.cubeworldnetworking.packets.*
import com.github.lastexceed.cubeworldnetworking.utils.*

object Dummys {
	fun onJoin(player: Player) {
		val dummy = CreatureUpdate(
			id = CreatureId(4321),
			//rotation = Vector3(90f, 0f, 0f),
			affiliation = Affiliation.Neutral,
			race = Race.Dummy,
			appearance = Appearance(
				unknownA = 0,
				unknownB = 0,
				hairColor = Vector3(0, 0, 0),
				unknownC = 0,
				flags = FlagSet<AppearanceFlag>(BooleanArray(16) { false }).apply { set(AppearanceFlag.LockedInPlace, true) },
				creatureSize = Vector3(1f, 1f, 1f),
				headModel = -1,
				hairModel = -1,
				handModel = -1,
				footModel = -1,
				bodyModel = 2111,
				tailModel = -1,
				shoulder2Model = -1,
				wingModel = 0,
				headSize = 0f,
				bodySize = 2f,
				handSize = 0f,
				footSize = 0f,
				shoulder2Size = 0f,
				weaponSize = 0f,
				tailSize = 0f,
				shoulder1Size = 0f,
				wingSize = 0f,
				bodyRotation = 0f,
				handRotation = Vector3(0f, 0f, 0f),
				feetRotation = 0f,
				wingRotation = 0f,
				tailRotation = 0f,
				bodyOffset = Vector3(0f, 0f, 0f),
				headOffset = Vector3(0f, 0f, 0f),
				handOffset = Vector3(0f, 0f, 0f),
				footOffset = Vector3(0f, 0f, 0f),
				tailOffset = Vector3(0f, 0f, 0f),
				wingOffset = Vector3(0f, 0f, 0f),
			)
		)

		repeat(4) {
			player.send(
				dummy.copy(
					id = CreatureId(4321 + it.toLong()),
					position = Vector3(
						549317507493 + it * 0xC0000,
						551630083025,
						267640
					)
				)
			)
		}
	}
}