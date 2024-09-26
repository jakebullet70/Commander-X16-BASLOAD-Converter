    2 rem midway campaign
    3 poke 53280,6:poke 53281,1
   10 print"{clr}{down}{down}{down}{down}{down}{down}{down}{down}{down}{blk}";tab(8);"{rvon}                       "
   12 print tab(8);"{rvon} ** midway campaign ** ":pi=~/180
   14 print tab(8);"{rvon}                       ":print
   20 dimf(5,7),c%(7,9),s%(9,9),w(5),n$(7),t$(5),c1(3),d$(4),p$(5)
   22 printtab(11);"copyright 1980 by"
   25 print:printtab(14);"avalon hill":printtab(10);"microcomputer games":print
   26 print"{up}{up}{up}{up}{up}{up}{up}{up}{up}"
   30 v0%=0:v1%=0:t%=720:d%=3:m$="jc67*":fori=0to5:forj=2to7:readf(i,j):next:next
   60 data0,1,0,25,.1,.02,0,1,0,18,.2,.01,0,1,0,25,.1,.01,0,3,0,25,.1,.06,0,4
   70 data0,25,.1,.04,2,5,0,0,.25,.04,270,90,525,230,60,560,230,60,560,25,20,380
   80 fori=0to5:readj,k,l:gosub10000:next:fori=0to7:forj=0to3:readc%(i,j):next
   85 forj=4to8:c%(i,j)=0:next:next:fori=0to5:readw(i):next:fori=0to9:s%(i,9)=-1
   90 next:s6=.041:s7=.043:fori=0to7:readn$(i):next:fori=0to5:readt$(i):next
   95 fori=0to4:readd$(i):next:fori=0to5:readp$(i):next
  100 data25,20,380,0,0,0,0,21,21,21,0,30,23,30,0,21,21,21,0,21,21,21,3,27,38
  110 data14,3,27,35,15,4,25,37,13,5,14,14,10,1.5,1.4,1.3,1.3,1.2,1,akagi,kaga
  120 datasoryu,hiryu,enterprise,hornet,yorktown,midway,carriers,transports
  130 datacruisers,tf-16,tf-17,midway,none,light,heavy,sunk,destr'd,f4f's,zekes
  140 datasbd's,vals,tbd's,kates
  150 sd=54272:for i = sd to sd+24:poke i,0:next:poke sd+24,15
  190 deffns(x)=-(c%(x,8)>0)-(c%(x,8)>=60)-(c%(x,8)>=100):print"{down}{down}{down}{down}{down}{down}{down}{down}{down}"
  200 print"{down}{down} {blk}":print" O{CBM-Y}{CBM-Y}{CBM-Y}{CBM-Y}{CBM-Y}{CBM-Y}{CBM-Y}{CBM-Y}{CBM-Y}{CBM-Y}{CBM-Y}{CBM-Y}{CBM-Y}{CBM-Y}{CBM-Y}{CBM-Y}{CBM-Y}{CBM-Y}{CBM-Y}{CBM-Y}{CBM-Y}{CBM-Y}{CBM-Y}{CBM-Y}{CBM-Y}{CBM-Y}{CBM-Y}{CBM-Y}{CBM-Y}{CBM-Y}{CBM-Y}{CBM-Y}{CBM-Y}{CBM-Y}{CBM-Y}{CBM-Y}P"
  201 print" {CBM-H}      {rvon}    strategic   map    {rvof}       {CBM-N}"
  202 print" {CBM-H}                                    {CBM-N}"
  203 print" {CBM-H}"tab(7);:gosub10500
  204 print" {CBM-H}"tab(19)"{red}tf-16 c.";:c=f(3,4):gosub 10600:print"t{blk}       {CBM-N}"
  205 print" {CBM-H}"tab(19)"{red}tf-17 c.";:c=f(4,4):gosub 10600:print"t{blk}       {CBM-N}"
  210 fori=11to0step-1:a$="{rvon}{rvof}{red}{blk} {CBM-H}      . . . . . . . . . . . .       {CBM-N}"
  213 if i=7 then a$=" {CBM-H}{red}{rvon}jap.{rvof}{blk}  . . . . . . . . . . . . {blu}{rvon}u.s.{rvof}{blk}  {CBM-N}"
  214 if i=5 then a$="{rvon}{rvof} {CBM-H}{red}akagi{blk} . . . . . . . . . . . . {blu}midway{blk}{CBM-N}"
  215 if i=4 then a$="{rvon}{rvof} {CBM-H}{red}kaga{blk}  . . . . . . . . . . . . {blu}hornet{blk}{CBM-N}"
  216 if i=3 then a$="{rvon}{rvof} {CBM-H}{red}hiryu{blk} . . . . . . . . . . . . {blu}enter-{blk}{CBM-N}"
  217 if i=2 then a$="{rvon}{rvof} {CBM-H}{red}soryu{blk} . . . . . . . . . . . .  {blu}prise{blk}{CBM-N}"
  218 if i=1 then a$="{rvon}{rvof}{red}{blk} {CBM-H}      . . . . . . . . . . . . {blu}york-{blk} {CBM-N}"
  219 if i=0 then a$="{rvon}{rvof}{red}{blk} {CBM-H}      . . . . . . . . . . . .  {blu}town{blk} {CBM-N}"
  220 forj=0to5:ifint(f(j,1)/100)<>ior(f(j,2)=0andj<3)then270
  235 k=int(f(j,0)/100)+1:if k<1 or k>12 then270
  243 k=2*k+11:ifmid$(a$,k,1)="."then260
  246 k=k+1:ifmid$(a$,k,1)=" "then260
  250 k=k-2
  260 a$=left$(a$,k-1)+mid$(m$,f(j,3),1)+mid$(a$,k+1)
  270 next:print a$:next
  280 print" L{CBM-P}{CBM-P}{CBM-P}{CBM-P}{CBM-P}{CBM-P}{CBM-P}{CBM-P}{CBM-P}{CBM-P}{CBM-P}{CBM-P}{CBM-P}{CBM-P}{CBM-P}{CBM-P}{CBM-P}{CBM-P}{CBM-P}{CBM-P}{CBM-P}{CBM-P}{CBM-P}{CBM-P}{CBM-P}{CBM-P}{CBM-P}{CBM-P}{CBM-P}{CBM-P}{CBM-P}{CBM-P}{CBM-P}{CBM-P}{CBM-P}{CBM-P}{SHIFT-@}":gosub 10100
  300 a$="":input"{down}{blk}{rvon}fleet command ";a$
  301 if (a$ > "/" and a$ < ":") or (a$ > "@" and a$ < "z") then 304
  302 goto 340
  304 a$=left$(a$,1):x=asc(a$)
  305 if x<48 or x>84 then 340
  310 ifx>47andx<58then1210
  315 ifa$="n"then1200
  320 ifa$="t"then400
  325 ifa$="s"then500
  330 ifa$="m"then200
  335 ifa$="a"then600
  340 print" {blu}legal fleet commands are:":print" {blk}{rvon}t{rvof}{blu} to change tf forces"
  350 print" {blk}{rvon}s{rvof}{blu} to print status report":print" {blk}{rvon}m{rvof}{blu} to redraw map"
  360 print" {blk}{rvon}a{rvof}{blu} to enter aircraft command"
  370 print" use an {blk}{rvon}n{rvof}{blu} or {blk}{rvon}0{rvof}{blu} to execute only one":print" tactical turn."
  380 print" any number to proceed for that amount   of time.{blk}":goto 300
  400 input" which task force ";i:i=-3*(i=16)-4*(i=17):ifi=0then300
  410 input" new course ";j:ifj<0orj>360then300
  420 f(i,4)=j*pi:print" {blu}";t$(i);" on course ";:c=f(i,4)
  430 gosub10600:print"t{blk}":goto300
  500 print"{down}{blu}";tab(10);:gosub10550:print:printtab(10);"{blk}****** status ******{blu}"
  510 print"{down} {rvon}cv            on deck  below   damage"
  520 printtab(11);"cap vf vb vt vf vb vt":print
  530 fori=4to7:print" ";n$(i);tab(12);:c=c%(i,7):gosub10700:c=c%(i,4):gosub10700
  540 c=c%(i,5)+1000*(c%(i,5)>999):gosub10700:c=c%(i,6):gosub10700:forj=1to3
  550 c=c%(i,j):gosub10700:next:j=fns(i):ifi=7andj=3thenj=4
  560 printd$(j):iff(c%(i,0),2)=2andi<>7andfns(i)<3thenprint"  spotted";
  580 print:next:printtab(10);"{rvon}contacts{rvof}":gosub10100:ifl=0thenprint" {red}none"
  590 goto300
  600 a$=" ":input" {blk}carrier ";a$:i=asc(a$)
  610 i=-(4*(i=69)+5*(i=72)+6*(i=89)+7*(i=77)):ifi=0then300
  611 iffns(i)<2then620
  612 print " {blu}";n$(i);:iffns(i)=3thenprint" {red}destroyed.{blu}"
  613 iffns(i)=2thenprint" cannot operate aircraft."
  614 goto600
  620 input" aircraft command ";a$:a$=a$+" ":a$=left$(a$,2):ifa$="ca"then900
  630 ifa$="cl"then800
  633 a$=left$(a$,1):ifa$="a"then700
  640 ifa$="l"then1000
  650 print" {blu}legal aircraft commands are"
  660 print" {blk}a{blu}  to arm a strike":print" {blk}l{blu}  to launch a strike"
  665 print" {blk}ca{blu} to set cap"
  670 print" {blk}cl{blu} to clear decks{blk}":goto300
  700 ifc%(i,4)+c%(i,5)+c%(i,6)=0then730
  710 print" {blu}strike already arming.":goto600
  730 print" number of f4f's, sbd's, tbd's to spot":input" ";j,k,l:j=-j*(j>0)
  735 k=-k*(k>0):l=-l*(l>0):ifj>c%(i,1)thenj=c%(i,1)
  740 ifk>c%(i,2)thenk=c%(i,2)
  750 ifl>c%(i,3)thenl=c%(i,3)
  753 ifj+k+l=0then600
  755 c%(i,1)=c%(i,1)-j:c%(i,4)=j:c%(i,2)=c%(i,2)-k:c%(i,5)=k+1000:c%(i,6)=l
  760 c%(i,3)=c%(i,3)-l:print " {blu}";n$(i);" strike arming.":goto600
  800 gosub10200:print" {blu}";n$(i);" decks clear.":goto600
  900 c%(i,1)=c%(i,1)+c%(i,7):c%(i,7)=0:input" number of f4f's for cap ";j
  910 j=-j*(j>0):ifj>c%(i,1)then930
  920 c%(i,7)=j:c%(i,1)=c%(i,1)-j:goto960
  930 c%(i,7)=c%(i,1):c%(i,1)=0:j=j-c%(i,7)
  940 c%(i,7)=c%(i,7)-j*(j<c%(i,4))-c%(i,4)*(j>=c%(i,4))
  950 c%(i,4)=(j-c%(i,4))*(c%(i,4)>j)
  960 print " {blu}";n$(i);" has";c%(i,7);"f4f's up for cap.":goto600
 1000 ifc%(i,5)+c%(i,6)>0andc%(i,5)<1000then1020
 1010 print" {blu}";n$(i);" has no strike armed.":goto600
 1020 gosub10100:ifl>0then1030
 1025 print" {red}no targets.":goto600
 1030 input" {blk}contact number ";j:ifj>lorj<1then600
 1035 j=c1(int(j)):m=j:n=c%(i,0):gosub10800:ifr<=200then1050
 1040 print" {blu}range";-int(-r);"nm, out of range.":goto600
 1050 l=.3*r:ifi=7or(t%+l+l>=240andt%+l+l<=1140)then1070
 1070 ift%+l>=240andt%+l<1140then1090
 1080 print" {blu}night attacks not possible.":goto600
 1090 m=-1:fork=9to0step-1:ifs%(k,9)<0thenm=k
 1095 next:ifm>=0then1110
 1100 print" {blu}too many strikes aloft.":goto600
 1110 s%(m,0)=c%(i,4):s%(m,2)=c%(i,5):s%(m,4)=c%(i,6):c%(i,4)=0:c%(i,5)=0
 1120 c%(i,6)=0:s%(m,6)=j:s%(m,9)=i:s%(m,7)=t%+l:s%(m,8)=t%+l+l
 1130 s%(m,3)=1:s%(m,5)=0:s%(m,1)=-((s%(m,2)/(s%(m,2)+s%(m,4)))>rnd(0))
 1140 print" {blu}";n$(i);" strike launched.":goto600
 1200 a$="0"
 1210 t0%=t%+int(val(a$)*60):d0%=d%-(t0%>1440):t0%=t0%+1440*(d0%>d%):fori=4to7
 1220 c%(i,5)=c%(i,5)+1000*(c%(i,5)>999):next
 1300 f9%=0:form=1to2:n=5:gosub10800:ifr<15thenf(m,5)=0
 1310 ifj9%>0thenf(m,5)=25+7*(m=2):f(m,4)=270*pi
 1320 next:m=0:gosub10800:ifr<=250then1330
 1325 x=850-f(0,0):y=450-f(0,1):gosub10900:f(0,4)=a
 1330 ifr>100then1350
 1340 f(0,4)=f(0,4)+180*pi+360*pi*(180*pi>f(0,4))
 1350 fork=6to4step-1:iff(c%(k,0),2)=0orc%(k,8)=100then1360
 1355 x=f(c%(k,0),0)-f(0,0):y=f(c%(k,0),1)-f(0,1):gosub10900:f(0,4)=a
 1360 next:ifj9%>0thenf(0,4)=270*pi
 1400 fori=0to3:ifc%(i,7)=5orfns(i)>1then1440
 1405 c%(i,7)=c%(i,7)+c%(i,1):c%(i,1)=0:ifc%(i,7)<5then1420
 1415 c%(i,1)=c%(i,7)-5:c%(i,7)=5:goto1440
 1420 c%(i,7)=c%(i,7)+c%(i,4):c%(i,4)=0:ifc%(i,7)<=5then1440
 1430 c%(i,4)=c%(i,7)-5:c%(i,7)=5
 1440 next
 1500 s9%=0:a9%=0:a8%=0:ift%>1140then1700
 1505 s8%=0:fori=0to3:ifc%(i,4)+c%(i,5)+c%(i,6)>0thens8%=1
 1510 next:ifs8%=0then1590
 1520 i=3
 1523 i=i+1:iffns(i)>1then1525
 1524 k=c%(i,0):gosub11000:ifk>0then1530
 1525 ifi<7then1523
 1527 i=0
 1530 ifi>0then1550
 1531 i=3
 1533 i=i+1:iffns(i)>2then1540
 1535 k=c%(i,0):gosub11000:ifk>0then1542
 1540 ifi<7then1533
 1541 i=0
 1542 ifi>0then1550
 1545 k=5:gosub11000:i=7*k
 1550 s9%=c%(i,0):ifs9%<5then1590
 1555 i=-1
 1560 i=i+1:ifs%(i,6)<5ors%(i,9)=-1ors%(i,1)=-1then1580
 1570 s9%=0:goto1590
 1580 ifi<9then1560
 1590 iff(3,2)>0orf(4,2)>0thena9%=1
 1595 m=0:n=5:gosub10800:ifr>235then1620
 1600 l=60*r/235:ift%+l<240ort%+l+l>1140then1620
 1610 a8%=1:ifc%(3,2)<12thena9%=1
 1620 ifa9%>0thena8%=0
 1700 ifs9%<3then1770
 1705 j=-1
 1710 j=j+1:ifs%(j,9)=-1then1730
 1720 ifj<9then1710
 1725 goto1770
 1730 s%(j,6)=s9%:s%(j,9)=0:m=0:n=s9%:gosub10800:l=60*r/235:s%(j,7)=t%+l
 1740 s%(j,8)=t%+l+l:s%(j,0)=0:s%(j,2)=0:s%(j,4)=0:fori=0to3:iffns(i)>1then1767
 1760 s%(j,0)=s%(j,0)+c%(i,4):c%(i,4)=0:s%(j,2)=s%(j,2)+c%(i,5):c%(i,5)=0
 1765 s%(j,4)=s%(j,4)+c%(i,6):c%(i,6)=0
 1767 next:ifs%(j,2)+s%(j,4)=0thens%(j,9)=-1
 1768 s%(j,3)=1:s%(j,5)=0
 1769 ifs%(j,9)<>-1thens%(j,1)=-((s%(j,2)/(s%(j,2)+s%(j,4)))>rnd(0))
 1770 fori=0to3:gosub10200:iffns(i)>1then1830
 1775 ifa9%=0then1790
 1776 c%(i,4)=c%(i,1):c%(i,1)=0:c%(i,5)=c%(i,2):c%(i,2)=0:c%(i,6)=c%(i,3)
 1778 c%(i,3)=0:goto1820
 1790 ifa8%=0then1820
 1795 c%(i,4)=int(c%(i,1)/2):c%(i,5)=int(c%(i,2)/2):c%(i,6)=int(c%(i,3)/2)
 1800 c%(i,1)=c%(i,1)-c%(i,4):c%(i,2)=c%(i,2)-c%(i,5):c%(i,3)=c%(i,3)-c%(i,6)
 1820 ifs9%+a8%+a9%>0then1830
 1825 c%(i,7)=c%(i,7)+c%(i,1):c%(i,1)=0
 1830 next
 1900 t1%=30+int(30*rnd(0)):t%=t%+t1%:ift%>=t0%andd%=d0%thenf9%=1
 1910 d%=d%-(t%>1440):t%=t%+1440*(t%>1440):ift%>=t0%andd%=d0%thenf9%=1
 2000 fori=0to4:f(i,0)=f(i,0)+t1%*f(i,5)*sin(f(i,4))/60
 2010 f(i,1)=f(i,1)+t1%*f(i,5)*cos(f(i,4))/60:next
 2100 ift%>1140ort%<240then2220
 2105 p%=1-2*(t%<300or(t%>720andt%<780))
 2110 fori=0to2:iff(i,2)=2then2160
 2113 iff(i,5)=0thenf(i,2)=2
 2118 iff(i,2)=1andrnd(0)>3*s7then2160
 2120 ifrnd(0)>p%*s7andf(i,2)=0then2160
 2125 f(i,2)=f(i,2)-(f(i,2)<2):ifrnd(0)>3*s7then2140
 2135 f(i,2)=f(i,2)-(f(i,2)<2)
 2140 gosub 8000:print" {blu}pby spots japanese ";:iff(i,2)=1thenprint"ships";
 2150 iff(i,2)<>1thenprintt$(i);
 2155 print".":f9%=1
 2160 next:iff(0,2)=2thenf(0,3)=2
 2170 p%=1-(t%>720andt%<780):fori=3to4:iff(i,2)=2then2210
 2180 ifrnd(1)<p%*s6thenf(i,2)=1
 2185 iff(i,2)=0orrnd(0)>3*s6then2210
 2190 gosub8000:print" {blu}japanese scouts sighted over ";t$(i);".":f(i,2)=2:f9%=1
 2210 next:goto2300
 2220 fori=0to4:f(i,2)=0:next:f(0,3)=1
 2300 f7%=1:fori=0to9:if(s%(i,9)=-1)or(s%(i,7)>t%)or(s%(i,1)=-1)then4000
 2310 j=2:ifs%(i,6)<3thenj=1
 2311 fork=0to4step2:ifs%(i,k)=0thens%(i,k+1)=-1
 2316 ifrnd(0)>.2then2330
 2320 ifs%(i,1)=s%(i,k+1)thens%(i,1)=-1
 2321 s%(i,k+1)=-1
 2330 next:ifj=2then2360
 2331 fork=0to4step2:ifs%(i,k)=0ors%(i,k+1)>-1then2350
 2340 print" {blu}"n$(s%(i,9));" ";p$(j+k-1);" miss target.":form=1to100:next:f9%=1
 2350 next
 2360 ifs%(i,3)+s%(i,5)=-2ors%(i,2)+s%(i,4)=0then4000
 2365 f(c%(s%(i,9),0),2)=2:f(s%(i,6),2)=2
 2370 iff(0,2)=2thenf(0,3)=2
 2380 gosub 8100:ifj=1thenprint" {blu}{rvon}";n$(s%(i,9));
 2381 ifj=2thenprint" {red}{rvon}japanese";
 2390 print" strike attacking ";t$(s%(i,6));"!{blu}":m=3:gosub11100:f9%=1
 2401 k=-1
 2405 k=k+1:ifs%(i,6)=c%(k,0)andc%(k,8)<100then2600
 2410 ifk<7then2405
 2420 print" {blk}on the way in,{blu}":gosub10300:fork=2to4step2
 2430 ifs%(i,k)=0ors%(i,k+1)=-1then2520
 2440 print" {blu}";s%(i,k);" ";p$(j+k-1);" attack ";t$(s%(i,6));"!":m=1:gosub11100
 2450 e=f(s%(i,6),6)*(1+.25*(k=4)*(1-(j=1))):h%=0:n%=0
 2460 forl=1tos%(i,k):r=rnd(0):ifr<ethenh%=h%+1
 2470 if(k=2ors%(i,6)=5)and(r<2*eandr>=e)thenn%=n%+1
 2471 next
 2480 print" "p$(j+k-1);" make";h%;"hits";:ifk=2ors%(i,6)=5then2495
 2491 print".":m=1:gosub11100:goto2500
 2495 print:print"             and";n%;"near misses.":m=1:gosub11100
 2500 v%=(16-8*(k=4ands%(i,6)<>5))*h%+3*n%
 2505 print"{blk}";v%;"victory points awarded.{blu}":m=3:gosub11100
 2510 v0%=v0%-v%*(j=1):v1%=v1%-v%*(j=2):ifs%(i,6)<>2then2520
 2520 next:print" {blk}on the way out,{blu}":gosub10300:goto3950
 2600 c%=0:fork=0to7:ifc%(k,0)<>s%(i,6)then2620
 2610 c%=c%+c%(k,7):c%(k,7)=0
 2620 next:ifc%=0then2790
 2625 k=2-2*(rnd(0)>.5):ifs%(i,k+1)=-1ors%(i,k)=0thenk=2-2*(k=2)
 2631 ifk=4thenf7%=0
 2640 ifs%(i,k+1)=-1ors%(i,k)=0then2781
 2650 print" cap attacks ";p$(j+k-1);".":m=1:gosub11100
 2660 l1%=-s%(i,0)*(s%(i,1)=s%(i,k+1))
 2670 ifl1%>0thenprint" "p$(j-1);" defend ";p$(j+k-1);"."
 2671 m=1:gosub11100
 2680 e=(c%*w(j-1))/(l1%*w(-(j=1))+s%(i,k)*w(k-(j=1)))
 2690 e=-e*(e<.85)-.85*(e>=.85):h%=0:forl=1tos%(i,k):ifrnd(0)<ethenh%=h%+1
 2700 next:print" cap shoots down";h%;p$(j+k-1);".":m=1:gosub11100
 2710 s%(i,k)=s%(i,k)-h%:ifl1%=0then2781
 2715 print" "p$(j-1);" attack cap.":m=1:gosub11100
 2720 e=(l1%*w(-(j=1)))/(c%*w(j-1)):e=-e*(e<.85)-.85*(e>=.85):h%=0
 2730 forl=1toc%:ifrnd(0)<ethenh%=h%+1
 2740 next:print" "p$(j-1);" shoot down";h%;p$(-(j=1));".":m=1:gosub11100
 2750 c%=c%-h%:ifc%=0then2790
 2755 e=.5*(c%*w(j-1))/(l1%*w(-(j=1))):e=-e*(e<.85)-.85*(e>=.85):h%=0
 2760 forl=1tol1%:ifrnd(0)<ethenh%=h%+1
 2765 next:print" cap shoot down";h%;p$(j-1);".":m=1:gosub11100
 2780 s%(i,0)=s%(i,0)-h%
 2781 ifc%=0then2790
 2782 l=-1:fork=1toc%
 2783 l=-(l+1)*(l<7):ifs%(i,6)<>c%(l,0)orc%(l,8)>=60then2783
 2784 c%(l,7)=c%(l,7)+1:next
 2785 if(s%(i,3)=-1ors%(i,2)=0)and(s%(i,5)=-1ors%(i,4)=0)then3950
 2790 print" {blk}on the way in,{blu}":gosub10300
 2800 fork=4to2step-2:ifs%(i,k)=0ors%(i,k+1)=-1then2940
 2810 m%=0:forl=0to7:c%(l,9)=0:ifc%(l,8)<100andc%(l,0)=s%(i,6)thenm%=m%+1
 2823 next:ifm%=0then2940
 2825 o%=-1:forn=1tom%
 2827 o%=-(o%+1)*(o%<7):ifc%(o%,0)<>s%(i,6)orc%(o%,8)=100then2827
 2830 c%(o%,9)=int((s%(i,k)+m%-n)/m%):next
 2840 forl=0to7:ifc%(l,9)=0then2930
 2850 print " "c%(l,9);" ";p$(j+k-1);" attack ";n$(l);".":m=1:gosub11100
 2860 n%=0:h%=0:e=.2-(k=4)*.06*(j=1):l1%=0:gosub 8200:form=1toc%(l,9):r=rnd(0)
 2870 ifr>=ethen2880
 2875 print" {red}{rvon}hit !{rvof}{blu}";:h%=h%+1:l1%=l1%+1:goto2890
 2880 ifr>=2*eor(k=4andl<>7)then2890
 2883 print" {red}near miss{blu}";:l1%=l1%+2:n%=n%+1
 2890 ifl1%>5thenprint
 2892 l1%=-l1%*(l1%<6):next:ifl1%>0thenprint
 2895 print" "n$(l);" takes";h%;"hits";:ifn%=0then2903
 2900 print:print"              and";n%;"near misses";
 2903 print".":m=3:gosub11100
 2905 ifh%+n%=0then2930
 2906 d7%=0:ifh%+n%>0andc%(l,4)+c%(l,5)+c%(l,6)>0and(k=2orl=7)thend7%=1
 2907 ifd7%>0thenprint" {red}secondary explosions on ";n$(l);"!{blu}"
 2910 d8=16*(1+d7%-.5*(k=4andl<>7)):form=1toh%:gosub10400:next
 2920 d8=3*(1+2*d7%):form=1ton%:gosub10400:next
 2930 next
 2940 next:print" {blk}on the way out,{blu}":gosub10300
 3950 fork=0to4step2:s%(i,k+1)=-1:next
 4000 next
 4100 forl=0to7:ifc%(l,8)<10orc%(l,8)=100then4150
 4110 ifrnd(0)>.05*(1-(l<4))then4130
 4120 print" {red}explosion on ";n$(l);"!{blu}":m=1:gosub11100:d8=16:gosub10400
 4130 ifc%(l,8)=100orrnd(0)>.2*(1-(l>3))then4150
 4140 c%(l,8)=c%(l,8)-5*rnd(0):c%(l,8)=-c%(l,8)*(c%(l,8)>0)
 4150 next
 5000 v2%=0:forj=0to9:ifs%(j,9)=-1then5220
 5005 v2%=1:ift%<s%(j,8)then5220
 5008 ifs%(j,9)<4then5120
 5010 f9%=1:i=s%(j,9):iffns(i)>1then5050
 5020 print" {gry1}{rvon}strike landing on ";n$(i);".{blu}":m=1:gosub11100:gosub10200
 5030 c%(i,1)=c%(i,1)+s%(j,0):c%(i,2)=c%(i,2)+s%(j,2)
 5040 c%(i,3)=c%(i,3)+s%(j,4):goto5210
 5050 ifi>5or(c%(4,8)>60andc%(5,8)>60)then5060
 5055 k=4-(i=4):goto5110
 5060 k=3
 5061 k=k+1:ifc%(k,8)>60then5070
 5065 m=c%(i,0):n=c%(k,0):gosub10800:ifr<100then5100
 5070 ifk<7then5061
 5080 print" {gry1}{rvon}"n$(i);" strike splashes.{blu}":m=1:gosub11100:goto5210
 5100 ifrnd(0)>.8then5080
 5110 print" {gry1}{rvon}"n$(i);" strike diverted to ";n$(k)"{blu}":m=1:gosub11100
 5115 i=k:goto5020
 5120 l=0:fori=0to3:iffns(i)<2thenl=l+1
 5125 next:ifl=0then5210
 5130 fori=0to3:gosub10200:next:fork=0to4step2:m%=-1:fori=1tol
 5140 m%=-(m%+1)*(m%<3):iffns(m%)>1then5140
 5150 c%(m%,1+k/2)=c%(m%,1+k/2)+int((l+s%(j,k)-i)/l)
 5170 next:next:fori=0to3
 5180 ifc%(i,1)+c%(i,2)+c%(i,3)<96then5200
 5185 c%(i,1)=c%(i,1)+(c%(i,1)>0):c%(i,2)=c%(i,2)+(c%(i,2)>0)
 5186 c%(i,3)=c%(i,3)+(c%(i,3)>0):goto5180
 5200 next
 5210 s%(j,9)=-1
 5220 next
 6000 i=-1
 6001 i=i+1:iffns(i)<2then6010
 6005 ifi<3then6001
 6007 j9%=1
 6010 ifv2%=1then6050
 6015 ifj9%=1andf(0,0)<0then7000
 6020 i=-1
 6021 i=i+1:iffns(i)<3then6030
 6025 ifi<3then6021
 6026 goto7000
 6030 iff(3,0)>1200orf(4,0)>1200then7000
 6035 i=3
 6036 i=i+1:iffns(i)<3then6050
 6040 ifi<7then6036
 6045 goto7000
 6050 iff9%>0then for ps = 1 to 2000:next:goto 200
 6060 goto1300
 7000 gosub 8100:print"{clr}{down}{down}{down}{down}{down}     {blk}{rvon} the game is over {rvof}":print
 7008 printtab(5)"report:":v2%=0:v3%=0:p%=0
 7010 print"  cv";tab(13);"damage status"
 7020 fori=0to3:print"  "n$(i);tab(14);d$(fns(i))
 7030 forj=1to7:p%=p%+c%(i,j):next:s%=fns(i)
 7040 v2%=v2%-100*(s%=1)-300*(s%=2)-1000*(s%>2):next
 7050 p1%=p%:p%=0:fori=4to7:print"  "n$(i);tab(14);d$(fns(i)-(i=7andfns(i)=3))
 7060 forj=1to7:p%=p%+c%(i,j):next:s%=fns(i)
 7070 v3%=v3%-100*(s%=1)-300*(s%=2)-1000*(s%>2):next
 7080 p1%=272-p1%:print"{down}  japanese lost";p1%;"planes.":v2%=v2%+5*p1%
 7090 p%=269-p%:print"  u.s. lost";p%;"planes.":v3%=v3%+5*p%
 7100 ifv0%>0thenprint"  u.s. had";v0%;"points in other hits."
 7110 ifv1%>0thenprint"  japanese had";v1%;"points in other hits."
 7120 print"{down}  "t$(5);" has";:ifj9%>0thenprint" not";
 7130 print" fallen.":ifj9%=0thenv3%=v3%+1000
 7140 v0%=v0%+v2%:v1%=v1%+v3%:v%=v0%-v1%:ifv%<0thenprint"{down}  japanese";
 7150 ifv%>=0thenprint"{down}  us";
 7160 v%=abs(v%):a$="marginal":ifv%>=1000thena$="tactical"
 7170 ifv%>=2000thena$="strategic"
 7180 print" ";a$;" victory.":input"{down}{down}  go again (y/n)";a$:ifa$="y"thenrun
 7190 print"{clr}{down}{down}{down}{down}{down}{down}{down}{down}{down}{down}{down}   thank you for your time"
 7195 print"   playing {rvon}midway campaign{rvof}{down}{down}{down}{down}{down}":end
 8000 rem sound
 8010 for pl=1 to 2:for sp = 15 to 10 step -.3
 8020 poke sd+24,sp:poke sd+5,10:poke sd+4,17:poke sd+1,44:poke sd,10
 8030 poke sd+4,16:for ps = 1 to 40 :next ps,sp,pl:return
 8100 rem sound
 8110 poke sd+24,15:poke sd+5,15:poke sd+6,128:poke sd+4,33
 8120 for pl = 1 to 10
 8130 poke sd+1,37:poke sd,6:for ps = 1 to 150:next
 8140 poke sd+1,28:poke sd,16:for ps = 1 to 110:next
 8150 next:poke sd+4,0:return
 8200 rem sound
 8210 forpl=1to40
 8220 poke sd+4,129:poke sd+5,15:poke sd+1,100:pokesd,40:poke sd+4,0
 8221 for ps= 1to20:next
 8225 poke sd+4,129:poke sd+1,44:poke sd,10:next
 8230 poke sd+4,0:return
