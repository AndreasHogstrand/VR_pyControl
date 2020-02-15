dim total_signal        ' count of input signal toggles
dim in1
dim in5                 ' temporary variables
dim in6
dim time_in_intervals         ' subdivision of time into 20ms intervals
dim current_interval          ' interval which trial is currently in
dim waiting_for_1_on5         ' true if waiting for SignalIn(5) to go to 1
dim waiting_for_1_on6         ' true if waiting for SignalIn(6) to go to 1
dim waiting_for_1_on1         ' true if waiting for SignalIn(1) to go to 1
dim pattern_start_time        ' time when stim pattern was last started
dim time_stamp_1
dim time_stamp_5
dim time_stamp_6
dim pattern_start
dim blank
dim x
dim y

sub main()

  FinalSignalOut = 0        ' all outputs go to 0 when recording ends

  open "data.beh" for output as #1   ' creates .log file for printing to

  SignalOut(1) = on             ' Starts behavioural apparatus

  total_signal = 0              ' end recording when this reaches 50

  current_interval = 0          ' this number will be incremented every 20ms

  waiting_for_1_on1 = 1             ' initially we want to do something when input goes to 1
  waiting_for_1_on5 = 1             ' initially we want to do something when input goes to 1
  waiting_for_1_on6 = 1             ' initially we want to do something when input goes to 1

  pattern_start_time = 0

  blank = 0

  y = 1

  StartUnitRecording

  while (1)   ' loop forever, recording will be stopped when desired number of inputs reached

    time_in_intervals = TrialTime / 20  ' subdivide time into 20ms chunks

    if (time_in_intervals > current_interval) then

      ' the next 20ms interval has started, so print the state of the outputs

      in1 = SignalIn(1)
      in5 = SignalIn(5)
      in6 = SignalIn(6)

      ' increment interval counter

      current_interval = current_interval + 1

    else

      delayMS(1)   ' don't hog CPU time in a tight loop

    end if

    ' count only complete transitions on input

    if SignalIn(1) = 1 then
      if waiting_for_1_on1 = 1 then
      total_signal = total_signal + 1
        ' start of a new signal on the input
        print "attentional cue on"
        time_stamp_1 = TrialTime
        print #1, time_stamp_1,chr$(9), blank, chr$(9),blank, chr$(9),blank, chr$(9), total_signal
        ' now we need to wait for the input to go to 0 again
          waiting_for_1_on1 = 0
      end if
    else
      if waiting_for_1_on1 = 0 then
        ' end of the 1 input, so now we want to wait for the next
        waiting_for_1_on1 = 1
      end if
    end if

     if SignalIn(5) = 1 then
      if waiting_for_1_on5 = 1 then
        ' start of a new signal on the input
        print "LED and Speaker on"
        time_stamp_5 = TrialTime
        print #1, blank, chr$(9), time_stamp_5, chr$(9),blank, chr$(9),blank, chr$(9), total_signal
        ' now we need to wait for the input to go to 0 again

          waiting_for_1_on5 = 0
      end if
    else
      if waiting_for_1_on5 = 0 then
        ' end of the 1 input, so now we want to wait for the next
        waiting_for_1_on5 = 1
      end if
    end if

    if SignalIn(6) = 1 then
      if waiting_for_1_on6 = 1 then
        ' start of a new signal on the input if >200 ms since last one
        print "Sensor activated"
        time_stamp_6 = TrialTime
         print #1, blank, chr$(9), blank, chr$(9),time_stamp_6, chr$(9),blank, chr$(9), total_signal

        ' now we need to wait for the input to go to 0 again
        waiting_for_1_on6 = 0
      end if
    else
      if waiting_for_1_on6 = 0 then
        ' end of the 1 input, so now we want to wait for the next
        waiting_for_1_on6 = 1
        print #1, blank, chr$(9), blank, chr$(9),blank, chr$(9),blank, chr$(9), total_signal
      end if
    end if


    ' if we reach 50 pulses on the input, stop recording after 10 seconds

    if total_signal >= 101 then
      close #1  ' close log file
      print "stopping"
      delayMS(10000)
      StopUnitRecording
      while (1)
        ' trap the script here while recording is stopping
        delayMS(10)
      wend
    end if

  wend

end sub
