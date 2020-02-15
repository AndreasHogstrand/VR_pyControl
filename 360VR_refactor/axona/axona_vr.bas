dim time_in_intervals         ' subdivision of time into 20ms intervals
dim current_interval          ' interval which trial is currently in
dim SHARED _posedge_in00 = 1        ' true if waiting for SignalIn(1) to go to 1

Dim SHARED lengths(0 to 15) as Integer => {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
Dim SHARED max_events as Integer = 0

Dim SHARED ts_in00(0 to 1000) as Integer
Dim SHARED ts_in01(0 to 1000) as Integer
Dim SHARED ts_in02(0 to 1000) as Integer
Dim SHARED ts_in03(0 to 1000) as Integer
Dim SHARED ts_in04(0 to 1000) as Integer
Dim SHARED ts_in05(0 to 1000) as Integer
Dim SHARED ts_in06(0 to 1000) as Integer
Dim SHARED ts_in07(0 to 1000) as Integer
Dim SHARED ts_in08(0 to 1000) as Integer
Dim SHARED ts_in09(0 to 1000) as Integer
Dim SHARED ts_in10(0 to 1000) as Integer
Dim SHARED ts_in11(0 to 1000) as Integer
Dim SHARED ts_in12(0 to 1000) as Integer
Dim SHARED ts_in13(0 to 1000) as Integer
Dim SHARED ts_in14(0 to 1000) as Integer
Dim SHARED ts_in15(0 to 1000) as Integer

dim descr_in00 as String = "ttl00"
dim descr_in01 as String = "ttl01"
dim descr_in02 as String = "ttl02"
dim descr_in03 as String = "ttl03"
dim descr_in04 as String = "ttl04"
dim descr_in05 as String = "ttl05"
dim descr_in06 as String = "ttl06"
dim descr_in07 as String = "ttl07"
dim descr_in08 as String = "ttl08"
dim descr_in09 as String = "ttl09"
dim descr_in10 as String = "ttl10"
dim descr_in11 as String = "ttl11"
dim descr_in12 as String = "ttl12"
dim descr_in13 as String = "ttl13"
dim descr_in14 as String = "ttl14"
dim descr_in15 as String = "ttl15"

Dim SHARED SignalIn(0 to 1) as Integer => {0,0}
Dim SHARED delayMS(0 to 1) as Integer => {0,0}



sub add_element(ttl_channel as Integer, value as Integer)
  
  Select Case ttl_channel
    Case 0
      ts_in00(lengths(0)) = value

    Case 1
      ts_in01(lengths(1)) = value

    Case 2
      ts_in02(lengths(2)) = value

    Case 3
      ts_in03(lengths(3)) = value

    Case 4
      ts_in04(lengths(4)) = value

    Case 5
      ts_in05(lengths(5)) = value
      
    Case 6
      ts_in06(lengths(6)) = value

    Case 7
      ts_in07(lengths(7)) = value

    Case 8
      ts_in08(lengths(8)) = value

    Case 9
      ts_in09(lengths(9)) = value

    Case 10
      ts_in10(lengths(10)) = value

    Case 11
      ts_in11(lengths(11)) = value

    Case 12
      ts_in12(lengths(12)) = value

    Case 13
      ts_in13(lengths(13)) = value

    Case 14
      ts_in14(lengths(14)) = value

    Case 15
      ts_in15(lengths(15)) = value

    Case Else

  End Select

  lengths(ttl_channel) = lengths(ttl_channel) + 1

end sub

sub write_output_file()

  open "prova_basic.ttl" for output as #1   ' creates .ttl file containing the timestamps of the events

  print #1, "<ttlfile>"

  print #1, "<ttl00>"
  for i as integer = 0 to lengths(0) -1
    print #1, "<item timestamp=", chr$(34) , ts_in00(i), chr$(34), " />"
  next
  print #1, "</ttl00>"

  print #1, "<ttl01>"
  for i = 0 to lengths(1) -1
    print #1, "<item timestamp=", chr$(34) , ts_in01(i), chr$(34), " />"
  next
  print #1, "</ttl01>"

  print #1, "<ttl02>"
  for i = 0 to lengths(2) -1
    print #1, "<item timestamp=", chr$(34) , ts_in02(i), chr$(34), " />"
  next
  print #1, "</ttl02>"

  print #1, "<ttl03>"
  for i = 0 to lengths(3) -1
    print #1, "<item timestamp=", chr$(34) , ts_in03(i), chr$(34), " />"
  next
  print #1, "</ttl03>"

  print #1, "<ttl04>"
  for i = 0 to lengths(4) -1
    print #1, "<item timestamp=", chr$(34) , ts_in04(i), chr$(34), " />"
  next
  print #1, "</ttl04>"

  print #1, "<ttl05>"
  for i = 0 to lengths(5) -1
    print #1, "<item timestamp=", chr$(34) , ts_in05(i), chr$(34), " />"
  next
  print #1, "</ttl05>"

  print #1, "<ttl06>"
  for i = 0 to lengths(6) -1
    print #1, "<item timestamp=", chr$(34) , ts_in06(i), chr$(34), " />"
  next
  print #1, "</ttl06>"

  print #1, "<ttl07>"
  for i = 0 to lengths(7) -1
    print #1, "<item timestamp=", chr$(34) , ts_in07(i), chr$(34), " />"
  next
  print #1, "</ttl07>"

  print #1, "<ttl08>"
  for i = 0 to lengths(8) -1
    print #1, "<item timestamp=", chr$(34) , ts_in08(i), chr$(34), " />"
  next
  print #1, "</ttl08>"

  print #1, "<ttl09>"
  for i = 0 to lengths(9) -1
    print #1, "<item timestamp=", chr$(34) , ts_in09(i), chr$(34), " />"
  next
  print #1, "</ttl09>"

  print #1, "<ttl10>"
  for i = 0 to lengths(10) -1
    print #1, "<item timestamp=", chr$(34) , ts_in10(i), chr$(34), " />"
  next
  print #1, "</ttl10>"

  print #1, "<ttl11>"
  for i = 0 to lengths(11) -1
    print #1, "<item timestamp=", chr$(34) , ts_in11(i), chr$(34), " />"
  next
  print #1, "</ttl11>"

  print #1, "<ttl12>"
  for i = 0 to lengths(12) -1
    print #1, "<item timestamp=", chr$(34) , ts_in12(i), chr$(34), " />"
  next
  print #1, "</ttl12>"

  print #1, "<ttl13>"
  for i = 0 to lengths(13) -1
    print #1, "<item timestamp=", chr$(34) , ts_in13(i), chr$(34), " />"
  next
  print #1, "</ttl13>"

  print #1, "<ttl14>"
  for i = 0 to lengths(14) -1
    print #1, "<item timestamp=", chr$(34) , ts_in14(i), chr$(34), " />"
  next
  print #1, "</ttl14>"

  print #1, "<ttl15>"
  for i = 0 to lengths(15) -1
    print #1, "<item timestamp=", chr$(34) , ts_in15(i), chr$(34), " />"
  next
  print #1, "</ttl15>"

  print #1, "</ttlfile>"

end sub

Function posedge_in00() as Integer

  if SignalIn(0) = 1 then
    if _posedge_in00 = 1 then
      _posedge_in00 = 0
      return 1
    end if
  else 
    _posedge_in00 = 1
  end if 
  return 0

end Function

sub main()

  FinalSignalOut = 0        ' all outputs go to 0 when recording ends

  open "recording.log" for output as #2   ' creates .log file for a verbose log of the recording session

  'SignalOut(1) = on             ' Starts behavioural apparatus

  current_interval = 0          ' this number will be incremented every 20ms

  'StartUnitRecording

  while (1)   ' loop forever, recording will be stopped when desired number of inputs reached

    time_in_intervals = TrialTime / 20  ' subdivide time into 20ms chunks

    if (time_in_intervals > current_interval) then

      ' the next 20ms interval has started, so print the state of the outputs

      'in1 = SignalIn(1)
      'in5 = SignalIn(5)
      'in6 = SignalIn(6)

      ' increment interval counter

      current_interval = current_interval + 1

    else

      delayMS(1)=0   ' don't hog CPU time in a tight loop

    end if

    ' count only complete transitions on input

    trial_events = 0

    if posedge_in00() = 1 then
      print #2, "Event on channel 1 detected (", descr_in01, ")"
      trial_events = trial_events + 1
      time_stamp_1 = TrialTime
      add_element(0, time_stamp_1)
    end if

    ' TODO: termination condition

    if false then

      print #2, "Closing recording session"
      'StopUnitRecording
      print #2, "Recording session stopped successfully."

      print #2, "Saving output file"
      write_output_file()
      print #2, "Output file saved successfully"

      dim total_events as Integer = 0
      for i as Integer = 0 to 15
        total_events = total_events + lengths(i)
      next
      print #2, total_events, " events detected during the recording session."


      close #1  ' close log file
      close #2  ' close log file
      delayMS(10000)=0
      while (1)
        ' trap the script here while recording is stopping
        delayMS(10)=0
      wend
    end if

  wend

end sub


