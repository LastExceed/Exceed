package packets

import utils.*

interface CwSerializable {
	suspend fun writeTo(writer: Writer)
}