Imports System.IO
'Imports System.Text.RegularExpressions
Imports Microsoft.Data.Sqlite

Module modSupport

    Public pFLAG_linefeedUseLF As Boolean = False
    Public pFLAG_createLstFile As Boolean = True
    Public pFLAG_petcat As Boolean = False
    Public pFLAG_uCaseKeyWord As Boolean = True
    Public pFLAG_tabOut4 As Boolean = True
    Public pFLAG_tabOut3 As Boolean = False


    Public pERROR_CODE As Integer = 0

    Public pFileIn As String = ""


    Public oDB As New SqliteConnection
    Public baseSQL As String = "
            INSERT INTO 'src_data' 
	          ('sort_num','orig_line','orig_line_num','is_goto','is_gosub',
               'is_goto_then','is_on','is_rem') 
	        VALUES 
              (@sort_num,@orig_line,@orig_line_num,@is_goto,@is_gosub,
               @is_goto_then,@is_on,@is_rem)"

    Public Function UpdateCodeLine(pcode As String, orig_line_num As Integer) As Boolean

        Dim insCmd As SqliteCommand = oDB.CreateCommand()
        insCmd.CommandText = "UPDATE src_data SET orig_line = @code WHERE orig_line_num = @line"

        insCmd.Parameters.AddWithValue("@line", orig_line_num)
        insCmd.Parameters.AddWithValue("@code", pcode)
        Dim rowsAffected As Integer = insCmd.ExecuteNonQuery()

        If rowsAffected = -1 Then Return False
        Return True

    End Function

    Public Function IndexOfIgnoreCase(input As String, target As String) As Integer
        ' Convert both input and target to lowercase for case-insensitive comparison
        Dim inputLower As String = input.ToLower()
        Dim targetLower As String = target.ToLower()

        ' Use the built-in IndexOf on the lowercase strings
        Return inputLower.IndexOf(targetLower)
    End Function

    'Function ReplaceFirstOccurrence(input As String, search As String, replacement As String) As String
    '    '--- Create a Regex object with IgnoreCase option
    '    Dim regex As New Regex(Regex.Escape(search), RegexOptions.IgnoreCase)

    '    '--- Replace only the first occurrence
    '    Return regex.Replace(input, replacement, 1)
    'End Function

    Sub CleanUpSyntex(ByRef pl As String)

        If pl.Contains("IF", StringComparison.CurrentCultureIgnoreCase) AndAlso
                pl.Contains("THEN", StringComparison.CurrentCultureIgnoreCase) AndAlso
                pl.Contains("GOTO", StringComparison.CurrentCultureIgnoreCase) Then

            If Not pl.Contains("THENON", StringComparison.CurrentCultureIgnoreCase) AndAlso
                    Not pl.Contains("THEN ON", StringComparison.CurrentCultureIgnoreCase) Then

                pl = ReplaceIgnoreQuotesIgnoreCase(pl, "THENGOTO", "THEN ")
                pl = ReplaceIgnoreQuotesIgnoreCase(pl, "THEN GOTO", "THEN ")
                pl = ReplaceIgnoreQuotesIgnoreCase(pl, "THEN  GOTO", "THEN ")
            End If
        End If

    End Sub

    Public Function ContainsIgnoreQuotesIgnoreCase(input As String, target As String) As Boolean
        Dim insideQuotes As Boolean = False
        Dim i As Integer = 0
        Dim targetLower As String = target.ToLower() ' Convert target to lowercase for comparison

        While i < input.Length
            Dim currentChar As Char = input(i)

            ' Check for quotes
            If currentChar = """"c Then
                insideQuotes = Not insideQuotes
                i += 1
                Continue While
            End If

            ' If inside quotes, skip the characters
            If insideQuotes Then
                i += 1
                Continue While
            End If

            ' If not inside quotes, check for the target string (case-insensitive)
            If i <= input.Length - target.Length AndAlso
           input.Substring(i, target.Length).ToLower() = targetLower Then
                Return True ' Return true as soon as the target is found outside quotes
            End If

            i += 1
        End While

        ' If no match was found, return false
        Return False
    End Function


    Function GetLineNumFromStr(ByVal s As String) As String

        s = s.TrimStart

        Dim ret As String = ""
        For x = 0 To s.Length - 1
            Dim spart As String = s.Substring(x, 1)
            If IsNumeric(spart) Then
                ret &= spart
            Else
                Exit For
            End If
        Next
        Return ret

    End Function


    Public Function ReplaceIgnoreQuotesIgnoreCase(input As String, oldValue As String, newValue As String) As String
        Dim result As New System.Text.StringBuilder()
        Dim insideQuotes As Boolean = False
        Dim i As Integer = 0
        Dim oldValueLower As String = oldValue.ToLower() ' Convert oldValue to lowercase for comparison

        While i < input.Length
            Dim currentChar As Char = input(i)

            ' Check for quotes
            If currentChar = """"c Then
                insideQuotes = Not insideQuotes
                result.Append(currentChar)
                i += 1
                Continue While
            End If

            ' If inside quotes, do not replace
            If insideQuotes Then
                result.Append(currentChar)
                i += 1
                Continue While
            End If

            ' If not inside quotes, check for the oldValue (case-insensitive)
            If i <= input.Length - oldValue.Length AndAlso
                    input.Substring(i, oldValue.Length).ToLower() = oldValueLower Then

                result.Append(newValue) ' Append the replacement value
                i += oldValue.Length
            Else
                result.Append(currentChar) ' Append the original character
                i += 1
            End If

        End While

        Return result.ToString()
    End Function



    'Public Sub AddWithSpaceIfNeededIsStart(ByRef line As String, findMe As String, Optional UcaseIt As Boolean = False)

    '    '--- uses BYREF to adjust line value
    '    If line.StartsWith(findMe, StringComparison.CurrentCultureIgnoreCase) Then
    '        AddWithSpaceIfNeeded(line, findMe,, UcaseIt)
    '    End If

    'End Sub

    'Public Sub AddWithSpaceIfNeeded(ByRef line As String, findMe As String,
    '                                Optional addPrefixSpace As Boolean = False, Optional UcaseIt As Boolean = False)

    '    '--- uses BYREF to adjust line value
    '    If line.Contains(findMe, StringComparison.CurrentCultureIgnoreCase) AndAlso
    '       Not line.Contains(findMe & " ", StringComparison.CurrentCultureIgnoreCase) Then
    '        line = line.Replace(findMe,
    '                IIf(addPrefixSpace, " ", "") & findMe & " ",
    '                StringComparison.CurrentCultureIgnoreCase)


    '    End If

    '    If UcaseIt AndAlso findMe.Contains(findMe, StringComparison.CurrentCultureIgnoreCase) AndAlso line.Contains(findMe, StringComparison.CurrentCultureIgnoreCase) Then
    '        line = ReplaceIgnoreQuotes(line, findMe, findMe.ToUpper)
    '    End If


    'End Sub

    Public Function SplitIgnoringQuotes(input As String, delimiter As Char) As String()
        Dim result As New List(Of String)()
        Dim currentSection As New Text.StringBuilder()
        Dim inSingleQuotes As Boolean = False
        Dim inDoubleQuotes As Boolean = False

        For Each ch As Char In input
            If ch = "'"c Then
                inSingleQuotes = Not inSingleQuotes
            ElseIf ch = """"c Then
                inDoubleQuotes = Not inDoubleQuotes
            End If

            If ch = delimiter AndAlso Not inSingleQuotes AndAlso Not inDoubleQuotes Then
                ' If the character is the delimiter and we're not inside quotes, split here
                result.Add(currentSection.ToString().Trim())
                currentSection.Clear()
            Else
                ' Otherwise, keep building the current section
                currentSection.Append(ch)
            End If
        Next

        ' Add the final section
        If currentSection.Length > 0 Then
            result.Add(currentSection.ToString().Trim())
        End If

        Return result.ToArray()
    End Function

    Sub WriteOutFile(outLstStr As List(Of String))

        Dim fout = Path.ChangeExtension(pFileIn, "bl")
        If IO.File.Exists(fout) Then
            IO.File.Delete(fout)
        End If

        IO.File.WriteAllLines(fout, outLstStr)

    End Sub
End Module
