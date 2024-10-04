Module Program
    Sub Main(args As String())

        Console.WriteLine("Commander X16 BASIC Source converter")

        Dim o1 As New convert
        pFileIn = AppContext.BaseDirectory & "Test src code\test.bas"
        pFLAG_petcat = False
        pFLAG_linefeedUseLF = False
        pFLAG_createLstFile = False
        pFLAG_uCaseKeyWord = True
        o1.start()

        Console.WriteLine("") : Console.WriteLine("All done!") : Console.WriteLine("")
        Console.ReadKey()
        End

    End Sub
End Module
