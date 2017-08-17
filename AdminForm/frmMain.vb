Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Data.Odbc
Imports System.Math
Imports System.Net


Public Class frmMain
    Dim MysqlConn As MySqlConnection
    Dim BatchFileName As String = ""
    Dim BatchID As String = ""
    Dim flag As String = ""
    Dim BatchExists As String = ""
    Dim Machine_No As String = System.Configuration.ConfigurationSettings.AppSettings("Machine_No").ToString()
    Dim sourcefilepath As String = "" ' // e.g. "d:/test.docx"
    Dim ftpurl As String = System.Configuration.ConfigurationSettings.AppSettings("ftpurl").ToString() ' // e.g. ftp://serverip/foldername/foldername
    Dim ftpusername As String = System.Configuration.ConfigurationSettings.AppSettings("ftpusername").ToString() ' // e.g. username
    Dim ftppassword As String = System.Configuration.ConfigurationSettings.AppSettings("ftppassword").ToString() ' // e.g. password

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            If Me.WindowState = FormWindowState.Minimized Then
                ShowInTaskbar = False
                NotifyIcon.Visible = True
                NotifyIcon.ShowBalloonTip(5000)
                Batch_Files()
                Timer1.Interval = 1000
                Timer1.Start()
            End If
        Catch ex As MySqlException
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub Batch_Files()
        Try
            MysqlConn = New MySqlConnection()
            MysqlConn.ConnectionString = mvarCon
            MysqlConn.Open()

            Dim ds As New DataSet
            Dim dsSum As New DataSet
            Dim dsDist As New DataSet
            Dim Command1 = New MySqlCommand
            Dim firstString As String = ""
            Dim secondString As String = ""
            Dim sw1 As StreamWriter
            Dim querry As String

            MysqlConn = New MySqlConnection
            MysqlConn.ConnectionString = mvarCon
            Dim SDA As New MySqlDataAdapter
            Dim SDA1 As New MySqlDataAdapter

            Try
                MysqlConn.Open()

                querry = ""
                querry = "  SELECT DISTINCT DATE_FORMAT(`dDateEntered`, '%d%m%Y'),DATE_FORMAT(`dDateEntered`, '%Y%m%d') FROM `tsretail_mannequeen`.`tpospayment`  WHERE `iPaymentNo` > (SELECT IFNULL(MAX(`paymentno`),0) FROM `batchmaster`) "
                querry = querry & " UNION"
                querry = querry & " SELECT DATE_FORMAT(NOW(), '%d%m%Y'),DATE_FORMAT(NOW(), '%Y%m%d');"
                Command1 = Nothing
                Command1 = New MySqlCommand(querry, MysqlConn)
                SDA = Nothing
                SDA = New MySqlDataAdapter
                SDA.SelectCommand = Command1
                SDA.Fill(dsDist)
                For j As Integer = 0 To dsDist.Tables(0).Rows.Count - 1
                    ds = Nothing
                    ds = New DataSet

                    querry = ""
                    querry = " SELECT '" & Machine_No & "',IFNULL(DATE_FORMAT(ce.dDateEntered, '%d%m%Y'),'" & dsDist.Tables(0).Rows(j)(0).ToString() & "') as 'DATE','" & BatchID & "',rh.full_format as HOUR,IFNULL(MAX(ce.iPaymentNo),0) as 'iPaymentNo',"
                    querry = querry & " IFNULL(Count(ce.iPaymentNo),0) AS RECEIPT, IFNULL(CAST((SUM(ce.iTotalAmount) - SUM(ce.iDiscount)) AS DECIMAL(12,2)),0) as 'GPO',"
                    querry = querry & "   0.00 as 'GST',IFNULL(CAST((SUM(ce.iDiscount)) AS DECIMAL(12,2)),0) as 'Discount',0 as 'PAX'"
                    querry = querry & "  FROM  report_hours rh LEFT JOIN  tpospayment ce ON HOUR(ce.dDateEntered)=rh.hour AND DATE_FORMAT(ce.dDateEntered, '%d%m%Y') = '" & dsDist.Tables(0).Rows(j)(0).ToString() & "' GROUP BY rh.hour;"
                    Command1 = Nothing
                    Command1 = New MySqlCommand(querry, MysqlConn)
                    SDA = Nothing
                    SDA = New MySqlDataAdapter
                    SDA.SelectCommand = Command1
                    SDA.Fill(ds)

                    Threading.Thread.Sleep(1000)
                    BatchExists = ""
                    Create_File_Name(dsDist.Tables(0).Rows(j)(1).ToString())
                    Create_Batch_ID(dsDist.Tables(0).Rows(j)(0).ToString())

                    Dim slab As String = ""
                    For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                        If BatchExists = "N" Then
                            querry = ""
                            querry = "INSERT INTO `tsretail_mannequeen`.`batchmaster`(`machid`,`batchid`,`paymentdate`,`paymenthour`, `paymentno`) VALUES ('" & Machine_No & "','" & BatchID & "','" & ds.Tables(0).Rows(i)(1).ToString() & "','" & ds.Tables(0).Rows(i)(3).ToString & "','" & ds.Tables(0).Rows(i)(4).ToString & "');"
                            Command1 = Nothing
                            Command1 = New MySqlCommand(querry, MysqlConn)
                            Command1.ExecuteNonQuery()
                        End If

                        sw1 = File.AppendText(Application.StartupPath & "/TempBkup/" & BatchFileName)
                        firstString = ds.Tables(0).Rows(i)(0).ToString & "|" & ds.Tables(0).Rows(i)(1).ToString & "|" & BatchID & "|" & ds.Tables(0).Rows(i)(3).ToString & "|" & ds.Tables(0).Rows(i)(5).ToString & "|" & ds.Tables(0).Rows(i)(6).ToString() & "|"
                        firstString = firstString & ds.Tables(0).Rows(i)(7).ToString & "|" & ds.Tables(0).Rows(i)(8).ToString & "|" & ds.Tables(0).Rows(i)(9).ToString

                        sw1.WriteLine(firstString)
                        sw1.Close()
                    Next
                Next
                MysqlConn.Close()
            Catch ex1 As Exception
                MessageBox.Show(ex1.Message.ToString())
            End Try
            MysqlConn.Close()
            MysqlConn.Dispose()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Create_File_Name(ByVal strFile As String)
        Try
            Dim querry As String
            Dim Command1 = New MySqlCommand
            Dim SDA As New MySqlDataAdapter
            Dim dsFile As New DataSet

            Dim paths() As String = IO.Directory.GetFiles(Application.StartupPath & "/TempBkup/", Machine_No & "_" & strFile & "*.txt")
            If paths.Length > 0 Then 'if at least one file is found do something
                BatchFileName = Path.GetFileName(paths(0))
                System.IO.File.Delete(paths(0))
            Else
                querry = "SELECT DATE_FORMAT(NOW(), '%H%i%s');"
                Command1 = Nothing
                Command1 = New MySqlCommand(querry, MysqlConn)
                SDA.SelectCommand = Command1
                SDA.Fill(dsFile)
                BatchFileName = Machine_No & "_" & strFile & "_" & dsFile.Tables(0).Rows(0)(0).ToString() & ".txt"
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Create_Batch_ID(ByVal pdate As String)
        Try
            Dim Command1 = New MySqlCommand
            Dim SDA As New MySqlDataAdapter
            Dim dsBatch As New DataSet
            Dim querryBatch As String = ""
            BatchID = ""
            querryBatch = "SELECT COUNT(paymentdate) FROM `tsretail_mannequeen`.`batchmaster` WHERE paymentdate = '" & pdate & "';"
            Command1 = Nothing
            Command1 = New MySqlCommand(querryBatch, MysqlConn)
            SDA.SelectCommand = Command1
            dsBatch = Nothing
            dsBatch = New DataSet
            SDA.Fill(dsBatch)

            If Val(dsBatch.Tables(0).Rows(0)(0).ToString()) = 0 Then
                querryBatch = ""
                querryBatch = "SELECT IFNULL(MAX(batchid),0) + 1 FROM batchmaster;"
                Command1 = Nothing
                Command1 = New MySqlCommand(querryBatch, MysqlConn)
                SDA.SelectCommand = Command1
                dsBatch = Nothing
                dsBatch = New DataSet
                SDA.Fill(dsBatch)
                BatchID = dsBatch.Tables(0).Rows(0)(0).ToString()
                BatchExists = "N"
            Else
                querryBatch = ""
                querryBatch = "SELECT batchid FROM batchmaster WHERE paymentdate = '" & pdate & "' ;"
                Command1 = Nothing
                Command1 = New MySqlCommand(querryBatch, MysqlConn)
                SDA.SelectCommand = Command1
                dsBatch = Nothing
                dsBatch = New DataSet
                SDA.Fill(dsBatch)
                BatchID = dsBatch.Tables(0).Rows(0)(0).ToString()
                BatchExists = "Y"
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub FTP_File()
        Try
            Dim Command1 = New MySqlCommand
            Dim SDA As New MySqlDataAdapter
            Dim dsFile As New DataSet

            Dim paths() As String = IO.Directory.GetFiles(Application.StartupPath & "/TempBkup/")
            For i As Integer = 0 To paths.Length - 1
                Try
                    Dim FolderFileName As String = ""
                    FolderFileName = Application.StartupPath & "/TempBkup/" & Path.GetFileName(paths(i))

                    Dim filename As String = Path.GetFileName(FolderFileName)
                    Dim ftpfullpath As String = ftpurl & "/" & Path.GetFileName(paths(i))
                    Dim ftp As FtpWebRequest = DirectCast(FtpWebRequest.Create(ftpfullpath), FtpWebRequest)
                    ftp.Credentials = New NetworkCredential(ftpusername, ftppassword)

                    ftp.KeepAlive = True
                    ftp.UseBinary = True
                    ftp.Method = WebRequestMethods.Ftp.UploadFile

                    Dim fs As FileStream = File.OpenRead(FolderFileName)
                    Dim buffer As Byte() = New Byte(fs.Length - 1) {}
                    fs.Read(buffer, 0, buffer.Length)
                    fs.Close()

                    Dim ftpstream As Stream = ftp.GetRequestStream()
                    ftpstream.Write(buffer, 0, buffer.Length)
                    ftpstream.Close()

                    System.IO.File.Delete(paths(i))
                Catch ex As Exception

                End Try
            Next
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Try
            NotifyIcon.Visible = False
            Me.Close()
        Catch ex As Exception
        End Try
    End Sub
    Private Sub OnApplicationExit(ByVal sender As Object, ByVal e As EventArgs)
        Try
            NotifyIcon.Visible = False
            Me.Close()
        Catch
        End Try
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Try
            Call FTP_File()
            If DateTime.Now.ToString("HH") = "12" AndAlso Convert.ToInt32(DateTime.Now.ToString("mm")) >= 0 Then
                If flag <> "S" Then
                    Timer1.Stop()
                    Batch_Files()
                    flag = "S"
                    Timer1.Start()
                End If
            ElseIf DateTime.Now.Hour > 12 Then
                flag = ""
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
