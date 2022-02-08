plugins {
	kotlin("jvm") version "1.6.10"
}
group = "me.lastexceed"
version = "1.0-SNAPSHOT"

repositories {
	mavenCentral()
	mavenLocal()
}

dependencies {
	implementation("io.ktor", "ktor-network", "2.0.+")
	implementation("com.github.lastexceed", "CubeworldNetworking", "1.1.0")
}