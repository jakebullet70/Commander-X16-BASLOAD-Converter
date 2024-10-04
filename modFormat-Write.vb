Imports System.CodeDom.Compiler
Imports Microsoft.Data.Sqlite

Module modFormat_Write
    Private OutStr As New List(Of String)
    Private Indented = ""

    Enum SpacesType
        BEGINNING
        [END]
        BOTH
    End Enum

    Private arrKeyWordsMain() = {"FOR", "IF", "THEN", "DIM", "PRINT", "CONT",
        "READ", "END", "ON", "GOSUB", "GOTO", "NEXT", "THEN", "AND", "OR",
        "DEF FN", "DEFFN", "COLOR", "RUN", "INPUT", "STEP", "RESTORE", "RETURN",
        "STOP", "SYS", "WAIT", "CMD", "CLS", "INPUT"}

    Private arrKeyWordsFunct() = {"ASC", "CHR", "ABS", "ATN", "FRE", "COS", "EXP",
        "HEX", "INT(", "LEFT", "RIGHT", "MID", "RND", "SQR", "TAB", "TAN",
        "SPC", "SGN", "USR", "VAL"}

    'Private arrKeyWordsSpec1() = {"<=", ">=", "=", "<>", ">", "<"}

    '--- special stuff "INPUT

    Function CleanUpAndWriteOut() As Boolean

        If pFLAG_tabOut4 Then Indented = "    "
        If pFLAG_tabOut3 Then Indented = "   "

        Console.WriteLine("Pass 3")
        Dim cmd As SqliteCommand = oDB.CreateCommand()

        '--- grab all lines that need line redirection from #'s to labels
        cmd.CommandText = "SELECT * FROM src_data ORDER BY sort_num"

        '--- parse lines and add target line # to list
        Using reader As SqliteDataReader = cmd.ExecuteReader()
            FormatAndWrite(reader)
        End Using

        Console.WriteLine("Writing BASLOAD file")
        WriteOutFile(OutStr)

        Return True

    End Function

    Private Sub FormatAndWrite(Reader As SqliteDataReader)


        While Reader.Read()

            Dim pcode = Reader("orig_line").ToString
            If String.IsNullOrEmpty(pcode) Then Continue While

            If pFLAG_linefeedUseLF Then
                pcode = pcode.ReplaceLineEndings(vbLf)
            Else
                pcode = pcode.ReplaceLineEndings(vbCrLf)
            End If

            '--- 1 liners
            If pcode.StartsWith("LINE") AndAlso pcode.EndsWith(":"c) Then
                outStr.Add(pcode) : Continue While
            End If
            If pcode.StartsWith("REM", StringComparison.CurrentCultureIgnoreCase) Then
                OutStr.Add(Indented & pcode) : Continue While
            End If
            If pcode.StartsWith("DATA", StringComparison.CurrentCultureIgnoreCase) Then
                AddSpacesIfNeeded(pcode, "DATA", SpacesType.END)
                OutStr.Add(Indented & pcode) : Continue While
            End If

            '--- now lets break apart the line with ':'
            Dim lineNum As Integer = 1
            Dim fullLine As String = ""
            Dim multilines() As String = SplitIgnoringQuotes(pcode, ":")
            For Each pl As String In multilines

                For Each kw In arrKeyWordsMain
                    AddSpacesIfNeeded(pl, kw, SpacesType.BOTH)
                Next

                For Each kw In arrKeyWordsFunct
                    AddSpacesIfNeeded(pl, kw, SpacesType.BEGINNING)
                Next


                '--- special case fubar
                If ContainsIgnoreQuotesIgnoreCase(pl, " f or ") Then
                    '--- 'for' and 'or' get confused
                    AddSpacesIfNeeded(pl, " f or ", SpacesType.BOTH, "for")
                    AddSpacesIfNeeded(pl, "TO", SpacesType.BOTH)
                End If

                If ContainsIgnoreQuotesIgnoreCase(pl, "rest or e") Then
                    '--- 'restore' gets confused
                    AddSpacesIfNeeded(pl, "rest or e", SpacesType.BOTH, "restore")
                End If
                If ContainsIgnoreQuotesIgnoreCase(pl, "input #") Then
                    '--- 'input vs input#' gets confused
                    AddSpacesIfNeeded(pl, "input#", SpacesType.BOTH, "restore")
                End If
                'For Each kw In arrKeyWordsSpec1
                '    '--- {"<=", ">=", "="} ==> clean up
                '    If ContainsIgnoreQuotesIngnoreCase(pl, kw) Then
                '        AddSpacesIfNeeded(pl, kw, SpacesType.BOTH)
                '        Exit For
                '    End If
                'Next
                '--- end special case fubar


                If pFLAG_petcat Then ProcessPET(pl)

                '--- rebuld the line 
                fullLine &= pl & ":"

            Next

            Dim cleanLine = fullLine.Trim.TrimEnd(":")
            OutStr.Add(Indented & cleanLine)
            lineNum += 1


        End While

    End Sub

    Private Sub ProcessPET(ByRef fullLine As String)
        Console.WriteLine("Processing PET char's (not done)")
    End Sub

    Public Sub AddSpacesIfNeeded(ByRef line As String, ByVal findMe As String,
                                 ByVal AddSpaces As SpacesType, Optional replaceWithMe As String = "")

        '--- uses BYREF to adjust line value

        If replaceWithMe = "" Then replaceWithMe = findMe
        If ContainsIgnoreQuotesIgnoreCase(line, findMe) Then

            '--- strip starting and trailing spaces
            line = ReplaceIgnoreQuotesIgnoreCase(line, " " & findMe, replaceWithMe)
            line = ReplaceIgnoreQuotesIgnoreCase(line, findMe & " ", replaceWithMe)


            Select Case AddSpaces
                Case SpacesType.BOTH
                    line = ReplaceIgnoreQuotesIgnoreCase(line, findMe, " " & IIf(pFLAG_uCaseKeyWord, replaceWithMe.ToUpper, replaceWithMe) & " ")
                Case SpacesType.END
                    line = ReplaceIgnoreQuotesIgnoreCase(line, findMe, IIf(pFLAG_uCaseKeyWord, replaceWithMe.ToUpper, replaceWithMe) & " ")
                Case SpacesType.BEGINNING
                    line = ReplaceIgnoreQuotesIgnoreCase(line, findMe, " " & IIf(pFLAG_uCaseKeyWord, replaceWithMe.ToUpper, replaceWithMe))
            End Select

        End If


    End Sub


End Module
