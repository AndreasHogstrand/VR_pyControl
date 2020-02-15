#A simple test task with two states, a and b, that prints the state name via serial

import pyb
from pyControl.utility import *
from devices import *
"""
class Uart_basic():

    def __init__(self,  port):
        assert port.UART is not None, '! Audio board needs port with UART.'
        self.UART = pyb.UART(port.UART)
        self.UART.init(9600, bits=8, parity=None, stop=1)


    def out(self,  data):
    	self.UART.write(data)
"""

#Hardware

board = Breakout_1_2()  # Instantiate the breakout board object.

#uart_basic = Uart_basic(board.port_1)

# States and events.

states = ['a']

events = []

initial_state = 'a'
uart = pyb.UART(board.port_1.UART, 9600)
uart.init(9600, bits = 8, parity=None, stop=1, flow=0)

# Define behaviour. 

def a(event):
    if event == 'entry':
        print(uart.write('a'))
        timed_goto_state('a',1*second)
