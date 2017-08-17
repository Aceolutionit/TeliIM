Imports System.Data.Odbc

Public Class clsConnect
    Private com2 As OdbcCommand

    Private m_globalVar As String = ""

    Private MyConString As [String] = "DRIVER={MySQL ODBC 5.3 ANSI Driver};SERVER=127.0.0.1; DATABASE=tsretail_mannequeen; USER=root; PASSWORD=Ace@2017; OPTION=3;"

    'Private MyConString1 As [String] = "server=localhost;uid=root;pwd=ace@2017; database=tsretail_mannequeen;"

    'Private MyConString As [String] = mvarCon

    Private formActive As String

    Public Property setActive() As String
        Get
            Return Me.formActive
        End Get
        Set(value As String)
            Me.formActive = value
        End Set
    End Property

    Private m_theName As String

    Public Property TheName() As String
        Get
            Return Me.m_theName
        End Get
        Set(value As String)
            Me.m_theName = value
        End Set
    End Property

    Public Sub connectdata(ByRef con1 As OdbcConnection)
        Dim con As OdbcConnection = Nothing

        If con Is Nothing Then

            con = New OdbcConnection(MyConString)
        End If

        If con.State <> ConnectionState.Open Then

            con.Open()
        End If
        con1 = con
    End Sub

    Public Sub rsCmd(str As [String], con As OdbcConnection, ByRef com As OdbcCommand)
        'Now we will create a command
        Dim tspn As TimeSpan
        Try
            If con.State <> ConnectionState.Open Then
                con = New OdbcConnection(MyConString)
                con.Open()
            End If
            com2 = New OdbcCommand(str, con)
            com = com2
        Catch e As Exception
            MessageBox.Show("Unable to connect to database server. " & vbLf & " " + "Please contact your administrator if problem persist", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            com = Nothing
        End Try
    End Sub

    Public Sub rsCombo(str As [String], con As OdbcConnection, ByRef com As OdbcCommand)
        Dim com2 As OdbcCommand

        Try
            If con.State <> ConnectionState.Open Then
                con = New OdbcConnection(MyConString)
                con.Open()
            End If

            com2 = New OdbcCommand(str, con)
            com = com2
        Catch e As Exception
            MessageBox.Show("Unable to connect to database server. " & vbLf & " " + "Please contact your administrator if problem persist", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            com = Nothing
        End Try
    End Sub

    Public Sub rsLog(str As [String], con As OdbcConnection, ByRef com As OdbcCommand)
        Dim com2 As OdbcCommand

        Try
            If con.State <> ConnectionState.Open Then
                con = New OdbcConnection(MyConString)
                con.Open()
            End If

            com2 = New OdbcCommand(str, con)
            com = com2
        Catch e As Exception
            MessageBox.Show("Unable to connect to database server. " & vbLf & " " + "Please contact your administrator if problem persist", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            com = Nothing
        End Try
    End Sub

    Public Sub rsSearch(str As [String], con As OdbcConnection, ByRef com As OdbcCommand)
        Dim com2 As OdbcCommand
        'Now we will create a command
        Try
            If con.State <> ConnectionState.Open Then
                con = New OdbcConnection(MyConString)
                con.Open()
            End If

            com2 = New OdbcCommand(str, con)
            com = com2
        Catch e As Exception
            MessageBox.Show("Unable to connect to database server. " & vbLf & " " + "Please contact your administrator if problem persist", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            com = Nothing
        End Try
    End Sub

    Public Sub rsADD(str As [String], con As OdbcConnection, ByRef com As OdbcCommand)
        Dim com2 As OdbcCommand

        Try
            If con.State <> ConnectionState.Open Then
                con = New OdbcConnection(MyConString)
                con.Open()
            End If
            com2 = New OdbcCommand(str, con)
            com = com2
        Catch e As Exception
            MessageBox.Show("Unable to connect to database server. " & vbLf & " " + "Please contact your administrator if problem persist", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            com = Nothing
        End Try
    End Sub

    Public Sub rsDEL(str As [String], con As OdbcConnection, ByRef com As OdbcCommand)
        Dim com2 As OdbcCommand

        Try
            If con.State <> ConnectionState.Open Then
                con = New OdbcConnection(MyConString)
                con.Open()
            End If

            com2 = New OdbcCommand(str, con)
            com = com2
        Catch e As Exception
            MessageBox.Show("Unable to connect to database server. " & vbLf & " " + "Please contact your administrator if problem persist", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            com = Nothing
        End Try
    End Sub

    Public Sub rsValidate(str As [String], con As OdbcConnection, ByRef com As OdbcCommand)
        Dim com2 As OdbcCommand
        'Now we will create a command
        Try
            If con.State <> ConnectionState.Open Then
                con = New OdbcConnection(MyConString)
                con.Open()
            End If

            com2 = New OdbcCommand(str, con)
            com = com2
        Catch e As Exception
            MessageBox.Show("Unable to connect to database server. " & vbLf & " " + "Please contact your administrator if problem persist", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            com = Nothing
        End Try
    End Sub

    Public Sub rsDisplay(str As [String], con As OdbcConnection, ByRef com As OdbcCommand)
        Try

            If con.State <> ConnectionState.Open Then
                con = New OdbcConnection(MyConString)
                con.Open()
            End If

            com2 = New OdbcCommand(str, con)
            com = com2
        Catch e As Exception
            MessageBox.Show("Unable to connect to database server. " & vbLf & " " + "Please contact your administrator if problem persist", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            com = Nothing
        End Try
    End Sub

    Public Sub rsAdapter(str As [String], con As OdbcConnection, ByRef comAdap As OdbcDataAdapter)
        If con.State <> ConnectionState.Open Then
            con = New OdbcConnection(MyConString)
            con.Open()
        End If

        Dim comAdap2 As New OdbcDataAdapter(str, con)
        comAdap = comAdap2
    End Sub

    Public Sub rsAdapter2(str As [String], con As OdbcConnection, ByRef comAdap As OdbcDataAdapter)
        If con.State <> ConnectionState.Open Then
            con = New OdbcConnection(MyConString)
            con.Open()
        End If

        Dim comAdap2 As New OdbcDataAdapter(str, con)
        comAdap = comAdap2
    End Sub

    Public Sub disconnect()

    End Sub

    Public Property GlobalVar() As String
        Get
            Return m_globalVar
        End Get
        Set(value As String)
            m_globalVar = value
        End Set
    End Property

    Public Function KillSleepingConnections(iMinSecondsToExpire As Integer) As Integer
        Dim strSQL As String = "show processlist"
        Dim m_ProcessesToKill As System.Collections.ArrayList = New ArrayList()

        Dim myConn As New OdbcConnection("DRIVER={MySQL ODBC 5.1 Driver};SERVER=localhost;DATABASE=pos_db; USER=root;PASSWORD=@dm1n;")
        Dim myCmd As New OdbcCommand(strSQL, myConn)
        Dim MyReader As OdbcDataReader = Nothing

        Try
            myConn.Open()

            ' Get a list of processes to kill.
            MyReader = myCmd.ExecuteReader()
            While MyReader.Read()
                ' Find all processes sleeping with a timeout value higher than our threshold.
                Dim iPID As Integer = Convert.ToInt32(MyReader("Id").ToString())
                Dim strState As String = MyReader("Command").ToString()
                Dim iTime As Integer = Convert.ToInt32(MyReader("Time").ToString())

                If strState = "Sleep" AndAlso iTime >= iMinSecondsToExpire AndAlso iPID > 0 Then
                    ' This connection is sitting around doing nothing. Kill it.
                    m_ProcessesToKill.Add(iPID)
                End If
            End While

            MyReader.Close()

            For Each aPID As Integer In m_ProcessesToKill
                strSQL = "kill " + aPID
                myCmd.CommandText = strSQL
                myCmd.ExecuteNonQuery()
            Next
        Catch excep As Exception
        Finally
            If MyReader IsNot Nothing AndAlso Not MyReader.IsClosed Then
                MyReader.Close()
            End If

            If myConn IsNot Nothing AndAlso myConn.State = ConnectionState.Open Then
                myConn.Close()
            End If
        End Try

        Return m_ProcessesToKill.Count

    End Function
End Class
