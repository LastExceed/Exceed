import com.andreapivetta.kolor.*

suspend fun main() {
	println(Kolor.foreground("=====Exceed=====", Color.MAGENTA))
	Server.start()
}