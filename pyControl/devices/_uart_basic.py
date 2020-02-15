import pyb
import pyControl.hardware as hw

class Uart_basic():

    def __init__(self,  port):
        assert port.UART is not None, '! Audio board needs port with UART.'
        self.UART = pyb.UART(port.UART)
        self.UART.init(9600, bits=8, parity=None, stop=1)


    def write(char):
    	self.UART.write(char)