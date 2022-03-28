suspend fun main() {
	System.setProperty("io.ktor.utils.io.BufferSize", "8192")
	println("=====Exceed=====")
	Server.start()
}

//todo: sugar for sending server update with 1 subpacket

//TODO: features
//all damage visible to everyone by setting packet receiver as damage source
//login
//join/leave messages
//anticheat - character visuals, illegal items, ect
//visualize manashield using quest HP bar
//tombstones - go back to corpse to revive like in World of Warcraft - use negative time (-10100000) to visualize ghost world
//damage over time - bleeding, burning, poison