10000 l=l+175*rnd(0)-200*rnd(0)*(i<3):j=(j+k*rnd(0))*pi
10010 f(i,0)=850-l*sin(j)*(i<>5):f(i,1)=450-l*cos(j)*(i<>5)
10013 iff(i,0)>1199thenf(i,0)=1199
10014 iff(i,1)>1199thenf(i,1)=1199
10020 j=j+180*pi+360*pi*(j>180*pi):f(i,4)=j:ifi>2thenf(i,4)=205*pi
10030 return
10100 l=0:fork=0to2:iff(k,2)=0then10140
10110 l=l+1:print" {blu}contact";l;"at position";int(f(k,0)/100)+1;
10120 printint(f(k,1)/100)+1;:iff(k,2)=2thenprint t$(k);
10130 print:c1(l)=k
10140 next:return
10200 c%(i,5)=c%(i,5)+1000*(c%(i,5)>999):c%(i,1)=c%(i,1)+c%(i,4):c%(i,4)=0
10210 c%(i,2)=c%(i,2)+c%(i,5):c%(i,5)=0:c%(i,3)=c%(i,3)+c%(i,6):c%(i,6)=0
10220 return
10300 fork=0to4step2:ifs%(i,k)=0ors%(i,k+1)=-1then10340
10310 e=f(s%(i,6),7)*(-.4*(k=0)-.7*(k=2)-(k=4)):h%=0
10320 forl=1tos%(i,k):ifrnd(0)<ethenh%=h%+1
10330 next:print" "t$(s%(i,6));" aa shoots down";h%;p$(j+k-1);"."
10335 form=1to200:next:s%(i,k)=s%(i,k)-h%
10340 next:return
10400 ifc%(l,8)>=100thenreturn
10410 d9=rnd(0)*d8:ifl=7thend9=d9/3
10414 ifnot(k=2orl=7orrnd(0)<.5)then10431
10415 forl1=1to(6-(t%<240ort%>1140)):ifc%(l,l1)=0then10430
10420 forl2=1toc%(l,l1):ifrnd(0)<d9/100thenc%(l,l1)=c%(l,l1)-1
10429 next
10430 next
10431 c%(l,8)=c%(l,8)+d9:ifc%(l,8)<60thenreturn
10432 l1=i:i=l:gosub10200:i=l1:c%(l,1)=c%(l,1)+c%(l,7):c%(l,7)=0
10433 ifc%(l,8)<100thenreturn
10435 c%(l,8)=100:print" {red}";n$(l);:ifl=7thenprint" airbase is destroyed."
10450 print" blows up and sinks.{blu}"
10460 forl1=1to300:next:forl1=1to6:c%(l,l1)=0:next:return
10500 a$=str$(100+int(t%/60))+str$(100+t%-60*int(t%/60))
10510 print"  ";mid$(a$,3,2);":";mid$(a$,7,2);" ";str$(d%);
10515 print" june 1942           {CBM-N}"
10520 if mid$(a$,3,2)> "03" and mid$(a$,3,2)<"19"then poke 53281,1:return
10530 poke 53281,15:return
10550 a$=str$(100+int(t%/60))+str$(100+t%-60*int(t%/60))
10560 print"{down} ";mid$(a$,3,2);":";mid$(a$,7,2);" ";str$(d%);" june 1942"
10570 if mid$(a$,3,2)> "03" and mid$(a$,3,2)<"19"then poke 53281,1:return
10580 poke 53281,15:return
10600 a$=str$(int(c/pi+1000.5)):printmid$(a$,3,3);:return
10700 a$=str$(c):printmid$(a$,2+(c<10),2);" ";:return
10800 x=f(m,0)-f(n,0):y=f(m,1)-f(n,1):r=sqr(x*x+y*y):return
10900 a=(90-180*(x<0))*pi:ify=0thenreturn
10910 a=atn(x/y):ify<0then10920
10915 a=a-360*pi*(x<0):return
10920 a=a+180*pi:return
11000 m=0:n=k:gosub10800:iff(k,2)=0orr>235thenk=0
11010 l=r*60/235:ift%+l<240ort%+l+l>1140thenk=0
11020 k=-(k>0):return
11100 form1=1tom:form2=1to1000:next:next:return
