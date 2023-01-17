import org.jetbrains.kotlin.gradle.tasks.KotlinCompile

plugins {
	kotlin("jvm") version "1.8.0"
	application
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
	implementation("com.github.lastexceed", "CubeworldNetworking", "1.4.0")
	implementation("com.github.ziggy42", "kolor", "1.0.0")
	implementation("dev.kord:kord-core:0.8.0-M17")
	implementation(kotlin("stdlib-jdk8"))
}

application {
	mainClass.set("MainKt")
}
val compileKotlin: KotlinCompile by tasks
compileKotlin.kotlinOptions {
	jvmTarget = "1.8"
}
val compileTestKotlin: KotlinCompile by tasks
compileTestKotlin.kotlinOptions {
	jvmTarget = "1.8"
}