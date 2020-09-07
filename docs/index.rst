.. VR_pycontrol documentation master file, created by
   sphinx-quickstart on Tue Jun  9 09:56:05 2020.
   You can adapt this file completely to your liking, but it should at least
   contain the root `toctree` directive.

Welcome to VR_pycontrol's documentation!
========================================

.. toctree::
   :maxdepth: 2
   :caption: Contents:

VR_pycontrol is an integration of the Unity based Tripodi lab VR with open source pyControl experiment control and recording. Unity is used for 3D visualisation and geometry, while pyControl is used for high precision timing of experimental events. The pyControl portion of this project is python based, while the Unity portion of this project is scripted in C#.

**PyControl**

PyControl is a system of open source hardware and software for controlling behavioural experiments, built around the Micropython microcontroller. PyControl is used as the framework to run experiments in the VR_pycontrol project. Functionality and usage is well explained by its own `documentation <https://pycontrol.readthedocs.io/en/latest/>`_.

**Unity** 

Unity is used for 3D visualisation and geometry (collision detection). Unity is treated as a hardware device by PyControl, and communicated with over the behaviour ports' UART serial (`hardware devices in pyControl <https://pycontrol.readthedocs.io/en/latest/user-guide/hardware/>`_). The main functionality of Unity is to place and render the targets and pointer, and detect collisions between the two.


Indices and tables
==================

* :ref:`genindex`
* :ref:`modindex`
* :ref:`search`
