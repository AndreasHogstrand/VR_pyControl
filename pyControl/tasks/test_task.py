#A simple test task with two states, a and b

from pyControl.utility import *

# States and events.

states = ['a',
          'b']

events = []

initial_state = 'a'

# Define behaviour. 

def a(event):
    if event == 'entry':
        timed_goto_state('b', 0.5 * second)

def b(event):
    if event == 'entry':
        timed_goto_state('a', 0.5 * second)