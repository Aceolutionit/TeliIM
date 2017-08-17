Option Explicit On

#Region "Imports"
Imports System
Imports System.Data
Imports System.Data.OleDb
Imports MySql.Data.MySqlClient

#End Region
Module modConnection

    'Public mvarCon As String = "server=127.0.0.1; user id=root; password=Ace@2017; database=tsretail_mannequeen"
    Public mvarCon As String = System.Configuration.ConfigurationSettings.AppSettings("mysql_server").ToString()

End Module
