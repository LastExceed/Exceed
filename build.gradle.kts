plugins {
	kotlin("jvm") version "1.6.10"
}
group = "com.github.lastexceed"
version = "1.0-SNAPSHOT"

repositories {
	mavenLocal()
	mavenCentral()
	maven("https://jitpack.io")
}

dependencies {
	implementation("io.ktor", "ktor-network", "2.0.+")
	implementation("com.github.lastexceed", "CubeworldNetworking", "1.3.42")
}