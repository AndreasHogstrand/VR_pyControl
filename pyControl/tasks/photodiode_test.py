#A pycontrol implementation of the visual guided oriented task

import pyb
from pyControl.utility import *
from devices import *

#Hardware

board = Breakout_1_2()  # Instantiate the breakout board object.

photodiode = Analog_input(board.BNC_1,'photodiode',1000)

states = ['a']

events = ['a_event']

initial_state = 'a'

def a(event):
	if event == 'entry':
		photodiode.record()
		print('Started')

def run_end():
	photodiode.stop()