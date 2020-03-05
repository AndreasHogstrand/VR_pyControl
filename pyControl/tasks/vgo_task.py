#A pycontrol implementation of the visual guided oriented task

import pyb
from pyControl.utility import *
from devices import *

#Hardware

board = Breakout_1_2()  # Instantiate the breakout board object.


uart_basic = Uart_basic(board.port_1)

pyboard_button = Digital_input('X17', falling_event='target_on/off', pull='up')  # USR button on pyboard, temporary


# States and events.

states = ['white_noise', 'target1', 'target2', 'target3', 'target4', 'target5', 'target6', 'target7', 'target8', 'success', 'failure']

events = ['white_noise_off', 'check_uart', 'target_timeout', 'reward']

initial_state = 'white_noise'

v.target_position = 1
v.white_noise_duration = 1
v.target_timeout_duration = 5
v.reward_delay_duration = 0.1
v.inter_trial_duration = 1


def white_noise(event):
	"""Play white noise before trial start"""
	if event == 'entry':
		#Play white noise
		set_timer('white_noise_off', v.white_noise_duration*second)
	elif event == 'white_noise_off':
		v.target_position = randint(1,8) #Generate random target location
		uart_basic.write(chr(v.target_position+96)) #Send character corresponding to target location (a-h) to Unity
		set_timer('target_timeout', v.target_timeout_duration*second) #Begin timeout timer
		goto_state('target'+str(v.target_position))


def target1(event):
	"""Wait for collision or timeout"""
	if event == 'entry':
		if uart_basic.any() > 0: #flush the buffer
			uart_basic.read()
		set_timer('check_uart', 5*ms)
	if event == 'check_uart':
		if uart_basic.any() > 0:
			uart_basic.read()
			disarm_timer('target_timeout')
			goto_state('success')
		else:
			set_timer('check_uart', 5*ms)	
	elif event == 'target_timeout':
		goto_state('failure')

def target2(event):
	"""Wait for collision or timeout"""
	if event == 'entry':
		if uart_basic.any() > 0: #flush the buffer
			uart_basic.read()
		set_timer('check_uart', 5*ms)
	if event == 'check_uart':
		if uart_basic.any() > 0:
			uart_basic.read()
			disarm_timer('target_timeout')
			goto_state('success')
		else:
			set_timer('check_uart', 5*ms)	
	elif event == 'target_timeout':
		goto_state('failure')

def target3(event):
	"""Wait for collision or timeout"""
	if event == 'entry':
		if uart_basic.any() > 0: #flush the buffer
			uart_basic.read()
		set_timer('check_uart', 5*ms)
	if event == 'check_uart':
		if uart_basic.any() > 0:
			uart_basic.read()
			disarm_timer('target_timeout')
			goto_state('success')
		else:
			set_timer('check_uart', 5*ms)	
	elif event == 'target_timeout':
		goto_state('failure')

def target4(event):
	"""Wait for collision or timeout"""
	if event == 'entry':
		if uart_basic.any() > 0: #flush the buffer
			uart_basic.read()
		set_timer('check_uart', 5*ms)
	if event == 'check_uart':
		if uart_basic.any() > 0:
			uart_basic.read()
			disarm_timer('target_timeout')
			goto_state('success')
		else:
			set_timer('check_uart', 5*ms)	
	elif event == 'target_timeout':
		goto_state('failure')

def target5(event):
	"""Wait for collision or timeout"""
	if event == 'entry':
		if uart_basic.any() > 0: #flush the buffer
			uart_basic.read()
		set_timer('check_uart', 5*ms)
	if event == 'check_uart':
		if uart_basic.any() > 0:
			uart_basic.read()
			disarm_timer('target_timeout')
			goto_state('success')
		else:
			set_timer('check_uart', 5*ms)	
	elif event == 'target_timeout':
		goto_state('failure')

def target6(event):
	"""Wait for collision or timeout"""
	if event == 'entry':
		if uart_basic.any() > 0: #flush the buffer
			uart_basic.read()
		set_timer('check_uart', 5*ms)
	if event == 'check_uart':
		if uart_basic.any() > 0:
			uart_basic.read()
			disarm_timer('target_timeout')
			goto_state('success')
		else:
			set_timer('check_uart', 5*ms)	
	elif event == 'target_timeout':
		goto_state('failure')

def target7(event):
	"""Wait for collision or timeout"""
	if event == 'entry':
		if uart_basic.any() > 0: #flush the buffer
			uart_basic.read()
		set_timer('check_uart', 5*ms)
	if event == 'check_uart':
		if uart_basic.any() > 0:
			uart_basic.read()
			disarm_timer('target_timeout')
			goto_state('success')
		else:
			set_timer('check_uart', 5*ms)	
	elif event == 'target_timeout':
		goto_state('failure')

def target8(event):
	"""Wait for collision or timeout"""
	if event == 'entry':
		if uart_basic.any() > 0: #flush the buffer
			uart_basic.read()
		set_timer('check_uart', 5*ms)
	if event == 'check_uart':
		if uart_basic.any() > 0:
			uart_basic.read()
			disarm_timer('target_timeout')
			goto_state('success')
		else:
			set_timer('check_uart', 5*ms)	
	elif event == 'target_timeout':
		goto_state('failure')

def success(event):
	"""Deliver reward after reward delay, then return to trial start after inter trial delay"""
	if event == 'entry':
		set_timer('reward', v.reward_delay_duration*second, output_event=True) #Begin timeout timer
	elif event == 'reward':
		#play reward tone
		#deliver reward
		timed_goto_state('white_noise', v.inter_trial_duration*second)

def failure(event):
	"""Play failure tone, then return to trial start after inter trial delay"""
	if event == 'entry':
		#play failure tone
		timed_goto_state('white_noise', v.inter_trial_duration*second)





