#A simple test task with two states, a and b, that prints the state name via serial

import pyb
from pyControl.utility import *
from devices import *

#Hardware

board = Breakout_1_2()  # Instantiate the breakout board object.


uart_basic = Uart_basic(board.port_1)

# States and events.

states = ['a', 'b']

events = []

initial_state = 'a'

# Define behaviour. 

def a(event):
    if event == 'entry':
        uart_basic.write('a')
        timed_goto_state('b',2*second)

def b(event):
    if event == 'entry':
        uart_basic.write('b')
        timed_goto_state('a',2*second)