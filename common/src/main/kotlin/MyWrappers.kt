import io.ktor.util.toByteArray
import kotlinx.coroutines.io.*

suspend fun main() {
	val y = ByteChannel(true)
	y.writeInt(123)
	//val data = ByteArray(y.availableForRead)
	//y.readFully(data, 0, y.availableForRead)
	println("?")
	y.close()
	val data2 = y.toByteArray()
	println("!")
	println(data2.contentToString())
	println("done")
}