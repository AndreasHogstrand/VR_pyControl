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
        print('a')
        timed_goto_state('b',6*second)

def b(event):
    if event == 'entry':
        timed_goto_state('a',2*second)