#A pycontrol implementation of the visual guided oriented task

import pyb
from pyControl.utility import *
from devices import *

#Hardware

board = Breakout_1_2()  # Breakout board

unity_uart = Uart_basic(board.port_1) # UART object used to communicate with Unity

photodiode = Analog_input(board.BNC_1,'photodiode',1000, threshold=350, rising_event='photodiode_event')

# States and events.

states = ['white_noise', 'target1', 'failure']

events = ['white_noise_off', 'check_uart', 'photodiode_event', 'target_timeout', 'reward']

initial_state = 'white_noise'

v.target_position = 1
v.white_noise_duration = 1
v.target_timeout_duration = 2
v.reward_delay_duration = 0.1
v.inter_trial_duration = 1
v.counter = 0


def run_start():
	photodiode.record()


def run_end():
	photodiode.stop()


def white_noise(event):
	"""Play white noise before trial start"""
	if event == 'entry':
		#Play white noise
		if v.counter < 10:
			v.counter += 1
			set_timer('white_noise_off', v.white_noise_duration*second)
		else:
			stop_framework()
	elif event == 'white_noise_off':
		unity_uart.write(chr(97)) #Send character corresponding to target location (a-h) to Unity
		set_timer('target_timeout', v.target_timeout_duration*second) #Begin timeout timer
		goto_state('target1')


def target1(event):
	"""Wait for collision or timeout"""
	if event == 'entry':
		if unity_uart.any() > 0: #flush the buffer
			unity_uart.read()
		set_timer('check_uart', 5*ms)
	if event == 'check_uart':
		if unity_uart.any() > 0:
			unity_uart.read()
		else:
			set_timer('check_uart', 5*ms)	
	elif event == 'target_timeout':
		goto_state('failure')


def failure(event):
	"""Play failure tone, then return to trial start after inter trial delay"""
	if event == 'entry':
		#play failure tone
		timed_goto_state('white_noise', v.inter_trial_duration*second)





