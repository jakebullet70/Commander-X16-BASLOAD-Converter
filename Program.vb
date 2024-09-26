Imports System

Module Program
    Sub Main(args As String())

        Console.WriteLine("Commander X16 BASIC Source converter")
        Dim o As New convert

        Dim tmp(1) As String
        tmp(0) = "--petcat"
        tmp(1) = AppContext.BaseDirectory & "Test src code\test.bas"


        If o.Start(tmp) = False Then
            'If o.Start(args) = False Then
            If o.pERROR_CODE = -11 Then
                Console.WriteLine("error line:" & o.pLineNum)
            End If
            o.ShowHelp()
        Else
            Console.WriteLine("All done!")
        End If

        Console.WriteLine("")
        Console.ReadKey()
        End

    End Sub
End Module
