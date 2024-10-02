Imports System.Globalization
Imports System.Reflection.Emit
Imports Microsoft.Data.Sqlite

Public Class convert2

    Private oDB As New SqliteConnection

    Public pERROR_CODE As Integer = 0

    Public pFLAG_linefeedUseLF As Boolean = False
    Public pFLAG_createLstFile As Boolean = True
    Public pFLAG_petcat As Boolean = False
    Public pFLAG_ucaseKeyWord As Boolean = False

    Public pFileIn As String = ""

    Public Sub start()

        'AppContext.BaseDirectory & "Test src code\test.bas"
        oDB.ConnectionString = "Data Source=" & AppContext.BaseDirectory & "conversion.db3"
        oDB.Open()

        Dim ssql As SqliteCommand = oDB.CreateCommand()
        ssql.CommandText = "DELETE FROM 'src_data';"
        ssql.ExecuteNonQuery()

        ReadFileAndParse()

    End Sub

    Private Function ReadFileAndParse() As Boolean

        '--- read file into collection
        Console.WriteLine("Reading file: " & pFileIn)
        Dim fileIn() = IO.File.ReadAllLines(pFileIn)
        Console.WriteLine("Total lines: " & fileIn.Length)

        Dim sortNum As Integer = 1000
        Dim baseSQL As String = "
                INSERT INTO 'src_data' 
	                ('sort_num','orig_line','orig_line_num','is_goto','is_gosub','is_goto_then','is_on','is_rem') 
	             VALUES 
                    (@sort_num,@orig_line,@orig_line_num,@is_goto,@is_gosub,@is_goto_then,@is_on,@is_rem)"



        '--- split line # from code
        For Each s As String In fileIn

            Dim insertCommand As SqliteCommand = oDB.CreateCommand()
            insertCommand.CommandText = baseSQL

            '--- make line ending all the same just in case
            s = s.Trim.ReplaceLineEndings()
            Dim emptyLine As Boolean = False

            If String.IsNullOrEmpty(s) Then
                '--- do nothing
                emptyLine = True

            Else IsNumeric(s.Substring(0, 1))
                '--- its a line #
                Dim pLineNum = GetLineNumFromStr(s)
                Dim pCode = s.Substring(pLineNum.Length).Trim
                CleanUpSyntex(pCode)

                insertCommand.Parameters.AddWithValue("@sort_num", sortNum)
                insertCommand.Parameters.AddWithValue("@orig_line", pCode)
                insertCommand.Parameters.AddWithValue("@orig_line_num", pLineNum)

                insertCommand.Parameters.AddWithValue("@is_goto", IIf(isGoto(pCode), 1, 0))
                insertCommand.Parameters.AddWithValue("@is_gosub", IIf(isGosub(pCode), 1, 0))
                insertCommand.Parameters.AddWithValue("@is_goto_then", IIf(isGotoThen(pCode), 1, 0))
                insertCommand.Parameters.AddWithValue("@is_on", IIf(isOnGotoGosub(pCode), 1, 0))
                insertCommand.Parameters.AddWithValue("@is_rem", IIf(isRemark(pCode), 1, 0))

            End If

            If emptyLine = False Then
                '--- Execute the insert
                Dim rowsAffected As Integer = insertCommand.ExecuteNonQuery()
                sortNum += 10
            End If

        Next

    End Function

    Private Function isRemark(pCode As String) As Boolean
        Return pCode.StartsWith("REM", StringComparison.CurrentCultureIgnoreCase)
    End Function

    Private Function isOnGotoGosub(pCode As String) As Boolean
        Dim multiparts() As String = SplitIgnoringQuotes(pCode, ":")


        Return False
    End Function

    Private Function isGotoThen(pCode As String) As Boolean
        Dim multiparts() As String = SplitIgnoringQuotes(pCode, ":")
        For Each pl As String In multiparts

        Next

        Return False
    End Function

    Private Function isGosub(pCode As String) As Boolean

        Dim multiparts() As String = SplitIgnoringQuotes(pCode, ":")
        For Each pl As String In multiparts
            If pl.Contains("GOSUB", StringComparison.CurrentCultureIgnoreCase) Then
                Return True
            End If
        Next

        Return False
    End Function

    Private Function isGoto(pCode As String) As Boolean

        Dim multiparts() As String = SplitIgnoringQuotes(pCode, ":")
        For Each pl As String In multiparts
            If pl.Contains("GOTO", StringComparison.CurrentCultureIgnoreCase) Then
                Return True
            End If
        Next

        Return False
    End Function

End Class
