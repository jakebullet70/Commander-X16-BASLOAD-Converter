100 rem midway campaign
200 if e-1=6then970
300 IF 1=1 THENGOTO970
500 IFZ5<=4THENONZ4GOTO900,950,960,970
710 gosub810:gosub900
750 poke 53280,6:poke 53281, 1: print"he:lp"
790 REM ONZ4GOTO900,950,960,970
800 dimf(5,7),c%(7,9),s%(9,9),w(5),n$(7),t$(5),c1(3),d$(4),p$(5)
805 fori=0to4:readd$(i):next:fori=0to5:readp$(i):Next
810 datacruisers,tf-16,tf-17,midway,none,light,heavy,sunk,destr'd,f4f's,zekes
820 REM goto150:gosub200:goto 10
830 onggosub810,900
840 gosub 810:ifj=1thenprint" {blu}{rvon}";n$(s%(i,9));
850 print ""
860 IFZ5<=4THENONZ4GOSUB900,950,960,970
900 print "line 900"
950 print"line 950"
960 print  "line 960 gosub test"
970 rem 970
980 rem goto:3,140,830,984
990 end
999 goto100


