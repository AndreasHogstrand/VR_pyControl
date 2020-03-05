import pyb
import pyControl.hardware as hw

class Uart_basic():

    def __init__(self, port):
        assert port.UART is not None, '! Needs port with UART.'
        self.UART = pyb.UART(port.UART)
        self.UART.init(9600, bits = 8, parity=None, stop=1, flow=0, timeout=1)

    def write(self, char):
        self.UART.write(char)

    def read(self):
    	return(self.UART.read())

    def any(self):
		return(self.UART.any())