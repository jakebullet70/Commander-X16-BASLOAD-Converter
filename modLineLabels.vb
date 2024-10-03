Imports Microsoft.Data.Sqlite
Imports System.Data

Module modLineLabels

    Private oDB As New SqliteConnection

    '--- a list of every line that needs a label
    Private targetLblLst As New Collection


    Public Function InsertLineLabels(oDBConnection As SqliteConnection) As Boolean

        oDB = oDBConnection

        Console.WriteLine("Pass 2")
        Dim cmd As SqliteCommand = oDB.CreateCommand()

        '--- grab all lines that need line redirection from #'s to labels
        cmd.CommandText = "SELECT * FROM src_data WHERE 
                        (is_goto=1 OR is_gosub=1 OR is_goto_then=1 OR is_on=1) AND 
                        is_rem = 0 ORDER BY sort_num"

        '--- parse lines and add target line # to list
        Using reader As SqliteDataReader = cmd.ExecuteReader()
            ParseLine4Label(reader)
        End Using


        Return True
    End Function




    Private Sub ParseLine4Label(Reader As SqliteDataReader)
        While Reader.Read()

            Dim pcode As String = Reader("orig_line").ToString()
            Dim orig_line_num As Integer = Reader("orig_line_num")

            Dim multiparts() As String = SplitIgnoringQuotes(pCode, ":")
            For Each pl As String In multiparts

                '--- do ON one's 1st
                If ContainsIgnoreQuotes(pl, "ON") AndAlso ContainsIgnoreQuotes(pl, "GOSUB") Then
                    ParseONline(pl, "GOSUB", orig_line_num)
                    Continue For
                End If
                If ContainsIgnoreQuotes(pl, "ON") AndAlso ContainsIgnoreQuotes(pl, "GOTO") Then
                    ParseONline(pl, "GOTO", orig_line_num)
                    Continue For
                End If

                If ContainsIgnoreQuotes(pl, "GOTO") Then
                    Dim thenpos As Integer = IndexOfIgnoreCase(pl, "GOTO") + 4
                    Dim isline As String = GetLineNumFromStr(pl.Substring(thenpos)).Trim
                    If isline <> "" Then
                        InsertNewLabel(isline, GetSortNum4LineNum(isline))
                    End If
                    Continue For
                End If

                If ContainsIgnoreQuotes(pl, "GOSUB") Then
                    Dim thenpos As Integer = IndexOfIgnoreCase(pl, "GOSUB") + 5
                    Dim isline As String = GetLineNumFromStr(pl.Substring(thenpos)).Trim
                    If isline <> "" Then
                        InsertNewLabel(isline, GetSortNum4LineNum(isline))
                    End If
                    Continue For
                End If

                If ContainsIgnoreQuotes(pl, "IF") AndAlso ContainsIgnoreQuotes(pl, "THEN") AndAlso Not ContainsIgnoreQuotes(pl, "GOSUB") Then
                    '--- IF THEN's with no GOTO key word so normal
                    Dim thenpos As Integer = IndexOfIgnoreCase(pl, "THEN") + 4
                    Dim isline As String = GetLineNumFromStr(pl.Substring(thenpos)).Trim
                    If isline <> "" Then
                        InsertNewLabel(isline, GetSortNum4LineNum(isline))
                    End If
                    Continue For
                End If

            Next


        End While
    End Sub

    Private Sub ParseONline(pCode As String, sType As String, orig_line_num As Integer)

        Dim strNums = pCode.Substring(IndexOfIgnoreCase(pCode, sType) + sType.Length) '--- get just the numbers
        Dim multiparts() As String = strNums.Split(",")
        For Each pl As String In multiparts
            If IsNumeric(pl.Trim) Then
                InsertNewLabel(pl.Trim, GetSortNum4LineNum(pl.Trim))
            End If
        Next

    End Sub

    Private Function GetSortNum4LineNum(line As Integer) As Integer


        Dim cmd1 As SqliteCommand = oDB.CreateCommand()
        cmd1.CommandText = "Select sort_num FROM src_data WHERE orig_line_num = " & line
        Using reader As SqliteDataReader = cmd1.ExecuteReader()
            While reader.Read()
                Return reader(0)
            End While
        End Using

    End Function

    Private Sub InsertNewLabel(num As Integer, sort_num As Integer)

        Try
            '--- will error out if already added
            targetLblLst.Add(num, num)

            '--- never get here if line label has already been added
            Dim insCmd As SqliteCommand = oDB.CreateCommand()
            insCmd.CommandText = baseSQL

            Dim ll As String = "LINE" & num & ":"
            insCmd.Parameters.AddWithValue("@sort_num", sort_num - 5)
            insCmd.Parameters.AddWithValue("@orig_line", ll)
            insCmd.Parameters.AddWithValue("@orig_line_num", sort_num - 5)

            insCmd.Parameters.AddWithValue("@is_goto", 0)
            insCmd.Parameters.AddWithValue("@is_gosub", 0)
            insCmd.Parameters.AddWithValue("@is_goto_then", 0)
            insCmd.Parameters.AddWithValue("@is_on", 0)
            insCmd.Parameters.AddWithValue("@is_rem", 0)
            Dim rowsAffected As Integer = insCmd.ExecuteNonQuery()
            Dim a = 0

        Catch ex As Exception
            If ex.Message.StartsWith("Add failed. Duplicate") Then
                Console.WriteLine("Dupe label: " & num)
            Else
                Throw
            End If
            'Console.WriteLine("Dupe label: " & num)
        End Try


    End Sub

End Module
