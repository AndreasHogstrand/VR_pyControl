#A simple test task with two states, a and b, that prints the state name via serial

import pyb
from pyControl.utility import *
from devices import *

#Hardware

board = Breakout_1_2()  # Instantiate the breakout board object.


uart_basic = Uart_basic(board.port_1)

# States and events.

states = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'wait']

events = []

initial_state = 'wait'

v.target_state = 97

# Define behaviour. 

def wait(event):
	if event == 'entry':
		v.target_state = randint(97,104)
		timed_goto_state(chr(v.target_state),2*second)


def a(event):
    if event == 'entry':
        uart_basic.write('a')
        timed_goto_state('wait',4*second)

def b(event):
    if event == 'entry':
        uart_basic.write('b')
        timed_goto_state('wait',4*second)

def c(event):
    if event == 'entry':
        uart_basic.write('c')
        timed_goto_state('wait',4*second)

def d(event):
    if event == 'entry':
        uart_basic.write('d')
        timed_goto_state('wait',4*second)

def e(event):
    if event == 'entry':
        uart_basic.write('e')
        timed_goto_state('wait',4*second)

def f(event):
    if event == 'entry':
        uart_basic.write('f')
        timed_goto_state('wait',4*second)
        
def g(event):
    if event == 'entry':
        uart_basic.write('g')
        timed_goto_state('wait',4*second)

def h(event):
    if event == 'entry':
        uart_basic.write('h')
        timed_goto_state('wait',4*second)

