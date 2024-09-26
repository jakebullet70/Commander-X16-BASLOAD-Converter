Imports System.IO

Public Class convert

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

        If pFLAG_createLstFile Then
            CreateInfoFile()
        End If

        If ReadFile() = False Then Return False
        If GetAllGotosGosubs() = False Then Return False

        '--- now we shoud have all needed info to remap
        If RemapLineNums() = False Then Return False

        If pFLAG_petcat Then
            ProcessPETSCII()
        End If




        ' Write2File()

        '--- FIND POKES and put in lst file 


        If pFLAG_createLstFile Then UpdateInfoFile()

        Return True

    End Function

    Private Function RemapLineNums() As Boolean

        Console.WriteLine("Remaping lines numbers to labels")

        For Each gline As String In _gosubLst

            Dim oo As SrcInfo = _sourceCodeColl.Item(gline)
            pCode = oo.item.ToUpper.Trim

            Dim multilines() As String = pCode.Split(":")
            For Each partOfLine As String In multilines

                If Not partOfLine.Contains("GOSUB") Then Continue For

                RemapLineNumsGOs(partOfLine.Substring(partOfLine.IndexOf("GOSUB") + 5).Trim)

            Next
        Next

        For Each gline As String In _gotoLst

            Dim oo As SrcInfo = _sourceCodeColl.Item(gline)
            pCode = oo.item.ToUpper.Trim

            Dim multilines() As String = pCode.Split(":")
            For Each partOfLine As String In multilines

                Select Case True
                    Case Not partOfLine.Contains("GOTO") AndAlso
                             partOfLine.Contains("THEN") AndAlso
                             partOfLine.Contains("IF")
                        RemapLineNumsGOs(partOfLine.Substring(partOfLine.IndexOf("THEN") + 4).Trim)

                    Case partOfLine.Contains("GOTO")
                        RemapLineNumsGOs(partOfLine.Substring(partOfLine.IndexOf("GOTO") + 4).Trim)

                End Select

            Next
        Next

        WriteOutFile()

        Try
        Catch ex As Exception
            Console.WriteLine("Program error - remapping lines")
            Console.WriteLine(ex.ToString)
            pERROR_CODE = -15
            If Debugger.IsAttached Then Stop
            Return False
        End Try

        Console.WriteLine("")
        Return True


    End Function

    Private Sub RemapLineNumsGOs(lineOfCode As String)

        '--- will detect and process ON GOTO / GOSUB
        Dim multiGos() As String = lineOfCode.Split(",")
        For Each lineNum As String In multiGos

            If Not IsNumeric(lineNum) Then Continue For

            If lineNum <> "" AndAlso _sourceCodeColl.Contains(lineNum) Then
                '--- insert the new label
                Dim labelName = "LINE" & lineNum & ":"
                InsertIntoColection(lineNum, labelName)
            End If

        Next

    End Sub

    Private Sub WriteOutFile()

        Dim fout = Path.ChangeExtension(_FileIn, "bl")
        If IO.File.Exists(fout) Then
            IO.File.Delete(fout)
        End If


        Dim outStr As New List(Of String)
        For Each lines As SrcInfo In _sourceCodeColl

            If pFLAG_linefeedUseLF Then
                outStr.Add(lines.item.ReplaceLineEndings(vbLf))
            Else
                outStr.Add(lines.item.ReplaceLineEndings(vbCrLf))
            End If

        Next

        IO.File.WriteAllLines(fout, outStr)


    End Sub

    Private Sub InsertIntoColection(BeforeThisKey As String, lineLBL As String)
        Try
            Dim oo As New SrcInfo
            oo.item = lineLBL
            oo.key = lineLBL
            _sourceCodeColl.Add(oo, lineLBL, BeforeThisKey)
        Catch ex As Exception
            '--- LineLabel is already added so all is OK
        End Try
    End Sub


    Private Sub UpdateColection(AfterThisKey As String, lineRec As SrcInfo)
        'Try
        '    Dim oo As New SrcInfo
        '    oo.item = lineLBL
        '    oo.key = lineLBL
        '    _sourceCodeColl.Add(oo, lineLBL, BeforeThisKey)
        'Catch ex As Exception
        '    '--- LineLabel is already added so all is OK
        'End Try
    End Sub

    Private Function GetAllGotosGosubs() As Boolean

        Console.WriteLine("Finding GOTO / GOSUBS")
        Dim ttl_found As Integer = 0
        Try

            '--- 2nd pass
            For x = 1 To _sourceCodeColl.Count

                Dim o As SrcInfo = _sourceCodeColl(x)
                pLineNum = o.key
                pCode = o.item

                '--- get GOTO's aand GOSUBS
                '--- add to list
                Dim tmp As String = pCode.ToUpper.Trim

                Dim multilines() As String = tmp.Split(":")
                For Each partOfLine As String In multilines

                    If partOfLine.StartsWith("REM") Then Continue For

                    If partOfLine.Contains("GOSUB") Then
                        If _gosubLst.Contains(pLineNum) = False Then
                            _gosubLst.Add(pLineNum)
                            ttl_found += 1
                        End If
                    End If
                    If CheckGoto(partOfLine) Then
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

    Private Function CheckGoto(tmp As String) As Boolean

        If String.IsNullOrEmpty(tmp) Then Return False

        '--- tmp will be UCASE
        '--- need to  also check for THEN'S ==> 'THEN 6755' or THEN3455 syntax

        If tmp.Contains("GOTO") Then
            Return True
        End If

        '--- chec for valid missing goto (if q = 1 then 776)
        If tmp.Contains("IF") AndAlso tmp.Contains("THEN") Then

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

    Private Function ReadFile() As Boolean

        Try

            '--- read file into collection
            Console.WriteLine("Reading file: " & _FileIn)
            Dim fileIn() = IO.File.ReadAllLines(_FileIn)
            Console.WriteLine("Total lines: " & fileIn.Length)

            '--- split line # from code
            For Each s As String In fileIn
                pLineNum = "" : pCode = ""
                s = s.Trim.ReplaceLineEndings()
                If String.IsNullOrEmpty(s) Then
                    pLineNum = _blankLineCounterKey.ToString
                    _blankLineCounterKey = _blankLineCounterKey + 1
                    '--- do nothing
                ElseIf IsNumeric(s.Substring(0, 1)) Then
                    '--- its a line #
                    pLineNum = GetLineNumFromStr(s)
                    pCode = s.Substring(pLineNum.Length).Trim
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


    Private Sub ProcessPETSCII()
        Throw New NotImplementedException()
    End Sub





    'Private Function FormatCleanupLines() As Boolean

    '    Dim tmp As String = code.ToUpper.Trim
    '    If tmp.StartsWith("DATA") AndAlso Not tmp.StartsWith("DATA ") Then
    '        '-- replace 'data' with 'data '
    '    End If
    '    If tmp.StartsWith("PRINT") AndAlso Not tmp.StartsWith("PRINT ") Then
    '        '-- replace 'print' with 'print '
    '    End If

    'End Function




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

    Private Class SrcInfo
        Public item As String
        Public key As String
    End Class


End Class
