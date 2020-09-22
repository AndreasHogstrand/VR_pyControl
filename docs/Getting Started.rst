===============
Getting Started
===============

Pulling the Repository
########################

To begin with, fork/clone the master git repository, found at https://github.com/AndreasHogstrand/VR_pyControl . This is an open repository and should be freely accessible to clone: permission to push changes to the repository can be requested from an admin (HogstrandAndreas@gmail.com). Code is contained within 2 folders in the master directory: 360VR_refactor for Unity and Arduino scripts, and pyControl for Python scripts.

Setting up a PyControl environment
########################################
NOTE: Instructions on setting up PyControl are available in greater detail at the PyControl documentation: https://pycontrol.readthedocs.io/en/latest/

To begin using PyControl, a python environment should be established to run PyControl scripts. At the time of writing, the latest version of Python compatible with PyControl is 3.7.5. As this is not the latest version of Python, you may wish to create a seperate Python environment to run Pycontrol: this can be achieved by setting up a Python Virtual Environment (https://docs.python.org/3/tutorial/venv.html). Note that while Anaconda could also be used, at the time of development there was an incompatibiltiy with pyControl due to conda and pip using different package names.

Installing dependencies
*****************************
PyControl's dependencies may be installed using the following commands:

.. code-block:: none

	python -m pip install numpy
	python -m pip install pyserial
	python -m pip install pyqt5
	python -m pip install pyqtgraph

To test the environment has installed correctly, navigate to the GUI folder within the Pycontrol directory and run the gui using



.. code-block:: none

	python pyControl_GUI.py

If the command executes and opens the pyControl GUI, the installation has been successful.
