import org.jetbrains.kotlin.gradle.tasks.KotlinCompile

plugins {
    kotlin("jvm") version "1.4.0"
    application
}
group = "me.lastexceed"
version = "1.0-SNAPSHOT"

repositories {
    mavenCentral()
}

dependencies {
    implementation(kotlin("stdlib-jdk8"))
    implementation("io.ktor", "ktor-network", "1.4.+")
}

tasks.withType<KotlinCompile> {
    kotlinOptions {
        jvmTarget = "13"
        freeCompilerArgs = listOf("-XXLanguage:+InlineClasses")
    }
}
application {
    mainClassName = "MainKt"
}