import org.jetbrains.kotlin.gradle.tasks.KotlinCompile

plugins {
	kotlin("jvm") version "1.3.70"
	id("org.openjfx.javafxplugin") version "0.0.8"
}

allprojects {
	group = "LastExceed"
	version = "1.0-SNAPSHOT"

	tasks.withType<KotlinCompile> {
		kotlinOptions {
			jvmTarget = "13"
			freeCompilerArgs = listOf("-XXLanguage:+InlineClasses")
		}
	}
}

subprojects {
	apply(plugin = "java")
	apply(plugin = "org.jetbrains.kotlin.jvm")

	repositories {
		mavenCentral()
	}

	dependencies {
		implementation(kotlin("stdlib-jdk8"))
		implementation("io.ktor", "ktor-network", "1.3.+")
	}
}

project(":server") {
	dependencies {
		implementation(project(":common"))
	}
}

project(":client") {
	apply(plugin = "org.openjfx.javafxplugin")
	repositories {
		maven(url = "https://oss.sonatype.org/content/repositories/snapshots")
	}
	javafx {
		version = "13.0.1"
		modules(
			"javafx.controls",
			"javafx.fxml",
			"javafx.media",
			"javafx.web",
			"javafx.swing"
		)

		java {
			targetCompatibility = JavaVersion.VERSION_13
			sourceCompatibility = JavaVersion.VERSION_13
		}
	}
	dependencies {
		implementation(project(":common"))
		implementation("no.tornado:tornadofx:2.0.0-SNAPSHOT")
	}
}