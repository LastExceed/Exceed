import utils.*
import kotlin.test.*

class FlagSetTests {
	@Test
	fun byteToBooleanArray() {
		val testByte = 42.toByte()
		val array = testByte.toBooleanArray()
		assertEquals(8, array.size)
		assertFalse(array[0])
		assertTrue(array[1])
		assertFalse(array[2])
		assertTrue(array[3])
		assertFalse(array[4])
		assertTrue(array[5])
		assertFalse(array[6])
		assertFalse(array[7])
	}

	@Test
	fun booleanArrayToByte() {
		val testArray = BooleanArray(8)
		testArray[0] = false
		testArray[1] = true
		testArray[2] = false
		testArray[3] = true
		testArray[4] = false
		testArray[5] = true
		testArray[6] = false
		testArray[7] = false

		val byte = testArray.toByte()
		assertEquals(42.toByte(), byte)
	}
}