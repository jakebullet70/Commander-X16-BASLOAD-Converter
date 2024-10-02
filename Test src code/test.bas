1 rem midway campaign
2 if e-1=6then970
3 IF 1=1 THENGOTO970
5 IFZ5<=4THENONZ4GOTO900,950,960,970
7 gosub810:gosub900
8 poke 53280,6:poke 53281, 1: print"he:lp"
10 REM ONZ4GOTO900,950,960,970
13 print"{clr}{down}{down}{down}{down}{down}{down}{down}{down}{down}{blk}";tab(8);"{rvon}                       "
20 dimf(5,7),c%(7,9),s%(9,9),w(5),n$(7),t$(5),c1(3),d$(4),p$(5)
95 fori=0to4:readd$(i):next:fori=0to5:readp$(i):next
130 datacruisers,tf-16,tf-17,midway,none,light,heavy,sunk,destr'd,f4f's,zekes
140 REM goto150:gosub200:goto 10
150 rem goto here 150
200 rem gosub here 200
210 poke 444444
215 onggosub810,900
220 gosub 810:ifj=1thenprint" {blu}{rvon}";n$(s%(i,9));
810 print ""
830 IFZ5<=4THENONZ4GOTO900,950,960,970
900 print "line 900"
950 print"line 950"
960 print  "line 960 gosub test"
970 rem 970
980 rem goto:3,140,830,984
982 on f gosub 990,984
984 print:ifa = 1Then 200
985 goto990
990 end


