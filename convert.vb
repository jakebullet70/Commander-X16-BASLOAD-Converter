Imports System.IO
Imports System.Text.RegularExpressions
Imports Microsoft.Data.Sqlite
Imports Microsoft.VisualBasic

Public Class convert

    Private oDB As New SqliteConnection '= SqliteConnection("")

    Public pERROR_CODE As Integer = 0
    REM error codes:
    REM -1  = file not found
    REM -2  = no arguments passed
    REM -3  = error parseing arguments
    REM -5  = problem reading/parsing file
    REM -10 = GOTO-GOSUB reader
    REM -11 = error parser line numbers
    REM -15 = ERROR REMAPPING LINES

    Public pFLAG_linefeedUseLF As Boolean = False
    Public pFLAG_createLstFile As Boolean = True
    Public pFLAG_petcat As Boolean = False
    Public pFLAG_ucaseKeyWord As Boolean = False

    Private _FileIn As String = ""
    'Private _FileOut As String = ""
    Private _LFtype As String = Environment.NewLine

    Private _gotoLst As New List(Of String)
    Private _gosubLst As New List(Of String)
    'Private _lines2beCleanedUpLst As New List(Of String)
    Private _sourceCodeColl As New Collection

    '-- if a blank line is detected this line num will be inserted as the collection key
    Private _blankLineCounterKey As Integer = 9900000

    Public pLineNum, pCode As String ' curent line being worked on

    Public Sub ShowHelp()
        Console.WriteLine("------------------------------------------------------------")
        Console.WriteLine("      --- Command line help ---")
        Console.WriteLine("Example: ")
        Console.WriteLine("X16basloadConverter --lf ""c:\filepath\basfile.bas""")
        Console.WriteLine("--lf        ===>  Convert all line ending to Linux style")
        Console.WriteLine("--noinfo    ===>  Do not create an INFO file")
        Console.WriteLine("--petcat    ===>  assume input file converted with VICE petcat")
        Console.WriteLine("-------------------------------------------------------------")
    End Sub

    Public Function Start(arg() As String)

        oDB.ConnectionString = "Data Source=conversion.db3"
        oDB.Open()

        If arg.Length = 0 Then
            pERROR_CODE = -2
            Return False
        End If

        ParseArgs(arg)

        If Not IO.File.Exists(_FileIn) Then
            Console.WriteLine("")
            Console.WriteLine("Input file not found.")
            pERROR_CODE = -1
            Return False
        End If

        Return Process()

    End Function

    Private Function Process() As Boolean

        Console.WriteLine("Start...")

        pFLAG_ucaseKeyWord = True

        If pFLAG_createLstFile Then
            CreateInfoFile()
        End If

        If ReadFile() = False Then Return False
        If GetAllGotosGosubs() = False Then Return False

        '--- now we shoud have all needed info to remap
        InsertNewLineLabels()

        'If RemapLineNums() = False Then Return False

        If CleanUpAndWriteOutFile() = False Then Return False

        '--- FIND POKES, PEEKS, DEF FN, etc and put in lst file 

        If pFLAG_createLstFile Then UpdateInfoFile()

        Return True

    End Function


    Private Function CleanUpAndWriteOutFile() As Boolean

        Const ppADD_PREFIX_SPACE As Boolean = True
        Dim outStr As New List(Of String)

        For x = 1 To _sourceCodeColl.Count

            Dim isLabel As Boolean = False
            Dim isRemark As Boolean = False

            Dim o As SrcInfo = _sourceCodeColl(x)
            pLineNum = o.key
            pCode = o.item

            '--- fix LF vs CRLF
            If pFLAG_linefeedUseLF Then
                pCode = pCode.ReplaceLineEndings(vbLf)
            Else
                pCode = pCode.ReplaceLineEndings(vbCrLf)
            End If

            Dim pcodeOut = ""

            Dim multilines() As String = SplitIgnoringQuotes(pCode, ":")
            For Each pl As String In multilines

                If pl.StartsWith("LINE") AndAlso pl.Length > 5 AndAlso IsNumeric(pl.Substring(4, 1)) Then
                    '--- line label, bail
                    isLabel = True : pcodeOut = pl & ":"
                    Exit For
                End If

                If pl.StartsWith("REM", StringComparison.CurrentCultureIgnoreCase) Then
                    '--- remark statement, bail out
                    isRemark = True : pcodeOut = pl
                    Exit For
                End If

                AddWithSpaceIfNeededIsStart(pl, "FOR")
                If pl.StartsWith("FOR", StringComparison.CurrentCultureIgnoreCase) Then
                    AddWithSpaceIfNeeded(pl, "TO", ppADD_PREFIX_SPACE)
                End If

                AddWithSpaceIfNeeded(pl, "NEXT", ppADD_PREFIX_SPACE)
                AddWithSpaceIfNeededIsStart(pl, "DATA")
                AddWithSpaceIfNeededIsStart(pl, "DIM")

                If pl.Contains("ON", StringComparison.CurrentCultureIgnoreCase) AndAlso
                        (pl.Contains("GOTO", StringComparison.CurrentCultureIgnoreCase) OrElse
                        pl.Contains("GOSUB", StringComparison.CurrentCultureIgnoreCase)) Then
                    pl = ReplaceFirstOccurrence(pl, "ON", "ON ")
                End If

                AddWithSpaceIfNeeded(pl, "GOSUB", ppADD_PREFIX_SPACE, pFLAG_ucaseKeyWord)
                AddWithSpaceIfNeeded(pl, "GOTO", ppADD_PREFIX_SPACE, pFLAG_ucaseKeyWord)

                AddWithSpaceIfNeeded(pl, "IF")
                AddWithSpaceIfNeeded(pl, "THEN", ppADD_PREFIX_SPACE)
                AddWithSpaceIfNeeded(pl, "PRINT")
                AddWithSpaceIfNeeded(pl, "READ")


                'If Not pl.Contains(Chr(34)) Then
                '    ' if no string const ==> " "
                '    pl = pl.Replace("  ", " ").Trim '--- any 2 space's to 1
                'End If
                'pl.IndexOf()

                If Not isLabel AndAlso Not isRemark Then
                    pcodeOut &= pl.Trim & ": "
                End If

            Next



            If Not isLabel AndAlso Not isRemark Then
                For Each l In _gotoLst
                    If pcodeOut.Contains(l) Then
                        UpdateGotoWithLabels(pcodeOut, l)
                    End If
                Next
                outStr.Add(pcodeOut.Trim.TrimEnd(":"))
            Else
                outStr.Add(pcodeOut.Trim)
            End If

        Next

        WriteOutFile(outStr)

        Return True

    End Function

    Sub UpdateGotoWithLabels(ByRef pl As String, lineNum As String)

        Dim lbl = "LINE" & lineNum
        Select Case pl
            Case pl.Contains("GOTO", StringComparison.CurrentCultureIgnoreCase)
            Case pl.Contains("THEN", StringComparison.CurrentCultureIgnoreCase)
        End Select

    End Sub


    Sub InsertNewLineLabels()

        Console.WriteLine("Creating new line labels...")
        Dim refactoredLine As String

        '--- GOSUBS, inset new LineLabel if needed
        For Each gline As String In _gosubLst

            refactoredLine = ""
            Dim oo As SrcInfo = _sourceCodeColl.Item(gline)
            pCode = oo.item.ToUpper.Trim

            If pCode.StartsWith("REM", StringComparison.CurrentCultureIgnoreCase) Then
                Continue For
            End If

            Dim multilines() As String = SplitIgnoringQuotes(pCode, ":")
            For Each pl As String In multilines

                If Not pl.Contains("GOSUB") Then Continue For
                Dim nums = pl.Substring(pl.IndexOf("GOSUB") + 5).Trim
                InsertNewLineLabelIntoColl(nums)

                'refactoredLine &= pl.Replace(nums, " LINE" & nums) & ": "
            Next

            'refactoredLine = refactoredLine.Trim.TrimEnd(":")


        Next

        '--- GOTO'S, inset new LineLabel if needed
        For Each gline As String In _gotoLst

            Dim oo As SrcInfo = _sourceCodeColl.Item(gline)
            pCode = oo.item.ToUpper.Trim

            If pCode.StartsWith("REM", StringComparison.CurrentCultureIgnoreCase) Then
                Continue For
            End If

            Dim multilines() As String = SplitIgnoringQuotes(pCode, ":")
            For Each pl As String In multilines

                Select Case True
                    Case Not pl.Contains("GOTO") AndAlso
                             pl.Contains("THEN") AndAlso
                             pl.Contains("IF")
                        InsertNewLineLabelIntoColl(pl.Substring(pl.IndexOf("THEN") + 4).Trim)

                    Case pl.Contains("GOTO")
                        InsertNewLineLabelIntoColl(pl.Substring(pl.IndexOf("GOTO") + 4).Trim)

                End Select

            Next
        Next

    End Sub
    Private Sub InsertNewLineLabelIntoColl(LineNum2Find As String)

        '--- will detect and process ON GOTO / GOSUB
        Dim multiGos() As String = LineNum2Find.Split(",")
        For Each ln As String In multiGos

            If Not IsNumeric(ln) Then Continue For

            If ln <> "" AndAlso _sourceCodeColl.Contains(ln) Then

                '--- insert the new label
                InsertIntoColection(ln, "LINE" & ln & ":")

            End If

        Next

    End Sub
    Private Sub InsertIntoColection(BeforeThisKey As String, newline As String)
        Try
            Dim oo As New SrcInfo
            oo.item = newline
            oo.key = newline
            _sourceCodeColl.Add(oo, newline, BeforeThisKey)
        Catch ex As Exception
            '--- LineLabel is already added so all is OK
        End Try
    End Sub


    Private Function GetAllGotosGosubs() As Boolean

        Console.WriteLine("Finding GOTO / GOSUBS")
        Dim ttl_found As Integer = 0
        Try

            For x = 1 To _sourceCodeColl.Count

                Dim o As SrcInfo = _sourceCodeColl(x)
                pLineNum = o.key
                pCode = o.item

                '--- get GOTO's aand GOSUBS
                '--- add to list
                Dim tmp As String = pCode.ToUpper.Trim

                Dim multilines() As String = SplitIgnoringQuotes(tmp, ":")
                For Each partOfLine As String In multilines

                    If partOfLine.StartsWith("REM") Then Continue For

                    If partOfLine.Contains("GOSUB") Then
                        If _gosubLst.Contains(pLineNum) = False Then
                            _gosubLst.Add(pLineNum)
                            ttl_found += 1
                        End If
                    End If
                    If Check4Goto(partOfLine) Then
                        If _gotoLst.Contains(pLineNum) = False Then
                            _gotoLst.Add(pLineNum)
                            ttl_found += 1
                        End If
                    End If
                Next

            Next


        Catch ex As Exception
            Console.WriteLine("Program error - GOTO-GOSUB reader")
            Console.WriteLine(ex.ToString)
            pERROR_CODE = -10
            If Debugger.IsAttached Then Stop
            Return False
        End Try

        Console.WriteLine("GOTO / GOSUBS found: " & ttl_found)
        Return True
    End Function

    Private Function Check4Goto(tmp As String) As Boolean

        If String.IsNullOrEmpty(tmp) Then Return False

        '--- tmp will be UCASE
        '--- need to  also check for THEN'S ==> 'THEN 6755' or THEN3455 syntax

        If tmp.Contains("GOTO") Then
            Return True
        End If

        '--- chec for valid missing goto (if q = 1 then 776)
        If tmp.Contains("IF") AndAlso tmp.Contains("THEN") AndAlso
            (Not tmp.Contains("GOSUB")) Then

            Dim thenpos As Integer = tmp.IndexOf("THEN") + 4
            Dim isline As String = GetLineNumFromStr(tmp.Substring(thenpos)).Trim
            If isline <> "" AndAlso _sourceCodeColl.Contains(isline) Then
                Return True
            End If

        End If

        Return False

    End Function

    Private Function GetLineNumFromStr(s As String) As String
        Try

            s = s.TrimStart

            Dim ret As String = ""
            Dim spart As String = ""
            For x = 0 To s.Length - 1
                spart = s.Substring(x, 1)
                If IsNumeric(spart) Then
                    ret &= spart
                Else
                    Exit For
                End If
            Next
            Return ret

        Catch ex As Exception
            Console.WriteLine("Program error - parsing line numbers")
            pERROR_CODE = -11
            Console.WriteLine(ex.ToString)
            If Debugger.IsAttached Then Stop
        End Try

    End Function

    Private Sub WriteOutFile(outLstStr)

        Dim fout = Path.ChangeExtension(_FileIn, "bl")
        If IO.File.Exists(fout) Then
            IO.File.Delete(fout)
        End If

        IO.File.WriteAllLines(fout, outLstStr)

    End Sub


    Private Function ReadFile() As Boolean

        Try

            '--- read file into collection
            Console.WriteLine("Reading file: " & _FileIn)
            Dim fileIn() = IO.File.ReadAllLines(_FileIn)
            Console.WriteLine("Total lines: " & fileIn.Length)

            '--- split line # from code
            For Each s As String In fileIn
                pLineNum = "" : pCode = ""
                '--- make line ending all the same just in case
                s = s.Trim.ReplaceLineEndings()

                If String.IsNullOrEmpty(s) Then
                    pLineNum = _blankLineCounterKey.ToString
                    _blankLineCounterKey = _blankLineCounterKey + 1
                    '--- do nothing
                ElseIf IsNumeric(s.Substring(0, 1)) Then
                    '--- its a line #
                    pLineNum = GetLineNumFromStr(s)
                    pCode = s.Substring(pLineNum.Length).Trim
                    CleanUpSyntex(pCode)
                Else
                    pLineNum = _blankLineCounterKey.ToString
                    pCode = s.Trim
                    _blankLineCounterKey = _blankLineCounterKey + 1
                End If

                Dim oo As New SrcInfo
                oo.item = pCode : oo.key = pLineNum
                _sourceCodeColl.Add(oo, pLineNum)

            Next

        Catch ex As Exception
            Console.WriteLine("Program error - reading file")
            Console.WriteLine(ex.ToString)
            pERROR_CODE = -5
            If Debugger.IsAttached Then Stop
            Return False
        End Try

        Return True

    End Function

    Private Sub CleanUpSyntex(ByRef pl As String)

        If pl.Contains("IF", StringComparison.CurrentCultureIgnoreCase) AndAlso
                pl.Contains("THEN", StringComparison.CurrentCultureIgnoreCase) AndAlso
                pl.Contains("GOTO", StringComparison.CurrentCultureIgnoreCase) Then

            If Not pl.Contains("THENON", StringComparison.CurrentCultureIgnoreCase) AndAlso
                    Not pl.Contains("THEN ON", StringComparison.CurrentCultureIgnoreCase) Then

                pl = ReplaceIgnoreQuotes(pl, "THENGOTO", "THEN ")
                pl = ReplaceIgnoreQuotes(pl, "THEN GOTO", "THEN ")
                pl = ReplaceIgnoreQuotes(pl, "THEN  GOTO", "THEN ")
            End If
        End If

    End Sub

    Private Sub ProcessPETSCII()
        Throw New NotImplementedException()
    End Sub



    '=============================================================================================================================

    Private Sub CreateInfoFile()
        '--- backup and stuff!   do we need this?
        Dim infofile = _FileIn & ".INFO"
        If Path.Exists(infofile) Then
            Console.WriteLine("Deleting old info file: " & infofile)
            IO.File.Delete(infofile)
        End If
        Console.WriteLine("Making info file - step 1: " & infofile)
        FileCopy(_FileIn, infofile)
    End Sub
    Private Sub UpdateInfoFile()
        ' read in INFO file and add info
        ' lines processed / found GOTO and all info to the top
        ' re write file out
        Dim infofile = _FileIn & ".INFO"
        Console.WriteLine("Updating info file - final: " & infofile)
    End Sub


    Private Function ParseArgs(arg() As String) As Boolean
        For Each s As String In arg
            If s.StartsWith("--") Then

                s = s.ToUpper.Replace("--", "")
                Select Case s
                    Case "H", "HELP", "?"
                        ShowHelp()
                        Return False
                    Case "LF", "LINUX_LF"
                        pFLAG_linefeedUseLF = True
                    Case "NOINFO"
                        pFLAG_createLstFile = True
                    Case "PETCAT"
                        pFLAG_petcat = True
                    Case Else
                        ShowHelp()
                        pERROR_CODE = -3
                        Return False
                End Select
            End If

            _FileIn = s
        Next
    End Function

    Function ReplaceFirstOccurrence(input As String, search As String, replacement As String) As String
        ' Create a Regex object with IgnoreCase option
        Dim regex As New Regex(regex.Escape(search), RegexOptions.IgnoreCase)

        ' Replace only the first occurrence
        Return regex.Replace(input, replacement, 1)
    End Function




    Private Class SrcInfo
        Public item As String
        Public key As String
    End Class


End Class
