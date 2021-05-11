import org.jetbrains.kotlin.gradle.tasks.KotlinCompile

plugins {
	kotlin("jvm") version "1.5.0"
}
group = "me.lastexceed"
version = "1.0-SNAPSHOT"

repositories {
	mavenCentral()
	mavenLocal()
}

dependencies {
	implementation("io.ktor", "ktor-network", "1.5.+")
	implementation("me.lastexceed", "CubeworldNetworking", "1.0-SNAPSHOT")
}