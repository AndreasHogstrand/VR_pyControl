dim total_events        ' count of input signal toggles
dim trial_events        ' count of input signal toggles

dim in1
dim in5                 ' temporary variables
dim in6
dim time_in_intervals         ' subdivision of time into 20ms intervals
dim current_interval          ' interval which trial is currently in
dim waiting_for_1_on5         ' true if waiting for SignalIn(5) to go to 1
dim waiting_for_1_on6         ' true if waiting for SignalIn(6) to go to 1
dim waiting_for_1_on1         ' true if waiting for SignalIn(1) to go to 1
dim pattern_start_time        ' time when stim pattern was last started

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

dim pattern_start
dim blank
dim x
dim y

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

sub main()

add_element(0,560)
add_element(0,561)
add_element(0,562)
add_element(0,563)
add_element(0,564)
add_element(0,565)
add_element(0,566)
add_element(0,567)
add_element(0,568)
add_element(0,569)
add_element(10,567)
add_element(11,567)
add_element(12,567)
add_element(13,567)
add_element(14,567)
add_element(15,567)
add_element(0,599999)
add_element(1,599999)
add_element(2,599999)
add_element(3,599999)
add_element(4,599999)
add_element(5,599999)
add_element(6,599999)
add_element(7,599999)
add_element(8,599999)
add_element(9,599999)
add_element(10,599999)
add_element(11,599999)
add_element(12,599999)
add_element(13,599999)
add_element(14,599999)
add_element(15,599999)

write_output_file()

end sub

main()